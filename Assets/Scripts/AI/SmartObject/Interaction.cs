using UnityEngine;
using System.Collections.Generic;

public class Interaction
{
    public List<Action> actionSequence = new List<Action>();
    public int actualAction = 0;
    public BasicAgent interatingAgent = null;

    public string name;
    public StatusEffectDictionary statusEffect = new StatusEffectDictionary();
    public List<string> tagInteraction = new List<string>();
    public Dictionary<string, string> extraVariables = new Dictionary<string, string>();
    public SmartObject smartObject = null;
    public Interaction() { }

    public Interaction(Interaction i)
    {

    }

    public virtual bool load(Dictionary<string, string> dict)
    {
        name = dict["Name"];
        statusEffect[StatusEnum.Hunger] = System.Convert.ToSingle(dict["Hunger"]);
        statusEffect[StatusEnum.Energy] = System.Convert.ToSingle(dict["Energy"]);
        statusEffect[StatusEnum.Fun] = System.Convert.ToSingle(dict["Fun"]);
        statusEffect[StatusEnum.Unadventurous] = System.Convert.ToSingle(dict["Unadventurous"]);

        tagInteraction = new List<string>(dict["TagInteracao"].Split(','));

        foreach (string extra in dict.Keys)
        {
            extraVariables[extra] = dict[extra];            
        }
        return true;
    }

    public virtual bool load()
    {
        name = "";
        statusEffect[StatusEnum.Hunger] = 0.0f;
        statusEffect[StatusEnum.Energy] = 0.0f;
        statusEffect[StatusEnum.Fun] = 0.0f;
        statusEffect[StatusEnum.Unadventurous] = 0.0f;

        tagInteraction = new List<string>();
        return true;
    }

    public bool interact()
    {
        //The actual action is valid?
        if (!isValidAction())
        {
            interatingAgent.Log("Ending this interaction");
            endInteraction();
            return false;
        }
        //Is the actual action started?
        if (!actionSequence[actualAction].isStarted())
        {
            interatingAgent.Log("Starting Action\t" + actionSequence[actualAction].GetType());
            actionSequence[actualAction].start(interatingAgent);
            return actionSequence[actualAction].startCorrect;

        }
        //Is the actual action end?
        else if (actionSequence[actualAction].hasEnd(interatingAgent))
        {

            interatingAgent.Log("Ending Action\t" + actionSequence[actualAction].GetType());
            actionSequence[actualAction].end(interatingAgent);
            actualAction++;
            //Should I abbandon the mission???? Idk,  will know what to do on the next turn...
            return actionSequence[actualAction-1].endedCorrect;
        }
        //So just do what you have to do...
        actionSequence[actualAction].doThis(interatingAgent);
        return true;
    }

    public virtual Interaction clone()
    {
        Interaction clone = new Interaction();
        clone.actionSequence = actionSequence;
        clone.actualAction = actualAction;

        clone.load(extraVariables);
        clone.loadActionSequence();

        return clone;
    }

    public virtual void endInteraction()
    {
        interatingAgent.endInteraction();
    }

    public bool isValidAction()
    {
        if (actualAction >= actionSequence.Count)
            return false;
         
        return true;
    }

    public bool canInteract(BasicAgent basicAgent)
    {
        string aType = basicAgent.agentTag;
        //Verifica se o agente pode fazer isso...
        foreach (string tag in tagInteraction){
            switch (tag)
            {
            case "Hero":
                    //So test if is a hero class
                    if (aType == "Cleric" || aType == "Thief" || 
                        aType == "Warrior" || aType == "Archer" || 
                        aType == "Swordman" || aType == "Mage")
                        return true;
                    break;
                    //So test if is a enemy class
            case "Enemy":
                    break;
            }
            //This interaction don't is using a "big class" tag
            if(tag == aType)
            {
                return true;
            }

        }
        return false;
    }

    public virtual bool loadActionSequence() { return true; }
}

#region Basic Buildings

public class VisitBuilding : Interaction {

    //Extra Variables:
    // Location = here
    // FramesDuration .0f
    // eStatus .0f
    public VisitBuilding() {}

    public override bool loadActionSequence(){
        actionSequence.Add(new Come(smartObject.transform.position));
        actionSequence.Add(new Enter());
        actionSequence.Add(new ApplyStatusEffectOverTime(extraVariables));
        actionSequence.Add(new Leave());
        actionSequence.Add(new Come(Vector3.zero)); 
        return true;
    }
}

#endregion

#region Missions

public class BasicMissionInteraction : Interaction
{
    public override void endInteraction()
    {
        smartObject.endSmartObject();
    }
}

public class ExploreMissionInteraction : BasicMissionInteraction
{


    //Extra Variables:
    // Location = here (smartObject.transform.position)
    // FramesDuration .0f
    // eStatus .0f
    public ExploreMissionInteraction()
    {
    }

    public override bool loadActionSequence()
    {
        actionSequence.Add(new Come(smartObject.transform.position));
        return true;
    }
}

//public class AttackMissionInteraction : BasicMissionInteraction
//{
//     This requires some real smart thinking on the next week.REAL SMART THINKING THIS SHIT IS COMPLICATED
//      NOT COMPLEX BUT COMPLICATED D=

//    //Extra Variables
//    //Target will get from the SmartObject
//    public AttackMissionInteraction()   {    }

//    public override bool loadActionSequence()
//    {
//        actionSequence.Add(new Attack(smartObject.transform.parent.gameObject));
//        return true;
//    }
//}

#endregion

#region World Interactions

public class Stroll: Interaction {
    //Choose N positions to stroll around
}

public class LeaveCity : Interaction{
    public LeaveCity() { }
    public override bool loadActionSequence()
    {
        //Ask for the nearest Spawner to the Hero Controller

        //Come to that Spawner

        //"Leave" the city
        return true;
    }
}

#endregion