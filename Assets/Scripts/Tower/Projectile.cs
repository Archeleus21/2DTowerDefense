using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum, declared at top to be accessed by anything in project
//an enum is a set of constant values starts defaults as integers starting from 0
// enum "Type Name(whatever you want)" {"name of values"(they will equal 0,1,2...}; ends with semicolon
public enum ProType
{
    Rock, Arrow, Fireball
};

public class Projectile : MonoBehaviour
{
    [SerializeField] private int attackStrength;
    [SerializeField] private ProType projectileType;


    //----------------------------------------------
    //getters
    //----------------------------------------------
    public int AttackStrength
    {
        get
        {
            return attackStrength;
        }
    }

    public ProType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }

    //-----------------------------------------------------
    //colliders
    //-----------------------------------------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

}
