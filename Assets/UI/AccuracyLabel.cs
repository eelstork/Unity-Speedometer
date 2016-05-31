using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AccuracyLabel : MonoBehaviour {

	void Update () {

		Text text = GetComponent<Text> ();

		LocationService service = Input.location;
		if (service.status != LocationServiceStatus.Running) {
			text.text = "accuracy: NA";	
			return;
		}
		LocationInfo info = service.lastData;			
		text.text = "accuracy: " + info.horizontalAccuracy + " meters";

	}

}
