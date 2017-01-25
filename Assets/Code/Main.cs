using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Main : MonoBehaviour {

	public LetterStack todayStack;
	public static List<PhysicalLetter> discardedLetters = new List<PhysicalLetter>();
	public Image fade;
	public TOD_Sky sky;
	private State state = State.Playing;

	private static float minHour = 6f;
	private static float maxHour = 19f;

	// Use this for initialization
	void Awake () {

		Ecosystem.Start ();
		todayStack = new LetterStack ();
		todayStack.addLetters (4);
		fade.color = Hacks.ColorLerpAlpha (fade.color, 1f, 1f);
		state = State.On;
		sky.Cycle.Hour = minHour;

	}
	
	// Update is called once per frame
	void Update () {
	
		UpdateDiscarded ();

		if (state == State.Playing) {

			todayStack.Update ();

			if (todayStack.pLetterList.Count == 0) {
				
				ObjectGrabber.instance.ReturnGrabbedObject ();
				state = State.Off;

			}

		} else if (state == State.Off) {

			if (sky.Cycle.Hour == maxHour) {
				fade.color = Hacks.ColorLerpAlpha (fade.color, 1.1f, Time.deltaTime * 2f);
			}

			if (fade.color.a >= 1f) {

				Ecosystem.Simulate ();
				todayStack.originalNum = 0;
				todayStack.addLetters (4);
				sky.Cycle.Hour = minHour;
				state = State.On;

			}

		} else if (state == State.On) {

			fade.color = Hacks.ColorLerpAlpha (fade.color, -0.1f, Time.deltaTime * 2f);

			if (fade.color.a <= 0f) {

				state = State.Playing;

			}

		}

		float percentageHour =  1f - (float)todayStack.pLetterList.Count/(float)todayStack.originalNum;
		float targetHour = minHour + (maxHour - minHour) * percentageHour;

		sky.Cycle.Hour = Mathf.MoveTowards (sky.Cycle.Hour, targetHour, 0.01f);

		if (targetHour == maxHour) {

			sky.Cycle.Hour = Mathf.MoveTowards (sky.Cycle.Hour, targetHour, 0.1f);

		}


	}

	private void UpdateDiscarded() {

		foreach (PhysicalLetter pL in discardedLetters) {

			Vector3 targetPosition =  new Vector3(-5f, -3f, 3f);
			Vector3 targetEulerAngles = new Vector3 (0f, -90f, 80f);

			pL.gameObject.transform.localPosition = Vector3.Lerp (pL.gameObject.transform.localPosition, targetPosition, Time.deltaTime*4f);
			pL.gameObject.transform.localEulerAngles = Hacks.LerpVector3Angle(pL.gameObject.transform.localEulerAngles, targetEulerAngles, Time.deltaTime * 5f);

			if (Vector3.Distance (pL.gameObject.transform.localPosition, targetPosition) < 0.1f) {
				pL.gameObject.SetActive (false);
			}

		}

	}

	public PhysicalLetter GetCurrentTopLetter() {

		PhysicalLetter currentPL = todayStack.pLetterList [todayStack.pLetterList.Count -1];
		return currentPL;

	}

	private enum State {
		Playing,
		Off,
		On
	}


	public class LetterStack {

		public int originalNum;
		public GameObject gameobject;
		public List<PhysicalLetter> pLetterList = new List<PhysicalLetter>();
		private bool lastTimeGrabbed = false;

		public LetterStack() {

			gameobject = new GameObject();
			gameobject.name = "LetterStack";
			gameobject.layer = LayerMask.NameToLayer("Grabbable");
			gameobject.transform.SetParent(Camera.main.transform);
			gameobject.transform.localPosition = new Vector3(0.5f, -3.1f, 7.6f);
			gameobject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			gameobject.transform.SetParent(null);
			gameobject.AddComponent<Grabbable>();
			gameobject.GetComponent<Grabbable>().rotationGrabbed = new Vector3(-80f, 0f, 0f);
			gameobject.GetComponent<Grabbable>().positionGrabbed = new Vector3(0f, 0.1f, 2.6f);

		}

		public void addLetters(int num) {

			originalNum = num;

			float distance = 0f;
			int letterCounter = 0;

			for (int i = 0; i < num; i++) {
				PhysicalLetter pL = new PhysicalLetter ();
				TextAsset[] texts = Resources.LoadAll ("Messages", typeof(TextAsset)).Cast<TextAsset> ().ToArray ();
				TextAsset text = texts [letterCounter];
				pL.AssignMessage (new Message (text.name));
				pL.gameObject.transform.SetParent (gameobject.transform);
				pL.gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
				pL.gameObject.transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
				pL.gameObject.GetComponent<Grabbable> ().grabbableParent = gameobject.GetComponent<Grabbable> ();

				pL.targetLocalEulerAngles = new Vector3 (0f, Random.Range (-10f, 10f), 0f);
				pL.gameObject.transform.localEulerAngles = pL.targetLocalEulerAngles;
				pL.gameObject.transform.localPosition = new Vector3 (0f, distance, 0f);
				pLetterList.Add (pL);
				distance += 0.03f;

				letterCounter++;
			}

			AdjustPosition (1f, 1f);
			lastTimeGrabbed = false;

		}

		public void Update() {

			if (ObjectGrabber.instance.grabbedObject == gameobject && pLetterList.Count > 0) {

				PhysicalLetter currentPL = pLetterList [pLetterList.Count -1];

				foreach (TextMesh optionTM in currentPL.optionsTextMesh) {

					if (Hacks.isOver (optionTM.gameObject)) {
						
						optionTM.color = PhysicalLetter.selectedColor;

						if (Input.GetMouseButtonDown (0)) {

							if (currentPL.selectedOption != optionTM) {
								currentPL.selectedOption = optionTM;
								currentPL.tick.transform.localPosition = new Vector3 (currentPL.tick.transform.localPosition.x, currentPL.tick.transform.localPosition.y, currentPL.selectedOption.transform.localPosition.z + 0.1f);
								currentPL.tick.gameObject.SetActive (true);
							} else {
								currentPL.selectedOption = null;
								currentPL.tick.gameObject.SetActive (false);
							}

						}

					} else {
						
						optionTM.color = PhysicalLetter.unselectedColor;

					}

					if (currentPL.selectedOption == optionTM) {
						optionTM.color = new Color (0f, 0.4f, 0f);
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

			AdjustPosition (Time.deltaTime*3f, Time.deltaTime * 10f);

		}

		public void AdjustPosition(float speedAngles, float speedPosition) {

			float distance = 0f;

			foreach (PhysicalLetter pL in pLetterList) {

				pL.gameObject.transform.localEulerAngles = Hacks.LerpVector3Angle(pL.gameObject.transform.localEulerAngles, pL.targetLocalEulerAngles, speedAngles);
				pL.gameObject.transform.localPosition = Vector3.Lerp (pL.gameObject.transform.localPosition, new Vector3 (0f, distance, 0f), speedPosition);
				distance += 0.2f;

			}

		}

		public void UseLetter(PhysicalLetter pL) {

			pL.Use ();
			pLetterList.Remove (pL);
			pL.gameObject.transform.SetParent (Camera.main.transform);
			Main.discardedLetters.Add (pL);

		}

	}

}
