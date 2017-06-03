using UnityEngine;
using System.Collections;

public class YarnIconBehaviour : MonoBehaviour {
    SteeringBehavior st;

	// Use this for initialization
	void Awake () {
        st = GetComponent<SteeringBehavior>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == st.ePursuit.gameObject)
        {
            DestroyObject(this.gameObject);
        }

    }
}
