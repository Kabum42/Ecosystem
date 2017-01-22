using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineChartCreator : MonoBehaviour {

	[SerializeField] Sprite dotSprite;
	[SerializeField] Sprite lineSprite;
	float stepX;
	float stepY;

	public float minY = -2.4f;
	public float maxY = 2.5f;
	public float minX = -3.5f;
	public float maxX = 2.5f;
	public List<float> values = new List<float>();

	void Start () {
		stepX = (maxX - minX) / 12;
		stepY = (maxY - minY);

		RandomValues ();
	}
	
	void Update () {
		if (Input.GetKey (KeyCode.F1)) {
			RandomValues ();
		}
	}

	public void UpdateChart() {
		DestroyValues ();

		//THIS LINE FIXED DRAWING WHEN NOTEBOOK IS NOT GRABBED
		if(!Notebook.Instance.grabbed)
			Notebook.Instance.transform.eulerAngles = new Vector3(10f, 0f, 0f);

		for (int i = 0; i < values.Count; i++) {
			string name = "value" + i;
			GameObject dot = new GameObject (name);
			dot.transform.SetParent (this.transform);
			dot.transform.localScale = new Vector3(1,1,1);
			dot.AddComponent<SpriteRenderer> ();
			dot.GetComponent<SpriteRenderer> ().sprite = dotSprite;
			dot.transform.localPosition = new Vector3 (minX + ((i) * stepX), ((values [i]/100) * stepY) - maxY, 0);
			dot.transform.localEulerAngles = Vector3.zero;

			if (i < values.Count - 1) {
				string lineName = "line" + i;
				GameObject line = new GameObject (lineName);
				line.transform.SetParent (this.transform);
				line.AddComponent<SpriteRenderer> ();
				line.GetComponent<SpriteRenderer> ().sprite = lineSprite;
				line.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.5f);
				line.GetComponent<SpriteRenderer> ().sortingOrder = -1;
				CreateLine (line, new Vector3 (minX + ((i) * stepX), ((values [i] / 100) * stepY) - maxY, 0), new Vector3 (minX + ((i + 1) * stepX), ((values [i + 1] / 100) * stepY) - maxY, 0));
			}
		}
			
		if(!Notebook.Instance.grabbed)
			Notebook.Instance.transform.eulerAngles = new Vector3(90f, 0f, -90f);
	}

	void CreateLine(GameObject line, Vector3 initialPos, Vector3 finalPos) {
		Vector3 centerPos = (initialPos + finalPos) / 2f;
		line.transform.localPosition = centerPos;
		Vector3 direction = finalPos - initialPos;
		direction = Vector3.Normalize (direction);
		line.transform.right = direction;

		Vector3 scale = new Vector3 (1, 1, 1);
		scale.x = Vector3.Distance (initialPos, finalPos);
		line.transform.localScale = new Vector3 (scale.x * 7f, 0.5f, 1);
		line.transform.localEulerAngles = new Vector3 (0f, 0f, line.transform.localEulerAngles.z);
	}

	void RandomValues() {
		for (int i = 0; i < values.Count; i++) {
			values [i] = Random.Range (0, 100);
		}

		UpdateChart ();
	}

	void DestroyValues() {
		foreach (Transform child in this.transform) {
			Destroy (child.gameObject);
		}
	}
}
