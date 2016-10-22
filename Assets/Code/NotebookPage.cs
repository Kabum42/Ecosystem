using UnityEngine;
using System.Collections;

public class NotebookPage : MonoBehaviour {

	[SerializeField] GameObject lineChart;

	void Start () {
	
	}
	
	void Update () {
		lineChart.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z - 0.01f);
		lineChart.transform.rotation = transform.parent.rotation;
	}
}
