using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementManager : MonoBehaviour
{
    private MapSave mapSave;
    private static TileJson[,] tilesMap;
    [SerializeField] private Vector3 pacmanPosition;
    public static Action OnMoveUp;
    public static Action OnMoveDown;

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
        foreach(TileJson tile in tilesMap)
        {
            if(tile.type == TypeOfConstruction.PLAYER)
            pacmanPosition = new Vector3(tile.posX, transform.position.y, tile.posZ);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnMoveUp();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnMoveDown();
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
