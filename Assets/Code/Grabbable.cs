using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {

	public Grabbable grabbableParent;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;
    public bool justMouseIcon;
	public CursorOption cursor = CursorOption.Hand;

	private static float lastTimeChecked = 0f;
	private static CursorOption lastCursor = CursorOption.Arrow;

	void Start () {

		if (ObjectGrabber.instance == null) {
			ObjectGrabber.instance = Camera.main.GetComponent<ObjectGrabber> ();
		}

	}

	void Update () {

		if (ObjectGrabber.instance.trueOver() == this.gameObject && ObjectGrabber.instance.grabbedObject != this.gameObject) {

			setSpecialCursor (cursor);

			if (Input.GetMouseButtonDown (0) && !justMouseIcon) {

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

	void setSpecialCursor(CursorOption cO) {

		if (lastTimeChecked == Time.time) {
			// OVERRIDES
			lastCursor = cO;

		} else {

			setPrevious ();
			lastCursor = cO;

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

		Texture2D cursorTexture = Resources.Load ("2d/cursor_"+lastCursor.ToString(), typeof(Texture2D)) as Texture2D;
		Cursor.SetCursor (cursorTexture, Vector2.zero, CursorMode.Auto);

		lastTimeChecked = Time.time;

	}

	public enum CursorOption {
		Arrow,
		Hand,
		Ciervos,
		Conejos,
		Lobos,
		Osos
	}

}
