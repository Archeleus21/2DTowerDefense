using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtons : MonoBehaviour
{

    [SerializeField] private GameObject towerObject;
    [SerializeField] private Sprite dragSprite;
    [SerializeField] private int towerPrice;

    //--------------------------------------
    //getters
    //--------------------------------------
    public GameObject TowerObject
    {
        get
        {
            return towerObject;  //returns towerObject
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;  //returns sprite attached to button
        }
    }

    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
