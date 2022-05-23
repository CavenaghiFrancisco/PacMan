using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject image;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                view.RPC("DissapearRPC", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DissapearRPC()
    {
        image.SetActive(false);
    }
}
