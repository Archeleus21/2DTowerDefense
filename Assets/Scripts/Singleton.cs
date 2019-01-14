using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//where T : MonoBehaviour, is needed so that 
//the deriving class knows it still monobehavior
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour  
{
    private static T instance;

    //------------------------------
    //getter
    //------------------------------
    public static T Instance
    {
        get
        {
            //singleton
            if (instance == null)
            {
                //looks for any class thats T
                instance = FindObjectOfType<T>();  
            }
            else if (instance != FindObjectOfType<T>())
            {
                Destroy(FindObjectOfType<T>());
            }

            //dont destroy this
            DontDestroyOnLoad(FindObjectOfType<T>());

            return instance;
        }
    }
}
