using UnityEngine;
using System.Collections;

public class InteractiveObject: MonoBehaviour {

	MeshRenderer mRenderer;
	Shader defaultShader;
	Shader outlineShader;
	ObjectGrabber objectGrabber;

	public Vector3 rotationGrabbed;
	public Vector3 positionGrabbed;

	void Start () {
		mRenderer = GetComponent<MeshRenderer> ();
		defaultShader = mRenderer.material.shader;
		outlineShader = Shader.Find ("Outlined_Diffuse");
		objectGrabber = Camera.main.gameObject.GetComponent<ObjectGrabber> ();
	}
	
	void Update () {
		
		if (Hacks.isOver (this.gameObject)) {
			Outline (true);
			if (Input.GetMouseButtonDown (0)) {
				objectGrabber.Grab (this.gameObject);
			}
		} else {
			Outline (false);
		}

	}

	public void Outline(bool b) {
		
		if (b) {
			if (mRenderer.material.shader != outlineShader) {
				mRenderer.material.shader = outlineShader;
			}
		} else {
			if (mRenderer.material.shader != defaultShader) {
				mRenderer.material.shader = defaultShader;
			}
		}

	}
}
