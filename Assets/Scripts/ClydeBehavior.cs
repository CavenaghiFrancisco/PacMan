using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeBehavior : GhostBehavior
{
    [SerializeField] private ScriptableGhost clydeData;
    private Vector3 destiny = Vector2.zero;
    private float timer = 0;
    private float normalSpeed = 4;
    private float afraidSpeed = 2;

    public ScriptableGhost ClydeData
    {
        get { return clydeData; }
    }

    public float ReturnTimer
    {
        get { return timer; }
        set { timer = value; }
    }

    void Start()
    {
        clydeData.initPosition = transform.position;
        actualPosition = transform.position;
        clydeData.actualPosition = actualPosition;
        GameManager.OnActivatedPill += BecomeAfraid;
    }

    private void OnDestroy()
    {
        GameManager.OnActivatedPill -= BecomeAfraid;
    }

    // Update is called once per frame
    void Update()
    {
        clydeData.speed = normalSpeed;
        if (clydeData.actualMode == GhostModes.SCATTER)
        {
            destiny = MovementManager.GetNearestFreeTileToPoint(MovementManager.Corners["BottomLeft"].posX, MovementManager.Corners["BottomLeft"].posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.y), clydeData.speed);
            destiny = new Vector3(destiny.x, transform.position.y, destiny.y);
        }
        else if (clydeData.actualMode == GhostModes.CHASE)
        {
            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), clydeData.speed);
        }
        else if (clydeData.actualMode == GhostModes.AFRAID)
        {
            if (clydeData.speed == normalSpeed)
            {
                clydeData.speed = afraidSpeed;
            }

            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), clydeData.speed);
        }
        else if (clydeData.actualMode == GhostModes.RETURNING_HOME)
        {
            if (!body.activeSelf)
            {
                afraidBody.SetActive(false);
                body.SetActive(true);
            }
            if (timer == 1)
            {
                actualPosition = clydeData.initPosition;
                timer = 0;
                time = 0;
                actualDirection = DIRECTION.NONE;
                clydeData.actualMode = GhostModes.SCATTER;
            }
            else
            {
                Respawn(clydeData.initPosition, ref timer);
                if (timer >= 1)
                {
                    timer = 1;
                }
            }
        }
        if (clydeData.actualMode != GhostModes.RETURNING_HOME)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(destiny - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 3);
        }
    }

    private void BecomeAfraid()
    {
        if (clydeData.actualMode != GhostModes.RETURNING_HOME)
            clydeData.actualMode = GhostModes.AFRAID;
    }
}

