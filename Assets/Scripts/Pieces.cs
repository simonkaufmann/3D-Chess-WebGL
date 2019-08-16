using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pieces : MonoBehaviour
{

    string tagChessBoard = "Table";

    Piece[] whitePieces;
    Piece[] blackPieces;

    Field[,] fields;

    public int player = Field.WHITE;

    public bool myTurn = true;

    public bool blackCastlingSmall = true;
    public bool blackCastlingBig = true;
    public bool whiteCastlingSmall = true;
    public bool whiteCastlingBig = true;

    public Camera cameraWhite;
    public Camera cameraBlack;

    public bool active = false;

    Vector2 POSITION_OFF_SCREEN = new Vector2(-10000, -10000);

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

    Field selectedField = null;

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
        if (field != null)
        {
            field.highlight1 = true;
        }
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

        Piece piece = getPiece(field);
        if (piece != null)
        {
            piece.highlight1 = true;
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

    Piece getPiece(Field[,] fields, string str)
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

    Piece getPiece(Field f)
    {
        if (f == null)
        {
            return null;
        }
        if (f.player == Field.WHITE)
        {
            return (whitePieces[f.no]);
        }
        else if (f.player == Field.BLACK)
        {
            return (blackPieces[f.no]);
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
                Piece p = getPiece(fields, hit.transform.name);

                foreach (Field f in fields)
                {
                    Piece pieceField = getPiece(f);

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

    bool pieceIsType(Piece p, string type)
    {
        if (p.gameObject.transform.GetChild(0).transform.name.Contains(type))
        {
            return true;
        } else
        {
            return false;
        }
    }

    void highlight2Fields(List<Field> fs)
    {
        foreach (Field f in fs)
        {
            f.highlight2 = true;
        }
    }

    List<Field> getPawnMoves(Field[,] fields, int player, Field f)
    {
        return getPawnMoves(fields, player, f, false);
    }

    List<Field> getPawnMoves(Field[,] fields, int player, Field f, bool threatened)
    {
        List<Field> fs = new List<Field>();
        int row = f.row;
        int col = f.col;
        if (player == Field.WHITE)
        {
            if (row + 1 < Field.FIELDS_Y)
            {
                if (fields[col, row + 1].player == Field.EMPTY && !threatened)
                {
                    fs.Add(fields[col, row + 1]);

                    if (row == 1)
                    {
                        if (fields[col, row + 2].player == Field.EMPTY && !threatened)
                        {
                            fs.Add(fields[col, row + 2]);
                        }
                    }
                }

                if (col - 1 > 0)
                {
                    if (fields[col - 1, row + 1].player == Field.BLACK || threatened)
                    {
                        fs.Add(fields[col - 1, row + 1]);
                    }
                }

                if (col + 1 < Field.FIELDS_X)
                {
                    if (fields[col + 1, row + 1].player == Field.BLACK || threatened)
                    {
                        fs.Add(fields[col + 1, row + 1]);
                    }
                }
            }
        }

        if (player == Field.BLACK)
        {
            if (row - 1 >= 0)
            {
                if (fields[col, row - 1].player == Field.EMPTY && !threatened)
                {
                    fs.Add(fields[col, row - 1]);

                    if (row == 6)
                    {
                        if (fields[col, row - 2].player == Field.EMPTY && !threatened)
                        {
                            fs.Add(fields[col, row - 2]);
                        }
                    }
                }

                if (col - 1 > 0)
                {
                    if (fields[col - 1, row - 1].player == Field.WHITE || threatened)
                    {
                        fs.Add(fields[col - 1, row - 1]);
                    }
                }

                if (col + 1 < Field.FIELDS_X)
                {
                    if (fields[col + 1, row - 1].player == Field.WHITE || threatened)
                    {
                        fs.Add(fields[col + 1, row - 1]);
                    }
                }
            }
        }

        return fs;
    }

    List<Field> getHorizontalMoves(Field[,] fields, int player, Field f)
    {
        List<Field> fs = new List<Field>();
        int row = f.row;
        int col = f.col;

        for (int c = col + 1; c < Field.FIELDS_X; c++)
        {
            if (fields[c, row].player == Field.EMPTY)
            {
                fs.Add(fields[c, row]);
            }
            else
            {
                if (fields[c, row].player != player)
                {
                    fs.Add(fields[c, row]);
                }
                break;
            }
        }

        for (int c = col - 1; c >= 0; c--)
        {
            if (fields[c, row].player == Field.EMPTY)
            {
                fs.Add(fields[c, row]);
            }
            else
            {
                if (fields[c, row].player != player)
                {
                    fs.Add(fields[c, row]);
                }
                break;
            }
        }
        return fs;
    }

    List<Field> getVerticalMoves(Field[,] fields, int player, Field f)
    {
        List<Field> fs = new List<Field>();
        int row = f.row;
        int col = f.col;

        for (int r = row + 1; r < Field.FIELDS_Y; r++)
        {
            if (fields[col, r].player == Field.EMPTY)
            {
                fs.Add(fields[col, r]);
            }
            else
            {
                if (fields[col, r].player != player)
                {
                    fs.Add(fields[col, r]);
                }
                break;
            }
        }

        for (int r = row - 1; r >= 0; r--)
        {
            if (fields[col, r].player == Field.EMPTY)
            {
                fs.Add(fields[col, r]);
            }
            else
            {
                if (fields[col, r].player != player)
                {
                    fs.Add(fields[col, r]);
                }
                break;
            }
        }
        return fs;
    }

    List<Field> getDiagonalMoves(Field[,] fields, int player, Field f)
    {
        List<Field> fs = new List<Field>();
        int row = f.row;
        int col = f.col;

        int r = row + 1;
        int c = col + 1;
        while (r < Field.FIELDS_Y && c < Field.FIELDS_X)
        {
            if (fields[c, r].player == Field.EMPTY)
            {
                fs.Add(fields[c, r]);
            }
            else
            {
                if (fields[c, r].player != player)
                {
                    fs.Add(fields[c, r]);
                }
                break;
            }
            r++;
            c++;
        }

        r = row - 1;
        c = col - 1;
        while (r >= 0 && c >= 0)
        {
            if (fields[c, r].player == Field.EMPTY)
            {
                fs.Add(fields[c, r]);
            }
            else
            {
                if (fields[c, r].player != player)
                {
                    fs.Add(fields[c, r]);
                }
                break;
            }
            r--;
            c--;
        }

        r = row - 1;
        c = col + 1;
        while (r >= 00 && c < Field.FIELDS_X)
        {
            if (fields[c, r].player == Field.EMPTY)
            {
                fs.Add(fields[c, r]);
            }
            else
            {
                if (fields[c, r].player != player)
                {
                    fs.Add(fields[c, r]);
                }
                break;
            }
            r--;
            c++;
        }

        r = row + 1;
        c = col - 1;
        while (r < Field.FIELDS_Y && c >= 0)
        {
            if (fields[c, r].player == Field.EMPTY)
            {
                fs.Add(fields[c, r]);
            }
            else
            {
                if (fields[c, r].player != player)
                {
                    fs.Add(fields[c, r]);
                }
                break;
            }
            r++;
            c--;
        }
        return fs;
    }

    List<Field> getKnightMoves(Field[,] fields, int player, Field f)
    {
        List<Field> fs = new List<Field>();
        int row = f.row;
        int col = f.col;

        if (row + 2 < Field.FIELDS_Y)
        {
            if (col + 1 < Field.FIELDS_X)
            {
                if (fields[col + 1, row + 2].player != player)
                {
                    fs.Add(fields[col + 1, row + 2]);
                }
            }
            if (col - 1 >= 0)
            {
                if (fields[col - 1, row + 2].player != player)
                {
                    fs.Add(fields[col - 1, row + 2]);
                }
            }
        }

        if (row - 2 >= 0)
        {
            if (col + 1 < Field.FIELDS_X)
            {
                if (fields[col + 1, row - 2].player != player)
                {
                    fs.Add(fields[col + 1, row - 2]);
                }
            }
            if (col - 1 >= 0)
            {
                if (fields[col - 1, row - 2].player != player)
                {
                    fs.Add(fields[col - 1, row - 2]);
                }
            }
        }

        if (col + 2 < Field.FIELDS_X)
        {
            if (row + 1 < Field.FIELDS_Y)
            {
                if (fields[col + 2, row + 1].player != player)
                {
                    fs.Add(fields[col + 2, row + 1]);
                }
            }
            if (row - 1 >= 0)
            {
                if (fields[col + 2, row - 1].player != player)
                {
                    fs.Add(fields[col + 2, row - 1]);
                }
            }
        }

        if (col - 2 >= 0)
        {
            if (row + 1 < Field.FIELDS_Y)
            {
                if (fields[col - 2, row + 1].player != player)
                {
                    fs.Add(fields[col - 2, row + 1]);
                }
            }
            if (row - 1 >= 0)
            {
                if (fields[col - 2, row - 1].player != player)
                {
                    fs.Add(fields[col - 2, row - 1]);
                }
            }
        }
        return fs;
    }

    bool validKingField(Field[,] fields, int player, Field kingPosition, Field field)
    {
        if (field.player == player)
        {
            return false;
        }

        int kingFieldPlayer = kingPosition.player;
        kingPosition.player = Field.EMPTY; // We need to check moves while assuming king is not there

        bool isKingField = false;

        List<Field> allMoves = new List<Field>();

        Field opponentKingField = fields[0, 0];

        foreach (Field f in fields)
        {
            if (player == Field.WHITE)
            {
                if (f.player == Field.BLACK)
                {
                    allMoves.AddRange(getMoves(fields, f, true, false));


                    if (pieceIsType(getPiece(f), "King"))
                    {
                        opponentKingField = f;
                    }
                }
            } else if (player == Field.BLACK)
            {
                if (f.player == Field.WHITE)
                {
                    allMoves.AddRange(getMoves(fields, f, true, false));
                    
                    if (pieceIsType(getPiece(f), "King"))
                    {
                        opponentKingField = f;
                    }
                }
            }
        }

        if (allMoves.Contains(field))
        {
            isKingField = false;
        }
        else if (Mathf.Abs(opponentKingField.row - field.row) <= 1 && Mathf.Abs(opponentKingField.col - field.col) <= 1)
        {
            // too close to other king
            isKingField = false;
        }
        else
        {
            isKingField = true;
        }

        kingPosition.player = kingFieldPlayer;

        return isKingField;
    }

    List<Field> getKingMoves(Field[,] fields, int player, Field f)
    {
        List<Field> fs = new List<Field>();

        int row = f.row;
        int col = f.col;

        if (row + 1 < Field.FIELDS_Y)
        {
            if (col + 1 < Field.FIELDS_X)
            {
                if (validKingField(fields, player, f, fields[col + 1, row + 1]))
                {
                    fs.Add(fields[col + 1, row + 1]);
                }
            }

            if (validKingField(fields, player, f, fields[col, row + 1]))
            {
                fs.Add(fields[col, row + 1]);
            }

            if (col - 1 >= 0)
            {
                if (validKingField(fields, player, f, fields[col - 1, row + 1]))
                {
                    fs.Add(fields[col - 1, row + 1]);
                }
            }
        }

        if (col + 1 < Field.FIELDS_X)
        {
            if (validKingField(fields, player, f, fields[col + 1, row]))
            {
                fs.Add(fields[col + 1, row]);
            }
        }

        if (col - 1 >= 0)
        {
            if (validKingField(fields, player, f, fields[col - 1, row]))
            {
                fs.Add(fields[col - 1, row]);
            }
        }

        if (row - 1 >= 0)
        {
            if (col + 1 < Field.FIELDS_X)
            {
                if (validKingField(fields, player, f, fields[col + 1, row - 1]))
                {
                    fs.Add(fields[col + 1, row - 1]);
                }
            }

            if (validKingField(fields, player, f, fields[col, row - 1]))
            {
                fs.Add(fields[col, row - 1]);
            }

            if (col - 1 >= 0)
            {
                if (validKingField(fields, player, f, fields[col - 1, row - 1]))
                {
                    fs.Add(fields[col - 1, row - 1]);
                }
            }
        }

        if (player == Field.WHITE && whiteCastlingBig)
        {
            if (fields[1, 0].player == Field.EMPTY && fields[2, 0].player == Field.EMPTY && fields[3,0].player == Field.EMPTY)
            {
                fs.Add(fields[2, 0]);
            }
        }

        if (player == Field.WHITE && whiteCastlingSmall)
        {
            if (fields[5, 0].player == Field.EMPTY && fields[6, 0].player == Field.EMPTY)
            {
                fs.Add(fields[6, 0]);
            }
        }

        if (player == Field.BLACK && blackCastlingBig)
        {
            if (fields[1, 7].player == Field.EMPTY && fields[2, 7].player == Field.EMPTY && fields[3, 7].player == Field.EMPTY)
            {
                fs.Add(fields[2, 7]);
            }
        }

        if (player == Field.BLACK && blackCastlingSmall)
        {
            if (fields[5, 7].player == Field.EMPTY && fields[6, 7].player == Field.EMPTY)
            {
                fs.Add(fields[6, 7]);
            }
        }

        return fs;
    }

    Field[,] cloneFields(Field[,] fields)
    {
        Field[,] new_fields = new Field[fields.GetLength(0), fields.GetLength(1)];
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                new_fields[i, j] = fields[i, j].Clone();
            }
        }
        return new_fields;
    }

    List<Field> getMoves(Field[,] fields, Field f)
    {
        return getMoves(fields, f, false);
    }

    List<Field> getMoves(Field[,] fields, Field f, bool threatened)
    {
        return getMoves(fields, f, threatened, true);
    }

    List<Field> getMoves(Field[,] fields, Field f, bool threatened, bool checkForChess)
    {
        List<Field> fs = new List<Field>();

        Piece p = getPiece(f);
        if (p == null)
        {
            return fs;
        }

        /*if (checkForChess)
        {
            if (isCheck(fields) && !pieceIsType(getPiece(f), "King"))
            {
                return fs;
            }
        } */

        if (pieceIsType(p, "Pawn"))
        {
            fs = getPawnMoves(fields, f.player, f, threatened);
        }
        else if (pieceIsType(p, "Rook"))
        {
            fs = getHorizontalMoves(fields, f.player, f);
            fs.AddRange(getVerticalMoves(fields, f.player, f));
        }
        else if (pieceIsType(p, "Knight"))
        {
            fs = getKnightMoves(fields, f.player, f);
        }
        else if (pieceIsType(p, "Bishop"))
        {
            fs = getDiagonalMoves(fields, f.player, f);
        }
        else if (pieceIsType(p, "Queen"))
        {
            fs = getDiagonalMoves(fields, f.player, f);
            fs.AddRange(getHorizontalMoves(fields, f.player, f));
            fs.AddRange(getVerticalMoves(fields, f.player, f));
        }
        else if (pieceIsType(p, "King"))
        {
            if (threatened)
            {
                return fs;
            }
            else
            {
                fs = getKingMoves(fields, f.player, f);
            }
        }

        List<Field> removals = new List<Field>();
        foreach (Field field in fs)
        {
            if (field.player != Field.EMPTY)
            {
                Piece piece = getPiece(field);
                if (pieceIsType(piece, "King"))
                {
                    if (checkForChess)
                    {
                        removals.Add(field);
                    }
                }
            }
        }

        if (checkForChess)
        {
            foreach (Field field in fs)
            {
                Field[,] new_fields = cloneFields(fields);

                Field f1 = new_fields[f.col, f.row];
                Field f2 = new_fields[field.col, field.row];

                f2.no = f1.no;
                f2.player = f1.player;

                f1.player = Field.EMPTY;

                if (isCheck(new_fields))
                {
                    if (!removals.Contains(field))
                    {
                        removals.Add(field);
                    }
                }
            }
        }

        foreach (Field field in removals)
        {
            fs.Remove(field);
        }

        return fs;
    }

    bool isCheck(Field[,] fields)
    {
        foreach (Field f in fields)
        {
            if (f.player != Field.EMPTY && f.player != player)
            {
                List<Field> moves = getMoves(fields, f, true, false);
                foreach (Field fi in moves)
                {
                    if (fi.player == player && pieceIsType(getPiece(fi), "King"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void selectField(Field f)
    {
        foreach (Field field in fields)
        {
            field.highlight2 = false;
        }

        if (f == null)
        {
            selectedField = null;
            return;
        }

        if (f.player != Field.EMPTY)
        {
            selectedField = f;
        }

        if (f.player == Field.WHITE || f.player == Field.BLACK)
        {
            List<Field> fs = getMoves(fields, f);
            highlight2Fields(fs);
        }
    }

    void movePiece(Field f1, Field f2)
    {
        /*Piece p = getPiece(f1);
        Vector2 pos = Field.getFieldPos(new Vector2Int(f2.col, f2.row));

        if (f2.player != Field.EMPTY)
        {
            Piece p2 = getPiece(f2);
            GameObject p2go = p2.gameObject;
            p2go.transform.position = new Vector3(POSITION_OFF_SCREEN.x, p2go.transform.position.y, POSITION_OFF_SCREEN.y);
        }

        GameObject go = p.gameObject;
        go.transform.position = new Vector3(pos.x, go.transform.position.y, pos.y);*/

        f2.player = f1.player;
        f2.no = f1.no;
        f1.player = Field.EMPTY;

        placePieces();
    }

    void checkKingMoved()
    {
        if (fields[4, 0].player != Field.WHITE || pieceIsType(getPiece(fields[4, 0]), "King") == false)
        {
            whiteCastlingBig = false;
            whiteCastlingSmall = false;
        }

        if (fields[4, 7].player != Field.BLACK || pieceIsType(getPiece(fields[4, 7]), "King") == false)
        {
            blackCastlingBig = false;
            blackCastlingSmall = false;
        }

        if (fields[0, 0].player != Field.WHITE || pieceIsType(getPiece(fields[0, 0]), "Rook") == false)
        {
            whiteCastlingBig = false;
        }

        if (fields[7, 0].player != Field.WHITE || pieceIsType(getPiece(fields[7, 0]), "Rook") == false)
        {
            whiteCastlingBig = false;
        }

        if (fields[0, 7].player != Field.BLACK || pieceIsType(getPiece(fields[0, 7]), "Rook") == false)
        {
            blackCastlingSmall = false;
        }

        if (fields[7, 7].player != Field.BLACK || pieceIsType(getPiece(fields[7, 7]), "Rook") == false)
        {
            blackCastlingBig = false;
        }
    }

    void sendBoardStatus()
    {
        string[] str = new string[fields.GetLength(0) * fields.GetLength(1)];
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                str[i * fields.GetLength(1) + j] = JsonUtility.ToJson(fields[i, j]);
            }
        }

        checkKingMoved();

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        photonView.RPC("sendMove", RpcTarget.Others, str);
    }

    void placePieces()
    {
        List<Piece> remainingWhite = new List<Piece>();
        List<Piece> remainingBlack = new List<Piece>();
        foreach (Piece p in whitePieces)
        {
            remainingWhite.Add(p);
            p.gameObject.transform.position = new Vector3(POSITION_OFF_SCREEN.x, p.gameObject.transform.position.y, POSITION_OFF_SCREEN.y);
        }
        foreach (Piece p in blackPieces)
        {
            remainingBlack.Add(p);
            p.gameObject.transform.position = new Vector3(POSITION_OFF_SCREEN.x, p.gameObject.transform.position.y, POSITION_OFF_SCREEN.y);
        }

        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                Piece p = getPiece(fields[i, j]);
                if (p != null)
                {
                    Vector2 pos = Field.getFieldPos(new Vector2Int(fields[i, j].col, fields[i, j].row));
                    p.gameObject.transform.position = new Vector3(pos.x, p.gameObject.transform.position.y, pos.y);
                    if (fields[i, j].player == Field.WHITE)
                    {
                        remainingWhite.Remove(p);
                    } else if (fields[i, j].player == Field.BLACK)
                    {
                        remainingBlack.Remove(p);
                    }
                }
            }
        }

        for (int i = 0; i < remainingWhite.Count; i++)
        {
            Vector2 pos;
            if (i < 8)
            {
                pos = Field.getFieldPos(new Vector2Int(9, 7 - i % 8));
            } else
            {
                pos = Field.getFieldPos(new Vector2Int(10, 7 - i % 8));
            }


            remainingWhite[i].gameObject.transform.position = new Vector3(pos.x, remainingWhite[i].gameObject.transform.position.y, pos.y);
        }

        for (int i = 0; i < remainingBlack.Count; i++)
        {
            Vector2 pos;
            if (i < 8)
            {
                pos = Field.getFieldPos(new Vector2Int(-2, 7 - i % 8));
            }
            else
            {
                pos = Field.getFieldPos(new Vector2Int(-3, 7 - i % 8));
            }


            remainingBlack[i].gameObject.transform.position = new Vector3(pos.x, remainingBlack[i].gameObject.transform.position.y, pos.y);
        }
    }

    public void receiveBoardStatus(Field[] fs)
    {
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                if (fields[i, j].no != fs[i * fields.GetLength(1) + j].no || fields[i, j].player != fs[i * fields.GetLength(1) + j].player)
                {
                    fields[i, j].highlight3 = true;
                    Piece p = getPiece(fields[i, j]);
                    if (p != null)
                    {
                        p.highlight3 = true;
                    }
                }

                fields[i, j].no = fs[i * fields.GetLength(1) + j].no;
                fields[i, j].player = fs[i * fields.GetLength(1) + j].player;
            }
        }

        myTurn = true;
        checkKingMoved();

        placePieces();
    }

    void checkCastling(Field f)
    {
        if (whiteCastlingBig && f.row == 0 && f.col == 2 && selectedField.player == Field.WHITE && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[0, 0], fields[3, 0]);
        }

        if (whiteCastlingSmall && f.row == 0 && f.col == 6 && selectedField.player == Field.WHITE && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[7, 0], fields[5, 0]);
        }

        if (blackCastlingBig && f.row == 7 && f.col == 2 && selectedField.player == Field.BLACK && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[0, 7], fields[3, 7]);
        }

        if (blackCastlingSmall && f.row == 7 && f.col == 6 && selectedField.player == Field.BLACK && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[7, 7], fields[5, 7]);
        }
    }

    void setCameras()
    {
        if (player == Field.WHITE)
        {
            cameraWhite.gameObject.SetActive(true);
            cameraBlack.gameObject.SetActive(false);

            GameObject.Find("lightWhite").GetComponent<Light>().enabled = true;
            GameObject.Find("lightBlack").GetComponent<Light>().enabled = false;

            Image img = GameObject.Find("panelTurn").GetComponent<Image>();
            if (myTurn)
            {
                img.color = Color.white;
            }
            else
            {
                img.color = Color.black;
            }
        }
        else
        {
            cameraWhite.gameObject.SetActive(false);
            cameraBlack.gameObject.SetActive(true);

            GameObject.Find("lightWhite").GetComponent<Light>().enabled = false;
            GameObject.Find("lightBlack").GetComponent<Light>().enabled = true;

            Image img = GameObject.Find("panelTurn").GetComponent<Image>();
            if (myTurn)
            {
                img.color = Color.black;
            }
            else
            {
                img.color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        setCameras();

        if (!active)
        {

            return;
        }

        // Delete green highlighting from received fields after every click
        if (Input.GetMouseButtonDown(0))
        {
            foreach (Field f in fields)
            {
                f.highlight3 = false;
            }
            foreach (Piece p in whitePieces)
            {
                p.highlight3 = false;
            }
            foreach (Piece p in blackPieces)
            {
                p.highlight3 = false;
            }
        }

        // None selected -> highlight field under cursor and if clicked select that field
        if (selectedField == null)
        {
            Field f = getFieldByPiece();
            highlightField(f);
            highlightPiece(f);
            if (f != null)
            {
                if (Input.GetMouseButtonDown(0) && myTurn)
                {
                    if (f != null)
                    {
                        if (f.player == player)
                        {
                            selectField(f);
                        }
                    }
                }
            }
        }
        // I field is selected, then check wheter it needs to be moved
        else
        {
            if (myTurn)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Field f = getFieldByField();

                    List<Field> allMoves = getMoves(fields, selectedField);
                    if (allMoves.Contains(f))
                    {
                        checkCastling(f);
                        movePiece(selectedField, f);
                        sendBoardStatus();
                        myTurn = false;
                    }

                    selectField(null);
                }
            }
            else
            {
                selectField(null);
            }
        }

        Debug.DrawLine(new Vector3(-50.2f, -0.4f, 0), new Vector3(48.375f, -0.4f, 0), Color.green);
        Debug.DrawLine(new Vector3(0, -0.4f, -55.5f), new Vector3(0, -0.4f, 43.3f), Color.blue);
    }
}
