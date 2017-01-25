using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalInstantiator : MonoBehaviour {

    [SerializeField] Species specie;
	List<GameObject> animalsInScene = new List<GameObject> ();
//	float zoneMinX = 100f;
//	float zoneMaxX = -100f;
//	float zoneMinZ = 100f;
//	float zoneMaxZ = -100f;
	List<Vector3> circlePoints = new List<Vector3>();
	GameObject parent;
    LineRenderer line;
    GameObject circle;

	public float animalNum;
    public int segments;
	public float initialY;
	public Transform zoneCenter;
	public float zoneRadius;
	public GameObject animalPrefab;
	public int notebookPage;
	public Material circleMat;

	public float offsetz;
	public float offsetx;
	public float offsety;

	void Start () {
        //		GET ANIMAL POPULATION
        animalNum = Mathf.FloorToInt(Ecosystem.GetSpeciesData(specie).GetPercentage() * Ecosystem.GetSpeciesData(specie).maxVisual);

        parent = new GameObject (animalPrefab.name + "Zone");

		InstantiateAnimals ();

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
						hit.point.y + offsety /*animal.GetComponent<BoxCollider> ().bounds.extents.y*/, animal.transform.position.z);
				}
			}

			animal.transform.RotateAround (animal.transform.position, Vector3.up, Random.Range (0, 360));
			animal.transform.SetParent (parent.transform);
		}
	}

	void DrawCircle() {
		circle = new GameObject ("Circle " + specie);
		circle.transform.position = zoneCenter.position;
		line = circle.gameObject.AddComponent<LineRenderer> ();
		line.SetVertexCount (segments + 1);
		line.material = circleMat;
		line.SetWidth (3f, 3f);

		float x;
		float y = 1000f;
		float z;
		float ycount = 0f;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * (zoneRadius + 2f);
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * (zoneRadius + 2f);

			circlePoints.Add (new Vector3 (x, y, z));

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
			
		circle.transform.position = new Vector3 (circle.transform.position.x, 0f, circle.transform.position.z);

		SphereCollider sc = circle.AddComponent<SphereCollider> () as SphereCollider;
		sc.isTrigger = true;
		ZoneCollider zc = circle.AddComponent<ZoneCollider> () as ZoneCollider;
		zc.SetPage (notebookPage);
	}

    void RemoveAnimals() {
        animalsInScene.Clear();
        Destroy(GameObject.Find("Circle " + specie));
    }

    public void UpdateAnimals() {
        animalNum = Ecosystem.GetSpeciesData(specie).GetPercentage() * Ecosystem.GetSpeciesData(specie).maxVisual;

        RemoveAnimals();

        InstantiateAnimals();

        DrawCircle();
    }
}
