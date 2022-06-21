using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyBehavior : GhostBehavior
{
    [SerializeField] private ScriptableGhost inkyData;
    private Vector3 destiny = Vector2.zero;
    private float timer = 0;
    private float normalSpeed = 2.5f;
    private float afraidSpeed = 2;

    public ScriptableGhost InkyData
    {
        get { return inkyData; }
    }

    public float ReturnTimer
    {
        get { return timer; }
        set { timer = value; }
    }


    void Start()
    {
        inkyData.initPosition = LevelLoaderData.Instance.mapSave.inkyPosition;
        actualPosition = LevelLoaderData.Instance.mapSave.inkyPosition;
        inkyData.actualPosition = actualPosition;
        GameManager.OnActivatedPill += BecomeAfraid;
    }

    private void OnDestroy()
    {
        GameManager.OnActivatedPill -= BecomeAfraid;
    }

    // Update is called once per frame
    void Update()
    {
        inkyData.speed = normalSpeed;
        if (inkyData.actualMode == GhostModes.SCATTER)
        {
            destiny = MovementManager.GetNearestFreeTileToPoint(MovementManager.Corners["TopRight"].posX, MovementManager.Corners["TopRight"].posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.y), inkyData.speed);
            destiny = new Vector3(destiny.x, transform.position.y, destiny.y);
        }
        else if (inkyData.actualMode == GhostModes.CHASE)
        {
            destiny = new Vector3(MovementManager.PlayerPosition.x, transform.position.y, MovementManager.PlayerPosition.z);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), inkyData.speed);
        }
        else if (inkyData.actualMode == GhostModes.AFRAID)
        {
            if (inkyData.speed == normalSpeed)
            {
                inkyData.speed = afraidSpeed;
            }
            TileJson randomTile = MovementManager.GetRandomTile();
            destiny = new Vector3(randomTile.posX, transform.position.y, randomTile.posZ);
            Move(actualPosition, new Vector3(destiny.x, transform.position.y, destiny.z), inkyData.speed);
        }
        else if (inkyData.actualMode == GhostModes.RETURNING_HOME)
        {
            if (!body.activeSelf)
            {
                afraidBody.SetActive(false);
                body.SetActive(true);
            }
            if (timer == 1)
            {
                actualPosition = inkyData.initPosition;
                timer = 0;
                time = 0;
                actualDirection = DIRECTION.NONE;
                inkyData.actualMode = GhostModes.SCATTER;
            }
            else
            {
                Respawn(inkyData.initPosition, ref timer);
                if (timer >= 1)
                {
                    timer = 1;
                }
            }
        }
        if (inkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            Quaternion lookOnLook = Quaternion.LookRotation(destiny - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 3);
        }
    }

    private void BecomeAfraid()
    {
        if (inkyData.actualMode != GhostModes.RETURNING_HOME)
        {
            inkyData.actualMode = GhostModes.AFRAID;
            body.SetActive(false);
            afraidBody.SetActive(true);
        }
            
    }

}
