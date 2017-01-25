using UnityEngine;
using System.Collections;

public class LampBehaviour : MonoBehaviour {

    [SerializeField] Light lampLight;
    [SerializeField] MeshRenderer bulb;

	void Start () {
        lampLight.enabled = false;
        bulb.material.SetColor("_EmissionColor", Color.black);
    }

    void Update () {
        if (Hacks.isOver(this.gameObject))
            if (Input.GetMouseButtonDown(0))
            {
                Color newColor = (lampLight.enabled) ? Color.black : Color.white;
                lampLight.enabled = (lampLight.enabled) ? false : true;
                bulb.material.SetColor("_EmissionColor", newColor);
            }
	}
}
