using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;
    private GameObject controller;
    private Vector3 spawnPoint;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.x = 11;
        spawnPoint.y = 3;
        spawnPoint.z = -17;
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    

    void CreateController()
    {
       controller= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint, Quaternion.identity, 0, new object[]{PV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
