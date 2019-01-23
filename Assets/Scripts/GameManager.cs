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
    [SerializeField] private int totalEnemies = 3;  //total enemies
    [SerializeField] private int enemiesPerSpawn;  //amount of enemies per spawn

    [SerializeField] private Text moneyTextLabel;  //displays accumulated money
    [SerializeField] private Text waveTextLabel;  //displays current wave
    [SerializeField] private Text escapedEnemiesTextLabel;  //displays escaped enemies
    [SerializeField] private Text playButtonTextLabel;  //displays play text
    [SerializeField] private Button playButton;  //displays play button
    [SerializeField] private int totalWaves = 10;  //total waves in game

    private int waveNumber = 0;  //stores number of current wave
    private int totalMoney = 10;  //stores total player money
    private int totalEscapedEnemies = 0;  //stores number of escaped enemies
    private int escapedEnemiesPerRound = 0;  //stores escaped enemies per round
    private int totalEnemiesKilled = 0;  //stores total enemies killed
    private int EnemyToSpawn = 0;  //selects which enemy to spawn

    private GameStatus currentGameState = GameStatus.Play;  //handles current game state
    private AudioSource audioSource;

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
            moneyTextLabel.text = totalMoney.ToString();
        }
    }

    public int TotalEscapedEnemies
    {
        get
        {
            return totalEscapedEnemies;
        }
        set
        {
            totalEscapedEnemies = value;
        }
    }

    public int EscapedEnemiesPerRound
    {
        get
        {
            return escapedEnemiesPerRound;
        }
        set
        {
            escapedEnemiesPerRound = value;
        }
    }

    public int TotalEnemiesKilled
    {
        get
        {
            return totalEnemiesKilled;
        }
        set
        {
            totalEnemiesKilled = value;
        }
    }

    public AudioSource GameAudioSource
    {
        get
        {
            return audioSource;
        }
    }
    //--------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        playButton.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
    }

    private void Update()
    {
        DeselectTower();
    }

    //spawns enemy on delay
    IEnumerator SpawnEnemiesOnDelay()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    GameObject newEnemy = Instantiate(enemies[0], spawnPoint.transform.position, Quaternion.identity);
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

    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()
    {
        escapedEnemiesTextLabel.text = "Escaped " + TotalEscapedEnemies + "/10";

        if(EscapedEnemiesPerRound + TotalEnemiesKilled == totalEnemies)
        {
            SetCurrentGameState();
            ShowMenu();
        }
    }

    //sets game state based on certain criteria
    private void SetCurrentGameState()
    {
        if(TotalEscapedEnemies >= 10)  //checks if escaped limit is reached
        {
            currentGameState = GameStatus.GameOver;
        }
        else if(waveNumber == 0 && TotalEnemiesKilled + EscapedEnemiesPerRound == 0)  //starting new game
        {
            currentGameState = GameStatus.Play;
        }
        else if(waveNumber >= totalWaves)  //completed all rounds
        {
            currentGameState = GameStatus.Win;
        }
        else
        {
            currentGameState = GameStatus.Next;  //if no criteria above is met then player is still playing, go to next round
        }
    }

    //shows UI 
    private void ShowMenu()
    {
        switch(currentGameState)
        {
            case GameStatus.GameOver:
                playButtonTextLabel.text = "play Again!";
                //add gameover sounds
                break;
            case GameStatus.Next:
                playButtonTextLabel.text = "Next Wave!";
                break;
            case GameStatus.Play:
                playButtonTextLabel.text = "Play!";
                break;
            case GameStatus.Win:
                playButtonTextLabel.text = "Play!";
                break;
        }

        playButton.gameObject.SetActive(true);
    }

    //starts the game 
    public void PlayButtonPressed()
    {
        switch (currentGameState)
        {
            //if game is ready for next wave
            case GameStatus.Next:  
                waveNumber += 1;  //adds to the wave number
                totalEnemies += waveNumber;  //adds to the enemies per wave number
                break;
            //default setting when game starts
            default:
                audioSource.PlayOneShot(SoundManager.Instance.NewGameSFX);
                totalEnemies = 3;  //enemy start count
                TotalEscapedEnemies = 0;  //starting live or amount of enemies lost
                TotalMoney = 10;  //starting money
                TowerManager.Instance.DestroyAllTowers();  //clears board of all towers
                TowerManager.Instance.RenameBuildSiteTags();  //resets all build tiles so player can build on them
                moneyTextLabel.text = TotalMoney.ToString();  //displays money
                escapedEnemiesTextLabel.text = "Escaped " + TotalEscapedEnemies + " /10";  //displays label ui text
                break;
        }

        //resets all values
        DestroyAllEnemies();
        TotalEnemiesKilled = 0;
        EscapedEnemiesPerRound = 0;
        waveTextLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(SpawnEnemiesOnDelay());
        playButton.gameObject.SetActive(false);
    }

    //lets player deselect tower without costs
    private void DeselectTower()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDragSprite();  //stop tower from being on mouse pointer
            TowerManager.Instance.towerButtonPressed = null;
        }
    }
}
