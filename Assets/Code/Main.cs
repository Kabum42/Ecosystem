using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		PhysicalLetter pL = new PhysicalLetter ();
		pL.AssignMessage (new Message ("hehe"));

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
