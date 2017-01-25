using UnityEngine;
using System.Collections;

public class Worker {
    
    int daysLeft;

    public Worker() {
        daysLeft = 0;
    }

    public void GoToWork(int days) {
        daysLeft = days;
    }

    public void DayPassed() {
        daysLeft -= 1;
    }

    public bool isAvailable() {
        return (daysLeft == 0);
    }
}
