using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

public class SmartObjectManager : MonoBehaviour
{
    //List of active Smart Objects
    public List<SmartObject> smartObjects = new List<SmartObject>();

    //To load all parameters of Interactions. Remember there's alot of Interactions heritage here, don't touch this.
    public Factory<Interaction> fInteraction = new Factory<Interaction>();
    public Dictionary<string, Dictionary<string, string>> interactionsParams = new Dictionary<string, Dictionary<string, string>>();

    //To load all parameters of Smart Objects, internal use only
    Factory<SmartObject> fSmartObject = new Factory<SmartObject>();
    Dictionary<string, Dictionary<string, string>> soParams = new Dictionary<string, Dictionary<string, string>>();

    //The real stuff, clone this to use on all objects
    public Dictionary<string, Interaction> interactions = new Dictionary<string, Interaction>();
    public Dictionary<string, SmartObject> baseSmartObjects = new Dictionary<string, SmartObject>();

    public TextAsset xmlFileSmartObjects;
    public TextAsset xmlFileInteractions;

    public void LoadInteractionsVariables()
    {
        //Load xml file of propertys
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFileInteractions.text);
        XmlNodeList lInteraction = xmlDoc.GetElementsByTagName("Interaction");
        Dictionary<string, string> obj;

        foreach (XmlNode interactionInfo in lInteraction)
        {
            XmlNodeList interactions = interactionInfo.ChildNodes;
            obj = new Dictionary<string, string>();
            foreach (XmlNode descInteraction in interactions)
            {
                if(descInteraction.Name == "Name")
                    obj.Add("Name", descInteraction.InnerText);

                //Interaction Status
                else if(descInteraction.Name == "Hunger")
                    obj.Add("Hunger", descInteraction.InnerText);
                else if (descInteraction.Name == "Energy")
                    obj.Add("Energy", descInteraction.InnerText);
                else if (descInteraction.Name == "Fun")
                    obj.Add("Fun", descInteraction.InnerText);
                else if (descInteraction.Name == "Unadventurous")
                    obj.Add("Unadventurous", descInteraction.InnerText);

                else if (descInteraction.Name == "tagInteracao")
                {
                    if (!obj.ContainsKey("TagInteracao"))
                        obj["TagInteracao"] = "";
                    obj["TagInteracao"] += descInteraction.InnerText + ",";
                }
                else
                {
                    obj.Add(descInteraction.Name,descInteraction.InnerText);
                }
            }

            this.interactions[obj["Name"]].load(obj);
            interactionsParams.Add(obj["Name"], obj);
        }
    }

    public void LoadSmartObjects()
    {
        //Load xml file of propertys
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFileSmartObjects.text);
        XmlNodeList lInteraction = xmlDoc.GetElementsByTagName("SmartObject");
        Dictionary<string, string> obj;
        List<string> objInteractions;

        foreach (XmlNode SmartObjectInfo in lInteraction)
        {
            XmlNodeList values = SmartObjectInfo.ChildNodes;
            obj = new Dictionary<string, string>();
            objInteractions = new List<string>();
            foreach (XmlNode descSmartObject in values)
            {
                if (descSmartObject.Name == "Name")
                    obj.Add("Name", descSmartObject.InnerText);

                //Interaction Status
                else if (descSmartObject.Name == "Description")
                    obj.Add("Description", descSmartObject.InnerText);


                else if (descSmartObject.Name == "Interaction")
                {

                    objInteractions.Add(descSmartObject.InnerText);
                }
                else
                {
                    obj.Add(descSmartObject.Name, descSmartObject.InnerText);
                }
            }
            SmartObject newSmartObject = new SmartObject();
            newSmartObject.load(obj, objInteractions);

            baseSmartObjects.Add(newSmartObject.soName, newSmartObject);
            soParams.Add(obj["Name"], obj);
        }
    }

    public void LoadInteractionHash()
    {
        #region Missions
        interactions["AttackMission"] = new Interaction();
        fInteraction.Register("AttackMission", () => new Interaction());

        interactions["ExploreMission"] = new ExploreMissionInteraction();
        fInteraction.Register("ExploreMission", () => new ExploreMissionInteraction());

        interactions["DefenseMission"] = new Interaction();
        fInteraction.Register("DefenseMission", () => new Interaction());
        #endregion

        #region Buildings
        interactions["Visit"] = new VisitBuilding();
        fInteraction.Register("Visit", () => new VisitBuilding());

        interactions["VisitINN"] = new VisitBuilding();
        fInteraction.Register("VisitINN", () => new VisitBuilding());
        #endregion

        #region Guilds
        interactions["VisitMonastery"] = new VisitBuilding();
        fInteraction.Register("VisitMonastery", () => new VisitBuilding());

        interactions["VisitRoguesGuild"] = new VisitBuilding();
        fInteraction.Register("VisitRoguesGuild", () => new VisitBuilding());

        interactions["VisitWarriorsGuild"] = new VisitBuilding();
        fInteraction.Register("VisitWarriorsGuild", () => new VisitBuilding());

        interactions["VisitArchersTent"] = new VisitBuilding();
        fInteraction.Register("VisitArchersTent", () => new VisitBuilding());

        interactions["VisitSwordmansFort"] = new VisitBuilding();
        fInteraction.Register("VisitSwordmansFort", () => new VisitBuilding());

        interactions["VisitMagesTower"] = new VisitBuilding();
        fInteraction.Register("VisitMagesTower", () => new VisitBuilding());
        #endregion

        #region World
        interactions["Stroll"] = new Stroll();
        fInteraction.Register("Stroll", () => new Stroll());

        interactions["LeaveCity"] = new LeaveCity();
        fInteraction.Register("LeaveCity", () => new LeaveCity());
        #endregion
    }

    // Use this for initialization
    void Awake()
    {
        LoadInteractionHash();

        //Load from a .xml
        LoadInteractionsVariables();
        LoadSmartObjects();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addSmartObject(SmartObject so)
    {
        if(so != null)
            smartObjects.Add(so);
    }

    public void addSmartObject(GameObject gObject)
    {
        SmartObject so = gObject.GetComponent<SmartObject>();
        addSmartObject(so);
    }

    public void removeSmartObject(SmartObject so)
    {
        if(so != null)
        smartObjects.Remove(so);
    }

    public List<SmartObject> getAllSmartObjects()
    {
        return smartObjects;
    }

}
