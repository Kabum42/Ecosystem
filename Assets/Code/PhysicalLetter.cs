using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicalLetter {

	public GameObject gameObject;
	public Vector3 targetLocalEulerAngles = Vector3.zero;
	public static Color selectedColor = new Color(0f, 0f, 0f);
	public static Color unselectedColor = new Color (0.2f, 0.2f, 0.2f);

	private TextMesh senderTextMesh;
	private TextMesh informationTextMesh;
	private TextMesh optionSourceTextMesh;
	public SpriteRenderer tick;
	private MeshRenderer stamp;

	public List<TextMesh> optionsTextMesh = new List<TextMesh> ();
	public TextMesh selectedOption = null;

	private Message message;

	public PhysicalLetter() {

		gameObject = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Letter") as GameObject);
		informationTextMesh = gameObject.transform.FindChild ("Information").GetComponent<TextMesh> ();
		optionSourceTextMesh = gameObject.transform.FindChild ("OptionSource").GetComponent<TextMesh> ();
		optionSourceTextMesh.gameObject.SetActive (false);
		tick = gameObject.transform.FindChild ("Tick").GetComponent<SpriteRenderer> ();
		tick.color = selectedColor;
		tick.gameObject.SetActive (false);
		stamp = gameObject.transform.FindChild ("Stamp").GetComponent<MeshRenderer> ();

	}

	public void AssignMessage(Message m) {

		message = m;
		informationTextMesh.text = Hacks.TextMultiline (informationTextMesh.gameObject, m.information, 22f);

		float distanceBetweenOptions = 0.7f;
		float maxOffset = (m.options.Count -1) * distanceBetweenOptions;

		for (int i = 0; i < m.options.Count; i++) {

			GameObject physicalOption = MonoBehaviour.Instantiate (optionSourceTextMesh.gameObject);
			physicalOption.transform.SetParent (optionSourceTextMesh.transform.parent);
			physicalOption.transform.localScale = optionSourceTextMesh.transform.localScale;
			physicalOption.transform.localPosition = optionSourceTextMesh.transform.localPosition + new Vector3 (0f, 0f, maxOffset - (float)i * distanceBetweenOptions); ;
			physicalOption.GetComponent<TextMesh> ().text = Hacks.TextMultiline (physicalOption, m.options [i].text, 22f);
			optionsTextMesh.Add (physicalOption.GetComponent<TextMesh> ());
			physicalOption.SetActive (true);

		}
			
		stamp.material.mainTexture = Resources.Load<Texture> ("2D/" + m.sender);

	}

	public void Use() {

		int posOption = optionsTextMesh.IndexOf(selectedOption);

		foreach (Consequence consequence in message.options[posOption].consequences) {

			if (consequence.action == "Pob") {
				Ecosystem.GetSpeciesData (consequence.species).population += consequence.change * (float)Ecosystem.GetSpeciesData (consequence.species).populationCap;
			} else if (consequence.action == "Crec") {
				Ecosystem.GetSpeciesData (consequence.species).reproductionRate += consequence.change;
			} else if (consequence.action == Message.Faction.Gobierno.ToString()) {
				Ecosystem.friendshipGobierno += consequence.change;
			} else if (consequence.action == Message.Faction.Cooperativa.ToString()) {
				Ecosystem.friendshipCooperativa += consequence.change;
			} else if (consequence.action == Message.Faction.Ecologistas.ToString()) {
				Ecosystem.friendshipEcologistas += consequence.change;
			} else if (consequence.action == "Trabajador") {

			} else if (consequence.action == "Desbloquea") {

			}

		}

	}

}
