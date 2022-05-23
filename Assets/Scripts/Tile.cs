using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject construction;

    [SerializeField]  private bool constructed;
    private static int instance = 0;
    private int id;

    public bool Constructed
    {
        get { return constructed; }
        set { constructed = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    private void Start()
    {
        id = instance;
        instance++;
        LevelConstructor.OnConstruct += AssignConstruction;
        construction = null;
        constructed = false;
    }

    private void AssignConstruction(GameObject construction, int IDTile)
    {
        if(id == IDTile)
        {
            this.constructed = true;
            if (construction != null)
            {
                this.construction = construction;
            }
            else
            {
                Destroy(this.construction);
                this.construction = null;
                this.constructed = false;
            }
        }
    }
}
