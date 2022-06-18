using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBehavior : MonoBehaviour
{
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
        DIRECTION secondBetterWay = DIRECTION.NONE;
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
        if(newDirectionUp != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionUp.x,(int)newDirectionUp.z) != TypeOfConstruction.WALL)
        {
            distanceUp = (newDirectionUp - destinyPosiiton).magnitude;
            if(distanceUp < nearestDistance)
            {
                nearestDistance = distanceUp;
                secondBetterWay = betterWay;
                betterWay = DIRECTION.UP;
            }
        }
        if (newDirectionDown != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionDown.x, (int)newDirectionDown.z) != TypeOfConstruction.WALL)
        {
            distanceDown = (newDirectionDown - destinyPosiiton).magnitude;
            if (distanceDown < nearestDistance)
            {
                nearestDistance = distanceDown;
                secondBetterWay = betterWay;
                betterWay = DIRECTION.DOWN;
            }
        }
        if (newDirectionRight != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionRight.x, (int)newDirectionRight.z) != TypeOfConstruction.WALL)
        {
            distanceRight = (newDirectionRight - destinyPosiiton).magnitude;
            if (distanceRight < nearestDistance)
            {
                nearestDistance = distanceRight;
                secondBetterWay = betterWay;
                betterWay = DIRECTION.RIGHT;
            }
        }
        if (newDirectionLeft != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionLeft.x, (int)newDirectionLeft.z) != TypeOfConstruction.WALL)
        {
            distanceLeft = (newDirectionLeft - destinyPosiiton).magnitude;
            if (distanceLeft < nearestDistance)
            {
                nearestDistance = distanceLeft;
                secondBetterWay = betterWay;
                betterWay = DIRECTION.LEFT;
            }
        }
        actualDirection = CheckTheFinalBetterWay(betterWay, secondBetterWay,destinyPosiiton);

    }

    protected void Move(Vector3 initPos, Vector3 destinyPoint, float speed)
    {
        if (time <= 1)
        {
            switch (actualDirection)
            {
                case DIRECTION.UP:
                    transform.position = Vector3.Lerp(initPos, initPos + Vector3.forward, time += Time.deltaTime * speed);
                    if (time >= 1)
                    {
                        time = 1;
                        actualPosition = transform.position;
                        CheckNewDirectionByDestinyProximity(destinyPoint);
                        time = 0;
                    }
                    break;
                case DIRECTION.DOWN:
                    transform.position = Vector3.Lerp(initPos, initPos + Vector3.back, time += Time.deltaTime * speed);
                    if (time >= 1)
                    {
                        time = 1;
                        actualPosition = transform.position;
                        CheckNewDirectionByDestinyProximity(destinyPoint);
                        time = 0;
                    }
                    break;
                case DIRECTION.RIGHT:
                    transform.position = Vector3.Lerp(initPos, initPos + Vector3.right, time += Time.deltaTime * speed);
                    if (time >= 1)
                    {
                        time = 1;
                        actualPosition = transform.position;
                        CheckNewDirectionByDestinyProximity(destinyPoint);
                        time = 0;
                    }
                    break;
                case DIRECTION.LEFT:
                    transform.position = Vector3.Lerp(initPos, initPos + Vector3.left, time += Time.deltaTime * speed);
                    if (time >= 1)
                    {
                        time = 1;
                        actualPosition = transform.position;
                        CheckNewDirectionByDestinyProximity(destinyPoint);
                        time = 0;
                    }
                    break;
                case DIRECTION.NONE:
                    CheckNewDirectionByDestinyProximity(destinyPoint);
                    break;
            }
            
        }
    }

    protected DIRECTION CheckTheFinalBetterWay(DIRECTION firstOption, DIRECTION secondOption, Vector3 destinyPosition)
    {
        float distanceByFirstOption = GetNextTileDistance(firstOption, destinyPosition);
        float distanceBySecondOption = GetNextTileDistance(secondOption, destinyPosition);
        if(distanceByFirstOption > distanceBySecondOption)
        {
            return secondOption;
        }
        else
        {
            return firstOption;
        }
    }

    private float GetNextTileDistance(DIRECTION previousMove, Vector3 destinyPosition)
    {
        Vector3 nextPosition;
        switch (previousMove)
        {
            case DIRECTION.UP:
                nextPosition = transform.position + Vector3.forward;
                break;
            case DIRECTION.DOWN:
                nextPosition = transform.position + Vector3.back;
                break;
            case DIRECTION.RIGHT:
                nextPosition = transform.position + Vector3.right;
                break;
            case DIRECTION.LEFT:
                nextPosition = transform.position + Vector3.left;
                break;
        }
        Vector3 newDirectionUp = Vector3.zero;
        Vector3 newDirectionDown = Vector3.zero;
        Vector3 newDirectionRight = Vector3.zero;
        Vector3 newDirectionLeft = Vector3.zero;
        float distanceUp = 0;
        float distanceDown = 0;
        float distanceRight = 0;
        float distanceLeft = 0;
        float nearestDistance = 2000;
        switch (previousMove)
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
        if (newDirectionUp != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionUp.x, (int)newDirectionUp.z) != TypeOfConstruction.WALL)
        {
            distanceUp = (newDirectionUp - destinyPosition).magnitude;
            if (distanceUp < nearestDistance)
            {
                nearestDistance = distanceUp;
            }
        }
        if (newDirectionDown != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionDown.x, (int)newDirectionDown.z) != TypeOfConstruction.WALL)
        {
            distanceDown = (newDirectionDown - destinyPosition).magnitude;
            if (distanceDown < nearestDistance)
            {
                nearestDistance = distanceDown;
            }
        }
        if (newDirectionRight != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionRight.x, (int)newDirectionRight.z) != TypeOfConstruction.WALL)
        {
            distanceRight = (newDirectionRight - destinyPosition).magnitude;
            if (distanceRight < nearestDistance)
            {
                nearestDistance = distanceRight;
            }
        }
        if (newDirectionLeft != Vector3.zero && MovementManager.GetTileTypeByPosition((int)newDirectionLeft.x, (int)newDirectionLeft.z) != TypeOfConstruction.WALL)
        {
            distanceLeft = (newDirectionLeft - destinyPosition).magnitude;
            if (distanceLeft < nearestDistance)
            {
                nearestDistance = distanceLeft;
            }
        }
        return nearestDistance;
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
