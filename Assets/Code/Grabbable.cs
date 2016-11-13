using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	ObjectGrabber objectGrabber;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;

	private static float lastTimeChecked = 0f;
	private static CursorOption lastCursor = CursorOption.Arrow;

	void Start () {

		objectGrabber = Camera.main.gameObject.GetComponent<ObjectGrabber> ();

	}

	void Update () {

		if (Hacks.isOver (this.gameObject)) {

			setHand ();

			if (Input.GetMouseButtonDown (0)) {
				objectGrabber.Grab (this.gameObject);
			}
		} else {

			setArrow ();

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
