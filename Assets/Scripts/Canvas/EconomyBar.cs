using UnityEngine;
using System.Collections.Generic;

public class EconomyBar : MonoBehaviour {
    public List<GameObject> buttonGroups;
    bool showing = false;
    GameObject selectedButtonGroup;
    // Use this for initialization
    void Start()
    {
        hideEverything();
    }

    // Update is called once per frame
    void Update()
    {
        if (showing && Input.GetMouseButtonDown(1))
        {
            hideEverything();
            showing = false;
        }
    }


    void hideEverything()
    {
        Animator animBGroup;
        foreach (GameObject bGroup in buttonGroups)
        {
            animBGroup = bGroup.GetComponent<Animator>();
            animBGroup.SetInteger("Showing", 0);
        }
    }

    public void showButtonGroup(GameObject bGroup)
    {
        Animator animBGroup;
        hideEverything();
        animBGroup = bGroup.GetComponent<Animator>();
        animBGroup.SetInteger("Showing", 1);
        showing = true;
    }
}
