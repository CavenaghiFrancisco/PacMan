using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSave
{
    public int mapSizeX;
    public int mapSizeZ;
    public List<Vector3> wallPositions = new List<Vector3>();
    public List<Vector3> pointPositions = new List<Vector3>();
    public List<Vector3> pillPositions = new List<Vector3>();
    public List<Vector3> playerPositions = new List<Vector3>();
    public Vector3 blinkyPosition;
    public Vector3 inkyPosition;
    public Vector3 pinkyPosition;
    public Vector3 clydePosition;
    public List<int> tilePositionX = new List<int>();
    public List<int> tilePositionZ = new List<int>();
    public List<int> tileType = new List<int>();
}
