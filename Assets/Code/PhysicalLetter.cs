using UnityEngine;
using System.Collections;

public class PhysicalLetter {

	public GameObject gameObject;
	public Vector3 targetLocalEulerAngles = Vector3.zero;
	private TextMesh senderTextMesh;
	private TextMesh informationTextMesh;

	private Message message;

	public PhysicalLetter() {

		gameObject = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Letter") as GameObject);
		senderTextMesh = gameObject.transform.FindChild ("Sender").GetComponent<TextMesh> ();
		informationTextMesh = gameObject.transform.FindChild ("Information").GetComponent<TextMesh> ();

	}

	public void AssignMessage(Message m) {

		message = m;
		senderTextMesh.text = m.sender;
		informationTextMesh.text = m.information;

	}

}
