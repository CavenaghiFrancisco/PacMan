using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBehavior : MonoBehaviour
{
    [SerializeField] protected GameObject body;
    [SerializeField] protected GameObject afraidBody;

    protected enum DIRECTION
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        NONE,
    }
    protected float time = 0;
    protected bool canMove = false;
    protected Vector3 actualPosition = Vector3.zero;


    protected DIRECTION actualDirection = DIRECTION.NONE;

    public Vector3 ActualPosition
    {
        get { return actualPosition; }
        set { actualPosition = value; }
    }

    public float MoveTimer
    {
        get { return time; }
        set { time = value; }
    }

    public GameObject Body
    {
        get { return body; }
    }

    public GameObject AfraidBody
    {
        get { return afraidBody; }
    }


    protected void CheckNewDirectionByDestinyProximity(Vector3 destinyPosiiton)
    {
        Vector3 newDirectionUp = Vector3.zero;
        Vector3 newDirectionDown = Vector3.zero;
        Vector3 newDirectionRight = Vector3.zero;
        Vector3 newDirectionLeft = Vector3.zero;
        float distanceUp = 0;
        float distanceDown = 0;
        float distanceRight = 0;
        float distanceLeft = 0;
        float nearestDistance = 1000;
        DIRECTION betterWay = DIRECTION.NONE;
        switch (actualDirection)
        {
            case DIRECTION.UP:
                newDirectionUp = transform.position + Vector3.forward;
                newDirectionRight = transform.position + Vector3.right;
                newDirectionLeft = transform.position + Vector3.left;
                break;
            case DIRECTION.DOWN:
                newDirectionDown = transform.position + Vector3.back;
                newDirectionRight = transform.position + Vector3.right;
                newDirectionLeft = transform.position + Vector3.left;
                break;
            case DIRECTION.RIGHT:
                newDirectionDown = transform.position + Vector3.back;
                newDirectionUp = transform.position + Vector3.forward;
                newDirectionRight = transform.position + Vector3.right;
                break;
            case DIRECTION.LEFT:
                newDirectionDown = transform.position + Vector3.back;
                newDirectionLeft = transform.position + Vector3.left;
                newDirectionUp = transform.position + Vector3.forward;
                break;
            case DIRECTION.NONE:
                newDirectionDown = transform.position + Vector3.back;
                newDirectionLeft = transform.position + Vector3.left;
                newDirectionUp = transform.position + Vector3.forward;
                newDirectionRight = transform.position + Vector3.right;
                break;
        }
        if(newDirectionUp != Vector3.zero && MovementManager.GetTileTypeByPosition(Mathf.RoundToInt(newDirectionUp.x), Mathf.RoundToInt(newDirectionUp.z)) != TypeOfConstruction.WALL)
        {
            distanceUp = (newDirectionUp - destinyPosiiton).magnitude;
            if(distanceUp < nearestDistance)
            {
                nearestDistance = distanceUp;
                betterWay = DIRECTION.UP;
            }
        }
        if (newDirectionDown != Vector3.zero && MovementManager.GetTileTypeByPosition(Mathf.RoundToInt(newDirectionDown.x), Mathf.RoundToInt(newDirectionDown.z)) != TypeOfConstruction.WALL)
        {
            distanceDown = (newDirectionDown - destinyPosiiton).magnitude;
            if (distanceDown < nearestDistance)
            {
                nearestDistance = distanceDown;
                betterWay = DIRECTION.DOWN;
            }
        }
        if (newDirectionRight != Vector3.zero && MovementManager.GetTileTypeByPosition(Mathf.RoundToInt(newDirectionRight.x), Mathf.RoundToInt(newDirectionRight.z)) != TypeOfConstruction.WALL)
        {
            distanceRight = (newDirectionRight - destinyPosiiton).magnitude;
            if (distanceRight < nearestDistance)
            {
                nearestDistance = distanceRight;
                betterWay = DIRECTION.RIGHT;
            }
        }
        if (newDirectionLeft != Vector3.zero && MovementManager.GetTileTypeByPosition(Mathf.RoundToInt(newDirectionLeft.x), Mathf.RoundToInt(newDirectionLeft.z)) != TypeOfConstruction.WALL)
        {
            distanceLeft = (newDirectionLeft - destinyPosiiton).magnitude;
            if (distanceLeft < nearestDistance)
            {
                nearestDistance = distanceLeft;
                betterWay = DIRECTION.LEFT;
            }
        }
        actualDirection = betterWay;

    }

    protected void Move(Vector3 initPos, Vector3 destinyPoint, float speed)
    {
        if (time <= 1)
        {
            Vector3 direction = Vector3.zero;
            switch (actualDirection)
            {
                case DIRECTION.UP:
                    direction = Vector3.forward;
                    break;
                case DIRECTION.DOWN:
                    direction = Vector3.back;
                    break;
                case DIRECTION.RIGHT:
                    direction = Vector3.right;
                    break;
                case DIRECTION.LEFT:
                    direction = Vector3.left;
                    break;
                case DIRECTION.NONE:
                    CheckNewDirectionByDestinyProximity(destinyPoint);
                    break;
            }
            if(actualDirection != DIRECTION.NONE)
            {
                transform.position = Vector3.Lerp(initPos, initPos + direction, time += Time.deltaTime * speed);
                if (time >= 1)
                {
                    time = 1;
                    actualPosition = transform.position;
                    CheckNewDirectionByDestinyProximity(destinyPoint);
                    time = 0;
                }
            }

        }
    }

    protected void Respawn(Vector3 destinyPoint, ref float time)
    {
        if(time == 0)
        {
            transform.position = MovementManager.MiddleOfMap;
            transform.position = transform.position = Vector3.Lerp(MovementManager.MiddleOfMap, destinyPoint, time += Time.deltaTime/5);
        }
        else
        {
            transform.position = transform.position = Vector3.Lerp(MovementManager.MiddleOfMap, destinyPoint, time += Time.deltaTime/5);
        }
    }
}
