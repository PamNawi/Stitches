using UnityEngine;
using System.Collections;

public class IsoFix : MonoBehaviour
{

    public bool fix = true;
    //If active is set to true, The fix will be applied at each frame, useful if the camera rotate
    public bool active = false;
    public Vector3 fixPosition = new Vector3(-45f, 0f, 0f);

    private void OnEnable()
    {
        Fix();
    }

    private void Update()
    {
        if (active)
            Fix();
    }

    private void Fix()
    {
        if (fix)
            this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward) * Quaternion.Euler(fixPosition);
    }
}