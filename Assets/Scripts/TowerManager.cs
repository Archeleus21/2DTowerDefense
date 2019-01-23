using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private SpriteRenderer spriteRenderer;

    public TowerButtons towerButtonPressed { get; set; }  //getter and setter default

    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();

    private Collider2D buildTile;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();

        spriteRenderer.enabled = false; //disables sprite renderer
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
                buildTile = hit.collider;

                //changes tag to prevent multple towers on one spot
                hit.collider.tag = "BuildSiteFull";

                //adds buildtile/buildsite to list
                RegisterBuildSite(buildTile);

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

    //adds buildTag to list to keep track of which sites are used
    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    //adds tower to list to keep track of how many towers are built
    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    //clears list and resets tags so we can build on them again
    public void RenameBuildSiteTags()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildSite";
        }
        BuildList.Clear();
    }

    //makes towerbutton equal to tower thats being passed in.
    public void SelectedTower(TowerButtons towerSelected)
    {
        if (towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerButtonPressed = towerSelected;
            EnableDragSprite(towerButtonPressed.DragSprite);
        }
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        //checks if mouse is over a button
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //creates new tower based on which button was pressed
            Tower newTower = Instantiate(towerButtonPressed.TowerObject);

            //places specific tower at point clicked
            newTower.transform.position = hit.transform.position;

            //subtracts price of tower from player money
            BuyTower(towerButtonPressed.TowerPrice);

            //adds tower to the list
            RegisterTower(newTower);

            //disables spriteRenderer
            DisableDragSprite();
        }
    }

    //destroys towers after game restart and clears list so new towers can be built
    public void DestroyAllTowers()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    //subtracts money from player
    public void BuyTower(int price)
    {
        GameManager.Instance.SubtractMoney(price);
    }
}
