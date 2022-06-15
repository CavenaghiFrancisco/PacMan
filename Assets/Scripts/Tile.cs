using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject construction;
    [SerializeField]  private bool constructed;
    [SerializeField] private int posX;
    [SerializeField] private int posZ;
    [SerializeField] private TypeOfConstruction type;
    private static int instance = 0;
    private int id;
    private bool obligatoryWall;
    public static Action<TypeOfConstruction> OnPlayerButtonEnable;


    public bool Constructed
    {
        get { return constructed; }
        set { constructed = value; }
    }

    public bool ObligatoryWall
    {
        get { return obligatoryWall; }
        set { obligatoryWall = value; }
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int PosX
    {
        get { return posX; }
        set { posX = value; }
    }

    public int PosZ
    {
        get { return posZ; }
        set { posZ = value; }
    }

    public TypeOfConstruction Type
    {
        get { return type; }
        set { type = value; }
    }

    private void Awake()
    {
        LevelConstructor.OnNormalConstruct += AssignConstruction;
        LevelConstructor.OnEmpty += EmptyTile;
        LevelConstructor.OnClear += Clear;
        id = instance;
        instance++;
        construction = null;
        constructed = false;
    }

    private void AssignConstruction(GameObject construction, int x, int z, TypeOfConstruction type)
    {
        if(posX == x && posZ == z && !this.obligatoryWall)
        {
            if (!constructed)
            {
                this.constructed = true;
                if (construction != null)
                {
                    this.construction = construction;
                    this.type = type;
                }                
            }
        }
    }

    private void EmptyTile(int posX, int posZ)
    {
        if(this.posX == posX && this.posZ == posZ && !obligatoryWall)
        {
            if (construction)
            {
                Destroy(construction);
            }
            construction = null;
            constructed = false;
            OnPlayerButtonEnable(type);
            type = TypeOfConstruction.EMPTY;
        }   
    }

    private void Clear()
    {
        if (!obligatoryWall)
        {
            if (this.construction)
            {
                Destroy(construction.gameObject);
            }
            construction = null;
            constructed = false;
            OnPlayerButtonEnable(type);
            type = TypeOfConstruction.EMPTY;
        }
    }



    
}
