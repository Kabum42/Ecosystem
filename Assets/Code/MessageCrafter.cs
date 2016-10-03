using UnityEngine;
using System.Collections;

public class MessageCrafter : MonoBehaviour {

	public RectTransform parentUI;
	private static float speed = 200f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		handleNavigation ();
	
	}

	void handleNavigation() {

		if (Input.GetKey (KeyCode.UpArrow) && parentUI.anchoredPosition3D.y > 0f) {
			parentUI.position = parentUI.position + new Vector3 (0f, -Time.deltaTime * speed, 0f);
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			parentUI.position = parentUI.position + new Vector3 (0f, Time.deltaTime * speed, 0f);
		}



	}

}
