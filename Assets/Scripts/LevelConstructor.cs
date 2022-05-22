using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConstructor : MonoBehaviour
{
    [SerializeField] private GameObject prefabBase;
    [SerializeField] private GameObject prefabCube;
    [SerializeField] private GameObject Cube;
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeZ;
    [SerializeField] private Camera cam;
    private Vector3 mousePosition;
    private List<GameObject> tiles = new List<GameObject>();
    private TypeOfConstruction type = TypeOfConstruction.EMPTY;

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
                var tile = Instantiate(prefabBase, new Vector3(prefabBase.transform.localScale.x * i, -prefabBase.transform.localScale.y / 2, prefabBase.transform.localScale.z * j), Quaternion.identity);
                tiles.Add(tile);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        foreach (GameObject tile in tiles)
            tile.GetComponent<MeshRenderer>().material.color = Color.white;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit, 100))
        {
            foreach (GameObject tile in tiles)
            {
                if (hit.transform.gameObject == tile)
                {
                    tile.GetComponent<MeshRenderer>().material.color = Color.red;
                    if (Input.GetMouseButtonDown(0))
                    {
                        switch (type)
                        {
                            case TypeOfConstruction.EMPTY:
                                break;

                            case TypeOfConstruction.WALL:
                                Instantiate(prefabCube, new Vector3(tile.transform.position.x, prefabCube.transform.localScale.y / 2, tile.transform.position.z), Quaternion.identity);
                                break;
                            case TypeOfConstruction.BOX:
                                break;
                        }
                    }
                }
                
            }
        }           
    }

    
}
