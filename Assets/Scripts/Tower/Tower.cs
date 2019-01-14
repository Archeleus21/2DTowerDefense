using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackSpeed; //how fast a tower can shoot
    [SerializeField] private float attackRange; //how far a tower can see an enemy
    [SerializeField] private Projectile projectilePrefab;  //selects type of projectile

    private float projectileSpeed = 5f;
    //null until game sets target
    private Enemy targetEnemy = null;
    private float attackCounter;  //used to count time between attacks
    private bool isAttacking = false;

    private void Update()
    {
        attackCounter -= Time.deltaTime;  //subtract counter

        //used to target an enemy
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            //set nearestEnemy to the nearest enemy in range
            Enemy nearestEnemy = GetNearestEnemyInRange();

            //actively updates nearestEnemy
            if (nearestEnemy != null &&
                Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRange)
            {
                targetEnemy = GetNearestEnemyInRange();
            }
        }
        else
        {
            //checks if tower is ready to shoot
            if (attackCounter <= 0)
            {
                //tells tower its ok to shoot
                isAttacking = true;
                //reset counter to stop shooting
                attackCounter = attackSpeed;
            }
            else
            {
                //tells tower that it cant shoot
                isAttacking = false;
            }

            //resets enemy to null when not in range or dead so targetEnemy can update with new target
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRange)
            {
                targetEnemy = null;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(isAttacking)
        {
            AttackTargetEnemy();
        }
    }

    //shoot the enemy
    public void AttackTargetEnemy()
    {
        //runs once and resets to false
        isAttacking = false;

        //creates projectile
        Projectile newProjectile = Instantiate(projectilePrefab) as Projectile;

        //sets initial position for projectile
        newProjectile.transform.position = transform.position;

        //check if target exists
        if (targetEnemy == null)
        {
            //destroys projectile
            Destroy(newProjectile.gameObject);
        }
        else
        {
            //calls coroutine to move projectile
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    //moves projectile
    IEnumerator MoveProjectile(Projectile projectile)
    {
        //loops conditions, using .20f since it is an accurate representation of how close the enemy is to the tower
        while(GetTargetDistance(targetEnemy) > 0.20f && 
              projectile != null && 
              targetEnemy != null)
        {
            //direction of enemy
            Vector2 dir = targetEnemy.transform.localPosition - transform.localPosition;

            //angle of the direction, using tangent which is y over x 
            //then converts from radians to degrees(angles are always in radians and need to be converted to degrees)
            float angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //points the project in proper direction
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);

            //moves the projectile to the enemy
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, projectileSpeed * Time.deltaTime );

            yield return null;
        }

        //destroys projectile if the projectile has already been created or if no target exists
        if(projectile != null || targetEnemy == null)
        {
            Destroy(projectile.gameObject);
        }
    }

    //gets current target distance
    private float GetTargetDistance(Enemy thisEnemy)
    {
        //checks if target exists
        if(thisEnemy == null)
        {
            //sets target to nearest enemy
            thisEnemy = GetNearestEnemyInRange();
            //checks if target exists again
            if(thisEnemy == null)
            {
                //returns 0 if no target exists
                return 0f;
            }
        }

        //returns absolute value of target distance 
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    //traverses List in GameManager to find nearest enemies and loads them in a list
    private List<Enemy> GetEnemiesInRange()
    {
        //creates new list of enemiesInRange
        List<Enemy> enemiesInRange = new List<Enemy>();

        //looks in the list of enemies created by the GameManager
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            //takes towers position subtracts enemy position and checks if within tower range
            //then adds it to the list
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    //traverses GetEnemiesInRange List to find actually nearest enemy
    private Enemy GetNearestEnemyInRange()
    {
        //sets variable to null until loaded by game
        Enemy nearestEnemy = null;

        //sets variable to largest positive number
        float shortestDistance = float.PositiveInfinity;

        //goes through list and checks distances
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            //checks which enemy is shortest distance to tower
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < shortestDistance)
            {
                //sets closest enemy position to shortest distance variable
                shortestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);

                //sets nearest enemy to nearestEnemy variable
                nearestEnemy = enemy;
            }
        }

        //returns which enemy is shortest distance from tower
        return nearestEnemy;
    }
}
