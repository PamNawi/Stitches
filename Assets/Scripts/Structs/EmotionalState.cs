using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEditor;

public enum StatusEnum {Hunger, Energy, Fun, Unadventurous, endEnum};

[Serializable]
public class Status {
    public StatusEnum statusType;
    public float maximum = 100.0f, minimum = 0.0f, actual = 0.0f;
    public Status(){
    }

    public static Status operator -(Status s, float q)
    {
        Status sResult = new Status();
        sResult = s;
        if(sResult.actual- q >= sResult.minimum)
        {
            sResult.actual -= q;
        }
        return sResult;
    }

    public static Status operator +(Status s, float q)
    {
        Status sResult = new Status();
        sResult = s;
        if (sResult.actual - q >= sResult.minimum)
        {
            sResult.actual += q;
        }
        return sResult;
    }

    public float getPercentage()
    {
        return (actual - minimum) / (maximum - minimum) * 100;
    }
}

public class StatusDictionary : SerializableDictionary<StatusEnum, Status>
{
    public StatusDictionary()
    {
        for(StatusEnum status = 0; status != StatusEnum.endEnum; status++)
        {
            Status newStatus = new Status();
            newStatus.statusType = status;
            Add(status, newStatus);
        }
    }
}

[Serializable]
public class StatusEffectDictionary : SerializableDictionary<StatusEnum, float>{
    public StatusEffectDictionary()
    {
        for(StatusEnum status = 0; status != StatusEnum.endEnum; status++)
        {
            Add(status, 0.0f);
        }
    }
}

public class EmotionalState : MonoBehaviour
{
    public StatusDictionary status = new StatusDictionary();
    float happiness = 0.0f;
    public float baseMood;
    public StatusEffectDictionary decreaseOverTime = new StatusEffectDictionary();
    public void Start()
    {
        StartCoroutine(statusChangeOvertime());
    }
    public void randomizeStatus()
    {
        for (StatusEnum status = 0; status != StatusEnum.endEnum; status++)
        {
            this.status[status].actual = UnityEngine.Random.Range(this.status[status].minimum, this.status[status].maximum) ;
        }
    }

    public void effectStatus(StatusEffectDictionary statusEffect)
    {
        for (StatusEnum status = 0; status != StatusEnum.endEnum; status++)
        {
            this.status[status] += statusEffect[status];
        }
    }

    #region Calculate Status Impact

    public void CalculateTotalHappiness()
    {
        happiness = baseMood;
        happiness += CalculateHungerImpact(status[StatusEnum.Hunger].actual);
        happiness += CalculateEnergyImpact(status[StatusEnum.Energy].actual);
        happiness += CalculateFunImpact(status[StatusEnum.Fun].actual);
        happiness += CalculateUnadventurousImpact(status[StatusEnum.Unadventurous].actual);
    }

    public float getHappiness()
    {
        return happiness;
    }

    public float getTotalHappinessEffect(StatusEffectDictionary activityEffect) {
        float happinessImpact = happiness;
        StatusDictionary currentStatusImpact, futureStatusImpact;

        currentStatusImpact = status;
        futureStatusImpact = new StatusDictionary();

        futureStatusImpact[StatusEnum.Hunger].actual    = CalculateHungerImpact(currentStatusImpact[StatusEnum.Hunger].actual - activityEffect[StatusEnum.Hunger]);
        futureStatusImpact[StatusEnum.Energy].actual    = CalculateEnergyImpact(currentStatusImpact[StatusEnum.Energy].actual - activityEffect[StatusEnum.Energy]);
        futureStatusImpact[StatusEnum.Fun].actual       = CalculateFunImpact(currentStatusImpact[StatusEnum.Fun].actual - activityEffect[StatusEnum.Fun]);
        futureStatusImpact[StatusEnum.Unadventurous].actual     = CalculateUnadventurousImpact(currentStatusImpact[StatusEnum.Unadventurous].actual - activityEffect[StatusEnum.Unadventurous]);

        happinessImpact += futureStatusImpact[StatusEnum.Hunger].actual - currentStatusImpact[StatusEnum.Hunger].actual;
        happinessImpact += futureStatusImpact[StatusEnum.Energy].actual - currentStatusImpact[StatusEnum.Energy].actual;
        happinessImpact += futureStatusImpact[StatusEnum.Fun].actual - currentStatusImpact[StatusEnum.Fun].actual;
        happinessImpact += futureStatusImpact[StatusEnum.Unadventurous].actual - currentStatusImpact[StatusEnum.Unadventurous].actual;
        return happinessImpact;
    }

    float CalculateHungerImpact(float hunger)
    {
        return hunger * hunger;
    }

    float CalculateEnergyImpact(float energy)
    {
        return energy * energy;
    }

    float CalculateFunImpact(float fun)
    {
        return fun * fun;
    }

    float CalculateHygieneImpact(float hygiene)
    {
        return hygiene * hygiene;
    }

    float CalculateUnadventurousImpact(float Unadventurous)
    {
        return Unadventurous * Unadventurous;
    }
    #endregion

    IEnumerator statusChangeOvertime()
    {
        while (true)
        {
            effectStatus(decreaseOverTime);
            CalculateTotalHappiness();
            yield return new WaitForSeconds(1.0f);
        }
    }
}