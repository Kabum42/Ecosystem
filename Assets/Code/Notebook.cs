using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Notebook : MonoBehaviour {

	[SerializeField] List<GameObject> pages = new List<GameObject>();
	//[SerializeField] List<GameObject> separators = new List<GameObject>();
	[SerializeField] Transform pivotRot;
	int actualPage = 0;
	int desirePage = 0;
	bool turningPage = false;
	bool turningLeft;
	int turningCounter = 0;
	bool grabbed = false;

	public static Notebook Instance;

	void Start () {
		Instance = this;
	}
	
	void Update () {
		if (Camera.main.GetComponent<ObjectGrabber> ().grabbedObject == this.gameObject && !turningPage) {
			grabbed = true;
			CheckInput ();
		}
		else if (Camera.main.GetComponent<ObjectGrabber> ().grabbedObject != this.gameObject && grabbed) {
			grabbed = false;
			ShowPage (0);
		}
			
		if (turningPage) {
			if (actualPage != desirePage) {
				if (turningLeft)
					TurnPage (pages [actualPage], turningLeft);
				else
					TurnPage (pages [actualPage - 1], turningLeft);
			} else
				turningPage = false;
		}
	}

	void CheckInput() {
		/*foreach (GameObject s in separators) {
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
		}*/

		//SHOW NEXT PAGE
		if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
			if (actualPage < (pages.Count - 1)) {
				ShowPage (actualPage + 1);
			}
		}

		//SHOW PREVIOUS PAGE
		if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
			if (actualPage > 0) {
				ShowPage (actualPage - 1);
			}
		}
	}

	public void ShowPage(int i) {
		turningPage = true;
		turningLeft = (actualPage < i);
		turningCounter = (turningLeft) ? 0 : 160;
		desirePage = i;
	}

	void TurnPage(GameObject page, bool left) {
		if (left) {
			if (turningCounter < 160) {
				page.transform.RotateAround (pivotRot.position, transform.up, 10f);
				turningCounter += 10;
			}
			else {
				actualPage++;
				turningCounter = 0;
			}
		} 
		else {
			if (turningCounter > 0) {
				page.transform.RotateAround (pivotRot.position, transform.up, -10f);
				turningCounter -= 10;
			}
			else {
				actualPage--;
				turningCounter = 160;
			}
		}

		Debug.Log (turningCounter);
	}
}
