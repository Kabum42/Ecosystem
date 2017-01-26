using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ending : MonoBehaviour {

	public GameObject bad;
	public GameObject good;
	public Image fade;

	private bool showing = true;

	// Use this for initialization
	void Start () {

		if (Random.Range (0f, 100f) > 50f) {
			bad.SetActive (true);
		} else {
			good.SetActive (true);
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if (showing) {

			fade.color = Hacks.ColorLerpAlpha (fade.color, -0.1f, Time.deltaTime * 2f);

			if (Input.GetMouseButtonDown(0)) {
				showing = false;
			}

		} else {

			fade.color = Hacks.ColorLerpAlpha (fade.color, 1.1f, Time.deltaTime * 2f);

			if (fade.color.a >= 1f) {
				Ecosystem.Restart ();
				Application.LoadLevel (0);
			}


		}
	
	}
}
