using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkersPage : MonoBehaviour {

    [SerializeField] Sprite availableWorkerSprite;
    [SerializeField] Sprite unavailableWorkerSprite;
    [SerializeField] Sprite workingWorkerSprite;
    int workersAvailable;
    Transform fillDefaultPos;
    List<GameObject> barFills;
    List<SpriteRenderer> workersSprites;

    public GameObject fill;
    public int workerCost;
    public int money;

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

        workersSprites = new List<SpriteRenderer>();
        for (int i = 0; i < transform.GetChild(1).childCount; i++) {
            workersSprites.Add(transform.GetChild(1).GetChild(i).GetComponent<SpriteRenderer>());
            workersSprites[i].sprite = unavailableWorkerSprite;
        }

        for (int i = 0; i < workersAvailable; i++)
        {
            workersSprites[i].sprite = availableWorkerSprite;
        }
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

            workersSprites[workersAvailable - 1].sprite = availableWorkerSprite;
        }

        else if(WorkersBoard.Instance.GetWorkersCount() > workersAvailable) {
            WorkersBoard.Instance.RemoveWorker();
            Destroy(barFills[barFills.Count - 1]);
            barFills.RemoveAt(barFills.Count - 1);

            workersSprites[workersAvailable].sprite = unavailableWorkerSprite;
        }
    }
}
