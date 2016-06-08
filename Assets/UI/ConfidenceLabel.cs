using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfidenceLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();
		float c = device.confidence;

		Text text = GetComponent<Text> ();

		if (!device.ready) {
			text.text = ""; 
			return;
		}

		if (c < 0f) {
			text.text = "Stable location";
		} else {
			text.text = "Confidence: " + device.confidence.ToString("F2");
			text.text += " ("+device.used+"/"+device.samples+")";
		}

	}

}
