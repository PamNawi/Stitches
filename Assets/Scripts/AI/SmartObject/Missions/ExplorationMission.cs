using UnityEngine;


public class ExplorationMission : BasicMission {
    public float minDistance = 0.5f;

    public new void Start()
    {
        soName = "Exploration Mission";
        base.Start();
    }
}
