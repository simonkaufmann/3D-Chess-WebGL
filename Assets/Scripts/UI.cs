﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject panelRestart;
    public GameObject panelRoomSelection;

    public void startGame()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.gameStarted = true;

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("setGameStarted", RpcTarget.OthersBuffered);
    }

    public void restart()
    {
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("sendRestart", RpcTarget.OthersBuffered);
        restartWithoutRPC();
    }

    public void restartYes()
    {

    }

    public void restartNo()
    {

    }

    public void joinRoom()
    {

    }

    public void restartWithoutRPC()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toggleWhiteValueChanged()
    {
        GameObject toggleWhite = GameObject.Find("toggleWhite");
        Toggle tWhite = toggleWhite.GetComponent<Toggle>();

        GameObject toggleBlack = GameObject.Find("toggleBlack");
        Toggle tBlack = toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        if (tWhite.isOn)
        {
            tBlack.isOn = false;
            p.player = Field.WHITE;
            //photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.BLACK);
        } else
        {
            tBlack.isOn = true;
            p.player = Field.BLACK;
            //photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.WHITE);
        }
    }
    
    public void toggleBlackValueChanged()
    {
        GameObject toggleWhite = GameObject.Find("toggleWhite");
        Toggle tWhite = toggleWhite.GetComponent<Toggle>();

        GameObject toggleBlack = GameObject.Find("toggleBlack");
        Toggle tBlack = toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();
        //p.myTurn = !p.myTurn;

        //PhotonView photonView = gameObject.GetComponent<PhotonView>();
        if (tBlack.isOn)
        {
            p.player = Field.BLACK;
            tWhite.isOn = false;
            //photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.WHITE);
        }
        else
        {
            p.player = Field.WHITE;
            tWhite.isOn = true;
            //photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.BLACK);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        panelRestart = GameObject.Find("panelRestart");
        panelRestart.SetActive(false);
        panelRoomSelection = GameObject.Find("panelRoomSelection");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
