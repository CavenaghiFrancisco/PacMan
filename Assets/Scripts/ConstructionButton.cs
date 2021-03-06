using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionButton : MonoBehaviour
{
    private Image image;
    private bool selected;

    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = Color.blue;
    }

    private void Update()
    {
        if (selected)
        {
            image.color = Color.cyan;
        }
        else
        {
            image.color = Color.blue;
        }
    }
}
