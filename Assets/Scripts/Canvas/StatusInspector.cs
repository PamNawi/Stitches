using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;

[Serializable]
public class StatusSliderDictionary : SerializableDictionary<StatusEnum, Slider>
{
    public StatusSliderDictionary()
    {
        for (StatusEnum status = StatusEnum.Hunger; status != StatusEnum.endEnum; status++)
        {
            Add(status, null);
        }
    }
}

[CustomPropertyDrawer(typeof(StatusSliderDictionary))]
public class StatusSliderDictionaryDrawer : DictionaryDrawer<StatusEnum, Slider> { }

public class StatusInspector : MonoBehaviour {
    // Use this for initialization
    public StatusSliderDictionary statusSliders;
    public EmotionalState heroState = null;
    public HeroGameObjectDictionary heroSymbols;
    public HeroClass agentClass;
    public Text agentName;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (heroState)
        {
            for (StatusEnum status = StatusEnum.Hunger; status != StatusEnum.endEnum; status++)
            {
                statusSliders[status].value = heroState.status[status].getPercentage();
            }
        }
	
	}

    public void showHeroState(GameObject hero)
    {
        BasicHero ba = hero.GetComponent<BasicHero>();
        heroState = hero.GetComponent<EmotionalState>();
        agentName.text = ba.agentName;
        agentClass = ba.heroClass;
        for(HeroClass hc = 0; hc!= HeroClass.endEnum; hc++)
        {
            heroSymbols[hc].SetActive(false);
        }
        heroSymbols[agentClass].SetActive(true);
    }
}
