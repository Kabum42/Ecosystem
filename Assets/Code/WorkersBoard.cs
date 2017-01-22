using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkersBoard : MonoBehaviour {

    List<Worker> workersAvailable;
    List<Worker> workersWorking;
    GameObject[] keys;
    GameObject[] photos;

    public int totalWorkers;
    public int initialWorkers;

    public static WorkersBoard Instance;

    void Awake() {
        Instance = this;
    }

	void Start () {
        workersAvailable = new List<Worker>();
        workersWorking = new List<Worker>();
        keys = new GameObject[totalWorkers];
        photos = new GameObject[totalWorkers];

        for(int i = 0; i < totalWorkers; i++) {
            keys[i] = transform.GetChild(0).GetChild(i).gameObject;
            photos[i] = transform.GetChild(1).GetChild(i).gameObject;
        }

        int j = initialWorkers;
        for(int k = 0; k < photos.Length; k++) {
            bool t = (j > 0) ? true : false;
            photos[k].SetActive(t);
            j--;

            if (t) {
                workersAvailable.Add(new Worker());
            }
        }

        UpdateBoard();
    }
	
	void Update () {
	    
	}

    public bool UseWorker(int numWorkers, int numDays) {
        if (workersAvailable.Count >= numWorkers)
        {
            for(int i = 0; i < numWorkers; i++) {
                Worker worker = workersAvailable[workersAvailable.Count-1];
                workersAvailable.Remove(worker);
                worker.GoToWork(numDays);
                workersWorking.Add(worker);
            }

            return true;
        }

        else
            return false;
    }

    public void UpdateBoard() {
        foreach(Worker worker in workersWorking) {
            worker.DayPassed();

            if (worker.isAvailable()) {
                workersWorking.Remove(worker);
                workersAvailable.Add(worker);
            }
        }

        int i = workersAvailable.Count;
        foreach(GameObject key in keys) {
            bool t = (i > 0) ? true : false;
            key.SetActive(t);
            i--;
        }
    }
}
