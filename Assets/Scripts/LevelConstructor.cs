using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject prefabBase;
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject prefabBox;
    [SerializeField] private GameObject Cube;
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private Camera cam;
    private Vector3 mousePosition;
    private List<Tile> tiles = new List<Tile>();
    private TypeOfConstruction type = TypeOfConstruction.EMPTY;
    static public Action<GameObject,int> OnConstruct;
    private LayerMask layerMask = 1 << 6;

    public enum TypeOfConstruction
    {
        WALL,
        BOX,
        EMPTY,
    }

    public void ChangeTypeOfConstruction(int value)
    {
        type = (TypeOfConstruction)value;
    }

    // Start is called before the first frame update
    void Start()
    {
        sizeX = 10;
        sizeZ = 10;
        for(int i = 0; i < sizeX; i++)
        {
            for(int j = 0; j < sizeZ; j++)
            {
                Tile tile = Instantiate(prefabBase, new Vector3(prefabBase.transform.localScale.x * i, -prefabBase.transform.localScale.y / 2, prefabBase.transform.localScale.z * j), Quaternion.identity).GetComponent<Tile>();
                tiles.Add(tile);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        foreach (Tile tile in tiles)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit, 100,layerMask))
        {
            foreach (Tile tile in tiles)
            {
                if (hit.transform.gameObject == tile.gameObject)
                {
                    tile.GetComponent<MeshRenderer>().material.color = Color.red;
                    Construct(tile);
                }
                
            }
        }           
    }

    private void Construct(Tile tile)
    {
        if (Input.GetMouseButtonDown(0) )
        {
            GameObject construction = null;
            switch (type)
            {
                case TypeOfConstruction.EMPTY:
                    construction = null;
                    OnConstruct(construction, tile.ID);
                    break;
                case TypeOfConstruction.WALL:
                    if (!tile.Constructed)
                    {
                        construction = Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                        OnConstruct(construction, tile.ID);
                    }
                    break;
                case TypeOfConstruction.BOX:
                    if (!tile.Constructed)
                    {
                        construction = Instantiate(prefabBox, new Vector3(tile.transform.position.x, prefabBox.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    OnConstruct(construction, tile.ID);
                    }
                    break;
            }
        }
    }
    
}
