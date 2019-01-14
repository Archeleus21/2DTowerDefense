﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform exitPoint;  //exit
    [SerializeField] private Transform[] checkPoints;  //checkpoints
                     
    [SerializeField] private float navigationUpdate;  //used to update / control time between points
    [SerializeField] private int target = 0;  //keeps track of check points
    [SerializeField] private int healthPoints;  //enemy HP

    private Transform enemy;  //moves enemy
    private Collider2D enemyCollider;  //used to disable collider
    private Animator animator;

    private float navigationTime = 0; //time between checkpoints
    private bool isDead = false;

    //-------------------------------------------
    //Getters
    //-------------------------------------------

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
    //---------------------------------------------
    //---------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();  //gets enemy transform
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
        LoadCheckPointsIntoArray();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemyThroughCheckpoints();
    }

    //stores checkpoints at start of game
    private void LoadCheckPointsIntoArray()
    {
        checkPoints = new Transform[10];

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[0] == null)
            {
                checkPoints[0] = GameObject.Find("CheckPoint").transform;
            }
            else if (checkPoints[0] != null)
            {
                checkPoints[i] = GameObject.Find("CheckPoint (" + i + ")").transform;
            }
        }
    }

    //moves enemy though checkpoints "pathfinding"
    private void MoveEnemyThroughCheckpoints()
    {
        if (checkPoints != null && isDead == false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < checkPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, checkPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //cycles through checkpoints
        if(other.gameObject.tag == "Checkpoint")
        {
            target++;
        }
        //checks if at the last checkpoint
        else if(other.gameObject.tag == "Finish")
        {
            GameManager.Instance.UnregisterEnemy(this);
        }
        //checks if was hit by projectile
        else if(other.gameObject.tag == "Projectile")
        {
            //gives access to attackstrength variable
            Projectile newProjectile = other.gameObject.GetComponent<Projectile>();  
            takeDamage(newProjectile.AttackStrength); //takes damage equal to projectile strength
        }
    }

    //enemy takes damage
    public void takeDamage(int hitPoints)
    {
        if (healthPoints - hitPoints > 0)
        {
            healthPoints -= hitPoints;
            animator.Play("TakeDamage");
        }
        else
        {
            Die();
        }
    }

    //enemy dies
    public void Die()
    {
        animator.SetTrigger("Dying");
        isDead = true;
        enemyCollider.enabled = false;
    }
}
