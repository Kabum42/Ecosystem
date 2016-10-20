using UnityEngine;
using System.Collections;

public class Outliner : MonoBehaviour {

	MeshRenderer mRenderer;
	Shader defaultShader;
	Shader outlineShader;

	// Use this for initialization
	void Start () {
	
		mRenderer = GetComponent<MeshRenderer> ();
		defaultShader = mRenderer.material.shader;
		outlineShader = Shader.Find ("Outlined_Diffuse");

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Hacks.isOver (this.gameObject)) {
			Outline (true);
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
