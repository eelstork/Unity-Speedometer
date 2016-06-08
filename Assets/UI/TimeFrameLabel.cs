using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeFrameLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();
		Text text = GetComponent<Text> ();

		if (!device.ready) {
			text.text = "Timeframe N/A";
			return;
		}

		float f = device.timeframe;

		text.text = "Timeframe: " + f.ToString("F1") + " seconds";

	}

}
