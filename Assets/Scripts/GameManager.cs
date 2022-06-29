using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private MapSave mapInfo = LevelLoaderData.Instance.mapSave;
    private int lifes;
    private int points;
    public static Action OnActivatedPill;
    public static Action OnPhantomEaten;
    public static Action<int> OnAddedPoints;
    public static Action<int> OnLifeLose;
    public static Action OnLose;
    public static Action OnWin;
    PacmanBehavior player;
    BlinkyBehavior blinky;
    InkyBehavior inky;
    ClydeBehavior clyde;
    PinkyBehavior pinky;
    Animator blinkyAnim;
    Animator inkyAnim;
    Animator clydeAnim;
    Animator pinkyAnim;
    float phasesTimer = 0;
    float afraidTimer = 0;
    bool ghostsAreAfraid = false;
    GhostModes actualMode;
    [SerializeField] AudioSource pacmanEat;
    [SerializeField] AudioSource pillSound;

    private void Start()
    {
        Time.timeScale = 1;
        PacmanBehavior.OnCollisionWithPill += ActivePillEffect;
        PacmanBehavior.OnCollisionWithGhost += CollsionPlayerWithGhost;
        PacmanBehavior.OnCollisionWithPoint += AddPoint;
        player = GameObject.FindObjectOfType<PacmanBehavior>();
        blinky = GameObject.FindObjectOfType<BlinkyBehavior>();
        inky = GameObject.FindObjectOfType<InkyBehavior>();
        pinky = GameObject.FindObjectOfType<PinkyBehavior>();
        clyde = GameObject.FindObjectOfType<ClydeBehavior>();

        if (blinky)
            blinky.BlinkyData.actualMode = GhostModes.RETURNING_HOME;
        if (inky)
            inky.InkyData.actualMode = GhostModes.RETURNING_HOME;
        if (pinky)
            pinky.PinkyData.actualMode = GhostModes.RETURNING_HOME;
        if (clyde)
            clyde.ClydeData.actualMode = GhostModes.RETURNING_HOME;
        lifes = 3;
        points = mapInfo.pointPositions.Count + mapInfo.pillPositions.Count;
        
    }

    private void Update()
    {
        StartGamePhases();
        CheckAfraidState();
    }

    private void OnDestroy()
    {
        PacmanBehavior.OnCollisionWithPill  -= ActivePillEffect;
        PacmanBehavior.OnCollisionWithGhost -= CollsionPlayerWithGhost;
        PacmanBehavior.OnCollisionWithPoint -= AddPoint;
    }

    private void ActivePillEffect(GameObject pill)
    {
        points--;
        pillSound.Play();
        ghostsAreAfraid = true;
        OnActivatedPill();
        if (points == 0)
        {
            Time.timeScale = 0;
            OnWin();
        }
        Destroy(pill);
    }

    private void CollsionPlayerWithGhost(GameObject collisionedGhost)
    {
        if (collisionedGhost == blinky.gameObject)
        {
            if (blinky.BlinkyData.actualMode == GhostModes.AFRAID)
            {
                blinky.BlinkyData.actualMode = GhostModes.RETURNING_HOME;
                OnAddedPoints(200);
                pacmanEat.Play();
            }
            else
            {
                lifes--;
                if(lifes == 0)
                {
                    OnLifeLose(lifes);
                    OnLose();
                }
                else
                {
                    RestartPositions();
                    OnLifeLose(lifes);
                }
            }
        }
        else if (collisionedGhost == inky.gameObject)
        {
            if (inky.InkyData.actualMode == GhostModes.AFRAID)
            {
                inky.InkyData.actualMode = GhostModes.RETURNING_HOME;
                OnAddedPoints(200);
                pacmanEat.Play();
            }
            else
            {
                lifes--;
                if (lifes == 0)
                {
                    OnLifeLose(lifes);
                    OnLose();
                }
                else
                {
                    RestartPositions();
                    OnLifeLose(lifes);
                }
            }
        }
        else if (collisionedGhost == pinky.gameObject)
        {
            if (pinky.PinkyData.actualMode == GhostModes.AFRAID)
            {
                pinky.PinkyData.actualMode = GhostModes.RETURNING_HOME;
                OnAddedPoints(200);
                pacmanEat.Play();
            }
            else
            {
                lifes--;
                if (lifes == 0)
                {
                    OnLifeLose(lifes);
                    OnLose();
                }
                else
                {
                    RestartPositions();
                    OnLifeLose(lifes);
                }
            }
        }
        else if (collisionedGhost == clyde.gameObject)
        {
            if (clyde.ClydeData.actualMode == GhostModes.AFRAID)
            {
                clyde.ClydeData.actualMode = GhostModes.RETURNING_HOME;
                OnAddedPoints(200);
                pacmanEat.Play();
            }
            else
            {
                lifes--;
                if (lifes == 0)
                {
                    OnLifeLose(lifes);
                    OnLose();
                }
                else
                {
                    RestartPositions();
                    OnLifeLose(lifes);
                }
            }
        }
    }

    private void RestartPositions()
    { //TODO scriptable object innecesario, hacer eventos para cada script
        player.Keys.Clear();
        player.gameObject.transform.position = player.InitPosition;
        player.Position = player.InitPosition;
        player.CanMove = false;
        if (blinky)
        {
            blinky.gameObject.transform.position = blinky.BlinkyData.initPosition;
            blinky.BlinkyData.actualMode = GhostModes.RETURNING_HOME;
            blinky.ActualPosition = blinky.BlinkyData.initPosition;
            blinky.MoveTimer = 0;
            blinky.ReturnTimer = 0;
        }
        if (pinky)
        {
            pinky.gameObject.transform.position = blinky.BlinkyData.initPosition;
            pinky.PinkyData.actualMode = GhostModes.RETURNING_HOME;
            pinky.ActualPosition = blinky.BlinkyData.initPosition;
            pinky.MoveTimer = 0;
            pinky.ReturnTimer = 0;
        }
        if (clyde)
        {
            clyde.gameObject.transform.position = blinky.BlinkyData.initPosition;
            clyde.ClydeData.actualMode = GhostModes.RETURNING_HOME;
            clyde.ActualPosition = blinky.BlinkyData.initPosition;
            clyde.MoveTimer = 0;
            clyde.ReturnTimer = 0;
        }
        if (inky)
        {
            inky.gameObject.transform.position = blinky.BlinkyData.initPosition;
            inky.InkyData.actualMode = GhostModes.RETURNING_HOME;
            inky.ActualPosition = blinky.BlinkyData.initPosition;
            inky.MoveTimer = 0;
            inky.ReturnTimer = 0;
        }
        phasesTimer = 0;
    }

    private void AddPoint()
    {
        points--;
        OnAddedPoints(10);
        if (points == 0)
        {
            Time.timeScale = 0;
            OnWin();
        }
    }

    private void StartGamePhases()
    {
        actualMode = GhostModes.SCATTER;
        phasesTimer += Time.deltaTime;
        if(phasesTimer > 10)
        {
            actualMode = GhostModes.CHASE;
        }
        if (phasesTimer > 17)
        {
            actualMode = GhostModes.SCATTER;
        }
        if (phasesTimer > 27)
        {
            actualMode = GhostModes.CHASE;
        }
        if (phasesTimer > 34)
        {
            actualMode = GhostModes.SCATTER;
        }
        if (phasesTimer > 44)
        {
            actualMode = GhostModes.CHASE;
        }
        if (phasesTimer > 51)
        {
            actualMode = GhostModes.SCATTER;
        }
        if(phasesTimer > 61)
        {
            actualMode = GhostModes.CHASE;
        }
        if (blinky)
        {
            if (blinky.BlinkyData.actualMode != GhostModes.AFRAID && blinky.BlinkyData.actualMode != GhostModes.RETURNING_HOME)
            {
                blinky.BlinkyData.actualMode = actualMode;
            }
            if (blinky.BlinkyData.actualMode != GhostModes.AFRAID)
            {
                blinky.Body.SetActive(true);
                blinky.AfraidBody.SetActive(false);
            }
        }
        if (clyde)
        {
            if (clyde.ClydeData.actualMode != GhostModes.AFRAID && clyde.ClydeData.actualMode != GhostModes.RETURNING_HOME)
            {
                clyde.ClydeData.actualMode = actualMode;
            }
            if (clyde.ClydeData.actualMode != GhostModes.AFRAID)
            {
                clyde.Body.SetActive(true);
                clyde.AfraidBody.SetActive(false);
            }
        }
        if (inky)
        {
            if (inky.InkyData.actualMode != GhostModes.AFRAID && inky.InkyData.actualMode != GhostModes.RETURNING_HOME)
            {
                inky.InkyData.actualMode = actualMode;
            }
            if (inky.InkyData.actualMode != GhostModes.AFRAID)
            {
                inky.Body.SetActive(true);
                inky.AfraidBody.SetActive(false);
            }
        }
        if (pinky)
        {
            if (pinky.PinkyData.actualMode != GhostModes.AFRAID && pinky.PinkyData.actualMode != GhostModes.RETURNING_HOME)
            {
                pinky.PinkyData.actualMode = actualMode;
            }
            if (pinky.PinkyData.actualMode != GhostModes.AFRAID)
            {
                pinky.Body.SetActive(true);
                pinky.AfraidBody.SetActive(false);
            }
        }
    }

    private void CheckAfraidState()
    {
        if (pinky)
        {
            if (pinky.PinkyData.actualMode == GhostModes.AFRAID)
                pinky.AfraidBody.SetActive(true);
        }
        if (inky)
        {
            if (inky.InkyData.actualMode == GhostModes.AFRAID)
                inky.AfraidBody.SetActive(true);
        }
        if (clyde)
        {
            if (clyde.ClydeData.actualMode == GhostModes.AFRAID)
                clyde.AfraidBody.SetActive(true);
        }
        if (blinky)
        {
            if (blinky.BlinkyData.actualMode == GhostModes.AFRAID)
                blinky.AfraidBody.SetActive(true);
        }      
        if (ghostsAreAfraid)
        {
            afraidTimer = 0;
            ghostsAreAfraid = false;
        }
        afraidTimer += Time.deltaTime;
        if(afraidTimer > 4 && afraidTimer < 5)
        {
            ChangeBodyBasedOnTime(4, true);
        }
        if (afraidTimer > 5 && afraidTimer < 6)
        {
            ChangeBodyBasedOnTime(5, true);
        }
        if (afraidTimer > 6)
        {
            if (pinky)
            {
                if (pinky.PinkyData.actualMode != GhostModes.RETURNING_HOME)
                {
                    pinky.PinkyData.actualMode = actualMode;
                    pinky.Body.SetActive(true);
                    pinky.AfraidBody.SetActive(false);
                }
            }
            if (inky)
            {
                if (inky.InkyData.actualMode != GhostModes.RETURNING_HOME)
                {
                    inky.InkyData.actualMode = actualMode;
                    inky.Body.SetActive(true);
                    inky.AfraidBody.SetActive(false);
                }
            }
            if (clyde)
            {
                if (clyde.ClydeData.actualMode != GhostModes.RETURNING_HOME)
                {
                    clyde.ClydeData.actualMode = actualMode;
                    clyde.Body.SetActive(true);
                    clyde.AfraidBody.SetActive(false);
                }
            }
            if (blinky)
            {
                if (blinky.BlinkyData.actualMode != GhostModes.RETURNING_HOME)
                {
                    blinky.BlinkyData.actualMode = actualMode;
                    blinky.Body.SetActive(true);
                    blinky.AfraidBody.SetActive(false);
                }
            }
        }
    }

    private void ChangeBodyBasedOnTime(float time, bool normalBody)
    {
        if (afraidTimer > time && afraidTimer < time + 0.5f)
        {
            if(pinky)
                if (pinky.PinkyData.actualMode == GhostModes.AFRAID)
                {
                    pinky.Body.SetActive(normalBody);
                    pinky.AfraidBody.SetActive(!normalBody);
                }
            if (inky)
                if (inky.InkyData.actualMode == GhostModes.AFRAID)
                {
                    inky.Body.SetActive(normalBody);
                    inky.AfraidBody.SetActive(!normalBody);
                }
            if (clyde)
                if (clyde.ClydeData.actualMode == GhostModes.AFRAID)
                {
                    clyde.Body.SetActive(normalBody);
                    clyde.AfraidBody.SetActive(!normalBody);
                }  
            if(blinky)
                if (blinky.BlinkyData.actualMode == GhostModes.AFRAID)
                {
                    blinky.Body.SetActive(normalBody);
                    blinky.AfraidBody.SetActive(!normalBody);
                } 
        }
        if (afraidTimer > time+0.5 && afraidTimer < time + 1f)
        {
            if(pinky)
                if (pinky.PinkyData.actualMode == GhostModes.AFRAID)
                {
                    pinky.Body.SetActive(!normalBody);
                    pinky.AfraidBody.SetActive(normalBody);
                }
            if(inky)
                if (inky.InkyData.actualMode == GhostModes.AFRAID)
                {
                    inky.Body.SetActive(!normalBody);
                    inky.AfraidBody.SetActive(normalBody);
                }
            if(clyde)
                if (clyde.ClydeData.actualMode == GhostModes.AFRAID)
                {
                    clyde.Body.SetActive(!normalBody);
                    clyde.AfraidBody.SetActive(normalBody);
                }
            if(blinky)
                if (blinky.BlinkyData.actualMode == GhostModes.AFRAID)
                {
                    blinky.Body.SetActive(!normalBody);
                    blinky.AfraidBody.SetActive(normalBody);
                }
        }
    }
}
