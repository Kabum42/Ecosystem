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

	public bool grabbed = false;
    public Vector3 initialEulerAngles;
    public int turningPageVelocity;

	public static Notebook Instance;

    void Awake() {
        Instance = this;
    }

	void Start () {
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
				page.transform.RotateAround (pivotRot.position, transform.up, turningPageVelocity);
				turningCounter += turningPageVelocity;
			}
			else {
				actualPage++;
				turningCounter = 0;
			}
		} 
		else {
			if (turningCounter > 0) {
				page.transform.RotateAround (pivotRot.position, transform.up, turningPageVelocity*-1);
				turningCounter -= turningPageVelocity;
			}
			else {
				actualPage--;
				turningCounter = 160;
			}
		}
	}

    public IEnumerator OpenNotebook(int notebookPage)
    {
        Camera.main.GetComponent<ObjectGrabber>().Grab(this.gameObject);
        yield return new WaitForSeconds(0.5f);
        ShowPage(notebookPage);
    }
}
