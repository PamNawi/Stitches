using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    [HideInInspector] public Slider healthSlider;                                 // Reference to the UI's health bar.

    // Use this for initialization
    void Awake () {
        CanvasController canvas = GameObject.FindGameObjectWithTag("CanvasUI").GetComponent<CanvasController>();
        healthSlider = canvas.createNewHealthBar(gameObject.transform.position);
        healthSlider.value = currentHealth;
	}
	
	// Update is called once per frame
	void Update () {
        healthSlider.transform.position = gameObject.transform.position + new Vector3(0.25f,0.5f,0);
        healthSlider.value = currentHealth;
    }

    void OnDestroy()
    {
        if(healthSlider != null)
            Destroy(healthSlider.gameObject);
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
    }

    public bool isAlive()
    {
        if (currentHealth <= 0)
            return false;
        return true;
    }
}
