using UnityEngine;
using System.Collections.Generic;


public class BasicHero : BasicAgent
{
    public HeroClass heroClass;

    private void Awake()
    {
        emotionalState = GetComponent<EmotionalState>();
        followPath = GetComponent<FollowPath>();
        viewSight = GetComponentInChildren<ViewSight>();
        health = GetComponent<Health>();
        heroClass = 0;
        agentName = new NameGenerator().createNewName();

        agentTag = heroClass.ToString();
    }
}