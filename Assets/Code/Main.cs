using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	private LetterStack todayStack;

	// Use this for initialization
	void Start () {
	
		todayStack = new LetterStack ();
		todayStack.addLetters (5);

	}
	
	// Update is called once per frame
	void Update () {
	
		todayStack.Update ();

	}


	public class LetterStack {

		public GameObject gameobject;
		public List<PhysicalLetter> pLetterList = new List<PhysicalLetter>();
		private bool lastTimeGrabbed = false;

		public LetterStack() {

			gameobject = new GameObject();
			gameobject.transform.position = new Vector3(-0.46f, 5.23f, 14.63f);
			gameobject.AddComponent<Grabbable>();
			gameobject.GetComponent<Grabbable>().rotationGrabbed = new Vector3(-80f, 0f, 0f);
			gameobject.GetComponent<Grabbable>().positionGrabbed = new Vector3(0f, 0f, 3f);

		}

		public void addLetters(int num) {

			float distance = 0f;

			for (int i = 0; i < num; i++) {
				PhysicalLetter pL = new PhysicalLetter ();
				pL.AssignMessage (new Message ("hehe"));
				pL.gameObject.transform.SetParent (gameobject.transform);
				pL.gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
				pL.gameObject.transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
				pL.gameObject.GetComponent<Grabbable> ().grabbableParent = gameobject.GetComponent<Grabbable> ();

				pL.targetLocalEulerAngles = new Vector3 (0f, Random.Range (-10f, 10f), 0f);
				pL.gameObject.transform.localEulerAngles = pL.targetLocalEulerAngles;
				pL.gameObject.transform.localPosition = new Vector3 (0f, distance, 0f);
				pL.gameObject.transform.FindChild ("Sender").GetComponent<TextMesh> ().text = "" + i;
				pLetterList.Add (pL);
				distance += 0.03f;
			}

		}

		public void Update() {

			if (Grabbable.objectGrabber.grabbedObject == gameobject) {

				PhysicalLetter currentPL = pLetterList [pLetterList.Count -1];

				foreach (TextMesh optionTM in currentPL.optionsTextMesh) {

					if (Hacks.isOver (optionTM.gameObject)) {
						optionTM.color = Color.green;
					} else {
						optionTM.color = Color.gray;
					}

				}

				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					PhysicalLetter pL = pLetterList [0];
					pLetterList.Remove(pL);
					pLetterList.Add (pL);
				} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
					PhysicalLetter pL = pLetterList[pLetterList.Count - 1];
					pLetterList.Remove(pL);
					pLetterList.Insert (0, pL);
				}

				if (!lastTimeGrabbed) {
					foreach (PhysicalLetter pL in pLetterList) {
						pL.targetLocalEulerAngles = Vector3.zero;
					}
				}
				lastTimeGrabbed = true;

			} else {
				if (lastTimeGrabbed) {
					
					foreach (PhysicalLetter pL in pLetterList) {
						pL.targetLocalEulerAngles = new Vector3 (0f, Random.Range (-10f, 10f), 0f);
					}
				}
				lastTimeGrabbed = false;

			}

			float distance = 0f;

			foreach (PhysicalLetter pL in pLetterList) {

				pL.gameObject.transform.localEulerAngles = Hacks.LerpVector3Angle(pL.gameObject.transform.localEulerAngles, pL.targetLocalEulerAngles, Time.deltaTime*3f);
				pL.gameObject.transform.localPosition = Vector3.Lerp (pL.gameObject.transform.localPosition, new Vector3 (0f, distance, 0f), Time.deltaTime * 10f);
				distance += 0.03f;

			}

		}

	}

}
