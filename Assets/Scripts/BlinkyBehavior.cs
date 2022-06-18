using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyBehavior : GhostBehavior
{
    [SerializeField] private ScriptableGhost blinkyData;
    private Vector3 destiny = Vector2.zero;
    private float time = 0;
    public ScriptableGhost BlinkyData
    {
        get { return blinkyData; }
    }


    void Start()
    {
        blinkyData.initPosition = transform.position;
        actualPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), blinkyData.speed);
        }
        else if (blinkyData.actualMode == GhostModes.RETURNING_HOME)
        {
            if(time == 1)
            {
                actualPosition = blinkyData.initPosition;
                time = 0;
                blinkyData.actualMode = GhostModes.CHASE;
            }
            else
            {
                Respawn(blinkyData.initPosition, ref time);
                if (time >= 1)
                {
                    time = 1;
                }
            }
        }
        if(blinkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(destiny - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 3);
        }
    }
}
