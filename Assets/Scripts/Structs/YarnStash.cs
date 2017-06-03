using System;
using UnityEditor;
using UnityEngine;
public enum YarnColors { Black, White, Grey, Red, Orange, Yellow, Green, Blue, Violet, enumEnd};

[Serializable]
public class YarnDictionary : SerializableDictionary<YarnColors, int> {
    public YarnDictionary()
    {
        for (YarnColors color = 0; color != YarnColors.enumEnd; color++)
        {
            Add(color, 0);
        }
    }
    public static YarnDictionary operator -(YarnDictionary ys1, YarnDictionary ys2)
    {
        YarnDictionary ys3 = new YarnDictionary();
        for (YarnColors color = 0; color != YarnColors.enumEnd; color++)
        {
            ys3[color] = ys1[color] - ys2[color];
        }
        return ys3;
    }


    public static YarnDictionary operator +(YarnDictionary ys1, YarnDictionary ys2)
    {
        YarnDictionary ys3 = new YarnDictionary();
        for (YarnColors color = 0; color != YarnColors.enumEnd; color++)
        {
            ys3[color] = ys1[color] + ys2[color];
        }
        return ys3;
    }

}

[Serializable]
public class YarnIconDictionary : SerializableDictionary<YarnColors, GameObject>
{
    public YarnIconDictionary()
    {
        for(YarnColors color = 0; color != YarnColors.enumEnd; color++)
        {
            Add(color, null);
        }
    }
}

[Serializable]
public class YarnStash : MonoBehaviour
{
    public YarnDictionary stash;

    public YarnStash(){
        stash = new YarnDictionary();

    }

    public void addNewBallsToStash(YarnColors color, int quantity)
    {
        stash[color] += quantity;
    }

    public bool removeBallsFromStash(YarnColors color, int quantity)
    {
        if(stash[color] >= 0)
        {
            stash[color] -= quantity;
            return true;
        }
        return false;
    }
    /*Conversar com alguém sobre isso pq ooooh a dor de cabeça
    public static YarnStash operator -(YarnStash ys1, YarnStash ys2)
    {
        YarnStash ys3 = new YarnStash();
        ys3.stash = (ys1.stash - ys2.stash);
        return ys3;
    }

    public static YarnStash operator +(YarnStash ys1, YarnStash ys2)
    {
        YarnStash ys3 = new YarnStash();
        ys3.stash = (ys1.stash + ys2.stash);
        return ys3;
    }*/

}

