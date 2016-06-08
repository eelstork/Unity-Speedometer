using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();

		int speed = Mathf.RoundToInt(device.speedInKmPerHour);
		Text text = GetComponent<Text> ();

		if (!device.ready) {
			text.text = "Speed: N/A"; 
			return;
		}

		text.text = speed + " km/h";

	}

}
