using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Networking : MonoBehaviourPunCallbacks
{

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
        Debug.Log("Joined Room1");
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

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
