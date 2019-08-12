using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{

    string tagChessBoard = "Table";

    public static float BOARD_X_MIN = -50.2f;
    public static float BOARD_X_MAX = 43.3f;
    public static float BOARD_Y_MIN = -55.5f;
    public static float BOARD_Y_MAX = 43.3f;
    public static float FIELD_X = (BOARD_X_MAX - BOARD_X_MIN) / 8;
    public static float FIELD_Y = (BOARD_Y_MAX - BOARD_Y_MIN) / 8;

    public static string[] namesWhitePieces =
    {
        "wPawn1",
        "wPawn2",
        "wPawn3",
        "wPawn4",
        "wPawn5",
        "wPawn6",
        "wPawn7",
        "wPawn8",
        "wRook1",
        "wKnight1",
        "wBishopB",
        "wQueen",
        "wKing",
        "wBishopW",
        "wKnight2",
        "wRook2"
    };

    public static string[] namesBlackPieces =
    {
        "bPawn1",
        "bPawn2",
        "bPawn3",
        "bPawn4",
        "bPawn5",
        "bPawn6",
        "bPawn7",
        "bPawn8",
        "bRook1",
        "bKnight1",
        "bBishopW",
        "bKing",
        "bQueen",
        "bBishopB",
        "bKnight2",
        "bRook2"
    };

    GameObject[] whitePieces;
    GameObject[] blackPieces;

    // Start is called before the first frame update
    void Start()
    {
        whitePieces = new GameObject[namesWhitePieces.Length];
        blackPieces = new GameObject[namesBlackPieces.Length];

        for (int i = 0; i < namesWhitePieces.Length; i++)
        {
            whitePieces[i] = new GameObject();
            float xCoord = BOARD_X_MIN + FIELD_X / 2 + FIELD_X * (i % 8);
            if (namesWhitePieces[i].Equals("wQueen"))
            {
                Debug.Log("xCoord: " + xCoord);
            }
            float zCoord;
            if (i < 8)
                zCoord = BOARD_Y_MIN + FIELD_Y * 3 / 2;
            else
                zCoord = BOARD_Y_MIN + FIELD_Y / 2;
            whitePieces[i].transform.position = new Vector3(xCoord, 0, zCoord);
            whitePieces[i].transform.parent = gameObject.transform.Find("chessBoard");
            whitePieces[i].name = "parent_" + namesWhitePieces[i];
            GameObject.Find(namesWhitePieces[i]).transform.SetParent(whitePieces[i].transform);
        }

        for (int i = 0; i < namesBlackPieces.Length; i++)
        {
            blackPieces[i] = new GameObject();
            float xCoord = BOARD_X_MAX - FIELD_X / 2 - FIELD_X * (i % 8);
            float zCoord;
            if (i < 8)
                zCoord = BOARD_Y_MAX - FIELD_Y * 3 / 2;
            else
                zCoord = BOARD_Y_MAX - FIELD_Y / 2;
            blackPieces[i].transform.position = new Vector3(xCoord, 0, zCoord);
            blackPieces[i].transform.parent = gameObject.transform.Find("chessBoard");
            blackPieces[i].name = "parent_" + namesBlackPieces[i];
            GameObject.Find(namesBlackPieces[i]).transform.SetParent(blackPieces[i].transform);
        }

        var rend = whitePieces[11].transform.GetChild(0).GetComponent<Renderer>();
        rend.material.color = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse down");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
            }

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.Equals(tagChessBoard))
                {
                    Debug.Log("Table");
                    Debug.Log(hit.point.ToString());
                    whitePieces[11].transform.position = hit.point;
                }
            }
        }
        Debug.DrawLine(new Vector3(-50.2f, -0.4f, 0), new Vector3(48.375f, -0.4f, 0), Color.green);
        Debug.DrawLine(new Vector3(0, -0.4f, -55.5f), new Vector3(0, -0.4f, 43.3f), Color.blue);
    }
}
