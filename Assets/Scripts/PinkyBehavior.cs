using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyBehavior : GhostBehavior
{
    [SerializeField] private ScriptableGhost pinkyData;
    private Vector3 destiny = Vector2.zero;
    private float timer = 0;
    private float normalSpeed = 4;
    private float afraidSpeed = 2;

    public ScriptableGhost PinkyData
    {
        get { return pinkyData; }
    }

    public float ReturnTimer
    {
        get { return timer; }
        set { timer = value; }
    }

    void Start()
    {
        pinkyData.initPosition = LevelLoaderData.Instance.mapSave.pinkyPosition;
        actualPosition = LevelLoaderData.Instance.mapSave.pinkyPosition;
        pinkyData.actualPosition = actualPosition;
        GameManager.OnActivatedPill += BecomeAfraid;
    }

    private void OnDestroy()
    {
        GameManager.OnActivatedPill -= BecomeAfraid;
    }


    // Update is called once per frame
    void Update()
    {
        pinkyData.speed = normalSpeed;
        if (pinkyData.actualMode == GhostModes.SCATTER)
        {
            destiny = MovementManager.GetNearestFreeTileToPoint(MovementManager.Corners["BottomRight"].posX, MovementManager.Corners["BottomRight"].posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.y), pinkyData.speed);
            destiny = new Vector3(destiny.x, transform.position.y, destiny.y);
        }
        else if (pinkyData.actualMode == GhostModes.CHASE)
        {
            TileJson randomTile = MovementManager.GetTileAtMinimumDistance(4f, new Vector2(MovementManager.PlayerPosition.x, MovementManager.PlayerPosition.z));
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), pinkyData.speed);
        }
        else if (pinkyData.actualMode == GhostModes.AFRAID)
        {
            if (pinkyData.speed == normalSpeed)
            {
                pinkyData.speed = afraidSpeed;
            }
            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), pinkyData.speed);
        }
        else if (pinkyData.actualMode == GhostModes.RETURNING_HOME)
        {
            if (!body.activeSelf)
            {
                afraidBody.SetActive(false);
                body.SetActive(true);
            }
            if (timer == 1)
            {
                actualPosition = pinkyData.initPosition;
                timer = 0;
                time = 0;
                actualDirection = DIRECTION.NONE;
                pinkyData.actualMode = GhostModes.SCATTER;
            }
            else
            {
                Respawn(pinkyData.initPosition, ref timer);
                if (timer >= 1)
                {
                    timer = 1;
                }
            }
        }
        if (pinkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(destiny - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 3);
        }
    }

    private void BecomeAfraid()
    {
        if (pinkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            pinkyData.actualMode = GhostModes.AFRAID;
            body.SetActive(false);
            afraidBody.SetActive(true);
        }  
    }
}