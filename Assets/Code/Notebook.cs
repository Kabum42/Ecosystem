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

	public static Notebook Instance;

	void Start () {
		Instance = this;
	}
	
	void Update () {
		if (Camera.main.GetComponent<ObjectGrabber> ().grabbedObject == this.gameObject && !turningPage) {
			CheckInput ();
		}
			
		if (turningPage) {
			if (actualPage != desirePage) {
				if (turningLeft)
					TurnPage (pages [actualPage], turningLeft);
				else
					TurnPage (pages [actualPage - 1], turningLeft);
			} else
				turningPage = false;
				//WaitPageTurn ();
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
				turningPage = true;
				ShowPage (actualPage + 1);
				WaitPageTurn ();
			}
		}

		//SHOW PREVIOUS PAGE
		if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
			if (actualPage > 0) {
				turningPage = true;
				ShowPage (actualPage - 1);
				WaitPageTurn ();
			}
		}
	}

	public void ShowPage(int i) {
		turningLeft = (actualPage < i);
		desirePage = i;
	}

	void TurnPage(GameObject page, bool left) {
		if (left) {
			if (page.transform.eulerAngles.y < 160f) {
				page.transform.RotateAround (pivotRot.position, transform.up, 10f);
			} else
				actualPage++;
		} 
		else {
			if (page.transform.eulerAngles.y > 0f) {
				page.transform.RotateAround (pivotRot.position, transform.up, -10f);
			} else
				actualPage--;
		}
	}

	IEnumerator WaitPageTurn() {
		yield return new WaitForSeconds (1f);
		turningPage = false;
	}
}
