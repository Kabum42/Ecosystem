using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	ObjectGrabber objectGrabber;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;

	void Start () {

		objectGrabber = Camera.main.gameObject.GetComponent<ObjectGrabber> ();

	}

	void Update () {

		if (Hacks.isOver (this.gameObject)) {
			if (Input.GetMouseButtonDown (0)) {
				objectGrabber.Grab (this.gameObject);
			}
		}

	}

}
