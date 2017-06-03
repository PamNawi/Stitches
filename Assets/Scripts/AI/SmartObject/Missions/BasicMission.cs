using UnityEngine;
using System.Collections.Generic;

public class BasicMission : SmartObject {

    public int value;

    public virtual void raiseValue(int value)
    {
        this.value += value;
    }

    public override void endSmartObject()
    {
        foreach (BasicAgent b in interactions.Keys)
            b.AbandonSmartObject();
        GameObject.FindGameObjectWithTag("SmartObjectManager").GetComponent<SmartObjectManager>().removeSmartObject(this);
        GameObject.FindGameObjectWithTag("SmartObjectManager").GetComponent<SmartObjectManager>().removeSmartObject(this);
        DestroyObject(gameObject);
    }
}
