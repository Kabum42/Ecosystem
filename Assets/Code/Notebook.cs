using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notebook : MonoBehaviour {

	[SerializeField] List<GameObject> pages = new List<GameObject>();
	[SerializeField] List<GameObject> separators = new List<GameObject>();
	int actualPage = 0;

	void Start () {
	}
	
	void Update () {
		if (Camera.main.GetComponent<ObjectGrabber> ().grabbedObject == this.gameObject) {
			CheckInput ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha1))
			ShowPage (0);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			ShowPage (1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			ShowPage (2);
	}

	void CheckInput() {
		foreach (GameObject s in separators) {
			if (Hacks.isOver (s)) {
				s.GetComponent<Outliner> ().enabled = true;
				if (Input.GetMouseButtonDown (0)) {
					int index = separators.IndexOf (s);
					ShowPage (index + 1);
				}
			}
			else {
				if(s.GetComponent<Outliner>().isOutlined)
					s.GetComponent<Outliner> ().Outline (false);
				
				s.GetComponent<Outliner> ().enabled = false;
			}
		}
	}

	void ShowPage(int i) {
		if (actualPage < i) {
			for (int j = actualPage; j < i; j++) {
				TurnPage (pages [j], true);
			}
			actualPage = i;
		} else if (actualPage > i) {
			for (int j = i; j < actualPage; j++) {
				TurnPage (pages [j], false);
			}
			actualPage = i;
		}
	}

	void TurnPage(GameObject page, bool left) {
		if(left)
			//page.transform.RotateAround (rotPoint, page.transform.up, 180f);
			page.transform.Translate(page.transform.right * -2f);
		else
			//page.transform.RotateAround (rotPoint, page.transform.up, -180f);
			page.transform.Translate(page.transform.right * 2f);
	}
}
