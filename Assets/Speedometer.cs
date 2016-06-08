using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Referring: 
 * https://communityhealthmaps.nlm.nih.gov/2014/07/07/how-accurate-is-the-gps-on-my-smart-phone-part-2/
 */
public class Speedometer : MonoBehaviour{

	public enum Heuristic{ TWO_POINT, NO_CONFIDENCE, TIMEOUT } 

	[Header("Parameters")] // -------------------------------------

	[Tooltip("Max number of locations to buffer")]
	public int recordSize = 10;

	// Two-point method may pick earlier samples to 
	// increase accuracy, at the expensive of responsiveness
	[Tooltip("Recommended values from 0.25 (more responsive) to 0.75 (more accurate)")]
	float minConfidence = 0.35f;

	// Since the location tends to not update when not moving,
	// after a timeout (in seconds) we assume that speed is zero
	[Tooltip("After timeout, if no location updates assume immobility")]
	int timeout = 10;

	[Header("Output (do not edit)")] // ----------------------------

	// The method used to evaluate speed
	public Heuristic heuristic = Heuristic.TWO_POINT;
	// Speed, using heuristics to filter the data
	public float speedInKmPerHour = 0f;
	// Given location accuracy, confidence in the given speed reading.
	public float confidence = 0f;
	// Time elapsed between considered samples
	public float timeframe = 0f;
	// The distance between considered samples
	public float distance = 0f;
	// Possible variation between reported and actual distance
	public float distanceAccuracy = 0f;
	// Number of samples in-between considered samples
	public int skipped = 0;
	// Min/Max speed taking location accuracy into account.
	// This is always based on the two-point method.
	public float minSpeedInKmPerHour = 0f;
	public float maxSpeedInKmPerHour = 0f;

	// Speed, taking into account the two latest location samples
	public float rawSpeed=0f;
	// Distance between the latest two samples
	public float rawDistance=0f;

	// ------------------------------------------------------------------

	private double timestamp = 0;
	private List<LocationInfo> record = new List<LocationInfo> ();
	private LocationServiceController service;

	// -------------------------------------------------------------------

	/* Request location services to start */
	void Start(){

		service = gameObject.AddComponent<LocationServiceController> ();

	}

	/* Wait until services are ready */
	void Update(){

		if (!service.ready)return;
		AddValidSample ();
		if (record.Count >= 2) {
			UpdateSpeed ();
		}
		if (outOfDate) {
			speedInKmPerHour = 0f;
			heuristic = Heuristic.TIMEOUT;
		}
			
	}
	
	void AddValidSample(){

		LocationInfo sample = current;
		// discard duplicates
		if (record.Count > 0 && sample.timestamp == last.timestamp) {
			return;
		}
		timestamp = sample.timestamp;
		record.Add (sample);
		while (record.Count > recordSize) record.RemoveAt (0);

	}

	void UpdateSpeed(){

		heuristic = Heuristic.TWO_POINT;

		rawDistance = DistInKm (forelast, last);
		rawSpeed = SpeedInKmpH (rawDistance, TimeFrameInSeconds (forelast, last));
			
		SpeedEvaluation sel = new SpeedEvaluation();
		sel.minSpeed = 1f;
		sel.maxSpeed = -1f;

		for (int i = record.Count - 2; i >= 0; i--) {
			SpeedEvaluation val = new SpeedEvaluation ( record[i], last, (record.Count-2)-i );
			if (val.confidence > sel.confidence) {
				sel = val;
			}
			if (sel.confidence >= minConfidence) break;
		}
		assignSpeedEvaluation (sel);

		// Typically 0 confidence on speed readings
		// Signals overlapping samples as produced when the device
		// is not in motion.

		if (sel.confidence <= 0f) {
			speedInKmPerHour = 0f;
			heuristic = Heuristic.NO_CONFIDENCE;
		}

	}

	private void assignSpeedEvaluation(SpeedEvaluation val){
			speedInKmPerHour = val.speed;
			minSpeedInKmPerHour = val.minSpeed;
			maxSpeedInKmPerHour = val.maxSpeed;
			confidence = val.confidence;
			distance = val.distance;
			timeframe = val.timeframe;
			distanceAccuracy = val.distanceAccuracy;
			skipped = val.skipped;
			return;
	}

