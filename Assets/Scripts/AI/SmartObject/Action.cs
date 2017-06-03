using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Action
{
    public bool started = false;
    public bool startCorrect = false;
    public bool endedCorrect = false;
    public Dictionary<string,string> extraVariables;

    public virtual void start(BasicAgent basicAgent)  {
        started = true;
        startCorrect = true;
        endedCorrect = false;
    }
    public virtual void end(BasicAgent basicAgent)
    {
        endedCorrect = true;
    }
    public virtual bool hasEnd(BasicAgent basicAgent) { return true;  }
    public virtual void doThis(BasicAgent basicAgent) {  }
    public virtual bool isStarted()
    {
        return started;
    }
}

public class Come : Action
{
    public Vector3 position;
    public Come(Dictionary<string, string> extraVariables)
    {
        this.extraVariables = extraVariables;
        try
        {
            position = new Vector3(System.Convert.ToSingle(extraVariables["x"]),
                System.Convert.ToSingle(extraVariables["y"]),
                System.Convert.ToSingle(extraVariables["z"]));
        }
        catch
        {
            throw new ArgumentException("Something is missing to load Action " + GetType().ToString() + "." +
                "This action requires this params: framesDuration");
        }
    }
    public Come(Vector3 destiny)
    {
        position = destiny;
    }

    public override void start(BasicAgent basicAgent)
    {
        basicAgent.followPath.setDestiny(position);
        if (basicAgent.followPath.hasPath())
        {
            started = true;
            startCorrect = true;
        }
        else
        {
            started = false;
            startCorrect = false;
        }
    }

    public override bool hasEnd(BasicAgent basicAgent)
    {
        return !basicAgent.followPath.hasPath();
    }
}

public class Enter : Action
{

    public override void start(BasicAgent basicAgent)
    {
        base.start(basicAgent);
    }
    public override void doThis(BasicAgent basicAgent)
    {
    }
}

public class Leave : Action
{
    public override void start(BasicAgent basicAgent)
    {
        base.start(basicAgent);
    }
    public override void doThis(BasicAgent basicAgent)
    {

    }
}

public class Attack : Action
{
    GameObject target;
    Health health;
    Hero hero;
    public Attack(GameObject target)
    {
        this.target = target;
        health = target.GetComponent<Health>();
    }
    public override void start(BasicAgent basicAgent)
    {
        base.start(basicAgent);
        basicAgent.followPath.setDestiny(target.transform.position);
        hero = basicAgent as Hero;
        hero.target = target;
    }
    public override bool hasEnd(BasicAgent basicAgent)
    {
        return health.isAlive();
    }
    public override void doThis(BasicAgent basicAgent)
    {
        hero.Attack();
    }
}

public class TimeAction : Action
{
    public float framesDuration;
    public float startTime;

    public TimeAction(Dictionary<string, string> extraVariables) : base() {
        try
        {
            framesDuration = System.Convert.ToSingle(extraVariables["framesDuration"]);
        }
        catch
        {
            throw new ArgumentException("Something is missing to load Action " + GetType().ToString() + "." +
                "This action requires this params: framesDuration");
        }
    }
    public override void start(BasicAgent basicAgent)
    {
        startTime = Time.time;
        base.start(basicAgent);
    }
    public override bool hasEnd(BasicAgent basicAgent)
    {

        if (startTime + framesDuration > Time.time)
            return false;
        return true;
    }

}

public class ApplyStatusEffectOverTime : TimeAction
{
    StatusEffectDictionary statusFrames = new StatusEffectDictionary();
    public ApplyStatusEffectOverTime(Dictionary<string, string> extraVariables) : base(extraVariables)
    {
        try
        {
            this.framesDuration = System.Convert.ToSingle(extraVariables["framesDuration"]);
            statusFrames[StatusEnum.Hunger] = System.Convert.ToSingle(extraVariables["eHunger"]) / framesDuration;
            statusFrames[StatusEnum.Energy] = System.Convert.ToSingle(extraVariables["eEnergy"]) / framesDuration;
            statusFrames[StatusEnum.Fun] = System.Convert.ToSingle(extraVariables["eFun"]) / framesDuration;
            statusFrames[StatusEnum.Unadventurous] = System.Convert.ToSingle(extraVariables["eUnadventurous"]) / framesDuration;
        }
        catch
        {
            throw new ArgumentException("Something is missing to load Action " + GetType().ToString() + "." +
                "This action requires this params: framesDuration, eHunger, eEnergy, eFun, eUnadventurous");
        }
    }

    public override void doThis(BasicAgent basicAgent)
    {
        basicAgent.emotionalState.effectStatus(statusFrames);
    }
}

