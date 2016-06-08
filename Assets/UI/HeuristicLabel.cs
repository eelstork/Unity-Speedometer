using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeuristicLabel : MonoBehaviour {

	void Update () {

		Speedometer device = FindObjectOfType<Speedometer> ();
		Text text = GetComponent<Text> ();
		text.text = device.heuristicDescription;

	}

}
