using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Outliner : MonoBehaviour {

	[SerializeField] List<MeshRenderer> objectsToOutline = new List<MeshRenderer>();
	List<Shader> defaultShaders = new List<Shader> ();
	Shader defaultShader;
	Shader outlineShader;

	public Color outlineColor;
	public float outlineWidth;
	public bool isOutlined = false;

	void Start () {
		foreach (MeshRenderer mR in objectsToOutline) {
			defaultShaders.Add (mR.material.shader);
		}

		outlineShader = Shader.Find ("Outlined_Diffuse");
	}
	
	void Update () {
	
		if (Hacks.isOver (this.gameObject)) {
			Outline (true);
		} else {
			Outline (false);
		}

	}

	public void Outline(bool b) {
		
		for (int i = 0; i < objectsToOutline.Count; i++) {
			if (b) {
				if (objectsToOutline [i].material.shader != outlineShader) {
					objectsToOutline [i].material.shader = outlineShader;
					objectsToOutline [i].material.SetColor ("_OutlineColor", outlineColor);
					objectsToOutline [i].material.SetFloat ("_Outline", outlineWidth);
				}
			}
			else if (objectsToOutline [i].material.shader != defaultShaders [i])
				objectsToOutline [i].material.shader = defaultShaders [i];
		}

		isOutlined = b;

	}
}
