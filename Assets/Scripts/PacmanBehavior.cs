using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanBehavior : MonoBehaviour
{
    private float time = 0;
    private bool changingDirection = false;
    private List<KeyCode> keys = new List<KeyCode>();
    private int maxKeys = 2;
    private const KeyCode upLetter = KeyCode.W;
    private const KeyCode downLetter = KeyCode.S;
    private const KeyCode leftLetter = KeyCode.A;
    private const KeyCode rightLetter = KeyCode.D;
    private bool canMove = false;
    private Vector3 initPosition;

    public KeyCode UpLetter
    {
        get { return upLetter; }
    }

    public KeyCode DownLetter
    {
        get { return downLetter; }
    }

    public KeyCode RightLetter
    {
        get { return rightLetter; }
    }

    public KeyCode LeftLetter
    {
        get { return leftLetter; }
    }

    public int MaxKeys
    {
        get { return maxKeys; }
        set { maxKeys = value; }
    }

    public List<KeyCode> Keys
    {
        get { return keys; }
    }

    public Vector3 InitPosition
    {
        get { return initPosition; }
        set { initPosition = value; }
    }


    private void Update()
    {
        if (keys.Count > 0)
        {
            switch (keys[0])
            {
                case upLetter:
                    MoveLerp(initPosition, Vector3.forward);
                    break;
                case downLetter:
                    MoveLerp(initPosition, Vector3.back);
                    break;
                case rightLetter:
                    MoveLerp(initPosition, Vector3.right);
                    break;
                case leftLetter:
                    MoveLerp(initPosition, Vector3.left);
                    break;
            }
        }
        if(keys.Count == maxKeys)
        {
            changingDirection = true;
        }
       
    }



    private void MoveLerp(Vector3 initPos, Vector3 movingVector)
    {
        if (MovementManager.GetTileTypeByPosition((int)(transform.position.x + movingVector.x), (int)(transform.position.z + movingVector.z)) != TypeOfConstruction.WALL && time == 0)
        {
            canMove = true;
        }
        if (canMove)
        {
            if (time <= 1)
            {
                transform.position = Vector3.Lerp(initPos, initPos + movingVector, time += Time.deltaTime * 5);
                if (time >= 1)
                {
                    time = 1;
                    if (changingDirection)
                    {
                        MakeChangeDirection();
                    }
                    initPosition = transform.position;
                    if (MovementManager.GetTileTypeByPosition((int)(transform.position.x + movingVector.x), (int)(transform.position.z + movingVector.z)) != TypeOfConstruction.WALL)
                    {
                        time = 0;
                    }
                    else
                    {
                        canMove = false;
                    }
                    time = 0;
                }
            }
        }

        if (changingDirection && time == 0)
        {
            MakeChangeDirection();
        }

    }

    private void MakeChangeDirection()
    {
        if (keys.Count == 2)
        {
            bool canChange = false;
            KeyCode lastKey = keys[0];
        
            switch (keys[1])
            {
                case upLetter:
                    if (MovementManager.GetTileTypeByPosition((int)(transform.position.x), (int)(transform.position.z + Vector3.forward.z)) != TypeOfConstruction.WALL)
                    {
                        canChange = true;
                    }
                    break;
                case downLetter:
                    if (MovementManager.GetTileTypeByPosition((int)(transform.position.x), (int)(transform.position.z - Vector3.forward.z)) != TypeOfConstruction.WALL)
                    {
                        canChange = true;
                    }
                    break;
                case rightLetter:
                    if (MovementManager.GetTileTypeByPosition((int)(transform.position.x + Vector3.right.x), (int)(transform.position.z)) != TypeOfConstruction.WALL)
                    {
                        canChange = true;
                    }
                    break;
                case leftLetter:
                    if (MovementManager.GetTileTypeByPosition((int)(transform.position.x - Vector3.right.x), (int)(transform.position.z)) != TypeOfConstruction.WALL)
                    {
                        canChange = true;
                    }
                    break;
            }
            if (canChange)
            {
                CheckChangeDirection();
            }
            else
            {
                canChange = false;
                keys.Remove(keys[1]);
            }
        }
    }

    private void CheckChangeDirection()
    {
        changingDirection = false;
        keys[0] = keys[1];
        keys.Remove(keys[1]);
        canMove = false;
        time = 0;
    }
}
