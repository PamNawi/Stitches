using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System;

public class SmartObject : MonoBehaviour {
    public int maxAgents = 1;
    public Dictionary<BasicAgent, Interaction> interactions;
    public bool physic;
    protected SmartObjectManager soManager;
    public List<string> bInteractions;
    public string soName = "";
    string description;

    public void Start()
    {
        bInteractions = new List<string>();
        interactions = new Dictionary<BasicAgent, Interaction>();
        soManager = GameObject.FindGameObjectWithTag("SmartObjectManager").GetComponent<SmartObjectManager>();
        if (bInteractions.Count == 0 && soName != "")
        {
            try
            {
                //Clone this smart object based on one in the dictonary
                load();
            }
            catch (Exception ex)

            {
                Debug.Log("Problemas em localizar a tag/nome do Smart Object: " + soName + " para " + name);
                Debug.Log(ex.Message);
            }
        }
    }

    public void load()
    {
        SmartObject baseSO = soManager.baseSmartObjects[soName];

        description = baseSO.description;
        bInteractions = baseSO.bInteractions;
    }

    public void load(Dictionary<string, string> dict , List<string> bInteractions)
    {
        soName = dict["Name"];
        description = dict["Description"];
        this.bInteractions = bInteractions;
    }

    public void load(Dictionary<string,string> dict)
    {
        soName = dict["Name"];
        description = dict["Description"];
        bInteractions = new List<string>(dict["Interaction"].Split(','));
        bInteractions.Remove("");
    }

    public virtual bool startInteraction(BasicAgent agent, string interaction)
    {
        //Can I interact with another agent?
        if (!canInteract(agent, interaction))
        {
            //Sorry, I can't...
            return false;
        }
        //Yes, I can =)
        interactions.Add(agent, soManager.fInteraction.Create(interaction));
        interactions[agent].interatingAgent = agent;
        interactions[agent].smartObject = this;
        interactions[agent].load(soManager.interactionsParams[interaction]);
        interactions[agent].loadActionSequence();

        agent.Log("Starting Interaction\t" + interaction +"\t" + soName);
        return true;
    }

    public virtual bool canInteract(BasicAgent agent, string interaction)
    {
        if (maxAgents != -1 && maxAgents > interactions.Count)
            return false;
        //If I can manage another agent, so lets see with this agent CAN make this...
        return soManager.interactions[interaction].canInteract(agent);
    }

    public virtual bool interact(BasicAgent agent) {
        if (interactions.ContainsKey(agent))
            return interactions[agent].interact();
        throw new ArgumentException("No type registered for this id:" + agent);
        return false;
    }

    public virtual bool endInteraction(BasicAgent agent)
    {
        interactions.Remove(agent);
        return true;
    }

    public virtual void endSmartObject()
    {
        foreach(BasicAgent b in interactions.Keys)
            b.AbandonSmartObject();
        GameObject.FindGameObjectWithTag("SmartObjectManager").GetComponent<SmartObjectManager>().removeSmartObject(this);
    }
}
