using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Ghost")]
public class ScriptableGhost : ScriptableObject
{
    public bool alive;
    public float speed;
    public GhostModes actualMode;
    public Vector3 initPosition;
    public Vector3 destinyPosition;
    public Vector3 actualPosition;
}
