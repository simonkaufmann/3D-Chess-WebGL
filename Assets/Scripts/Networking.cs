using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void setPlayer(int player)
    {
        Pieces p = gameObject.GetComponent<Pieces>();
        p.player = player;
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
