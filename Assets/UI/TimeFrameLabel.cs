using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeFrameLabel : MonoBehaviour {

	void Update () {

		float f = (FindObjectOfType<Speedometer> ().timeframe);

		Text text = GetComponent<Text> ();
		text.text = "Timeframe: " + f.ToString("F1") + " seconds";

	}

}
