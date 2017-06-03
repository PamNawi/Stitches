using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    EconomyController economyController;
    MapController mapController;
    //MissionController missionController;
    HeroController heroController;

    public CanvasController canvasController;
    NewsManager newsManager;

    public uint round;
    public uint timeNextRound = 1;

    public bool log;
    public string logsFilePath = "C:/Users/PamNawi/Desktop/LogStitches/";

    string timestamp;

    // Use this for initialization
    void Start()
    {
        economyController = GetComponentInChildren<EconomyController>();
        mapController = GetComponentInChildren<MapController>();
        //missionController = GetComponentInChildren<MissionController>();
        heroController = GetComponentInChildren<HeroController>();

        //smartObjectsController = SmartObjectManager.Load("SmartObjects");
        newsManager = canvasController.GetComponentInChildren<NewsManager>();

        round = 0;
        StartCoroutine(Round());

        if (log)
        {
            timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");

            HeroControllerLogger hcLogger = GetComponentInChildren<HeroControllerLogger>();
            hcLogger.startLogging(logsFilePath, timestamp);

            EconomyControllerLogger eLogger = GetComponentInChildren<EconomyControllerLogger>();
            eLogger.startLogging(logsFilePath, timestamp, economyController);

            MapControllerLogger mLogger = GetComponentInChildren<MapControllerLogger>();
            mLogger.startLogging(logsFilePath, timestamp);
        }
    }

    void generateNews()
    {
        List<string> reports;

        reports = economyController.getReports();
        foreach(string report in reports)
        {
            newsManager.addReport(report);
        }

        //reports = missionController.getReports();
        foreach (string report in reports)
        {
            newsManager.addReport(report);
        }
    }

    IEnumerator Round()
    {
        while (true)
        {
            round++;

            economyController.nextRound(round);
            mapController.nextRound(round);
            //missionController.nextRound(round);
            heroController.nextRound(round);

            generateNews();
            yield return new WaitForSeconds(timeNextRound);
        }
    }

}
