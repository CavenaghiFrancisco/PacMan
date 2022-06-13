using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
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
    private MapSave mapSave = LevelLoader.Instance.mapSave;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mapSave.mapSizeZ; i++)
        {
            for (int j = 0; j < mapSave.mapSizeX; j++)
            {
                Instantiate(prefabBase, new Vector3(prefabBase.transform.localScale.x * j, -prefabBase.transform.localScale.y / 2, prefabBase.transform.localScale.z * i), Quaternion.identity);
            }
        }
        foreach(Vector3 wall in mapSave.wallPositions)
        {
            Instantiate(prefabCube, wall + new Vector3(0,prefabCube.transform.localScale.y/2,0), Quaternion.identity);
        }
        foreach (Vector3 point in mapSave.pointPositions)
        {
            Instantiate(prefabPoint, point + new Vector3(0, prefabPoint.transform.localScale.y / 2, 0), Quaternion.identity);
        }
        foreach (Vector3 pill in mapSave.pillPositions)
        {
            Instantiate(prefabPill, pill + new Vector3(0, prefabPill.transform.localScale.y / 2, 0), Quaternion.identity);
        }
        foreach (Vector3 player in mapSave.playerPositions)
        {
            Instantiate(prefabPlayer, player + new Vector3(0, 0.1f, 0), Quaternion.identity);
        }
        Instantiate(prefabBlinky, mapSave.blinkyPosition, Quaternion.identity);
        Instantiate(prefabInky, mapSave.pinkyPosition, Quaternion.identity);
        Instantiate(prefabPinky, mapSave.inkyPosition, Quaternion.identity);
        Instantiate(prefabClyde, mapSave.clydePosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
