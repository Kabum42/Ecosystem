using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGrabber : MonoBehaviour {

	public GameObject grabbedObject;
	private Vector3 originalPosition = Vector3.zero;
	private Vector3 originalRotation = Vector3.zero;
	private Transform originalParent = null;
	private List<ReturningObject> returningObjects = new List<ReturningObject>();

	// Use this for initialization
	void Start () {
	
		grabbedObject = null;

		Message m = new Message ("hehe");

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0) && !Hacks.isOver(grabbedObject)) {

			if (grabbedObject != null) {
				ReturnGrabbedObject ();
			}

		}

		if (grabbedObject != null) {

			Vector3 targetPosition = this.transform.position + this.transform.forward * grabbedObject.GetComponent<InteractiveObject> ().positionGrabbed.z + this.transform.right * grabbedObject.GetComponent<InteractiveObject> ().positionGrabbed.x + this.transform.up * grabbedObject.GetComponent<InteractiveObject> ().positionGrabbed.y ;
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, targetPosition, Time.deltaTime * 5f);
			grabbedObject.transform.localEulerAngles = Hacks.LerpVector3Angle(grabbedObject.transform.localEulerAngles, grabbedObject.GetComponent<InteractiveObject> ().rotationGrabbed, Time.deltaTime*5f);

		}

		foreach (ReturningObject r in returningObjects) {

			r.gameObject.transform.position = Vector3.Lerp (r.gameObject.transform.position, r.originalPosition, Time.deltaTime * 5f);
			r.gameObject.transform.eulerAngles = Hacks.LerpVector3Angle(r.gameObject.transform.eulerAngles, r.originalRotation, Time.deltaTime*5f);

		}
	
	}

	public void Grab (GameObject g) {

		if (g != grabbedObject) {

			if (grabbedObject != null) {
				ReturnGrabbedObject ();
			}

			FreeObject (g);
			originalPosition = g.transform.position;
			originalRotation = g.transform.eulerAngles;
			originalParent = g.transform.parent;
			grabbedObject = g;

			grabbedObject.transform.SetParent (this.transform);
		}

	}

	private void ReturnGrabbedObject() {

		grabbedObject.transform.SetParent (originalParent);
		returningObjects.Add (new ReturningObject (grabbedObject, originalPosition, originalRotation));

		grabbedObject = null;

	}

	public void FreeObject(GameObject g) {

		ReturningObject r = null;

		foreach (ReturningObject r2 in returningObjects) {
			if (r2.gameObject == g) {
				r = r2;
				break;
			}
		}

		if (r != null) {
			returningObjects.Remove (r);
		}

	}

	public class ReturningObject {

		public Vector3 originalPosition;
		public Vector3 originalRotation;
		public GameObject gameObject;

		public ReturningObject(GameObject auxGameObject, Vector3 auxPosition, Vector3 auxRotation) {

			gameObject = auxGameObject;
			originalPosition = auxPosition;
			originalRotation = auxRotation;

		}

	}

}
