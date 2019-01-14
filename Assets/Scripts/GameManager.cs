using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum GameStatus
{
    Next, Play, GameOver, Win
};

public class GameManager : Singleton<GameManager> //type is Game Manager
{
    [SerializeField] private GameObject spawnPoint;  //where enemies spawn
    [SerializeField] private GameObject[] enemies;  //holds enemies
    [SerializeField] private int maxEnemiesOnScreen;  //max enemies allowed
    [SerializeField] private int totalEnemies;  //total enemies
    [SerializeField] private int enemiesPerSpawn;  //amount of enemies per spawn

    [SerializeField] private Text moneyText;  //displays accumulated money
    [SerializeField] private Text waveText;  //displays current wave
    [SerializeField] private Text escapedEnemiesText;  //displays escaped enemies
    [SerializeField] private Text playButtonText;  //displays play text
    [SerializeField] private Button playButton;  //displays play button
    [SerializeField] private int totalWaves = 10;  //total waves in game

    private int waveNumber = 0;  //stores number of current wave
    private int totalMoney = 10;  //stores total player money
    private int totalEscapedEnemies = 0;  //stores number of escaped enemies
    private int escapedEnemiesPerRound = 0;  //stores escaped enemies per round
    private int totalEnemiesKilled = 0;  //stores total enemies killed
    private int EnemyToSpawn = 0;  //selects which enemy to spawn

    private GameStatus currentGameState = GameStatus.Play;  //handles current game state

    const float spawnDelay = 1f;  //delay between enemies

    //list to store enemies that are spawned
    public List<Enemy> EnemyList = new List<Enemy>();

    //--------------------------------------
    //getters/setters
    //--------------------------------------
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            moneyText.text = totalMoney.ToString();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemiesOnDelay());
    }

    //spawns enemy on delay
    IEnumerator SpawnEnemiesOnDelay()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[1], spawnPoint.transform.position, Quaternion.identity);
                }
            }
        }

        yield return new WaitForSeconds(spawnDelay);
        StartCoroutine(SpawnEnemiesOnDelay());
    }

    //adds enemy to list to find the closest enemy
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    //removes enemy from list on death then destroys the gameobject
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    //removes remaining enemies from list at end of round
    public void DestroyAllEnemies()
    {
        //destroys all enemies
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        //clears enemy list
        EnemyList.Clear();
    }

    //add money
    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }
}
