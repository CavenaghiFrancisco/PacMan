using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    private MapSave mapSave;
    private static TileJson[,] tilesMap;
    [SerializeField] private PacmanBehavior player;
    


    private void Start()
    {
        
        mapSave = LevelLoaderData.Instance.mapSave;
        tilesMap = new TileJson[mapSave.mapSizeX, mapSave.mapSizeZ];

        for (int i = 0; i < mapSave.mapSizeZ; i++)
        {
            for (int j = 0; j < mapSave.mapSizeX; j++)
            {
                TileJson tileJsonAux = new TileJson();
                tileJsonAux.posX = mapSave.tilePositionX[i * mapSave.mapSizeZ + j];
                tileJsonAux.posZ = mapSave.tilePositionZ[i * mapSave.mapSizeZ + j];
                tileJsonAux.type = (TypeOfConstruction)mapSave.tileType[i * mapSave.mapSizeZ + j];
                tilesMap[i, j] = tileJsonAux;
            }
        }        
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

    
}
