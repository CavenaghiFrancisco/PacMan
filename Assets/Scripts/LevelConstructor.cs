using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject prefabBase;
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject prefabBox;
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private Camera cam;
    private Vector3 mousePosition;
    private List<Tile> tiles = new List<Tile>();
    private TypeOfConstruction type = TypeOfConstruction.EMPTY;
    static public Action<GameObject,int,int> OnNormalConstruct;
    static public Action<int,int> OnEmpty;
    static public Action OnClear;
    private LayerMask layerMask = 1 << 6;
    private bool horizontalMirror = false;
    private bool verticalMirror = false;
    private bool noneMirror = true;
    private bool bothMirror = false;

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
        ButtonsManager.OnHorizontalMirrorEnable += SetHorizontalMirror;
        ButtonsManager.OnVerticalMirrorEnable += SetVerticalMirror;
        ButtonsManager.OnMirrorDisable += SetNoneMirror;
        ButtonsManager.OnBothMirrorEnable += SetBothMirror;
        for (int i = 0; i < sizeZ; i++)
        {
            for(int j = 0; j < sizeX; j++)
            {
                Tile tile = Instantiate(prefabBase, new Vector3(prefabBase.transform.localScale.x * j, -prefabBase.transform.localScale.y / 2, prefabBase.transform.localScale.z * i), Quaternion.identity).GetComponent<Tile>();
                tiles.Add(tile);
                tile.name = j + " " + i;
                tile.PosX = j;
                tile.PosZ = i;
                if (i == 0 || j == 0 || i == sizeZ-1 || j == sizeX-1)
                {
                    tile.ObligatoryWall = true;
                }
            }
            
        }
        cam.transform.position = new Vector3(sizeX / 2, sizeZ > sizeX ? sizeZ : sizeX, sizeZ / 2);
        foreach(Tile tile in tiles)
        {
            if (tile.ObligatoryWall)
            {
                GameObject construction = Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                OnNormalConstruct?.Invoke(construction,tile.PosX,tile.PosZ);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit, 100,layerMask))
        {
            foreach (Tile tile in tiles)
            {
                if (hit.transform.gameObject == tile.gameObject)
                {
                    Construct(tile);
                }
                
            }
        }           
    }

    private void Construct(Tile tile)
    {
        if (Input.GetMouseButton(0) )
        {
            GameObject construction = null;
            switch (type)
            {
                case TypeOfConstruction.EMPTY:
                    OnEmpty?.Invoke(tile.PosX, tile.PosZ);
                    if (horizontalMirror || bothMirror)
                    {
                        OnEmpty?.Invoke(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ);
                    }
                    if (verticalMirror || bothMirror)
                    {
                        OnEmpty?.Invoke(tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ);
                    }
                    if (bothMirror)
                    {
                        OnEmpty?.Invoke(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ);
                    }
                    break;
                case TypeOfConstruction.WALL:
                    if (!tile.Constructed && !tile.ObligatoryWall)
                    {
                        construction = Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.BOX:
                    if (!tile.Constructed && !tile.ObligatoryWall)
                    {
                        construction = Instantiate(prefabBox, new Vector3(tile.transform.position.x, prefabBox.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
            }
            if(construction != null)
            {
                OnNormalConstruct?.Invoke(construction, tile.PosX, tile.PosZ);
                if (horizontalMirror || bothMirror)
                {
                    GameObject constructionHorMirror = null;
                    if (tile.PosX != (tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX) && construction && !GetTile(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX,tile.PosZ).Constructed)
                    {
                        constructionHorMirror = Instantiate(construction, new Vector3(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, construction.transform.localScale.y / 2, tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionHorMirror, tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ);
                    }
                    
                }
                if (verticalMirror || bothMirror)
                {
                    GameObject constructionVerMirror = null;
                    if (tile.PosZ != (tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ) && construction && !GetTile(tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ).Constructed)
                    {
                        constructionVerMirror = Instantiate(construction, new Vector3(tile.PosX, construction.transform.localScale.y / 2, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionVerMirror, tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ);
                    }

                }
                if (bothMirror)
                {
                    GameObject constructionVerMirror = null;
                    if (tile.PosZ != (tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ) && tile.PosX != (tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX) && construction && !GetTile(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ).Constructed)
                    {
                        constructionVerMirror = Instantiate(construction, new Vector3(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, construction.transform.localScale.y / 2, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionVerMirror, tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ);
                    }
                }
            }
            
            


        }
    }

    public void Clear()
    {
        foreach(Tile tile in tiles)
        {
            OnClear?.Invoke();
        }
    }
    
    private void SetVerticalMirror()
    {
        verticalMirror = !verticalMirror;
        noneMirror = false;
        bothMirror = false;
    }

    private void SetHorizontalMirror()
    {
        horizontalMirror = !horizontalMirror;
        noneMirror = false;
        bothMirror = false;
    }

    private void SetNoneMirror()
    {
        bothMirror = false;
        verticalMirror = false;
        horizontalMirror = false;
        noneMirror = true;
    }

    private void SetBothMirror()
    {
        bothMirror = true;
        verticalMirror = false;
        horizontalMirror = false;
        noneMirror = true;
    }

    private Tile GetTile(int posX, int posZ)
    {
        foreach(Tile tile in tiles)
        {
            if(tile.PosX == posX && tile.PosZ == posZ)
            {
                return tile;
            }
        }
        return null;
    }
}
