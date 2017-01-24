using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGrabber : MonoBehaviour {

	public static ObjectGrabber instance;
	public GameObject grabbedObject;
	private Vector3 originalPosition = Vector3.zero;
	private Vector3 originalRotation = Vector3.zero;
	private Transform originalParent = null;
	private List<ReturningObject> returningObjects = new List<ReturningObject>();

	// Use this for initialization
	void Start () {
	
		instance = this;
		grabbedObject = null;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0) && trueOver() != grabbedObject) {

			if (grabbedObject != null) {
				ReturnGrabbedObject ();
			}

		}

		if (grabbedObject != null) {

			Grabbable grabbable = grabbedObject.GetComponent<Grabbable> ();
			grabbedObject.transform.localPosition = Vector3.Lerp (grabbedObject.transform.localPosition, grabbable.positionGrabbed, Time.deltaTime * 5f);
			grabbedObject.transform.localEulerAngles = Hacks.LerpVector3Angle(grabbedObject.transform.localEulerAngles, grabbable.rotationGrabbed, Time.deltaTime*5f);

		}

		foreach (ReturningObject r in returningObjects) {

			r.gameObject.transform.position = Vector3.Lerp (r.gameObject.transform.position, r.originalPosition, Time.deltaTime * 5f);
			r.gameObject.transform.eulerAngles = Hacks.LerpVector3Angle(r.gameObject.transform.eulerAngles, r.originalRotation, Time.deltaTime*5f);

		}
	
	}

	public GameObject trueOver() {

		GameObject[] gameObjects = Hacks.getOverAll ();
		GameObject g = null;

		foreach (GameObject g2 in gameObjects) {

			if (g2.GetComponent<Grabbable> () != null) {
				g = g2;
				break;
			}

		}

		if (g != null) {
			while (g.GetComponent<Grabbable> ().grabbableParent != null) {
				g = g.GetComponent<Grabbable> ().grabbableParent.gameObject;
			}
		}




		return g;

	}

	public void Grab (GameObject g) {
		if (g != grabbedObject) {

			if (grabbedObject != null) {
				ReturnGrabbedObject ();
			}

			originalPosition = g.transform.position;
			originalRotation = g.transform.eulerAngles;
			originalParent = g.transform.parent;
			grabbedObject = g;
			FreeObject (g);

			if (g.GetComponent<Outliner> () != null) {
				g.GetComponent<Outliner> ().Outline (false);
				g.GetComponent<Outliner> ().enabled = false;
			}

			grabbedObject.transform.SetParent (this.transform);
		}

	}

	public void ReturnGrabbedObject() {

		grabbedObject.transform.SetParent (originalParent);
		returningObjects.Add (new ReturningObject (grabbedObject, originalPosition, originalRotation));

		if (grabbedObject.GetComponent<Outliner> () != null) {
			grabbedObject.GetComponent<Outliner> ().enabled = true;
		}

		grabbedObject = null;

	}

	private void FreeObject(GameObject g) {

		ReturningObject r = null;

		foreach (ReturningObject r2 in returningObjects) {
			if (r2.gameObject == g) {
				r = r2;
				break;
			}
		}

		if (r != null) {
			returningObjects.Remove (r);
			originalPosition = r.originalPosition;
			originalRotation = r.originalRotation;
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
