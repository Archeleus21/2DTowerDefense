using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private TowerButtons towerButtonPressed;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //used to detect mouse click to place towers
        if(Input.GetMouseButtonDown(0))
        {
            //gets mouse pos on sreen
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);  

             //finds world point (where we clicked) from 0,0 (bottom left Corner)
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); 

            //checks if it collided with the proper object, and if so then places tower
            if (hit.collider.tag == "BuildSite" && spriteRenderer.enabled)
            {
                //changes tag to prevent multple towers on one spot
                hit.collider.tag = "BuildSiteFull";

                //calls placetower function passing in the position of where the raycast hit
                PlaceTower(hit);
            }
        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    //takes spriterendered and follows the mouse position
    public void FollowMouse()
    {
        //stores sprite position = mouse position
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //changes sprite position to mouse position
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    //turns on spriteRenderer and sets sprite
    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    //turns off spriteRenderer
    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
    }

    //makes towerbutton equal to tower thats being passed in.
    public void SelectedTower(TowerButtons towerSelected)
    {
        towerButtonPressed = towerSelected;
        EnableDragSprite(towerButtonPressed.DragSprite);
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        //checks if mouse is over a button
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //creates new tower based on which button was pressed
            GameObject newTower = Instantiate(towerButtonPressed.TowerObject);

            //places specific tower at point clicked
            newTower.transform.position = hit.transform.position;

            //disables spriteRenderer
            DisableDragSprite();
        }
    }
}
