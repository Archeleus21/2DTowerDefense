using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> 
{
    [SerializeField] private AudioClip rockSFX;
    [SerializeField] private AudioClip arrowSFX;
    [SerializeField] private AudioClip fireballSFX;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip levelSFX;
    [SerializeField] private AudioClip towerBuiltSFX;
    [SerializeField] private AudioClip newGameSFX;
    [SerializeField] private AudioClip gameOverSFX;


    public AudioClip RockSFX
    {
        get
        {
            return rockSFX;
        }
    }

    public AudioClip ArrowSFX
    {
        get
        {
            return arrowSFX;
        }
    }

    public AudioClip FireballSFX
    {
        get
        {
            return fireballSFX;
        }
    }

    public AudioClip HitSFX
    {
        get
        {
            return hitSFX;
        }
    }

    public AudioClip DeathSFX
    {
        get
        {
            return deathSFX;
        }
    }

    public AudioClip LevelSFX
    {
        get
        {
            return levelSFX;
        }
    }

    public AudioClip TowerBuiltSFX
    {
        get
        {
            return towerBuiltSFX;
        }
    }

    public AudioClip NewGameSFX
    {
        get
        {
            return newGameSFX;
        }
    }

    public AudioClip GameOverSFX
    {
        get
        {
            return gameOverSFX;
        }
    }
}
