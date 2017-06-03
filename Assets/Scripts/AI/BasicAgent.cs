using UnityEngine;
using System.Collections.Generic;


public class BasicAgent : MonoBehaviour
{
    public bool log;
    public SmartObject interacting;
    [HideInInspector]    public EmotionalState emotionalState;
    [HideInInspector]    public FollowPath followPath;
    [HideInInspector]    public ViewSight viewSight;
    [HideInInspector]    public Health health;
    public string agentName = "";

    public string agentTag = "Basic Agent";
    public GameObject agentSymbol;

    protected string fileName = "C:/Users/PamNawi/Desktop/LogStitches/HerosLogs/";
    protected System.IO.StreamWriter file;

    private void Awake()
    {
        followPath = GetComponent<FollowPath>();
        health = GetComponent<Health>();
        viewSight = GetComponent<ViewSight>();
        emotionalState = GetComponent<EmotionalState>();

        if (log)
            createLogFile();
    }

    protected void createLogFile()
    {
        fileName += agentName + ".txt";
        file = new System.IO.StreamWriter(fileName, true);
    }

    public void Log(string str)
    {
        if (!log)
            return;
        file.WriteLine(System.DateTime.Now.ToString("HHmmssffff") + '\t' + str);
    }

    public virtual void Update()
    {
        bool view = false;
        //Verify my sensors;
        view = ViewAround();
        //There's nothing wrong;
        if (!view)
            DoMyStuff();
    }

    private void OnDestroy()
    {
        if(log)
            file.Close();
    }

    public void DoMyStuff()
    {
        if (!interacting )
        {
            chooseNextSmartObject();
            if (!interacting)
                return;
        }
        interacting.interact(this);
    }

    public void endInteraction()
    {
        interacting.endInteraction(this);
        interacting = null;
    }

    public void AbandonSmartObject()
    {
        interacting = null;
        if (followPath.hasPath())
        {
            followPath.abandonPath();

        }
    }

    public virtual void chooseNextSmartObject()
    {
        SmartObjectManager soManager = GameObject.FindGameObjectWithTag("SmartObjectManager").GetComponent<SmartObjectManager>();
        List<SmartObject> options = soManager.getAllSmartObjects();
        SmartObject choosen = null;
        string interaction = "";

        float betterHappinessChange = emotionalState.getHappiness();
        float happinessChange;
        float d1, d2;
        interacting = null;
        foreach (SmartObject so in options)
        {
            foreach(string i in so.bInteractions)
            {
                happinessChange = emotionalState.getTotalHappinessEffect(soManager.interactions[i].statusEffect);
                if (so.canInteract(this,i))
                {
                    if (happinessChange > betterHappinessChange)
                    {
                        betterHappinessChange = happinessChange;
                        choosen = so;
                        interaction = i;
                    }
                    if (happinessChange == betterHappinessChange)
                    {
                        if (choosen == null)
                        {
                            choosen = so;
                            interaction = i;
                        }
                        else
                        {
                            d1 = Vector3.Distance(transform.position, so.transform.position);
                            d2 = Vector3.Distance(transform.position, choosen.transform.position);
                            if (d2 > d1)
                            {
                                choosen = so;
                                interaction = i;
                            }
                        }
                    }
                }
            }
            
        }
        if (choosen != null)
        {
            if (choosen.startInteraction(this,interaction))
            {
                interacting = choosen;
            }
            else
                return;
        }
    }

    public virtual bool ViewAround()
    {
        //There's something on my view?

        //What hell I should do?
        return false;
    }

}