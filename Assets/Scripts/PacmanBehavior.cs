using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanBehavior : MonoBehaviour
{
    private float time = 0;
    private bool changingDirection = false;

    private void Start()
    {
        MovementManager.OnMoveUp += MoveUp;
        MovementManager.OnMoveDown += MoveDown;
    }

    private void MoveUp()
    {
        time = 0;
        StartCoroutine(MoveLerp(transform.position,Vector3.forward));
    }

    private void MoveDown()
    {
        time = 0;
        StartCoroutine(MoveLerp(transform.position, -Vector3.forward));
    }

    private IEnumerator MoveLerp(Vector3 initPosition, Vector3 movingVector)
    {
        while(time < 1)
        {
            transform.position = Vector3.Lerp(initPosition, initPosition + new Vector3(0, 0, 1), time += Time.deltaTime * 4);
            if (time > 1)
            {
                time = 1;
                if (changingDirection)
                {
                    changingDirection = false;
                    yield break;
                }
                initPosition = transform.position;
                if (MovementManager.GetTileTypeByPosition((int)transform.position.x, (int)transform.position.z + 1) != TypeOfConstruction.WALL)
                {
                    time = 0;
                    yield return null;
                }
                else
                    yield break;
            }
            yield return null;
            
        }
    }
}
