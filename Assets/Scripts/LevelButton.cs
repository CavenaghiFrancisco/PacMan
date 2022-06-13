using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private string nameOfMap;
    private Image image;
    private Text textBttn;
    [SerializeField] private int id;
    public static Action<string, int> OnGetLevel;
    private bool selected;

    public string Path
    {
        get { return path; }
        set { path = value; }
    }

    public string NameOfMap
    {
        get { return nameOfMap; }
        set { nameOfMap = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        textBttn = transform.GetChild(0).transform.GetComponent<Text>();
    }

    private void Start()
    {
        textBttn.text = nameOfMap;
    }

    private void Update()
    {
        if(selected)
        image.color = Color.cyan;
        else
        image.color = Color.white;
    }

    public void GetLevel()
    {
        OnGetLevel(path,id);
        selected = true;
    }
}
