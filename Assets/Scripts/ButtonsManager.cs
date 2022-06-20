using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private List<ConstructionButton> buttons;
    [SerializeField] private ConstructionButton noneMirror;
    [SerializeField] private ConstructionButton verticalMirror;
    [SerializeField] private ConstructionButton horizontalMirror;
    [SerializeField] private ConstructionButton bothMirror;
    [SerializeField] private Button saveBttn;
    [SerializeField] private Button playerBttn;
    [SerializeField] private Button inkyBttn;
    [SerializeField] private Button pinkyBttn;
    [SerializeField] private Button blinkyBttn;
    [SerializeField] private Button clydeBttn;
    static public Action OnVerticalMirrorEnable;
    static public Action OnHorizontalMirrorEnable;
    static public Action OnMirrorDisable;
    static public Action OnBothMirrorEnable;
    static public Action OnSave;

    private void Start()
    {
        saveBttn.interactable = false;
        LevelConstructor.OnPlayerFound += DisableButton;
        LevelConstructor.OnPlayableMap += EnableSaveBttn;
        LevelConstructor.OnBlinkyFound += DisableButton;
        LevelConstructor.OnInkyFound += DisableButton;
        LevelConstructor.OnClydeFound += DisableButton;
        LevelConstructor.OnPinkyFound += DisableButton;
        Tile.OnPlayerButtonEnable += EnableButton;
        Tile.OnPlayerButtonEnable += DisableSaveBttn;

        noneMirror.Selected = true;
        OnMirrorDisable?.Invoke();
    }

    private void OnDestroy()
    {
        LevelConstructor.OnPlayerFound -= DisableButton;
        LevelConstructor.OnPlayableMap -= EnableSaveBttn;
        LevelConstructor.OnBlinkyFound -= DisableButton;
        LevelConstructor.OnInkyFound -= DisableButton;
        LevelConstructor.OnClydeFound -= DisableButton;
        LevelConstructor.OnPinkyFound -= DisableButton;
        Tile.OnPlayerButtonEnable -= EnableButton;
        Tile.OnPlayerButtonEnable -= DisableSaveBttn;
    }

    private void Update()
    {
        if(!verticalMirror.Selected && !horizontalMirror.Selected && !bothMirror.Selected)
        {
            noneMirror.Selected = true;
        }
        GameObject bttn = EventSystem.current.currentSelectedGameObject;
        if (bttn == pinkyBttn.gameObject || bttn == inkyBttn.gameObject || bttn == blinkyBttn.gameObject || bttn == clydeBttn.gameObject || bttn == playerBttn.gameObject)
        {
            foreach(ConstructionButton button in buttons)
            {
                button.Selected = false;
            }
        }
    }

    public void HighlightButton(ConstructionButton button)
    {
        foreach(ConstructionButton bttn in buttons)
        {
            if(bttn.gameObject == button.gameObject)
            {
                bttn.Selected = true;
            }
            else
            {
                bttn.Selected = false;
            }
        }
    }

    public void ToggleVerticalMirror()
    {
        verticalMirror.Selected = !verticalMirror.Selected;
        noneMirror.Selected = false;
        bothMirror.Selected = false;
        OnVerticalMirrorEnable?.Invoke();
    }

    public void ToggleHorizontalMirror()
    {
        horizontalMirror.Selected = !horizontalMirror.Selected;
        noneMirror.Selected = false;
        bothMirror.Selected = false;
        OnHorizontalMirrorEnable?.Invoke();
    }

    public void DisableMirror()
    {
        verticalMirror.Selected = false;
        horizontalMirror.Selected = false;
        bothMirror.Selected = false;
        noneMirror.Selected = true;
        OnMirrorDisable?.Invoke();
    }

    public void ToggleBothMirror()
    {
        verticalMirror.Selected = false;
        horizontalMirror.Selected = false;
        bothMirror.Selected = !bothMirror.Selected;
        noneMirror.Selected = false;
       OnBothMirrorEnable?.Invoke();
    }

    private void EnableSaveBttn()
    {
        saveBttn.interactable = true;
    }

    private void DisableSaveBttn(TypeOfConstruction type)
    {
        saveBttn.interactable = false;
    }

    public void Save()
    {
        OnSave();
    }

    public void DisableButton(TypeOfConstruction type)
    {
        switch (type)
        {
            case TypeOfConstruction.PLAYER:
                playerBttn.interactable = false;
                break;
            case TypeOfConstruction.INKY:
                inkyBttn.interactable = false;
                break;
            case TypeOfConstruction.BLINKY:
                blinkyBttn.interactable = false;
                break;
            case TypeOfConstruction.PINKY:
                pinkyBttn.interactable = false;
                break;
            case TypeOfConstruction.CLYDE:
                clydeBttn.interactable = false;
                break;
        }
    }

    public void EnableButton(TypeOfConstruction type)
    {
        switch (type)
        {
            case TypeOfConstruction.PLAYER:
                playerBttn.interactable = true;
                break;
            case TypeOfConstruction.INKY:
                inkyBttn.interactable = true;
                break;
            case TypeOfConstruction.BLINKY:
                blinkyBttn.interactable = true;
                break;
            case TypeOfConstruction.PINKY:
                pinkyBttn.interactable = true;
                break;
            case TypeOfConstruction.CLYDE:
                clydeBttn.interactable = true;
                break;
        }
    }

}


