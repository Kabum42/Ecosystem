﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MessageCrafter : MonoBehaviour {

	public RectTransform parentUI;
	public GameObject optionButton;
	public GameObject optionTemplate;
	public GameObject consequenceTemplate;

	private List<OptionCraft> options = new List<OptionCraft> ();

	private static float speed = 200f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			handleCLick ();
		}

		handleNavigation ();
		repositionUI ();
	
	}

	void handleNavigation() {

		if (Input.GetKey (KeyCode.UpArrow) && parentUI.anchoredPosition3D.y > 0f) {
			parentUI.position = parentUI.position + new Vector3 (0f, -Time.deltaTime * speed, 0f);
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			parentUI.position = parentUI.position + new Vector3 (0f, Time.deltaTime * speed, 0f);
		}
			
	}

	void handleCLick() {

		GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

		if (clickedObject == optionButton) {
			addOption ();
		}

		OptionCraft optionToDelete = null;

		foreach (OptionCraft o in options) {
			if (clickedObject == o.deleteButton) {
				optionToDelete = o;
				break;
			} else if (clickedObject == o.consequenceButton) {
				o.addConsequence ();
				break;
			} else {
				if (o.checkConsequenceDelete (clickedObject)) {
					break;
				}
			}
		}

		if (optionToDelete != null) {
			optionToDelete.Destroy ();
			options.Remove (optionToDelete);
		}

	}


	void addOption () {

		OptionCraft o = new OptionCraft (this);
		options.Add (o);

	}

	void repositionUI() {

		float yPos = -94f;

		foreach (OptionCraft o in options) {
			o.option.GetComponent<RectTransform>().anchoredPosition = new Vector2 (o.option.GetComponent<RectTransform>().anchoredPosition.x, yPos);
			yPos -= 40f;
			foreach (GameObject consequence in o.consequences) {
				consequence.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (consequence.GetComponent<RectTransform> ().anchoredPosition.x, yPos);
				yPos -= 40f;
			}
			o.consequenceButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (o.consequenceButton.GetComponent<RectTransform> ().anchoredPosition.x, yPos);
			yPos -= 40f;
		}

		optionButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (optionButton.GetComponent<RectTransform> ().anchoredPosition.x, yPos);

	}


}

public class OptionCraft {

	private MessageCrafter mC;
	public GameObject option;
	public GameObject deleteButton;
	public GameObject consequenceButton;

	public List<GameObject> consequences = new List<GameObject>();

	public OptionCraft(MessageCrafter auxMC) {

		mC = auxMC;

		option = MonoBehaviour.Instantiate (mC.optionTemplate);
		option.SetActive (true);
		option.transform.SetParent (mC.optionTemplate.transform.parent);
		option.transform.localScale = mC.optionTemplate.transform.localScale;
		option.transform.position = mC.optionTemplate.transform.position;

		deleteButton = option.transform.FindChild ("DeleteButton").gameObject;

		consequenceButton = option.transform.FindChild ("ConsequenceButton").gameObject;
		consequenceButton.transform.SetParent (option.transform.parent);

	}

	public void addConsequence() {

		GameObject consequence = MonoBehaviour.Instantiate (mC.consequenceTemplate);
		consequence.SetActive (true);
		consequence.transform.SetParent (option.transform.parent);
		consequence.transform.localScale = Vector3.one;
		consequence.transform.position = mC.consequenceTemplate.transform.position;

		consequences.Add (consequence);

	}

	public bool checkConsequenceDelete(GameObject clickedObject) {

		GameObject consequenceToDelete = null;

		foreach (GameObject consequence in consequences) {
			if (clickedObject == consequence.transform.FindChild ("DeleteButton").gameObject) {
				consequenceToDelete = consequence;
				break;
			}
		}

		if (consequenceToDelete != null) {
			consequences.Remove (consequenceToDelete);
			MonoBehaviour.Destroy (consequenceToDelete);
			return true;
		} else {
			return false;
		}

	}

	public void Destroy() {

		MonoBehaviour.Destroy (option);
		MonoBehaviour.Destroy (consequenceButton);

		foreach (GameObject consequence in consequences) {
			MonoBehaviour.Destroy (consequence);
		}

	}

}