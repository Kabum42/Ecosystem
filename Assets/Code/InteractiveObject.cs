using UnityEngine;
using System.Collections;

public class InteractiveObject: MonoBehaviour {

	MeshRenderer mRenderer;
	Shader defaultShader;

	void Start () {
		mRenderer = GetComponent<MeshRenderer> ();
		defaultShader = mRenderer.material.shader;
	}
	
	void Update () {
	
	}

	public void Outline(bool b) {
		if (b)
			mRenderer.material.shader = Shader.Find ("Outlined_Diffuse");
		else
			mRenderer.material.shader = defaultShader;
	}
}
