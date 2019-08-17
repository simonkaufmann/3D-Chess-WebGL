using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Networking : MonoBehaviourPunCallbacks
{

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        Pieces p = gameObject.GetComponent<Pieces>();
        p.active = true;

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        Pieces pieces = gameObject.GetComponent<Pieces>();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && room.PlayerCount == 2)
        {
            bool whiteActive = pieces.toggleWhite.activeInHierarchy;
            bool blackActive = pieces.toggleBlack.activeInHierarchy;
            pieces.toggleWhite.SetActive(true);
            pieces.toggleBlack.SetActive(true);
            pieces.toggleWhite.GetComponent<Toggle>().isOn = false;
            pieces.toggleWhite.SetActive(whiteActive);
            pieces.toggleBlack.SetActive(blackActive);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created room");
        Pieces p = gameObject.GetComponent<Pieces>();
        p.active = true;
    }

    [PunRPC]
    public void sendMove(string[] str)
    {
        Field[] fs = new Field[str.Length];
        for (int i = 0; i < fs.GetLength(0); i++)
        {
            fs[i] = JsonUtility.FromJson<Field>(str[i]);
        }
        Pieces p = gameObject.GetComponent<Pieces>();
        p.receiveBoardStatus(fs);
    }

    [PunRPC]
    public void setGameStarted()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.gameStarted = true;
    }

    [PunRPC]
    public void setPlayer(int player)
    {
        Pieces pieces = gameObject.GetComponent<Pieces>();
        bool whiteActive = pieces.toggleWhite.activeInHierarchy;
        bool blackActive = pieces.toggleBlack.activeInHierarchy;
        pieces.toggleWhite.SetActive(true);
        pieces.toggleBlack.SetActive(true);
        if (player == Field.WHITE)
        {
            pieces.toggleWhite.GetComponent<Toggle>().isOn = true;
        } else if (player == Field.BLACK)
        {
            pieces.toggleWhite.GetComponent<Toggle>().isOn = false;
        }
        pieces.toggleWhite.SetActive(whiteActive);
        pieces.toggleBlack.SetActive(blackActive);
    }

    [PunRPC]
    public void setTurn(int turn)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.turn = turn;
    }

    [PunRPC]
    public void setWhitePieces(string[] whitePieces)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.setWhitePieces(whitePieces);
    }

    [PunRPC]
    public void setBlackPieces(string[] blackPieces)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.setBlackPieces(blackPieces);
    }

    [PunRPC]
    public void setWon()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.won = true;
        p.gameEnded = true;
    }

    [PunRPC]
    public void setDraw()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.draw = true;
        p.gameEnded = true;
    }

    [PunRPC]
    public void sendRestart()
    {
        gameObject.GetComponent<UI>().restart();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Pieces p = gameObject.GetComponent<Pieces>();
            p.roomFull = false;
            PhotonNetwork.ConnectUsingSettings();
        } else
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null && room.PlayerCount == 2)
            {
                Pieces p = gameObject.GetComponent<Pieces>();
                p.roomFull = true;
            } else
            {
                Pieces p = gameObject.GetComponent<Pieces>();
                p.roomFull = false;
            }
        }
    }
}
