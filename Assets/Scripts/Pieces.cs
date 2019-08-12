using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{

    string tagChessBoard = "Table";

    Piece[] whitePieces;
    Piece[] blackPieces;

    Field[,] fields;

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

    void setFieldStartingPosition(Field[,] fields)
    {
        for (int i = 0; i < fields.GetLength(1); i++)
        {
            fields[0, i].no = i + 8;
            fields[0, i].player = Field.WHITE;

            fields[1, i].no = i;
            fields[1, i].player = Field.WHITE;

            fields[6, i].no = 7 - i;
            fields[6, i].player = Field.BLACK;

            fields[7, i].no = 15 - i;
            fields[7, i].player = Field.BLACK;
        }
    }

    void initialiseFields(Field[,] fields)
    {
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                fields[i, j] = new Field(i, j);
            }
        }
    }

    void initialisePieces(Piece[] whitePieces, Piece[] blackPieces)
    {
        for (int i = 0; i < namesWhitePieces.Length; i++)
        {
            whitePieces[i] = new Piece();
            float xCoord = Field.BOARD_X_MIN + Field.FIELD_X / 2 + Field.FIELD_X * (i % Field.FIELDS_X);
            float zCoord;
            if (i < 8)
                zCoord = Field.BOARD_Y_MIN + Field.FIELD_Y * 3 / 2;
            else
                zCoord = Field.BOARD_Y_MIN + Field.FIELD_Y / 2;
            whitePieces[i].gameObject.transform.position = new Vector3(xCoord, 0, zCoord);
            whitePieces[i].gameObject.transform.parent = gameObject.transform.Find("chessBoard");
            whitePieces[i].gameObject.name = "parent_" + namesWhitePieces[i];
            GameObject.Find(namesWhitePieces[i]).transform.SetParent(whitePieces[i].gameObject.transform);
        }

        for (int i = 0; i < namesBlackPieces.Length; i++)
        {
            blackPieces[i] = new Piece();
            float xCoord = Field.BOARD_X_MAX - Field.FIELD_X / 2 - Field.FIELD_X * (i % Field.FIELDS_X);
            float zCoord;
            if (i < 8)
                zCoord = Field.BOARD_Y_MAX - Field.FIELD_Y * 3 / 2;
            else
                zCoord = Field.BOARD_Y_MAX - Field.FIELD_Y / 2;
            blackPieces[i].gameObject.transform.position = new Vector3(xCoord, 0, zCoord);
            blackPieces[i].gameObject.transform.parent = gameObject.transform.Find("chessBoard");
            blackPieces[i].gameObject.name = "parent_" + namesBlackPieces[i];
            GameObject.Find(namesBlackPieces[i]).transform.SetParent(blackPieces[i].gameObject.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("blueprintFieldHighlighter").GetComponent<Renderer>().enabled = false;

        whitePieces = new Piece[namesWhitePieces.Length];
        blackPieces = new Piece[namesBlackPieces.Length];

        fields = new Field[8, 8];

        initialiseFields(fields);

        setFieldStartingPosition(fields);

        initialisePieces(whitePieces, blackPieces);
    }

    void highlightField(Vector2Int field)
    {
        foreach (Field f in fields)
        {
            f.highlight1 = false;
        }
        fields[field.x, field.y].highlight1 = true;
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
                Vector2Int? field = Field.getField(new Vector2(h.point.x, h.point.z));

                if (field != null)
                {
                    highlightField((Vector2Int) field);
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
                whitePieces[11].gameObject.transform.position = (Vector3) fieldPos3;
            }
        }

        Debug.DrawLine(new Vector3(-50.2f, -0.4f, 0), new Vector3(48.375f, -0.4f, 0), Color.green);
        Debug.DrawLine(new Vector3(0, -0.4f, -55.5f), new Vector3(0, -0.4f, 43.3f), Color.blue);
    }
}
