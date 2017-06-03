using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControllerLogger : Logger {
    public override void startLogging(string path)
    {
        base.startLogging(path);
        StartCoroutine(UpdateLog());
    }

    public override void startLogging(string path, string timestamp)
    {
        base.startLogging(path, timestamp);
        StartCoroutine(UpdateLog());
    }

    IEnumerator UpdateLog()
    {
        while (true)
        {
            GameObject[] allBuildings = GameObject.FindGameObjectsWithTag("Building");
            PlaceableBuilding pBuilding;
            //Write this block on the file
            file.WriteLine("#####################");
            //Timestamp
            file.WriteLine(System.DateTime.Now.ToString("HHmmssffff"));
            //FPS
            file.WriteLine(1.0f / Time.deltaTime);
            //Foreach hero on the list:
            //HeroName, Class
            foreach (GameObject building in allBuildings) {
                pBuilding = building.GetComponent<PlaceableBuilding>();
                file.WriteLine(pBuilding.name);
            }
            yield return new WaitForSeconds(timeNextLog);
        }
    }
}
