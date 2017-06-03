using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public enum HeroClass {Cleric, Thief, Warrior, Archer, Swordman, Mage, endEnum};

[Serializable]
public class HeroGameObjectDictionary : SerializableDictionary<HeroClass, GameObject >
{
    public HeroGameObjectDictionary()
    {
        for(HeroClass hero = 0; hero != HeroClass.endEnum; hero++)
        {
            Add(hero, null);
        }
    }
}

[Serializable]
public class HeroFloatDictionary : SerializableDictionary<HeroClass, float>
{
    public HeroFloatDictionary()
    {
        for(HeroClass hero = 0; hero != HeroClass.endEnum; hero++)
        {
            Add(hero, 0.0f);
        }
    }

    public static HeroFloatDictionary operator -(HeroFloatDictionary ha1 , HeroFloatDictionary ha2)
    {
        HeroFloatDictionary ha3 = new HeroFloatDictionary();
        for (HeroClass hc = 0; hc != HeroClass.endEnum; hc++)
        {
            ha3[hc] = ha1[hc] - ha2[hc];
        }
        return ha3;
    }


    public static HeroFloatDictionary operator +(HeroFloatDictionary ha1, HeroFloatDictionary ha2)
    {
        HeroFloatDictionary ha3 = new HeroFloatDictionary();
        for (HeroClass hc = 0; hc != HeroClass.endEnum; hc++)
        {
            ha3[hc] = ha1[hc] + ha2[hc];
        }
        return ha3;
    }
}

[Serializable]
public class Herosheet : MonoBehaviour
{
    public HeroGameObjectDictionary heros;

    public Herosheet()
    {
        heros = new HeroGameObjectDictionary();
    }
}

