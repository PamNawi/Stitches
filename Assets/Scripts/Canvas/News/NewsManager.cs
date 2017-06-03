using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewsManager : MonoBehaviour {

    public List<Report> reports;
    public int showTime = 10;
    public float scrollTime;
    public int showingCharacters;
    public Text newsUIText;

    int showingReport = -1;
    int countCharacter = 0;
    float nextScroll = 0.0f;
    string spaceString;
	// Use this for initialization
	void Start () {
        reports = new List<Report>();
        showingReport = -1;
        for (int i = 0; i < showingCharacters * 2; i++)
            spaceString += " ";


        addReport("Macacos não voam então não invente moda nessa parada, ok?");
    }
	
	// Update is called once per frame
	void Update () {
        if (reports != null)
        {
            if (reports.Count > 0)
                showReport();
            else if (showingReport == -1)
                newsUIText.text = "";
        }
    }

    void showReport() {
        if (showingReport == -1)
            showingReport = 0;

        if (countCharacter >= reports[showingReport].text.Length)
        {

            while (countCharacter != 0)
            {
                showingReport = (showingReport + 1) % reports.Count;
                if (reports[showingReport].isOld())
                {
                    removeReport(showingReport);
                    if(reports.Count == 0)
                    {
                        showingReport = -1;
                        return;
                    }
                }
                else
                {
                    reports[showingReport].showTime();
                    countCharacter = 0;
                }
            }
        }
        if (Time.time > nextScroll )
        {
            countCharacter++;
            newsUIText.text = reports[showingReport].text.Substring(countCharacter);
            nextScroll = Time.time + scrollTime / reports[showingReport].text.Length;
        }
    }

    public void addReport(string report)
    {
        Report r = new Report();
        r.text = spaceString + report;
        r.showNumbers = 10;

        reports.Add(r);
    }

    public void removeReport(int idReport)
    {
        reports.RemoveAt(idReport);
    }

    public void removeReport(Report r)
    {
        reports.Remove(r);
    }
}
