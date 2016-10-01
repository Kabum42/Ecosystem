using UnityEngine;
using System.Collections;

public class CursorMovement : MonoBehaviour {

	private int posX = 0;
	private int posY = 0;
	private float posCooldown = 0f;

	[Range(0f, 0.5f)]
	public float lateralThreshold = 0.25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (posCooldown <= 0f) {
			handleChangePos ();
		} else {
			posCooldown -= Time.deltaTime;
		}


		handleRotation ();
	
	}

	void handleChangePos() {

		if (posX > -1 && Input.mousePosition.x < Screen.width * lateralThreshold) {
			posX--;
			posCooldown = 1f;
		} else if (posX < 1 && Input.mousePosition.x > Screen.width - Screen.width * lateralThreshold) {
			posX++;
			posCooldown = 1f;
		}

	}

	void handleRotation() {

		Vector3 targetRotation = new Vector3 (0f, posX * 30f, 0f);

		Camera.main.transform.eulerAngles = Hacks.LerpVector3Angle(Camera.main.transform.eulerAngles, targetRotation, Time.deltaTime * 2f);

	}

}
