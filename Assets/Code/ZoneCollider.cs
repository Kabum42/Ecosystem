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
                    StartCoroutine(Notebook.Instance.OpenNotebook(notebookPage));
            }
        }
    }

    public void SetPage(int i) {
		notebookPage = i;
	}
}
