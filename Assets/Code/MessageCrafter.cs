using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MessageCrafter : MonoBehaviour {

	public RectTransform parentUI;
	public GameObject optionButton;
	public GameObject optionTemplate;

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
			}
		}

		if (optionToDelete != null) {
			Destroy (optionToDelete.rTransform.gameObject);
			options.Remove (optionToDelete);
		}

	}


	void addOption () {

		OptionCraft o = new OptionCraft (ref optionTemplate);
		options.Add (o);

	}

	void repositionUI() {

		float yPos = -94f;

		foreach (OptionCraft o in options) {
			o.rTransform.anchoredPosition = new Vector2 (o.rTransform.anchoredPosition.x, yPos);
			yPos -= 50f;
		}

		optionButton.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (optionButton.GetComponent<RectTransform> ().anchoredPosition.x, yPos);

	}


}

public class OptionCraft {

	public RectTransform rTransform;
	public GameObject deleteButton;
	public List<GameObject> consequences = new List<GameObject>();

	public OptionCraft(ref GameObject optionTemplate) {

		rTransform = MonoBehaviour.Instantiate (optionTemplate).GetComponent<RectTransform>();
		rTransform.gameObject.SetActive (true);
		rTransform.transform.SetParent (optionTemplate.transform.parent);
		rTransform.transform.localScale = optionTemplate.transform.localScale;
		rTransform.transform.position = optionTemplate.transform.position;

		deleteButton = rTransform.transform.FindChild ("DeleteButton").gameObject;

	}

}
