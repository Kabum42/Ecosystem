using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	public Grabbable grabbableParent;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;

	private static float lastTimeChecked = 0f;
	private static CursorOption lastCursor = CursorOption.Arrow;

	void Start () {

		if (ObjectGrabber.instance == null) {
			ObjectGrabber.instance = Camera.main.GetComponent<ObjectGrabber> ();
		}

	}

	void Update () {

		if (ObjectGrabber.instance.trueOver() == this.gameObject && ObjectGrabber.instance.grabbedObject != this.gameObject) {

			setHand ();

			if (Input.GetMouseButtonDown (0)) {

				Grab ();

			}
		} else {

			setArrow ();

		}

	}

	public void Grab() {

		if (grabbableParent == null) {

			ObjectGrabber.instance.Grab (this.gameObject);

		} else {

			grabbableParent.Grab ();

		}

	}

	void setHand() {

		if (lastTimeChecked == Time.time) {
			// OVERRIDES
			lastCursor = CursorOption.Hand;

		} else {

			setPrevious ();
			lastCursor = CursorOption.Hand;

		}

	}

	void setArrow() {

		if (lastTimeChecked == Time.time) {

			// DOESN'T OVERRIDE

		} else {

			setPrevious ();
			lastCursor = CursorOption.Arrow;

		}

	}

	void setPrevious() {

		if (lastCursor == CursorOption.Arrow) {
			
			Texture2D cursorTexture = Resources.Load ("2d/cursor_arrow", typeof(Texture2D)) as Texture2D;
			Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);

		} else if (lastCursor == CursorOption.Hand) {
			
			Texture2D cursorTexture = Resources.Load ("2d/cursor_hand", typeof(Texture2D)) as Texture2D;
			Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);

		}

		lastTimeChecked = Time.time;

	}

	private enum CursorOption {
		Arrow,
		Hand
	}

}
