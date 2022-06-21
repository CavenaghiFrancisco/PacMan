using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGhost : MonoBehaviour
{
    [SerializeField] float speed = 0;
    Vector3 initPosition;

    private void Start()
    {
        initPosition = transform.position;
    }

    private void Update()
    {
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(initPosition, initPosition + Vector3.up, time);
    }
}
