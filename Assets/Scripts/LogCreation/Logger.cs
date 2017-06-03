using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Logger : MonoBehaviour
{
    public string fileName = "HeroControllerLogger";
    protected System.IO.StreamWriter file;
    //Only used on the "childs" who needs
    public int timeNextLog = 10;

    private void OnDestroy()
    {
        if (file != null)
            file.Close();
    }

    public virtual void startLogging(string path)
    {
        fileName = path + fileName + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".txt";
        file = new System.IO.StreamWriter(fileName, true);
    }

    public virtual void startLogging(string path, string timestamp)
    {
        fileName = path + fileName + timestamp + ".txt";
        file = new System.IO.StreamWriter(fileName, true);
    }
}
