using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectGrabber : MonoBehaviour {

	public GameObject grabbedObject;
	private Vector3 originalPosition = Vector3.zero;
	private List<ReturningObject> returningObjects = new List<ReturningObject>();

	// Use this for initialization
	void Start () {
	
		grabbedObject = null;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0) && !Hacks.isOver(grabbedObject)) {

			if (grabbedObject != null) {
				returningObjects.Add (new ReturningObject (originalPosition, grabbedObject));
			}
			grabbedObject = null;

		}

		if (grabbedObject != null) {

			Vector3 targetPosition = this.transform.position + this.transform.forward * 1f;
			grabbedObject.transform.position = Vector3.Lerp (grabbedObject.transform.position, targetPosition, Time.deltaTime * 5f);

		}

		foreach (ReturningObject r in returningObjects) {

			r.gameObject.transform.position = Vector3.Lerp (r.gameObject.transform.position, r.originalPosition, Time.deltaTime * 5f);

		}
	
	}

	public void Grab (GameObject g) {

		if (g != grabbedObject) {
			RemoveFromReturningObjects (g);
			originalPosition = g.transform.position;
			grabbedObject = g;
		}

	}

	private void RemoveFromReturningObjects(GameObject g) {

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
		public GameObject gameObject;

		public ReturningObject(Vector3 auxOriginalPosition, GameObject auxGameObject) {

			originalPosition = auxOriginalPosition;
			gameObject = auxGameObject;

		}

	}

}
