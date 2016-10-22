using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notebook : MonoBehaviour {

	[SerializeField] List<GameObject> pages = new List<GameObject>();
	List<GameObject> separators = new List<GameObject>();
	Vector3 rotPoint;
	int actualPage = 0;

	public bool isGrabbed = false;

	void Start () {
		rotPoint = transform.GetChild (0).transform.position;
	}
	
	void Update () {
		if (isGrabbed) {
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
				int index = separators.IndexOf (s);
				ShowPage (index);
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
			page.transform.RotateAround (rotPoint, page.transform.up, 180f);
		else
			page.transform.RotateAround (rotPoint, page.transform.up, -180f);
	}
}
