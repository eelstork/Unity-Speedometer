using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationServiceStatusLabel : MonoBehaviour {

	void Update () {
		
		Text text = GetComponent<Text> ();
		LocationServiceStatus status = Input.location.status;
		text.text = "Location services: "+status;

	}

}
