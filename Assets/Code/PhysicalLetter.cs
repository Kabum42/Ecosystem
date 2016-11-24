using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalLetter {

	public GameObject gameObject;
	public Vector3 targetLocalEulerAngles = Vector3.zero;
	public static Color selectedColor = new Color(0.1f, 0.1f, 0.1f);
	public static Color unselectedColor = new Color (0.4f, 0.4f, 0.4f);

	private TextMesh senderTextMesh;
	private TextMesh informationTextMesh;
	private TextMesh optionSourceTextMesh;
	public SpriteRenderer tick;

	public List<TextMesh> optionsTextMesh = new List<TextMesh> ();
	public TextMesh selectedOption = null;

	private Message message;

	public PhysicalLetter() {

		gameObject = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Letter") as GameObject);
		senderTextMesh = gameObject.transform.FindChild ("Sender").GetComponent<TextMesh> ();
		informationTextMesh = gameObject.transform.FindChild ("Information").GetComponent<TextMesh> ();
		optionSourceTextMesh = gameObject.transform.FindChild ("OptionSource").GetComponent<TextMesh> ();
		optionSourceTextMesh.gameObject.SetActive (false);
		tick = gameObject.transform.FindChild ("Tick").GetComponent<SpriteRenderer> ();
		tick.color = selectedColor;
		tick.gameObject.SetActive (false);

	}

	public void AssignMessage(Message m) {

		message = m;
		senderTextMesh.text = m.sender;
		informationTextMesh.text = m.information;

		float distanceBetweenOptions = 0.7f;
		float maxOffset = (m.options.Count -1) * distanceBetweenOptions;

		for (int i = 0; i < m.options.Count; i++) {

			GameObject physicalOption = MonoBehaviour.Instantiate (optionSourceTextMesh.gameObject);
			physicalOption.transform.SetParent (optionSourceTextMesh.transform.parent);
			physicalOption.transform.localScale = optionSourceTextMesh.transform.localScale;
			physicalOption.transform.localPosition = optionSourceTextMesh.transform.localPosition + new Vector3 (0f, 0f, maxOffset - (float)i * distanceBetweenOptions); ;
			physicalOption.GetComponent<TextMesh> ().text = m.options [i].text;
			optionsTextMesh.Add (physicalOption.GetComponent<TextMesh> ());
			physicalOption.SetActive (true);

		}

	}

	public void Use() {

		int posOption = optionsTextMesh.IndexOf(selectedOption);

		foreach (Consequence consequence in message.options[posOption].consequences) {

			Ecosystem.GetSpeciesData (consequence.species).population += consequence.change;

		}

	}

}
