using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedLabel : MonoBehaviour {

	void Update () {

		int speed = (int)(FindObjectOfType<Speedometer> ().speedInKmPerHour);

		Text text = GetComponent<Text> ();
		text.text = speed + " km/h";

	}

}
