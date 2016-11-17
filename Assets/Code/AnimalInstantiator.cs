﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalInstantiator : MonoBehaviour {

	int animalNum = 10;
	List<GameObject> animalsInScene = new List<GameObject> ();
	float zoneMinX = 100f;
	float zoneMaxX = -100f;
	float zoneMinZ = 100f;
	float zoneMaxZ = -100f;
	List<Vector3> circlePoints = new List<Vector3>();

	public int segments;
	LineRenderer line;
	GameObject circle;
	public float initialY;
	public Transform zoneCenter;
	public float zoneRadius;
	public GameObject animalPrefab;

	public float offsetz;
	public float offsetx;

	void Start () {
		//Ecosystem.Start ();

		InstantiateAnimals ();

		//GET ANIMAL POPULATION
		//Ecosystem.speciesDataList [4].population;

		circle = new GameObject ("Circle");
		circle.transform.position = zoneCenter.position;
		line = circle.gameObject.AddComponent<LineRenderer> ();
		line.SetVertexCount (segments + 1);
		line.material.color = Color.white;
		line.SetWidth (.5f, .5f);
		DrawCircle ();
	}
	
	void Update () {

	}

	void InstantiateAnimals() {
		float minX = zoneCenter.position.x - zoneRadius + 0.5f;
		float maxX = zoneCenter.position.x + zoneRadius - 0.5f;
		float minZ = zoneCenter.position.z - zoneRadius + 0.5f;
		float maxZ = zoneCenter.position.z + zoneRadius - 0.5f;

		for (int i = 0; i < animalNum; i++) {
			GameObject animal = Instantiate (animalPrefab, zoneCenter.position, Quaternion.identity) as GameObject;
			float x = Random.Range (minX, maxX);
			float z = Random.Range (minZ, maxZ);
			animal.transform.position = new Vector3 (x, initialY, z);
			animalsInScene.Add (animal);

			Ray ray = new Ray (animal.transform.position, Vector3.down);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.transform.tag == "terrain") {
					animal.transform.position = new Vector3 (animal.transform.position.x, 
						hit.point.y + animal.GetComponent<BoxCollider> ().bounds.extents.y, animal.transform.position.z);
				}
			}
		}
	}

	void DrawCircle() {
		float x;
		float y = 50f;
		float z;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * (zoneRadius + 2f);
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * (zoneRadius + 2f);

			circlePoints.Add (new Vector3 (x, y, z));
			Debug.Log (circlePoints [i]);

			line.SetPosition (i, circlePoints[i]);

			angle += (360f / segments);

			Vector3 pos = new Vector3 (circlePoints [i].x + circle.transform.position.x, circlePoints [i].y, circlePoints [i].z + circle.transform.position.z);
			RaycastHit hit;

			line.SetPosition (i, new Vector3 (pos.x, pos.y, pos.z));

			if (Physics.Raycast (pos, Vector3.down, out hit, Mathf.Infinity)) {
				if (hit.transform.tag == "terrain") {
					line.SetPosition (i, new Vector3 (circlePoints [i].x, hit.point.y, circlePoints [i].z));
				}
			}

			line.useWorldSpace = false;
		}
	}
}