using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Slider healthBarPrefab;
    public Text MoneyText;
    public MapController mapController;
    public EconomyController economyController;

    public Texture[] buildingIcons;
    public YarnIconDictionary yarnIcons;
    public Button YarnBasketIcon;
    public GameObject puffExplosion;
    public Vector2 sizeButton = new Vector2(100, 100);
    private GameObject selectedGameObject;


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //Debug.Log(Input.mousePosition + "," + mousePosition);
        Physics.Raycast(ray, out hit, 100.0f);
        if (Input.GetMouseButton(0) && hit.collider.name == "WorldPlane")
        {
            Debug.Log(hit.point);
        }
        if (selectedGameObject != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            
            if(selectedGameObject.tag == "Tile")
                selectedGameObject.transform.position = new Vector3((int)mousePosition.x, (int)mousePosition.y, Camera.main.nearClipPlane);
            else
                selectedGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0.0f);
            if (mapController.isValidPositionToCreate(selectedGameObject, selectedGameObject.transform.position))
            {
                selectedGameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color", Color.white);
                if (Input.GetMouseButtonDown(0))
                {
                    mapController.createNewObject(selectedGameObject, selectedGameObject.transform.position);
                    selectedGameObject = null;
                }
            }
            else
                selectedGameObject.GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color", Color.red);
        }

        if (Input.GetMouseButtonDown(1))
        {
            DestroyObject(selectedGameObject);
            selectedGameObject = null;
        }
    }

    public Slider createNewHealthBar(Vector3 position)
    {
        Slider healthBar = Instantiate(healthBarPrefab, position, Quaternion.identity) as Slider;
        healthBar.transform.SetParent(gameObject.transform);
        healthBar.transform.position = position;
        healthBar.transform.localScale = new Vector3(1, 1, 1);
        healthBar.transform.SetAsFirstSibling();
        return healthBar;
    }

    public void destroySlider(Slider slider) { }

    public void createSelectedGameObject(GameObject gObject) 
    {
        if (!selectedGameObject && economyController.canKnitThis(gObject))
        {
            selectedGameObject = Instantiate(gObject, Input.mousePosition, Quaternion.identity) as GameObject;
        }
        else if (selectedGameObject)
        {
            DestroyObject(selectedGameObject);
            selectedGameObject = null;
        }
    }

    public void Unravel(GameObject gObject)
    {
        YarnStash yarnStash = gObject.GetComponent<YarnStash>();
        int total;
        GameObject newYarnIcon;
        SteeringBehavior st;
        Vector3  force = new Vector3();
        if (yarnStash == null)
            return;
        else
        {
            for (YarnColors color = YarnColors.Black; color != YarnColors.enumEnd; color++)
            {
                total = yarnStash.stash[color];
                for(int i = 0; i < total; i++)
                {
                    newYarnIcon = Instantiate(yarnIcons[color], gObject.transform.position, Quaternion.identity) as GameObject;
                    st = newYarnIcon.GetComponent<SteeringBehavior>();
                    force.x = Random.Range(-st.maxVelocity/2, st.maxVelocity/2);
                    force.y = Random.Range(-st.maxVelocity/2, st.maxVelocity/2);
                    st.velocity = force / (st.mass * 12) ;
                    st.ePursuit = YarnBasketIcon.gameObject;
                    newYarnIcon.transform.parent = transform;
                    
                }
            }
        }
        Instantiate(puffExplosion, gObject.transform.position, Quaternion.identity);
    }
}