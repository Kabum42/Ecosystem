using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestChangeReproduction : MonoBehaviour {

	public float simulationRate = 1f;
	private float simulationAux = 0f;
	public Text textSource;
	public List<Text> texts = new List<Text> ();
	public List<float> reproductionRates = new List<float>();

	// Use this for initialization
	void Start () {
	
		Ecosystem.Start ();

		int i = 0;

		foreach (SpeciesData sp in Ecosystem.speciesDataList) {
			Text t = Instantiate (textSource);
			t.transform.SetParent (textSource.transform.parent);
			t.transform.localScale = textSource.transform.localScale;
			t.rectTransform.anchoredPosition = new Vector2 (0f, -30f -30f*i);
			t.gameObject.SetActive (true);
			texts.Add (t);
			reproductionRates.Add (sp.reproductionRate);
			i++;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		simulationAux += Time.deltaTime;
		if (simulationAux > simulationRate) {
			simulationAux -= simulationRate;
			Ecosystem.Simulate ();
		}

		for (int i = 0; i < Ecosystem.speciesDataList.Count; i++) {
			Ecosystem.speciesDataList [i].reproductionRate = reproductionRates [i];
			texts [i].text = Ecosystem.speciesDataList [i].species.ToString () + ": " + Mathf.Floor(Ecosystem.speciesDataList [i].population);
		}

	}
}
