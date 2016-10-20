using UnityEngine;
using System.Collections;

public class CursorMovement : MonoBehaviour {

	private float posY = 0;

	[Range(0f, 180f)]
	public float limitRotation = 30f;

	public float speedRotation = 50f;

	[Range(0f, 0.5f)]
	public float lateralThreshold = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//if (posCooldown <= 0f) {
			handleChangePos ();
		//} else {
		//	posCooldown -= Time.deltaTime;
		//}


		handleRotation ();
	
	}

	void handleChangePos() {

		if (posY > -limitRotation && Input.mousePosition.x < Screen.width * lateralThreshold) {
			posY -= Time.deltaTime * speedRotation;
		} else if (posY < limitRotation && Input.mousePosition.x > Screen.width - Screen.width * lateralThreshold) {
			posY += Time.deltaTime * speedRotation;
		}

	}

	void handleRotation() {

		Vector3 targetRotation = new Vector3 (this.transform.eulerAngles.x, posY, 0f);

		Camera.main.transform.eulerAngles = Hacks.LerpVector3Angle(Camera.main.transform.eulerAngles, targetRotation, Time.deltaTime * 10f);

	}

}
