using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	ObjectGrabber objectGrabber;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;

	private static float lastTimeChecked = 0f;
	private static string consensusTexture = "none";

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

			consensusTexture = "hand";

		} else {

			setPrevious ();

			consensusTexture = "hand";
			lastTimeChecked = Time.time;

		}

	}

	void setArrow() {

		if (lastTimeChecked == Time.time) {



		} else {

			setPrevious ();

			consensusTexture = "arrow";
			lastTimeChecked = Time.time;

		}

	}

	void setPrevious() {

		Texture2D cursorTexture = Resources.Load ("2d/cursor_"+consensusTexture, typeof(Texture2D)) as Texture2D;
		Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);

	}

}
