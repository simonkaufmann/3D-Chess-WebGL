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
        "bBishopB",
        "bKing",
        "bQueen",
        "bBishopW",
        "bKnight2",
        "bRook2"
    };

    void setFieldStartingPosition(Field[,] fields)
    {
        for (int i = 0; i < fields.GetLength(1); i++)
        {
            fields[i, 0].no = i + 8;
            fields[i, 0].player = Field.WHITE;

            fields[i, 1].no = i;
            fields[i, 1].player = Field.WHITE;

            for (int j = 2; j < 6; j++)
            {
                fields[i, j].player = Field.EMPTY;
            }

            fields[i, 6].no = 7 - i;
            fields[i, 6].player = Field.BLACK;

            fields[i, 7].no = 15 - i;
            fields[i, 7].player = Field.BLACK;
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

    void highlightField(Field field)
    {
        foreach (Field f in fields)
        {
            f.highlight1 = false;
        }
        field.highlight1 = true;
    }

    void highlightPiece(Field field)
    {
        foreach (Piece p in whitePieces)
        {
            p.highlight1 = false;
        }

        foreach (Piece p in blackPieces)
        {
            p.highlight1 = false;
        }

        if (field.player == Field.WHITE)
        {
            whitePieces[field.no].highlight1 = true;
        } else if (field.player == Field.BLACK)
        {
            blackPieces[field.no].highlight1 = true;
        }
    }

    bool isPiece(string str)
    {
        bool contained = false;
        foreach (string s in namesWhitePieces)
        {
            if (s.Equals(str))
            {
                contained = true;
            }
        }

        foreach (string s in namesBlackPieces)
        {
            if (s.Equals(str))
            {
                contained = true;
            }
        }

        return contained;
    }

    Piece getPiece(string str)
    {
        foreach (Piece p in whitePieces)
        {
            if (p.gameObject.transform.GetChild(0).transform.name.Equals(str))
            {
                return p;
            }
        }

        foreach(Piece p in blackPieces)
        {
            if (p.gameObject.transform.GetChild(0).transform.name.Equals(str))
            {
                return p;
            }
        }

        return null;
    }

    Field getFieldByPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (isPiece(hit.transform.name))
            {
                Piece p = getPiece(hit.transform.name);

                foreach (Field f in fields)
                {
                    Piece pieceField = null;
                    if (f.player == Field.WHITE)
                    {
                        pieceField = whitePieces[f.no];
                    } else if (f.player == Field.BLACK)
                    {
                        pieceField = blackPieces[f.no];
                    }

                    if (Object.ReferenceEquals(p, pieceField))
                    {
                        return f;
                    }
                }

                return null;
            } else
            {
                return getFieldByField();
            }
        }
        return null;
    }

    Field getFieldByField()
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
                    return fields[((Vector2Int)field).x, ((Vector2Int)field).y];
                }
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {

        Field f = getFieldByPiece();
        if (f != null)
        {
            highlightField(f);
            highlightPiece(f);
        }


        /*if (Input.GetMouseButtonDown(0))
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
        }*/

        Debug.DrawLine(new Vector3(-50.2f, -0.4f, 0), new Vector3(48.375f, -0.4f, 0), Color.green);
        Debug.DrawLine(new Vector3(0, -0.4f, -55.5f), new Vector3(0, -0.4f, 43.3f), Color.blue);
    }
}
