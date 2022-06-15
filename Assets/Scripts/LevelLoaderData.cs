using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoaderData : MonoBehaviour
{
    public static LevelLoaderData Instance;
    public MapSave mapSave;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }    
}
