using UnityEngine;
using System.Collections;

/* Speedometer uses this to start location services */
public class LocationServiceController : MonoBehaviour {

	// Let system popup be displayed to enable location services?
	public bool enableByRequest = true;

	// How long to wait before giving up starting the service (seconds)
	public int maxWait = 10;

	// Value set by this class to indicate service is ready to used
	public bool ready = false;

	IEnumerator Start(){

		LocationService service = Input.location;

		// First, check if user has location service enabled
		// (If not enabled, OS may display a popup to authorize it)
		if (!enableByRequest && !service.isEnabledByUser) {
			print("Location Services not enabled by user");
			yield break;
		}

		// Start service before querying location
		service.Start();

		// Wait until service initializes
		while (service.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1){
			print("Timed out");
			yield break;
		}

		// Connection has failed
		if (service.status == LocationServiceStatus.Failed) {
			print("Unable to determine device location");
			yield break;
		} else {
			// Access granted and location value could be retrieved
			print("Location: " + service.lastData.latitude + " " + service.lastData.longitude + " " + service.lastData.altitude + " " + service.lastData.horizontalAccuracy + " " + service.lastData.timestamp);
		}

		// Stop service if there is no need to query location updates continuously
		//service.Stop();
		print("Service started");

		ready = true;

	}
}