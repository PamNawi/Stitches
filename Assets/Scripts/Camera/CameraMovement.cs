using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    public float minDistanceToBorder;
    public float movementVelocity;
	
	// Update is called once per frame
	void Update () {
        //float scrHeight = Screen.height;
        //float scrWidth = Screen.width;

        //Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //float yBorder = scrHeight - mousePos.y;
        //float xBorder = scrWidth - mousePos.x;

        Vector3 movement = Vector3.zero;

        /*if (yBorder <= minDistanceToBorder)
        {
            movement = new Vector3(movement.x, movement.y, minDistanceToBorder - yBorder);
        }
        else if(mousePos.y < minDistanceToBorder) {
            movement = new Vector3(movement.x, movement.y, -(minDistanceToBorder - mousePos.y));
        }
        if(xBorder <= minDistanceToBorder)
        {
            movement = new Vector3(minDistanceToBorder - xBorder, movement.y, movement.z);
        }
        else if(mousePos.x < minDistanceToBorder)
        {
            movement = new Vector3(-(minDistanceToBorder - mousePos.x), movement.y, movement.z);
        }*/

        float forward = Input.GetAxis("Forward");
        float right = Input.GetAxis("Right");

        movement = new Vector3(movement.x + right * movementVelocity * minDistanceToBorder, movement.y + forward * movementVelocity * minDistanceToBorder, movement.z);

        transform.Translate(movement * Time.deltaTime * movementVelocity, Space.World);
	
	}
}
