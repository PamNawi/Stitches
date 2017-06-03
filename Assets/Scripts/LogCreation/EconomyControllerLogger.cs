using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyControllerLogger : Logger
{
    EconomyController eController;

    public void startLogging(string path, EconomyController eController)
    {
        base.startLogging(path);
        this.eController = eController;
        StartCoroutine(UpdateLog());
    }

    public void startLogging(string path, string timestamp , EconomyController eController )
    {
        base.startLogging(path, timestamp);
        this.eController = eController;
        StartCoroutine(UpdateLog());
    }

    IEnumerator UpdateLog()
    {
        while (true)
        {
            //Write this block on the file
            file.WriteLine("#####################");
            //Timestamp, Total number of Heros
            file.WriteLine(System.DateTime.Now.ToString("HHmmssffff"));
            //Foreach hero on the list:
            //HeroName, Class
            for (YarnColors color = YarnColors.Black; color != YarnColors.enumEnd; color++)
            {
                file.WriteLine(color.ToString() + "\t" + eController.getStash().stash[color].ToString());
            }            
            yield return new WaitForSeconds(timeNextLog);
        }
    }
}