	LocationInfo last		{ get { return record [record.Count-1]; } }
	LocationInfo forelast	{ get { return record [record.Count-2]; } }
	LocationInfo current	{ get { return Input.location.lastData; } }

	private float secondsSinceLastSample{
		get{ return (float)(secondsSinceEpoch - timestamp); }
	}

	bool outOfDate{
		get{ return secondsSinceLastSample > timeout; }
	}
		
	public static bool IsSameLocation(LocationInfo A, LocationInfo B){
		return (	
			A.latitude==B.latitude
			&& A.longitude==B.longitude
			&& A.horizontalAccuracy==B.horizontalAccuracy );
	}

	public static float TimeFrameInSeconds(LocationInfo A, LocationInfo B){
		return (float)(B.timestamp-A.timestamp);
	}

	public static float SpeedInKmpH(float distInKm, float timeInSeconds){

		if (distInKm < 0f) return 0f;
		return (distInKm / timeInSeconds) * 3600;

	}

	public static float[] DistRangeInKm(LocationInfo A, LocationInfo B){
	
		float d = DistInKm (A, B);
		float r = HorizontalDistanceAccuracyInMeters(A,B)*0.001f;
		return new float[]{ d - r, d + r };

	}

	public static float HorizontalDistanceAccuracyInMeters(LocationInfo A, LocationInfo B){

		return A.horizontalAccuracy + B.horizontalAccuracy;

	}

	public static float DistInKm(LocationInfo A, LocationInfo B){

		float lat1 = A.latitude;
		float lon1 = A.longitude;
		float lat2 = B.latitude;
		float lon2 = B.longitude;
		float R = 6371e3f; // ~ earth radius in metres
		float φ1 = lat1*Mathf.Deg2Rad;
		float φ2 = lat2*Mathf.Deg2Rad;
		float Δφ = (lat2-lat1)*Mathf.Deg2Rad;
		float Δλ = (lon2-lon1)*Mathf.Deg2Rad;
		float a = Mathf.Sin(Δφ/2) * Mathf.Sin(Δφ/2) 
			+ Mathf.Cos(φ1) * Mathf.Cos(φ2) * Mathf.Sin(Δλ/2) * Mathf.Sin(Δλ/2);
		float c = 2f * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1f-a));
		float d = R * c;
		return (float)d * 0.001f;  

	}

	public string heuristicDescription{
		get{
			if (!ready)
				return "Waiting for data";
			switch (heuristic) {
			case Heuristic.TWO_POINT:
				return "Two-point method";
			case Heuristic.NO_CONFIDENCE:
				return "Samples overlap";
			case Heuristic.TIMEOUT:
				return secondsSinceLastSample.ToString ("F0") + " seconds timeout";
			}
			return "???";
		}
	}

	public bool ready{ get{ return samples>=2; } }

	public int used{ get{ return skipped+2; } }
	public int samples{ get{ return record.Count; } }

	private double secondsSinceEpoch{
		get{
			return (System.DateTime.UtcNow 
				- new System.DateTime (1970, 1, 1)).TotalSeconds;
		}
	}

	struct SpeedEvaluation{

		public float speed, distance, timeframe;
		public float distanceAccuracy;
		public float minSpeed, maxSpeed;

		public int skipped;

		public SpeedEvaluation(LocationInfo A, LocationInfo B, int skipped){

			distance  = DistInKm(A,B);
			timeframe = TimeFrameInSeconds(A,B);
			speed     = SpeedInKmpH(distance,timeframe);
				
			float[] D = DistRangeInKm(A,B);
			distanceAccuracy = HorizontalDistanceAccuracyInMeters(A,B);

			minSpeed = SpeedInKmpH(D[0],timeframe);
			maxSpeed = SpeedInKmpH(D[1],timeframe);

			this.skipped=skipped;

		}
			
		public float confidence{
			get{ 
				return minSpeed / maxSpeed; 
			}
		}

	}

}