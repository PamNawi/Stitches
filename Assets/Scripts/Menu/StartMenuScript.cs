using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas optionsMenu;

    public Button startText;
    public Button optionsText;
    public Button exitText;

    public string leavelToLoad = "ProfileMenu";
    // Use this for initialization
    void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        optionsMenu = optionsMenu.GetComponent<Canvas>();

        startText   = startText.GetComponent<Button>();
        optionsText = optionsText.GetComponent<Button>();
        exitText    = exitText.GetComponent<Button>();

        quitMenu.enabled = false;
        optionsMenu.enabled = false;
  	}

    public void ExitPress()
    {
        quitMenu.enabled = true;
        optionsMenu.enabled = false;

        startText.enabled = false;
        optionsText.enabled = false;
        exitText.enabled = false;

    }
    public void NoPress()
    {
        quitMenu.enabled = false;
        optionsMenu.enabled = false;

        startText.enabled = true;
        optionsText.enabled = false;
        exitText.enabled = true;
    }
    public void StartGame()
    {
        //Application.LoadLevel(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ShowOptions()
    {
        quitMenu.enabled = false;
        optionsMenu.enabled = true;

        startText.enabled = false;
        optionsText.enabled = false;
        exitText.enabled = false;
    }
    public void CloseOptions()
    {
        quitMenu.enabled = false;
        optionsMenu.enabled = false;

        startText.enabled = true;
        optionsText.enabled = true;
        exitText.enabled = true;
    }
    public void StartProfileMenu()
    {
        Application.LoadLevel(leavelToLoad); // oi?
    }
}
