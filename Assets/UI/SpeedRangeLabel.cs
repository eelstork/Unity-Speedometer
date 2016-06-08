using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedRangeLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();

		Text text = GetComponent<Text> ();

		if (!device.ready) {
			text.text = "Speed range: N/A"; 
			return;
		}

		if (device.confidence < 0f) {
			text.text = "";
		} else {
			int minSpeed = (int)(device.minSpeedInKmPerHour);
			int maxSpeed = (int)(device.maxSpeedInKmPerHour);
			text.text = "(" + minSpeed + "~" + maxSpeed + "km/h)";
		}

	}

}
