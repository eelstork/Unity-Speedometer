using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RawLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();

		float rawD = (device.rawDistance*1000);
		float rawS = (device.rawSpeed);

		Text text = GetComponent<Text> ();

		if (!device.ready) {
			text.text = "(waiting for data)";
			return;
		}


		text.text = "(raw: "+rawS.ToString("F1")+" km/h over " + rawD.ToString ("F0") + " meters)";

	}

}
