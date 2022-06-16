using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject prefabBase;
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject prefabPoint;
    [SerializeField] private GameObject prefabPill;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabInky;
    [SerializeField] private GameObject prefabPinky;
    [SerializeField] private GameObject prefabBlinky;
    [SerializeField] private GameObject prefabClyde;
    [SerializeField] private GameObject overrideBox;
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private Camera cam;
    [SerializeField] private TMPro.TMP_InputField inputField;
    private List<Vector3> walls = new List<Vector3>();
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> players = new List<Vector3>();
    private List<Vector3> pills = new List<Vector3>();
    private Vector3 pacmanPositionInMap;
    private Vector3 blinkyPositionInMap;
    private Vector3 inkyPositionInMap;
    private Vector3 pinkyPositionInMap;
    private Vector3 clydePositionInMap;
    private Vector3 mousePositionInMap;
    private List<Tile> tiles = new List<Tile>();
    private TypeOfConstruction type = TypeOfConstruction.EMPTY;
    static public Action<GameObject, int, int, TypeOfConstruction> OnNormalConstruct;
    static public Action<int, int> OnEmpty;
    static public Action OnClear;
    static public Action OnSaveMap;
    static public Action OnPlayableMap;
    static public Action<TypeOfConstruction> OnPlayerFound;
    static public Action<TypeOfConstruction> OnInkyFound;
    static public Action<TypeOfConstruction> OnBlinkyFound;
    static public Action<TypeOfConstruction> OnPinkyFound;
    static public Action<TypeOfConstruction> OnClydeFound;
    private LayerMask layerMask = 1 << 6;
    private bool horizontalMirror = false;
    private bool verticalMirror = false;
    private bool noneMirror = true;
    private bool bothMirror = false;
    private bool playerSpawned;
    private bool pinkySpawned;
    private bool blinkySpawned;
    private bool inkySpawned;
    private bool clydeSpawned;
    private bool saving;
    private string mapJson;
    private string mapName;
    private string mapJsonEncoded;
    private string path;
    [SerializeField] private TMPro.TMP_InputField verticalSize;
    [SerializeField] private TMPro.TMP_InputField horizontalSize;
    private int horizontalSizeMinLimit = 15;
    private int verticalSizeMinLimit = 15;
    private int horizontalSizeMaxLimit =30;
    private int verticalSizeMaxLimit = 30;
    [SerializeField] private Button levelCreatorBttn;
    [SerializeField] private GameObject creatorPanel;
    [SerializeField] private GameObject editorPanel;
    private List<int> tilePositionXInMap = new List<int>();
    private List<int> tilePositionZInMap = new List<int>();
    private List<int> tileTypeInMap = new List<int>();


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
        ButtonsManager.OnSave += SaveMap;
        ScrollManager.OnLoadingLevel += LoadMap;
        levelCreatorBttn.interactable = false;
    }

    public void CheckMapSize()
    {
        string verSizeString = verticalSize.text.ToString();
        string horSizeString = horizontalSize.text.ToString();
        int vertSize;
        int horSize;
        if(int.TryParse(verSizeString, out vertSize) && int.TryParse(horSizeString, out horSize))
        {
            if (vertSize >= verticalSizeMinLimit && horSize >= horizontalSizeMinLimit && vertSize <= verticalSizeMaxLimit && horSize <= horizontalSizeMaxLimit)
            {
                levelCreatorBttn.interactable = true;
                sizeZ = vertSize;
                sizeX = horSize;
            }
            else
            {
                levelCreatorBttn.interactable = false;
            }
        }
        else
        {
            levelCreatorBttn.interactable = false;
        }
        
    }

    public void CreateMap()
    {
        creatorPanel.SetActive(false);
        editorPanel.SetActive(true);
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
        if (!saving)
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
        }
        walls.Clear();
        players.Clear();
        points.Clear();
        pills.Clear();
        playerSpawned = false;
        pinkySpawned = false;
        blinkySpawned = false;
        inkySpawned = false;
        clydeSpawned = false;
        tilePositionXInMap.Clear();
        tilePositionZInMap.Clear();
        tileTypeInMap.Clear();
        foreach (Tile tile in tiles)
        {
            
            tilePositionXInMap.Add(tile.PosX);
            tilePositionZInMap.Add(tile.PosZ);
            tileTypeInMap.Add((int)tile.Type);
            switch (tile.Type)
            {
                case TypeOfConstruction.WALL:
                    walls.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y,(int)tile.transform.position.z));
                    break;
                case TypeOfConstruction.PLAYER:
                    players.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    OnPlayerFound(TypeOfConstruction.PLAYER);
                    playerSpawned = true;
                    break;
                case TypeOfConstruction.PILL:
                    pills.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    break;
                case TypeOfConstruction.INKY:
                    inkyPositionInMap = new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z);
                    OnInkyFound(TypeOfConstruction.INKY);
                    inkySpawned = true;
                    break;
                case TypeOfConstruction.PINKY:
                    pinkyPositionInMap = new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z);
                    OnPinkyFound(TypeOfConstruction.PINKY);
                    pinkySpawned = true;
                    break;
                case TypeOfConstruction.BLINKY:
                    blinkyPositionInMap = new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z);
                    OnBlinkyFound(TypeOfConstruction.BLINKY);
                    blinkySpawned = true;
                    break;
                case TypeOfConstruction.CLYDE:
                    clydePositionInMap = new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z);
                    OnClydeFound(TypeOfConstruction.CLYDE);
                    clydeSpawned = true;
                    break;
                case TypeOfConstruction.POINT:
                    points.Add(new Vector3((int)tile.transform.position.x, (int)tile.transform.position.y, (int)tile.transform.position.z));
                    break;
            }
        }
        if(points.Count>0 && players.Count > 0)
        {
            OnPlayableMap();
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
                        construction = Instantiate(prefabPoint, new Vector3(tile.transform.position.x, prefabPoint.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.PILL:
                    if (!tile.Constructed && !tile.ObligatoryWall)
                    {
                        type = TypeOfConstruction.PILL;
                        construction = Instantiate(prefabPill, new Vector3(tile.transform.position.x, prefabPill.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.PLAYER:
                    if (!tile.Constructed && !tile.ObligatoryWall && !playerSpawned)
                    {
                        type = TypeOfConstruction.PLAYER;
                        construction = Instantiate(prefabPlayer, new Vector3(tile.transform.position.x, 0.1f, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.INKY:
                    if (!tile.Constructed && !tile.ObligatoryWall && !inkySpawned)
                    {
                        type = TypeOfConstruction.INKY;
                        construction = Instantiate(prefabInky, new Vector3(tile.transform.position.x, 0f, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.PINKY:
                    if (!tile.Constructed && !tile.ObligatoryWall && !pinkySpawned)
                    {
                        type = TypeOfConstruction.PINKY;
                        construction = Instantiate(prefabPinky, new Vector3(tile.transform.position.x, 0f, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.BLINKY:
                    if (!tile.Constructed && !tile.ObligatoryWall && !blinkySpawned)
                    {
                        type = TypeOfConstruction.BLINKY;
                        construction = Instantiate(prefabBlinky, new Vector3(tile.transform.position.x, 0f, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
                case TypeOfConstruction.CLYDE:
                    if (!tile.Constructed && !tile.ObligatoryWall && !clydeSpawned)
                    {
                        type = TypeOfConstruction.CLYDE;
                        construction = Instantiate(prefabClyde, new Vector3(tile.transform.position.x, 0f, tile.transform.position.z), Quaternion.identity);
                    }
                    break;
            }
            if (construction != null)
            {
                OnNormalConstruct?.Invoke(construction, tile.PosX, tile.PosZ,type);
                if(type != TypeOfConstruction.PLAYER && type != TypeOfConstruction.CLYDE && type != TypeOfConstruction.PINKY && type != TypeOfConstruction.BLINKY && type != TypeOfConstruction.INKY)
                {
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
        saving = true;
        MapSave mapSave = new MapSave
        {
            mapSizeX = sizeX,
            mapSizeZ = sizeZ,
            playerPositions = players,
            wallPositions = walls,
            pointPositions = points,
            inkyPosition = inkyPositionInMap,
            blinkyPosition = blinkyPositionInMap,
            pinkyPosition = pinkyPositionInMap,
            clydePosition = clydePositionInMap,
            pillPositions = pills,
            tilePositionX = tilePositionXInMap,
            tilePositionZ = tilePositionZInMap,
            tileType = tileTypeInMap
        };
        mapJson = JsonUtility.ToJson(mapSave,true);
        mapName = inputField.text;
        mapJsonEncoded = Base64Encode(mapJson);
        path = Application.persistentDataPath + "/maps";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        if(File.Exists(path + "/" + mapName + ".dat"))
        {
            overrideBox.SetActive(true);
        }
        else
        {
            ConfirmSave();
        }
    }

    public void ConfirmSave()
    {
        File.WriteAllText(path + "/" + mapName + ".dat", mapJsonEncoded);
        OnSaveMap();
        HideOverrideBox();
        saving = false;
        Debug.Log(Base64Decode(mapJsonEncoded));
    }

    public void HideOverrideBox()
    {
        overrideBox.SetActive(false);
        saving = false;
    }


    public void LoadMap(string pathToBeLoaded)
    {
        if (File.Exists(pathToBeLoaded))
        {
            string file = File.ReadAllText(pathToBeLoaded);
            Debug.Log(Base64Decode(file));
            LevelLoaderData.Instance.mapSave = JsonUtility.FromJson<MapSave>(Base64Decode(file));
            SceneManager.LoadScene("PacmanGame");
        }
    }

    
}


