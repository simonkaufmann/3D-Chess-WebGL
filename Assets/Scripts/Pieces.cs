using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{

    string tagChessBoard = "Table";

    public static float BOARD_X_MIN = -50.2f;
    public static float BOARD_X_MAX = 48.5f; //43.3f;
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

    Vector2Int? getField(Vector2 coord)
    {
        coord.x = (coord.x - BOARD_X_MIN) / FIELD_X;
        coord.y = (coord.y - BOARD_Y_MIN) / FIELD_Y;
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        if (x >= 0 && x <= 7 && y >= 0 && y <= 7)
        {
            return new Vector2Int(x, y);
        }

        return null;
    }

    Vector2 getFieldPos(Vector2Int field)
    {
        Vector2 fieldPos = new Vector2();
        fieldPos.x = BOARD_X_MIN + FIELD_X / 2 + field.x * FIELD_X;
        fieldPos.y = BOARD_Y_MIN + FIELD_Y / 2 + field.y * FIELD_Y;
        return fieldPos;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hit;

        Vector3? fieldPos3 = null;

        hit = Physics.RaycastAll(ray);
        foreach (RaycastHit h in hit)
        {
            if (h.transform.name.Equals(tagChessBoard))
            {
                Vector2Int? field = getField(new Vector2(h.point.x, h.point.z));

                if (field != null)
                {
                    var fieldHighlighter = gameObject.transform.Find("fieldHighlighter");
                    Vector2 fieldPos = getFieldPos((Vector2Int) field);
                    fieldPos3 = new Vector3(fieldPos.x, fieldHighlighter.position.y, fieldPos.y);
                    fieldHighlighter.transform.position = new Vector3(fieldPos.x, fieldHighlighter.position.y, fieldPos.y);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit h;
            if (Physics.Raycast(ray, out h))
            {
                Debug.Log(h.transform.name);
            }

            if (fieldPos3 != null)
            {
                whitePieces[11].transform.position = (Vector3) fieldPos3;
            }
        }

        Debug.DrawLine(new Vector3(-50.2f, -0.4f, 0), new Vector3(48.375f, -0.4f, 0), Color.green);
        Debug.DrawLine(new Vector3(0, -0.4f, -55.5f), new Vector3(0, -0.4f, 43.3f), Color.blue);
    }
}
