using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    private static MapSave mapSave;
    private static TileJson[,] tilesMap;
    [SerializeField] private PacmanBehavior player;
    private static Dictionary<string, TileJson> corners = new Dictionary<string, TileJson>();
    private static Vector3 playerPosition;
    private static Vector3 middleOfMap;

    public static Dictionary<string, TileJson> Corners
    {
        get { return corners; }
    }

    public static Vector3 PlayerPosition
    {
        get { return playerPosition; }
    }

    public static Vector3 MiddleOfMap
    {
        get { return middleOfMap; }
    }

    private void Start()
    {
        
        mapSave = LevelLoaderData.Instance.mapSave;
        tilesMap = new TileJson[mapSave.mapSizeZ, mapSave.mapSizeX];
        middleOfMap = new Vector3(mapSave.mapSizeX/2, Camera.main.transform.position.y, mapSave.mapSizeZ/2+5);

        for (int i = 0; i < mapSave.mapSizeZ; i++)
        {
            for (int j = 0; j < mapSave.mapSizeX; j++)
            {
                TileJson tileJsonAux = new TileJson();
                tileJsonAux.posX = mapSave.tilePositionX[i * (mapSave.mapSizeX) + j];
                tileJsonAux.posZ = mapSave.tilePositionZ[i * (mapSave.mapSizeX) + j];
                tileJsonAux.type = (TypeOfConstruction)mapSave.tileType[i * (mapSave.mapSizeX) + j];
                tilesMap[i, j] = tileJsonAux;
            }
        }
        corners.Add("TopRight",tilesMap[mapSave.mapSizeZ - 1, mapSave.mapSizeX - 1]);
        corners.Add("TopLeft",tilesMap[mapSave.mapSizeZ - 1,0 ]);
        corners.Add("BottomRight",tilesMap[0, mapSave.mapSizeX - 1]);
        corners.Add("BottomLeft", tilesMap[0, 0]);
    }

    private void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PacmanBehavior>();
            foreach (TileJson tile in tilesMap)
            {
                if (tile.type == TypeOfConstruction.PLAYER)
                    player.InitPosition = new Vector3(tile.posX, 0.1f, tile.posZ);
            }
        }
        if (Input.GetButtonDown("Up"))
        {
            AddKeyMoveToPlayer(player.UpLetter);
        }
        else if (Input.GetButtonDown("Down"))
        {
            AddKeyMoveToPlayer(player.DownLetter);
        }
        else if (Input.GetButtonDown("Right"))
        {
            AddKeyMoveToPlayer(player.RightLetter);
        }
        else if (Input.GetButtonDown("Left"))
        {
            AddKeyMoveToPlayer(player.LeftLetter);
        }
        playerPosition = player.transform.position;
    }

    private void AddKeyMoveToPlayer(KeyCode key)
    {
        if (player.Keys.Count < player.MaxKeys)
        {
            if (player.Keys.Count > 0)
            {
                if (player.Keys[0] != key)
                {
                    player.Keys.Add(key);
                }
            }
            else
            {
                player.Keys.Add(key);
            }
        }
    }


    public static TypeOfConstruction GetTileTypeByPosition(int posX, int posZ)
    {
        foreach(TileJson tile in tilesMap)
        {
            if(tile.posX == posX && tile.posZ == posZ)
            {
                return tile.type;
            }
        }
        return TypeOfConstruction.NULL;
    }

    public static Vector2 GetNearestFreeTileToPoint(int pointX, int pointZ)
    {
        float nearestDistance = 200;
        float distance = 0;
        Vector2 nearestPoint = Vector2.zero;
        foreach(TileJson tile in tilesMap)
        {
            if (tile.type != TypeOfConstruction.WALL)
            {
                distance = Mathf.Sqrt(Mathf.Pow(tile.posX - pointX, 2) + Mathf.Pow(tile.posZ - pointZ, 2));
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPoint = new Vector2(tile.posX, tile.posZ);
                }
            }
        }
        return nearestPoint;
    }


    public static TileJson GetRandomTile()
    {
        return tilesMap[UnityEngine.Random.Range(0, mapSave.mapSizeZ),UnityEngine.Random.Range(0, mapSave.mapSizeX)];
    }
    
}
