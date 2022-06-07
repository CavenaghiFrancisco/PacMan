using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private LevelButton bttnPrefab;
    [SerializeField] private List<string> levels = new List<string>();
    [SerializeField] private List<string> levelsName = new List<string>();
    [SerializeField] private List<LevelButton> buttons = new List<LevelButton>();
    private string actualPath;
    private int actualID = -1;
    private RectTransform contentRT;

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
                levelsName[i] = levelsName[i].Replace(".dat", "");
            }

            if (oldSize < levels.Count && oldSize == 0)
            {
                for (int k = 0; k < levels.Count; k++)
                {
                    LevelButton lvlButton = Instantiate(bttnPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    lvlButton.Path = levelsPath[k];
                    lvlButton.NameOfMap = levelsName[k];
                    lvlButton.transform.SetParent(content.transform);
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
                    buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 60 * buttons[j].ID, 0);
                }
                contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 60);
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
                    buttons.Add(lvlBttn);
                    int idIncrementer = 0;
                    foreach (LevelButton button in buttons)
                    {
                        button.ID = idIncrementer;
                        idIncrementer++;
                    }
                    for (int j = 0; j < buttons.Count; j++)
                    {
                        buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 60 * buttons[j].ID, 0);
                    }
                    contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 60);
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
            buttons[j].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -10 - 60 * buttons[j].ID, 0);
        }
        contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, 10 + (buttons.Count) * 60);
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

    public void ChooseLevel()
    {

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
