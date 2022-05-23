using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    PhotonView view;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + new Vector3(horizontal * Time.deltaTime, 0, vertical * Time.deltaTime));
    }
}
