using UnityEngine;
using System.Collections;

public class InteractiveObject: MonoBehaviour {

	MeshRenderer mRenderer;
	Shader defaultShader;
	Shader outlineShader;

	void Start () {
		mRenderer = GetComponent<MeshRenderer> ();
		defaultShader = mRenderer.material.shader;
		outlineShader = Shader.Find ("Outlined_Diffuse");
	}
	
	void Update () {
		
		if (Hacks.isOver (gameObject)) {
			Outline (true);
		} else {
			Outline (false);
		}

	}

	public void Outline(bool b) {
		if (b)
			mRenderer.material.shader = outlineShader;
		else
			mRenderer.material.shader = defaultShader;
	}
}
