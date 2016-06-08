using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();

		float d = (device.distance*1000);
		float a = device.distanceAccuracy;

		Text text = GetComponent<Text> ();

		text.text = "Distance: " + d.ToString ("F0") + " meters (+/- "+a.ToString("F0")+")";
		if (device.skipped > 0) {
			text.text += " (skipped " + device.skipped + ")";
		}

	}

}
