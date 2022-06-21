using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private LevelButton bttnPrefab;
    [SerializeField] private List<string> levels = new List<string>();
    [SerializeField] private List<string> levelsName = new List<string>();
    [SerializeField] private List<LevelButton> buttons = new List<LevelButton>();
    [SerializeField] private Button loadButton;
    private string actualPath;
    private int actualID = -1;
    private RectTransform contentRT;
    public static Action<string> OnLoadingLevel;

    private void Awake()
    {
        contentRT = content.GetComponent<RectTransform>();
        string path = Application.persistentDataPath + "/maps";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        ShowLevels();
    }

    private void Start()
    {
        LevelConstructor.OnSaveMap += ShowLevels;
        LevelButton.OnGetLevel += SelectLevel;
    }

    private void OnDestroy()
    {
        LevelConstructor.OnSaveMap -= ShowLevels;
        LevelButton.OnGetLevel -= SelectLevel;
    }

    private void Update()
    {
        loadButton.interactable = false;
        loadButton.image.color = Color.gray;
        foreach (LevelButton button in buttons)
        {
            if (button.Selected)
            {
                loadButton.interactable = true;
                loadButton.image.color = Color.white;
                break;
            }
            else
            {
                loadButton.interactable = false;
            }
        }
    }

    void ShowLevels()
    {
        int oldSize = levels.Count;
        levels.Clear();
        levelsName.Clear();
        string path = Application.persistentDataPath + "/maps";
        levels = Directory.GetFiles(path).OrderByDescending(d => new FileInfo(d).LastWriteTime).ToList();
        if (levels.Count != 0)
        {
            levelsName = levels;
            List<string> levelsPath = new List<string>();
            for (int i = 0; i < levels.Count; i++)
            {
                levelsPath.Add(levels[i]);
                levelsName[i] = levels[i].Replace(path + "\\", "");
                levelsName[i] = levelsName[i].Replace(".txt", "");
            }

            if (oldSize < levels.Count && oldSize == 0)
            {
                for (int k = 0; k < levels.Count; k++)
                {
                    LevelButton lvlButton = Instantiate(bttnPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    lvlButton.Path = levelsPath[k];
                    lvlButton.NameOfMap = levelsName[k];
                    lvlButton.transform.SetParent(content.transform);
                    lvlButton.transform.localScale = new Vector3(1, 1, 1);
                    buttons.Add(lvlButton);
                }
                int IDIncrementer = 0;
                foreach (LevelButton button in buttons)
                {
                    button.ID = IDIncrementer;
                    IDIncrementer++;
                }
                for (int j = 0; j < buttons.Count; j++)
                {
                    buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 70 * buttons[j].ID, 0);
                }
                contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 70);
            }
            else
            {
                int repeated = 0;
                foreach (LevelButton button in buttons)
                {

                    if (button.NameOfMap == levelsName[0])
                    {
                        repeated++;
                    }
                }
                if (repeated == 0)
                {
                    LevelButton lvlBttn = Instantiate(bttnPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    lvlBttn.Path = levelsPath[0];
                    lvlBttn.NameOfMap = levelsName[0];
                    lvlBttn.transform.SetParent(content.transform);
                    lvlBttn.transform.localScale = new Vector3(1, 1, 1);
                    buttons.Add(lvlBttn);
                    int idIncrementer = 0;
                    foreach (LevelButton button in buttons)
                    {
                        button.ID = idIncrementer;
                        idIncrementer++;
                    }
                    for (int j = 0; j < buttons.Count; j++)
                    {
                        buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 70 * buttons[j].ID, 0);
                    }
                    contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 70);
                }
            }
            
        }
        
    }

    private void OrganizeLevels()
    {
        int IDIncrementer = 0;
        foreach (LevelButton button in buttons)
        {
            button.ID = IDIncrementer;
            IDIncrementer++;
        }
        for (int j = 0; j < buttons.Count; j++)
        {
            buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 70 * buttons[j].ID, 0);
        }
        contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 70);
    }

    private void SelectLevel(string path, int id)
    {
        actualPath = path;
        actualID = id;
        foreach(LevelButton button in buttons)
        {
            if(button.ID != id)
            button.Selected = false;
        }
    }

    public void LoadLevel()
    {
        OnLoadingLevel(actualPath);
    }

    public void DeleteLevel()
    {
        
        if(actualID >= 0)
        {
            LevelButton auxBttn = null;
            GameObject auxBttnObject = null;
            foreach (LevelButton button in buttons)
            {
                if (button.ID == actualID)
                {
                    auxBttn = button;
                    auxBttnObject = button.gameObject;
                }
            }
            File.Delete(actualPath);
            Destroy(auxBttnObject);
            buttons.Remove(auxBttn);
            OrganizeLevels();
            actualID = -1;
            actualPath = "";
        }
        
    }

    
}
