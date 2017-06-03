using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
    public float timer = 2f;
    public string leavelToLoad = "Level1";

	// Use this for initialization
	void Start () {
        StartCoroutine("DisplayScene");
	
	}

    IEnumerator DisplayScene()
    {
        yield return new WaitForSeconds(timer);
        Application.LoadLevel(leavelToLoad); // oi?
    }
}
