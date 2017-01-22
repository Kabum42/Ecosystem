using UnityEngine;
using System.Collections;

public class ZoneCollider : MonoBehaviour {

	int notebookPage;
	Camera camera;

	void Start() {
        camera = Camera.main;
	}

	void Update () {
        if (Hacks.isOver(this.gameObject))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!Notebook.Instance.grabbed)
                    StartCoroutine(OpenNotebook());
            }
        }
    }

    public void SetPage(int i) {
		notebookPage = i;
	}

	IEnumerator OpenNotebook() {
        Camera.main.GetComponent<ObjectGrabber>().Grab(Notebook.Instance.gameObject);
        yield return new WaitForSeconds(0.5f);
		Notebook.Instance.ShowPage (notebookPage);
	}
}
