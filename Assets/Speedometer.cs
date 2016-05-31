using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Referring: 
 * https://communityhealthmaps.nlm.nih.gov/2014/07/07/how-accurate-is-the-gps-on-my-smart-phone-part-2/
 */
public class Speedometer : MonoBehaviour{

	public float speedInKmPerHour = 0f;
	public float timeframe = 0f;
	public int recordSize = 2;

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
			UpdateSpeed (forelast, last);
		}
			
	}
	
	void AddValidSample(){

		LocationInfo sample = current;
		// discard duplicates
		if (record.Count > 0 && sample.timestamp == last.timestamp) {
			return;
		}
		record.Add (sample);
		while (record.Count > recordSize) record.RemoveAt (0);

	}

	void UpdateSpeed(LocationInfo a, LocationInfo b){
		
		float D = Speedometer.distInKm (a, b); 
		timeframe = (float)(b.timestamp - a.timestamp);
		float speedInKmPerSecond = D / timeframe;
		speedInKmPerHour = speedInKmPerSecond * 3600;

	}

	LocationInfo last		{ get { return record [record.Count-1]; } }
	LocationInfo forelast	{ get { return record [record.Count-2]; } }
	LocationInfo current	{ get { return Input.location.lastData; } }

	public static float distInKm(LocationInfo A, LocationInfo B){

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

}