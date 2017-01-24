using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkersPage : MonoBehaviour {

    public GameObject fill;
    public int workerCost;

    public int money;
    int workersAvailable;
    Transform fillDefaultPos;
    List<GameObject> barFills;

	void Start () {
        barFills = new List<GameObject>();
        barFills.Add(transform.GetChild(0).GetChild(0).gameObject);
        fillDefaultPos = barFills[0].transform;

        GameObject barFill = Instantiate(fill, transform.GetChild(0)) as GameObject;
        barFill.transform.position = fillDefaultPos.position;
        barFill.transform.rotation = fillDefaultPos.rotation;
        barFill.transform.localScale = fillDefaultPos.localScale;
        barFills.Add(barFill);

        workersAvailable = WorkersBoard.Instance.initialWorkers;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
            money += 10;
        if (Input.GetKeyDown(KeyCode.F2))
            money -= 10;

        //UPDATE NOTEBOOK BAR FILL;
        barFills[workersAvailable].transform.localPosition = new Vector3(fillDefaultPos.localPosition.x,
                                                                        barFills[workersAvailable-1].transform.localPosition.y + (money - ((workersAvailable - 1) * workerCost)) * 0.001f,
                                                                        fillDefaultPos.localPosition.z);

        workersAvailable = Mathf.FloorToInt(money / workerCost) + WorkersBoard.Instance.initialWorkers;
        Debug.Log(workersAvailable);

        if(WorkersBoard.Instance.GetWorkersCount() < workersAvailable) {
            WorkersBoard.Instance.AddWorker();

            barFills[workersAvailable-1].transform.localPosition = new Vector3(fillDefaultPos.localPosition.x,
                                                                                barFills[workersAvailable - 2].transform.localPosition.y + ((workerCost * (workersAvailable)) - ((workersAvailable - 1) * workerCost)) * 0.001f,
                                                                                fillDefaultPos.localPosition.z);

            GameObject barFill = Instantiate(fill, transform.GetChild(0)) as GameObject;
            barFill.transform.position = fillDefaultPos.position;
            barFill.transform.rotation = fillDefaultPos.rotation;
            barFill.transform.localScale = fillDefaultPos.localScale;
            barFills.Add(barFill);
        }

        else if(WorkersBoard.Instance.GetWorkersCount() > workersAvailable) {
            WorkersBoard.Instance.RemoveWorker();
            Destroy(barFills[barFills.Count - 1]);
            barFills.RemoveAt(barFills.Count - 1);
        }
	}

    public void UpdatePage() {
        
    }
}
