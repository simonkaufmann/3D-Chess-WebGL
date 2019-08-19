using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Networking : MonoBehaviourPunCallbacks
{

    public static int CHECKMATE = 0;
    public static int GIVENUP = 1;

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        UI ui = gameObject.GetComponent<UI>();

        Pieces p = gameObject.GetComponent<Pieces>();
        p.connected = true;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);

        UI ui = gameObject.GetComponent<UI>();
        ui.roomJoinError(message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");

        Pieces p = gameObject.GetComponent<Pieces>();
        p.roomJoined = true;

        UI ui = gameObject.GetComponent<UI>();

        // If other player already in game, then choose colour black for own player
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        Pieces pieces = gameObject.GetComponent<Pieces>();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && room.PlayerCount == 2)
        {
            pieces.panelChooseColour.SetActive(true);
            pieces.toggleWhite.GetComponent<Toggle>().isOn = false;
            pieces.panelChooseColour.SetActive(false);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created room");
        Pieces p = gameObject.GetComponent<Pieces>();
        p.connected = true;
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

        pieces.panelChooseColour.SetActive(true);
        if (player == Field.WHITE)
        {
            pieces.toggleWhite.GetComponent<Toggle>().isOn = true;
        } else if (player == Field.BLACK)
        {
            pieces.toggleWhite.GetComponent<Toggle>().isOn = false;
        }
        pieces.panelChooseColour.SetActive(false);
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
    public void setWon(int code)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        if (code == GIVENUP)
        {
            p.wonGivenUp = true;
        } else if (code == CHECKMATE)
        {
            p.wonCheckmate = true;
        }
        p.gameEnded = true;
    }

    [PunRPC]
    public void setDraw()
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.draw = true;
        p.gameEnded = true;
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
