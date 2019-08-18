using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public string username;
    public string otherUsername;

    public void startGame()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.gameStarted = true;

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("setGameStarted", RpcTarget.OthersBuffered);
    }

    public void restart()
    {
        showRestartDialog();
    }

    public void restartYes()
    {
        restartWithoutRPC();
    }

    public void restartNo()
    {
        hideRestartDialog();
    }

    public void roomJoinError(string message)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.txtRoomError.GetComponent<Text>().text =  "Fehler: " + message;
    }

    public void joinRoom()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        PhotonNetwork.LocalPlayer.NickName = p.txtInputPlayerName.GetComponent<Text>().text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        string roomName = p.txtInputRoom.GetComponent<Text>().text;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void showRestartDialog()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.restartDialog = true;
    }

    public void hideRestartDialog()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.restartDialog = false;
    }

    public void restartWithoutRPC()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toggleWhiteValueChanged()
    {
        Pieces pieces = gameObject.GetComponent<Pieces>();
        Toggle tWhite = pieces.toggleWhite.GetComponent<Toggle>();

        Toggle tBlack = pieces.toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        if (tWhite.isOn)
        {
            tBlack.isOn = false;
            p.player = Field.WHITE;
        } else
        {
            tBlack.isOn = true;
            p.player = Field.BLACK;
        }
    }
    
    public void toggleBlackValueChanged()
    {
        Pieces pieces = gameObject.GetComponent<Pieces>();
        Toggle tWhite = pieces.toggleWhite.GetComponent<Toggle>();

        Toggle tBlack = pieces.toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();

        if (tBlack.isOn)
        {
            p.player = Field.BLACK;
            tWhite.isOn = false;
        }
        else
        {
            p.player = Field.WHITE;
            tWhite.isOn = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Room room = PhotonNetwork.CurrentRoom;

        if (room != null)
        {
            PhotonView photonView = gameObject.GetComponent<PhotonView>();
            Pieces pieces = gameObject.GetComponent<Pieces>();

            pieces.txtWaitForPlayerRoomName.GetComponent<Text>().text = "Spielname: " + room.Name;

            Player[] players = PhotonNetwork.PlayerListOthers;
            if (pieces.player == Field.WHITE)
            {
                pieces.txtPlayerWhite.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                pieces.txtTurnPlayerWhite.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                if (players.Length >= 1)
                {
                    pieces.txtPlayerBlack.GetComponent<Text>().text = players[0].NickName;
                    pieces.txtTurnPlayerBlack.GetComponent<Text>().text = players[0].NickName;
                }
            }
            else
            {
                pieces.txtPlayerBlack.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                pieces.txtTurnPlayerBlack.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                if (players.Length >= 1)
                {
                    pieces.txtPlayerWhite.GetComponent<Text>().text = players[0].NickName;
                    pieces.txtTurnPlayerWhite.GetComponent<Text>().text = players[0].NickName;
                }
            }
        }
    }
}
