using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationLabel : MonoBehaviour {

	// format: "40.446° N 79.982° W"
	void Update () {

		LocationInfo loc = Input.location.lastData;

		Text text = GetComponent<Text> ();

		if (Input.location.status != LocationServiceStatus.Running) {
			text.text="Location unknown";
			return;
		}

		text.text = loc.latitude.ToString ("F6") + "° N "
		+ loc.longitude.ToString ("F6") + "° W";

	}

}
