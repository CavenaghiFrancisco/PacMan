using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private List<ConstructionButton> buttons;
    [SerializeField] private ConstructionButton noneMirror;
    [SerializeField] private ConstructionButton verticalMirror;
    [SerializeField] private ConstructionButton horizontalMirror;
    [SerializeField] private ConstructionButton bothMirror;
    static public Action OnVerticalMirrorEnable;
    static public Action OnHorizontalMirrorEnable;
    static public Action OnMirrorDisable;
    static public Action OnBothMirrorEnable;

    private void Start()
    {
        noneMirror.Selected = true;
        OnMirrorDisable?.Invoke();
    }

    private void Update()
    {
        if(!verticalMirror.Selected && !horizontalMirror.Selected && !bothMirror.Selected)
        {
            noneMirror.Selected = true;
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
}


