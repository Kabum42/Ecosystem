using UnityEngine;
using System.Collections;

public class ZoneCollider : MonoBehaviour {

	int notebookPage;
	Camera camera;

	void Start() {
		camera = GameObject.Find ("CameraRenderTexture").GetComponent<Camera> ();
	}

	void Update () {
//		if (isOver (this.gameObject, camera)) {
//			if (Input.GetMouseButtonDown (0)) {
//				StartCoroutine (OpenNotebook ());
//			}
//		}
	}

	public void SetPage(int i) {
		notebookPage = i;
	}

	IEnumerator OpenNotebook() {
		Camera.main.GetComponent<ObjectGrabber> ().Grab (Notebook.Instance.gameObject);
		yield return new WaitForSeconds (0.5f);
		Notebook.Instance.ShowPage (notebookPage);
	}

//	bool isOver(GameObject target, Camera cam) {
//		Ray aimingRay = cam.ScreenPointToRay (Input.mousePosition);
//		RaycastHit hit;
//
//		if (Physics.Raycast (aimingRay, out hit)) {
//			Debug.DrawRay (Input.mousePosition, (hit.point - Input.mousePosition));
//			if (hit.collider.gameObject == target) {
//				return true;
//			}
//
//		} else if (target == null) {
//			return true;
//		}
//
//		return false;
//	}
}
