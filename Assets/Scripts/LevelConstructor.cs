using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject prefabBase;
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject prefabBox;
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private Camera cam;
    [SerializeField] private TMPro.TMP_InputField inputField;
    private List<Vector3> walls = new List<Vector3>();
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> players = new List<Vector3>();
    private List<Vector3> ghosts = new List<Vector3>();
    private Vector3 mousePosition;
    private List<Tile> tiles = new List<Tile>();
    private TypeOfConstruction type = TypeOfConstruction.EMPTY;
    static public Action<GameObject, int, int, TypeOfConstruction> OnNormalConstruct;
    static public Action<int, int> OnEmpty;
    static public Action OnClear;
    private LayerMask layerMask = 1 << 6;
    private bool horizontalMirror = false;
    private bool verticalMirror = false;
    private bool noneMirror = true;
    private bool bothMirror = false;
    

    public void ChangeTypeOfConstruction(int value)
    {
        type = (TypeOfConstruction)value;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        ButtonsManager.OnHorizontalMirrorEnable += SetHorizontalMirror;
        ButtonsManager.OnVerticalMirrorEnable += SetVerticalMirror;
        ButtonsManager.OnMirrorDisable += SetNoneMirror;
        ButtonsManager.OnBothMirrorEnable += SetBothMirror;
        ButtonsManager.OnSave += SaveMap;
        for (int i = 0; i < sizeZ; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Tile tile = Instantiate(prefabBase, new Vector3(prefabBase.transform.localScale.x * j, -prefabBase.transform.localScale.y / 2, prefabBase.transform.localScale.z * i), Quaternion.identity).GetComponent<Tile>();
                tiles.Add(tile);
                tile.Type = TypeOfConstruction.EMPTY;
                tile.name = j + " " + i;
                tile.PosX = j;
                tile.PosZ = i;
                if (i == 0 || j == 0 || i == sizeZ - 1 || j == sizeX - 1)
                {
                    tile.ObligatoryWall = true;
                }
            }

        }
        cam.transform.position = new Vector3(sizeX % 2 == 0 ? sizeX / 2 - 0.5f : sizeX / 2, sizeZ > sizeX ? sizeZ + 1 : sizeX + 1, sizeZ % 2 == 0 ? sizeZ / 2 - 0.5f : sizeZ / 2);
        foreach (Tile tile in tiles)
        {
            if (tile.ObligatoryWall)
            {
                tile.Type = TypeOfConstruction.WALL;
                GameObject construction = Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
        {
            foreach (Tile tile in tiles)
            {
                if (hit.transform.gameObject == tile.gameObject)
                {
                    Construct(tile);
                }

            }
        }
        walls.Clear();
        players.Clear();
        ghosts.Clear();
        points.Clear();
        foreach(Tile tile in tiles)
        {
            switch (tile.Type)
            {
                case TypeOfConstruction.WALL:
                    walls.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y,(int)tile.transform.position.z));
                    break;
                case TypeOfConstruction.PLAYER:
                    players.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    break;
                case TypeOfConstruction.GHOST:
                    ghosts.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    break;
                case TypeOfConstruction.POINT:
                    points.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    break;
            }
        }
    }

    private void Construct(Tile tile)
    {
        if (Input.GetMouseButton(0))
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
                        type = TypeOfConstruction.WALL;
                        construction = Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.POINT:
                    if (!tile.Constructed && !tile.ObligatoryWall)
                    {
                        type = TypeOfConstruction.POINT;
                        construction = Instantiate(prefabBox, new Vector3(tile.transform.position.x, prefabBox.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
            }
            if (construction != null)
            {
                OnNormalConstruct?.Invoke(construction, tile.PosX, tile.PosZ,type);
                if (horizontalMirror || bothMirror)
                {
                    GameObject constructionHorMirror = null;
                    if (tile.PosX != (tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX) && construction && !GetTile(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ).Constructed)
                    {
                        constructionHorMirror = Instantiate(construction, new Vector3(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, construction.transform.localScale.y / 2, tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionHorMirror, tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ, type);
                    }

                }
                if (verticalMirror || bothMirror)
                {
                    GameObject constructionVerMirror = null;
                    if (tile.PosZ != (tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ) && construction && !GetTile(tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ).Constructed)
                    {
                        constructionVerMirror = Instantiate(construction, new Vector3(tile.PosX, construction.transform.localScale.y / 2, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionVerMirror, tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ, type);
                    }

                }
                if (bothMirror)
                {
                    GameObject constructionVerMirror = null;
                    if (tile.PosZ != (tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ) && tile.PosX != (tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX) && construction && !GetTile(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ).Constructed)
                    {
                        constructionVerMirror = Instantiate(construction, new Vector3(tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, construction.transform.localScale.y / 2, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ), Quaternion.identity);
                        OnNormalConstruct?.Invoke(constructionVerMirror, tile.PosX < sizeX ? sizeX - 1 - tile.PosX : sizeX - tile.PosX, tile.PosZ < sizeZ ? sizeZ - 1 - tile.PosZ : sizeZ - tile.PosZ, type);
                    }
                }
            }




        }
    }

    public void Clear()
    {
        foreach (Tile tile in tiles)
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
        bothMirror = !bothMirror;
        verticalMirror = false;
        horizontalMirror = false;
        noneMirror = true;
    }

    private Tile GetTile(int posX, int posZ)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.PosX == posX && tile.PosZ == posZ)
            {
                return tile;
            }
        }
        return null;
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private void SaveMap()
    {
        MapSave mapSave = new MapSave
        {
            ghostPositions = ghosts,
            playerPositions = players,
            wallPositions = walls,
            pointPositions = points,
        };
        string mapJson = JsonUtility.ToJson(mapSave,true);
        string mapName = inputField.text;
        string mapJsonEncoded = Base64Encode(mapJson);
        string path = Application.persistentDataPath + "/maps";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllText(path + "/" + mapName + ".dat", mapJsonEncoded);
    }

    private void LoadMap()
    {
        
    }

    private class MapSave
    {
        public List<Vector3> wallPositions = new List<Vector3>();
        public List<Vector3> pointPositions = new List<Vector3>();
        public List<Vector3> playerPositions = new List<Vector3>();
        public List<Vector3> ghostPositions = new List<Vector3>();
    }
}


