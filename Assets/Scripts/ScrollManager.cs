using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject bttnPrefab;
    [SerializeField] private string[] levels;

    void ShowLevels()
    {
        string path = Application.persistentDataPath + "/maps";
        levels = Directory.GetFiles(path);
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = levels[i].Replace(path + "\\", "");
            levels[i] = levels[i].Replace(".dat", "");
        }
        Debug.Log(levels[0]);
    }

    private void Update()
    {
        ShowLevels();
    }
}
