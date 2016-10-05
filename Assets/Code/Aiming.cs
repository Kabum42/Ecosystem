using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour {

	GameObject selectedObject;

	void Start () {
	
	}
	
	void Update () {
		RayCastMouse ();
	}

	void RayCastMouse() {
		Ray aimingRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (aimingRay, out hit)) {
			if (hit.collider.tag == "interactive") {
				if (selectedObject == null) {
					selectedObject = hit.collider.gameObject;
					selectedObject.SendMessage ("Outline", true);
				}
				else if (hit.collider.name != selectedObject.name) {
					selectedObject.SendMessage ("Outline", false);
					selectedObject = null;

					selectedObject = hit.collider.gameObject;
					selectedObject.SendMessage ("Outline", true);
				}
			}
			else if (selectedObject != null) {
				selectedObject.SendMessage ("Outline", false);
				selectedObject = null;
			}
		}
		else if (selectedObject != null) {
			selectedObject.SendMessage ("Outline", false);
			selectedObject = null;
		}
	}
}
