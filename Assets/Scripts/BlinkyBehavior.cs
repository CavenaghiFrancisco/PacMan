using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyBehavior : GhostBehavior
{
    [SerializeField] private ScriptableGhost blinkyData;
    private Vector3 destiny = Vector2.zero;
    private float timer = 0;
    private float normalSpeed = 4;
    private float afraidSpeed = 2;

    public ScriptableGhost BlinkyData
    {
        get { return blinkyData; }
    }

    public float ReturnTimer
    {
        get { return timer; }
        set { timer = value; }
    }

    void Start()
    {
        blinkyData.initPosition = transform.position;
        actualPosition = transform.position;
        blinkyData.actualPosition = actualPosition;
        GameManager.OnActivatedPill += BecomeAfraid;
    }

    private void OnDestroy()
    {
        GameManager.OnActivatedPill -= BecomeAfraid;
    }

    // Update is called once per frame
    void Update()
    {
        blinkyData.speed = normalSpeed;
        if (blinkyData.actualMode == GhostModes.SCATTER)
        {
            destiny = MovementManager.GetNearestFreeTileToPoint(MovementManager.Corners["TopLeft"].posX,MovementManager.Corners["TopLeft"].posZ);
            Move(actualPosition,new Vector3(destiny.x, transform.position.y,destiny.y), blinkyData.speed);
            destiny = new Vector3(destiny.x, transform.position.y, destiny.y);
        }
        else if(blinkyData.actualMode == GhostModes.CHASE)
        {
            destiny = new Vector3(MovementManager.PlayerPosition.x,transform.position.y, MovementManager.PlayerPosition.z);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z),blinkyData.speed);
        }
        else if (blinkyData.actualMode == GhostModes.AFRAID)
        {
            if(blinkyData.speed == normalSpeed)
            {
                blinkyData.speed = afraidSpeed; 
            }
            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), blinkyData.speed);
        }
        else if (blinkyData.actualMode == GhostModes.RETURNING_HOME)
        {
            if (!body.activeSelf)
            {
                afraidBody.SetActive(false);
                body.SetActive(true);
            }
            if (timer == 1)
            {
                actualPosition = blinkyData.initPosition;
                timer = 0;
                time = 0;
                actualDirection = DIRECTION.NONE;
                blinkyData.actualMode = GhostModes.SCATTER;
            }
            else
            {
                Respawn(blinkyData.initPosition, ref timer);
                if (timer >= 1)
                {
                    timer = 1;
                }
            }
        }
        if(blinkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(destiny - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 3);
        }
    }

    private void BecomeAfraid()
    {
        if(blinkyData.actualMode != GhostModes.RETURNING_HOME)
        blinkyData.actualMode = GhostModes.AFRAID;
    }
}
