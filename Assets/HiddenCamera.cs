using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenCamera : MonoBehaviour {

    public static Camera camera;
    // Use this for initialization
    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
