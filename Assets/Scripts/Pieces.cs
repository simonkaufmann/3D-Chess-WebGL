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
    Field[,] oldFields;

    public int player = Field.WHITE;
    int oldPlayer = Field.WHITE;

    bool cameraTop = false;

    public int turn = Field.WHITE;

    public bool blackCastlingSmall = true;
    public bool blackCastlingBig = true;
    public bool whiteCastlingSmall = true;
    public bool whiteCastlingBig = true;

    public Camera cameraWhite;
    public Camera cameraBlack;
    public Camera cameraWhiteTop;
    public Camera cameraBlackTop;

    public bool giveUpDialog = false;
    public bool restartDialog = false;
    public bool connected = false;
    public bool roomJoined = false;
    public bool gameEnded = false;
    public bool gameStarted = false;
    public bool wonCheckmate = false;
    public bool wonGivenUp = false;
    public bool checkmate = false;
    public bool draw = false;
    public bool roomFull = false;

    public GameObject buttonGiveUp;
    public GameObject txtInputRoom;
    public GameObject txtRoomError;
    public GameObject txtInputPlayerName;
    public GameObject txtWaitForPlayerRoomName;
    public GameObject txtPlayerWhite;
    public GameObject txtPlayerBlack;
    GameObject panelLoad;
    GameObject panelPawnPromotion;
    GameObject panelCheck;
    GameObject panelCheckmate;
    public GameObject txtTurnPlayerWhite;
    public GameObject txtTurnPlayerBlack;
    GameObject panelTurnPlayerWhite;
    GameObject panelTurnPlayerBlack;
    GameObject panelWonCheckmate;
    GameObject panelWonGivenUp;
    GameObject panelWaitForPlayer;
    public GameObject toggleWhite;
    public GameObject toggleBlack;
    GameObject panelDraw;
    Field fieldPawnPromotion = null;
    public GameObject panelChooseColour;
    GameObject panelTurn;
    GameObject panelLostGivenUp;
    public GameObject panelRestart;
    public GameObject panelGiveUp;
    public GameObject panelRoomSelection;

    Vector2 POSITION_OFF_SCREEN = new Vector2(-10000, -10000);

    public static int ROTATION_ANGLE_WHITE = -90;
    public static int ROTATION_ANGLE_BLACK = 90;

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

    public string[] typesWhitePieces =
    {
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Rook",
        "Knight",
        "Bishop",
        "Queen",
        "King",
        "Bishop",
        "Knight",
        "Rook"
    };

    public string[] typesBlackPieces =
    {
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Pawn",
        "Rook",
        "Knight",
        "Bishop",
        "King",
        "Queen",
        "Bishop",
        "Knight",
        "Rook"
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
        oldFields = cloneFields(fields);
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
            // For rotating knights:
            whitePieces[i].gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_WHITE, 0));
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
            // For rotating knights:
            blackPieces[i].gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_BLACK, 0));
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

        buttonGiveUp = GameObject.Find("buttonGiveUp");
        txtInputRoom = GameObject.Find("txtInputRoom");
        txtRoomError = GameObject.Find("txtRoomError");
        txtInputPlayerName = GameObject.Find("txtInputPlayerName");
        txtWaitForPlayerRoomName = GameObject.Find("txtWaitForPlayerRoomName");
        txtPlayerWhite = GameObject.Find("txtPlayerWhite");
        txtPlayerBlack = GameObject.Find("txtPlayerBlack");
        panelLoad = GameObject.Find("panelLoad");
        panelPawnPromotion = GameObject.Find("panelPawnPromotion");
        panelPawnPromotion.SetActive(false);
        panelCheck = GameObject.Find("panelCheck");
        panelCheck.SetActive(false);
        panelCheckmate = GameObject.Find("panelCheckmate");
        panelCheckmate.SetActive(false);
        txtTurnPlayerWhite = GameObject.Find("txtTurnPlayerWhite");
        txtTurnPlayerBlack = GameObject.Find("txtTurnPlayerBlack");
        txtTurnPlayerBlack.SetActive(false);
        panelTurnPlayerWhite = GameObject.Find("panelTurnPlayerWhite");
        panelTurnPlayerBlack = GameObject.Find("panelTurnPlayerBlack");
        panelTurnPlayerBlack.SetActive(false);
        panelWonCheckmate = GameObject.Find("panelWonCheckmate");
        panelWonCheckmate.SetActive(false);
        panelWonGivenUp = GameObject.Find("panelWonGivenUp");
        panelWonGivenUp.SetActive(false);
        panelWaitForPlayer = GameObject.Find("panelWaitForPlayer");
        panelWaitForPlayer.SetActive(false);
        toggleWhite = GameObject.Find("toggleWhite");
        toggleBlack = GameObject.Find("toggleBlack");
        panelDraw = GameObject.Find("panelDraw");
        panelDraw.SetActive(false);
        panelTurn = GameObject.Find("panelTurn");
        panelTurn.SetActive(false);
        panelChooseColour = GameObject.Find("panelChooseColour");
        panelChooseColour.SetActive(false);
        panelRestart = GameObject.Find("panelRestart");
        panelRestart.SetActive(false);
        panelGiveUp = GameObject.Find("panelGiveUp");
        panelGiveUp.SetActive(false);
        panelRoomSelection = GameObject.Find("panelRoomSelection");
        panelRoomSelection.SetActive(false);
        panelLostGivenUp = GameObject.Find("panelLostGivenUp");
        panelLostGivenUp.SetActive(false);

        /*foreach (Field f in fields)
        {
            f.player = Field.EMPTY;
            //f.highlight3 = true;
        }
        fields[4, 0].player = Field.WHITE;
        fields[4, 0].no = 12;
        fields[5, 0].player = Field.WHITE;
        fields[5, 0].no = 8;
        fields[6, 0].player = Field.WHITE;
        fields[6, 0].no = 15;
        fields[7, 6].player = Field.WHITE;
        fields[7, 6].no = 7;
        fields[7, 7].no = 11;
        fields[7, 7].player = Field.BLACK;
        fields[1, 1].no = 1;
        fields[1, 6].player = Field.BLACK;
        fields[2, 6].no = 2;
        placePieces();*/
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

    void highlightPiece(Piece p)
    {
        foreach (Piece pi in whitePieces)
        {
            pi.highlight1 = false;
        }

        foreach (Piece pi in blackPieces)
        {
            pi.highlight1 = false;
        }

        if (p == null)
        {
            return;
        }

        p.highlight1 = true;
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

        foreach (Piece p in blackPieces)
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

    Piece getPieceByPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (isPiece(hit.transform.name))
            {
                for (int i = 0; i < namesWhitePieces.Length; i++)
                {
                    if (namesWhitePieces[i].Equals(hit.transform.name))
                    {
                        return whitePieces[i];
                    }
                }
                for (int i = 0; i < namesBlackPieces.Length; i++)
                {
                    if (namesBlackPieces[i].Equals(hit.transform.name))
                    {
                        return blackPieces[i];
                    }
                }
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
        if (p == null)
        {
            return false;
        }
        for (int i = 0; i < whitePieces.Length; i++)
        {
            if (Object.ReferenceEquals(p, whitePieces[i]))
            {
                if (type.Equals(typesWhitePieces[i]))
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < blackPieces.Length; i++)
        {
            if (Object.ReferenceEquals(p, blackPieces[i]))
            {
                if (type.Equals(typesBlackPieces[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void highlight2Fields(List<Field> fs)
    {
        foreach (Field field in fields)
        {
            field.highlight2 = false;
        }
        if (fs == null)
        {
            return;
        }
        foreach (Field f in fs)
        {
            f.highlight2 = true;
        }
    }

    void highlight2Pieces(List<Field> fs)
    {
        foreach (Piece p in whitePieces)
        {
            p.highlight2 = false;
        }
        foreach (Piece p in blackPieces)
        {
            p.highlight2 = false;
        }
        if (fs == null)
        {
            return;
        }
        foreach (Field f in fs)
        {
            Piece piece = getPiece(f);
            if (piece != null)
            {
                piece.highlight2 = true;
            }
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

        // For EnPassant:
        if (player == Field.WHITE && row == 4)
        {
            if (col > 0)
            {
                if (oldFields[col - 1, 6].player == Field.BLACK && pieceIsType(getPiece(oldFields[col - 1, 6]), "Pawn")
                    && fields[col - 1, 6].player == Field.EMPTY && pieceIsType(getPiece(fields[col - 1, 4]), "Pawn")
                    && fields[col - 1, 5].player == Field.EMPTY)
                {
                    fs.Add(fields[col - 1, 5]);
                }
            }

            if (col < Field.FIELDS_X - 1)
            {
                if (oldFields[col + 1, 6].player == Field.BLACK && pieceIsType(getPiece(oldFields[col + 1, 6]), "Pawn")
                    && fields[col + 1, 6].player == Field.EMPTY && pieceIsType(getPiece(fields[col + 1, 4]), "Pawn")
                    && fields[col + 1, 5].player == Field.EMPTY)
                {
                    fs.Add(fields[col + 1, 5]);
                }
            }
        }

        if (player == Field.BLACK && row == 3)
        {
            if (col > 0)
            {
                if (oldFields[col - 1, 1].player == Field.BLACK && pieceIsType(getPiece(oldFields[col - 1, 1]), "Pawn")
                    && fields[col - 1, 1].player == Field.EMPTY && pieceIsType(getPiece(fields[col - 1, 3]), "Pawn")
                    && fields[col - 1, 2].player == Field.EMPTY)
                {
                    fs.Add(fields[col - 1, 2]);
                }
            }

            if (col < Field.FIELDS_X - 1)
            {
                if (oldFields[col + 1, 1].player == Field.BLACK && pieceIsType(getPiece(oldFields[col + 1, 1]), "Pawn")
                    && fields[col + 1, 1].player == Field.EMPTY && pieceIsType(getPiece(fields[col + 1, 3]), "Pawn")
                    && fields[col + 1, 2].player == Field.EMPTY)
                {
                    fs.Add(fields[col + 1, 2]);
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
        if (kingPosition != null)
        {
            kingPosition.player = Field.EMPTY; // We need to check moves while assuming king is not there
        }

        bool isKingField = false;

        List<Field> allMoves = new List<Field>();

        Field opponentKingField = new Field(0, 0);

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

    bool canCastleBig(int player)
    {
        if (player == Field.WHITE)
        {
            if (!(fields[1, 0].player == Field.EMPTY) || !(fields[2, 0].player == Field.EMPTY) || !(fields[3, 0].player == Field.EMPTY))
            {
                return false; // check whether fields are free
            }
            if (!validKingField(fields, Field.WHITE, null, fields[1, 0]) || !validKingField(fields, Field.WHITE, null, fields[2, 0]) || !validKingField(fields, Field.WHITE, null, fields[3, 0]))
            {
                return false; // check whether none of the fields is in check
            }
            return (whiteCastlingBig && !isCheck(fields, Field.WHITE));
        } else if (player == Field.BLACK)
        {
            if (!(fields[1, 7].player == Field.EMPTY) || !(fields[2, 7].player == Field.EMPTY) || !(fields[3, 7].player == Field.EMPTY))
            {
                return false; // check whether fields are free
            }
            if (!validKingField(fields, Field.BLACK, null, fields[1, 7]) || !validKingField(fields, Field.BLACK, null, fields[2, 7]) || !validKingField(fields, Field.BLACK, null, fields[3, 7]))
            {
                return false; // check whether none of the fields is in check
            }
            return (blackCastlingBig && !isCheck(fields, Field.BLACK));
        }
        return false;
    }

    bool canCastleSmall(int player)
    {
        if (player == Field.WHITE)
        {
            if (!(fields[5, 0].player == Field.EMPTY) || !(fields[6, 0].player == Field.EMPTY))
            {
                return false; // check whether fields are free
            }
            if (!validKingField(fields, Field.WHITE, null, fields[5, 0]) || !validKingField(fields, Field.WHITE, null, fields[6, 0]))
            {
                return false; // check whether none of the fields is in check
            }
            return (whiteCastlingSmall && !isCheck(fields, Field.WHITE));
        }
        else if (player == Field.BLACK)
        {
            if (!(fields[5, 7].player == Field.EMPTY) || !(fields[6, 7].player == Field.EMPTY))
            {
                return false; // check whether fields are free
            }
            if (!validKingField(fields, Field.BLACK, null, fields[5, 7]) || !validKingField(fields, Field.BLACK, null, fields[6, 7]))
            {
                return false; // check whether none of the fields is in check
            }
            return (blackCastlingSmall  && !isCheck(fields, Field.BLACK));
        }
        return false;
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

        if (player == Field.WHITE && canCastleBig(Field.WHITE))
        {
            fs.Add(fields[2, 0]);
        }

        if (player == Field.WHITE && canCastleSmall(Field.WHITE))
        {
            fs.Add(fields[6, 0]);
        }

        if (player == Field.BLACK && canCastleBig(Field.BLACK))
        {
            fs.Add(fields[2, 7]);
        }

        if (player == Field.BLACK && canCastleSmall(Field.BLACK))
        {
            fs.Add(fields[6, 7]);
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
        return isCheck(fields, player);
    }

    bool isCheck(Field[,] fields, int player)
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
        if (f == null)
        {
            selectedField = null;
            highlight2Fields(null);
            //highlight2Pieces(null);
            highlight4Field(null);
            highlight4Piece(null);
            return;
        }

        highlightField(f);
        highlightPiece(f);

        if (f.player != Field.EMPTY)
        {
            selectedField = f;
        }

        if (f.player == Field.WHITE || f.player == Field.BLACK)
        {
            List<Field> fs = getMoves(fields, f);
            highlight2Fields(fs);
            //highlight2Pieces(fs);
        }
    }

    Field movePieceEnpassant(Field f1, Field f2)
    {
        Field enpassant = null;

        int col = f1.col;
        int row = f1.row;

        if (player == Field.WHITE && row == 4)
        {
            if (col > 0)
            {
                if (oldFields[col - 1, 6].player == Field.BLACK && pieceIsType(getPiece(oldFields[col - 1, 6]), "Pawn")
                    && fields[col - 1, 6].player == Field.EMPTY && pieceIsType(getPiece(fields[col - 1, 4]), "Pawn")
                    && fields[col - 1, 5].player == Field.EMPTY)
                {
                    if (f2.row == 5 && f2.col == col - 1)
                    {
                        enpassant = fields[col - 1, 4];
                    }
                }
            }

            if (col < Field.FIELDS_X - 1)
            {
                if (oldFields[col + 1, 6].player == Field.BLACK && pieceIsType(getPiece(oldFields[col + 1, 6]), "Pawn")
                    && fields[col + 1, 6].player == Field.EMPTY && pieceIsType(getPiece(fields[col + 1, 4]), "Pawn")
                    && fields[col + 1, 5].player == Field.EMPTY)
                {
                    if (f2.row == 5 && f2.col == col + 1)
                    {
                        if (f2.row == 5 && f2.col == col + 1)
                        {
                            enpassant = fields[col + 1, 4];
                        }
                    }
                }
            }
        }

        if (player == Field.BLACK && row == 3)
        {
            if (col > 0)
            {
                if (oldFields[col - 1, 1].player == Field.BLACK && pieceIsType(getPiece(oldFields[col - 1, 1]), "Pawn")
                    && fields[col - 1, 1].player == Field.EMPTY && pieceIsType(getPiece(fields[col - 1, 3]), "Pawn")
                    && fields[col - 1, 2].player == Field.EMPTY)
                {
                    if (f2.row == 2 && f2.col == col - 1)
                    {
                        enpassant = fields[col - 1, 3];
                    }
                }
            }

            if (col < Field.FIELDS_X - 1)
            {
                if (oldFields[col + 1, 1].player == Field.BLACK && pieceIsType(getPiece(oldFields[col + 1, 1]), "Pawn")
                    && fields[col + 1, 1].player == Field.EMPTY && pieceIsType(getPiece(fields[col + 1, 3]), "Pawn")
                    && fields[col + 1, 2].player == Field.EMPTY)
                {
                    if (f2.row == 2 && f2.col == col + 1)
                    {
                        enpassant = fields[col + 1, 3];
                    }
                }
            }
        }

        return enpassant;
    }

    bool pawnPromotion(Piece p)
    {
        if (p == null)
        {
            return false;
        }
        if (!pieceIsType(p, "Queen") && !pieceIsType(p, "Rook") && !pieceIsType(p, "Knight") && !pieceIsType(p, "Bishop"))
        {
            return false;
        }

        for (int i = 0; i < whitePieces.Length; i++)
        {
            if (Object.ReferenceEquals(whitePieces[i], p))
            {
                if (player == Field.WHITE)
                {
                    Piece pawn = getPiece(fieldPawnPromotion);
                    string name = pawn.gameObject.transform.GetChild(0).name;
                    Destroy(pawn.gameObject);
                    pawn.gameObject = GameObject.Instantiate(whitePieces[i].gameObject);
                    pawn.gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_WHITE, 0));
                    pawn.gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                    typesWhitePieces[fieldPawnPromotion.no] = typesWhitePieces[i];
                } else if (player == Field.BLACK)
                {
                    for (int j = blackPieces.Length - 1; j >= 0; j--)
                    {
                        if (typesWhitePieces[i].Equals(typesBlackPieces[j]))
                        {
                            Piece pawn = getPiece(fieldPawnPromotion);
                            string name = pawn.gameObject.transform.GetChild(0).name;
                            Destroy(pawn.gameObject);
                            pawn.gameObject = GameObject.Instantiate(blackPieces[j].gameObject);
                            pawn.gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_BLACK, 0));
                            pawn.gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                            typesBlackPieces[fieldPawnPromotion.no] = typesBlackPieces[j];
                        }
                    }
                }
            }
        }

        for (int i = 0; i < blackPieces.Length; i++)
        {
            if (Object.ReferenceEquals(blackPieces[i], p))
            {
                if (player == Field.BLACK)
                {
                    Piece pawn = getPiece(fieldPawnPromotion);
                    string name = pawn.gameObject.transform.GetChild(0).name;
                    Destroy(pawn.gameObject);
                    pawn.gameObject = GameObject.Instantiate(blackPieces[i].gameObject);
                    pawn.gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_BLACK, 0));
                    pawn.gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                    typesBlackPieces[fieldPawnPromotion.no] = typesBlackPieces[i];
                }
                else if (player == Field.WHITE)
                {
                    for (int j = whitePieces.Length - 1; j >= 0; j--)
                    {
                        if (typesBlackPieces[i].Equals(typesWhitePieces[j]))
                        {
                            Piece pawn = getPiece(fieldPawnPromotion);
                            string name = pawn.gameObject.transform.GetChild(0).name;
                            Destroy(pawn.gameObject);
                            pawn.gameObject = GameObject.Instantiate(whitePieces[j].gameObject);
                            pawn.gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_WHITE, 0));
                            pawn.gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                            typesWhitePieces[fieldPawnPromotion.no] = typesWhitePieces[j];
                        }
                    }
                }
            }
        }

        fieldPawnPromotion = null;
        panelPawnPromotion.SetActive(false);
        placePieces();
        return true;
    }

    bool pawnPromotion(Field f)
    {
        return pawnPromotion(getPiece(f));
    }

    void movePiecePawnPromotion()
    {
        for (int i = 0; i < Field.FIELDS_X; i++)
        {
            if (fields[i, 7].player == Field.WHITE && pieceIsType(getPiece(fields[i, 7]), "Pawn"))
            {
                fieldPawnPromotion = fields[i, 7];
                turnAllCentreTextsOff();
                panelPawnPromotion.SetActive(true);
                return;

            }
            if (fields[i, 0].player == Field.BLACK && pieceIsType(getPiece(fields[i, 0]), "Pawn"))
            {
                fieldPawnPromotion = fields[i, 0];
                turnAllCentreTextsOff();
                panelPawnPromotion.SetActive(true);
                return;
            }
        }
    }

    void movePiece(Field f1, Field f2)
    {
        Field enpassant = movePieceEnpassant(f1, f2);

        if (enpassant != null)
        {
            enpassant.player = Field.EMPTY; // take pawn from enpassant field
        }

        oldFields = cloneFields(fields);

        f2.player = f1.player;
        f2.no = f1.no;
        f1.player = Field.EMPTY;

        placePieces();

        movePiecePawnPromotion();
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
            whiteCastlingSmall = false;
        }

        if (fields[0, 7].player != Field.BLACK || pieceIsType(getPiece(fields[0, 7]), "Rook") == false)
        {
            blackCastlingBig = false;
        }

        if (fields[7, 7].player != Field.BLACK || pieceIsType(getPiece(fields[7, 7]), "Rook") == false)
        {
            blackCastlingSmall = false;
        }
    }

    void sendBoardStatus()
    {
        PhotonView photonView = gameObject.GetComponent<PhotonView>();

        string[] str = new string[fields.GetLength(0) * fields.GetLength(1)];
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                str[i * fields.GetLength(1) + j] = JsonUtility.ToJson(fields[i, j]);
            }
        }

        checkKingMoved();

        photonView.RPC("sendMove", RpcTarget.AllBufferedViaServer, str);
        photonView.RPC("setWhitePieces", RpcTarget.AllBufferedViaServer, typesWhitePieces);
        photonView.RPC("setBlackPieces", RpcTarget.AllBufferedViaServer, typesBlackPieces);
        if (player == Field.WHITE)
        {
            photonView.RPC("setTurn", RpcTarget.AllBufferedViaServer, Field.BLACK);
        }
        else
        {
            photonView.RPC("setTurn", RpcTarget.AllBufferedViaServer, Field.WHITE);
        }
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

    void checkCheck()
    {
        List<Field> allMoves = new List<Field>();
        Piece[] ownPieces;
        if (player == Field.WHITE)
        {
            ownPieces = whitePieces;
        } else
        {
            ownPieces = blackPieces;
        }
        foreach (Piece p in ownPieces)
        {
            Field f = getFieldOfPiece(p);
            if (f != null)
            {
                allMoves.AddRange(getMoves(fields, f));
            }
        }

        if (allMoves.Count == 0)
        {
            if (isCheck(fields, player))
            {
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                photonView.RPC("setWon", RpcTarget.OthersBuffered, Networking.CHECKMATE);
                gameEnded = true;
                checkmate = true;
            }
            else
            {
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                photonView.RPC("setDraw", RpcTarget.OthersBuffered);
                gameEnded = true;
                draw = true;
            }
        }
        else
        {
            if (isCheck(fields, player))
            {
                turnAllCentreTextsOff();
                panelCheck.SetActive(true);
            }
        }
    }

    public void receiveBoardStatus(Field[] fs)
    {

        bool fieldChanged = false;
        for (int i = 0; i < fields.GetLength(0); i++)
        {
            for (int j = 0; j < fields.GetLength(1); j++)
            {
                if (fs[i * fields.GetLength(1) + j].player != fields[i, j].player || fields[i, j].no != fs[i * fields.GetLength(1) + j].no)
                {
                    fieldChanged = true;
                }
            }
        }

        if (fieldChanged)
        {
            oldFields = cloneFields(fields);
        }

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

        checkKingMoved();

        if (isCheck(fields))
        {
            turnAllCentreTextsOff();
            panelCheck.SetActive(true);
        }

        placePieces();

        checkCheck();
    }

    void checkCastling(Field f)
    {
        if (canCastleBig(Field.WHITE) && f.row == 0 && f.col == 2 && selectedField.player == Field.WHITE && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[0, 0], fields[3, 0]);
        }

        if (canCastleSmall(Field.WHITE) && f.row == 0 && f.col == 6 && selectedField.player == Field.WHITE && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[7, 0], fields[5, 0]);
        }

        if (canCastleBig(Field.BLACK) && f.row == 7 && f.col == 2 && selectedField.player == Field.BLACK && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[0, 7], fields[3, 7]);
        }

        if (canCastleSmall(Field.BLACK) && f.row == 7 && f.col == 6 && selectedField.player == Field.BLACK && pieceIsType(getPiece(selectedField), "King"))
        {
            movePiece(fields[7, 7], fields[5, 7]);
        }
    }

    void setCameras()
    {
        if (player == Field.WHITE)
        {
            cameraWhite.gameObject.SetActive(false);
            cameraBlack.gameObject.SetActive(false);
            cameraWhiteTop.gameObject.SetActive(false);
            cameraBlackTop.gameObject.SetActive(false);
            if (cameraTop)
            {
                cameraWhiteTop.gameObject.SetActive(true);
            }
            else
            {
                cameraWhite.gameObject.SetActive(true);
            }

            GameObject.Find("lightWhite").GetComponent<Light>().enabled = true;
            GameObject.Find("lightBlack").GetComponent<Light>().enabled = false;
        }
        else
        {
            cameraWhite.gameObject.SetActive(false);
            cameraBlack.gameObject.SetActive(false);
            cameraWhiteTop.gameObject.SetActive(false);
            cameraBlackTop.gameObject.SetActive(false);
            if (cameraTop)
            {
                cameraBlackTop.gameObject.SetActive(true);
            }
            else
            {
                cameraBlack.gameObject.SetActive(true);
            }
            GameObject.Find("lightWhite").GetComponent<Light>().enabled = false;
            GameObject.Find("lightBlack").GetComponent<Light>().enabled = true;
        }
    }

    void restSendMove()
    {
        sendBoardStatus();
        if (player == Field.WHITE)
        {
            turn = Field.BLACK;
        }
        else if (player == Field.BLACK)
        {
            turn = Field.WHITE;
        }
    }

    Field getFieldOfPiece(Piece p)
    {
        int colour = 0;
        int no = 0;
        bool pieceFound = false;
        for (int i = 0; i < whitePieces.Length; i++)
        {
            if (Object.ReferenceEquals(whitePieces[i], p))
            {
                colour = Field.WHITE;
                no = i;
                pieceFound = true;
            }
        }
        for (int i = 0; i < blackPieces.Length; i++)
        {
            if (Object.ReferenceEquals(blackPieces[i], p))
            {
                colour = Field.BLACK;
                no = i;
                pieceFound = true;
            }
        }
        if (pieceFound)
        {
            foreach (Field f in fields)
            {
                if (f.player == colour && f.no == no)
                {
                    return f;
                }
            }
        }
        return null;
    }

    public void setWhitePieces(string[] whitePieces)
    {
        for (int i = 0; i < typesWhitePieces.Length; i++)
        {
            if (!this.typesWhitePieces[i].Equals(whitePieces[i]))
            {
                string name = this.whitePieces[i].gameObject.transform.GetChild(0).name;
                Destroy(this.whitePieces[i].gameObject);

                for (int j = 0; j < whitePieces.Length; j++)
                {
                    if (whitePieces[i].Equals(typesWhitePieces[j]))
                    {
                        this.whitePieces[i].gameObject = GameObject.Instantiate(this.whitePieces[j].gameObject);
                        this.whitePieces[i].gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_WHITE, 0));
                        this.whitePieces[i].gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                        break;
                    }
                }
                this.typesWhitePieces[i] = whitePieces[i];
            }
        }
        placePieces();
    }

    public void setBlackPieces(string[] blackPieces)
    {
        for (int i = 0; i < typesBlackPieces.Length; i++)
        {
            if (!this.typesBlackPieces[i].Equals(blackPieces[i]))
            {
                string name = this.blackPieces[i].gameObject.transform.GetChild(0).name;
                Destroy(this.blackPieces[i].gameObject);

                for (int j = 0; j < blackPieces.Length; j++)
                {
                    if (blackPieces[i].Equals(typesBlackPieces[j]))
                    {
                        this.blackPieces[i].gameObject = GameObject.Instantiate(this.blackPieces[j].gameObject);
                        this.blackPieces[i].gameObject.transform.Rotate(new Vector3(0, ROTATION_ANGLE_BLACK, 0));
                        this.blackPieces[i].gameObject.transform.GetChild(0).name = name; // so that getFieldByPiece() gives back correct result
                        break;
                    }
                }
                this.typesBlackPieces[i] = blackPieces[i];
            }
        }
        placePieces();
    }

    void highlight4Field(Field f)
    {
        foreach (Field field in fields)
        {
            field.highlight4 = false;
        }
        if (f == null)
        {
            return;
        }
        f.highlight4 = true;
    }

    void highlight4Piece(Field f)
    {
        foreach (Piece p in whitePieces)
        {
            p.highlight4 = false;
        }
        foreach (Piece p in blackPieces)
        {
            p.highlight4 = false;
        }

        Piece piece = getPiece(f);
        if (piece != null)
        {
            piece.highlight4 = true;
        }
    }

    void checkCameraTop()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cameraTop = !cameraTop;
        }
    }

    void checkPlayer()
    {
        if (player != oldPlayer)
        {
            PhotonView photonView = gameObject.GetComponent<PhotonView>();
            if (player == Field.WHITE)
            {
                photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.BLACK);
            }
            else if (player == Field.BLACK)
            {
                photonView.RPC("setPlayer", RpcTarget.OthersBuffered, Field.WHITE);
            }
        }
        oldPlayer = player;
    }

    bool checkGameStarted()
    {
        if (gameStarted)
        {
            panelChooseColour.SetActive(false);
            buttonGiveUp.SetActive(true);

            if (turn == Field.WHITE)
            {
                txtTurnPlayerWhite.SetActive(true);
                panelTurnPlayerWhite.SetActive(true);
                txtTurnPlayerBlack.SetActive(false);
                panelTurnPlayerBlack.SetActive(false);
            }
            else
            {
                txtTurnPlayerBlack.SetActive(true);
                panelTurnPlayerBlack.SetActive(true);
                txtTurnPlayerWhite.SetActive(false);
                panelTurnPlayerWhite.SetActive(false);
            }
            return true;
        } else
        {
            turnAllCentreTextsOff(panelChooseColour);
            panelChooseColour.SetActive(true);
            buttonGiveUp.SetActive(false);
            return false;
        }
    }

    void deleteHighlight3()
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

    void checkEnded()
    {
        if (gameEnded)
        {
            buttonGiveUp.SetActive(false);
            if (wonCheckmate)
            {
                turnAllCentreTextsOff();
                panelWonCheckmate.SetActive(true);
            }
            else if (wonGivenUp)
            {
                turnAllCentreTextsOff();
                panelWonGivenUp.SetActive(true);
            }
            else if (draw)
            {
                turnAllCentreTextsOff();
                panelDraw.SetActive(true);
            } else if (checkmate)
            {
                turnAllCentreTextsOff();
                panelCheckmate.SetActive(true);
            }
            else
            {
                panelLostGivenUp.SetActive(true);
            }
        }
        else
        {
            panelWonCheckmate.SetActive(false);
            panelWonGivenUp.SetActive(false);
            panelDraw.SetActive(false);
            panelLostGivenUp.SetActive(false);
        }
    }

    public void turnAllCentreTextsOff(bool withAll)
    {
        panelLoad.SetActive(false);
        panelCheck.SetActive(false);
        panelCheckmate.SetActive(false);
        panelPawnPromotion.SetActive(false);
        panelWaitForPlayer.SetActive(false);
        panelWonCheckmate.SetActive(false);
        panelWonGivenUp.SetActive(false);
        panelDraw.SetActive(false);
        panelLostGivenUp.SetActive(false);
        if (withAll)
        {
            panelChooseColour.SetActive(false);
            panelRestart.SetActive(false);
            panelGiveUp.SetActive(false);
        }
    }

    public void  turnAllCentreTextsOff()
    {
        turnAllCentreTextsOff(true);
    }

    public void turnAllCentreTextsOff(GameObject without)
    {
        // Necessary to not trun off panels with buttons because otherweise buttons will not respond
        turnAllCentreTextsOff(false);
        if (!Object.ReferenceEquals(without, panelRestart))
        {
            panelRestart.SetActive(false);
        }
        if (!Object.ReferenceEquals(without, panelGiveUp))
        {
            panelGiveUp.SetActive(false);
        }
        if (!Object.ReferenceEquals(without, panelChooseColour))
        {
            panelChooseColour.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        setCameras();

        if (!connected)
        {
            turnAllCentreTextsOff();
            panelTurn.SetActive(false);
            panelLoad.SetActive(true);
            return;
        }
        else
        {
            panelLoad.SetActive(false);
        }

        if (!roomJoined)
        {
            panelTurn.SetActive(false);
            turnAllCentreTextsOff();
            panelRoomSelection.SetActive(true);
            return;
        } else
        {
            panelRoomSelection.SetActive(false);
        }

        // Once we reach that status point panelTurn should be activated
        panelTurn.SetActive(true);

        if (restartDialog)
        {
            turnAllCentreTextsOff(panelRestart);
            panelRestart.SetActive(true);
            return;
        } else
        {
            panelRestart.SetActive(false);
        }

        if (giveUpDialog)
        {
            turnAllCentreTextsOff(panelGiveUp);
            panelGiveUp.SetActive(true);
            return;
        }
        else
        {
            panelGiveUp.SetActive(false);
        }

        if (!roomFull)
        {
            turnAllCentreTextsOff();
            buttonGiveUp.SetActive(false);
            panelWaitForPlayer.SetActive(true);
            return;
        }
        else
        {
            panelWaitForPlayer.SetActive(false);
        }

        checkPlayer();

        if (!checkGameStarted())
        {
            return;
        }

        checkCameraTop();

        checkEnded();

        if (gameEnded)
        {
            highlight4Field(null);
            highlightField(null);
            highlightPiece((Field) null);
            highlight4Piece(null);
            highlight2Fields(null);
            deleteHighlight3();
      
            return;
        }

        // Delete green highlighting from received fields after every click
        if (Input.GetMouseButtonDown(0))
        {
            deleteHighlight3();
            panelCheck.SetActive(false);
        }

        // None selected -> highlight field under cursor and if clicked select that field
        if (selectedField == null)
        {
            Field f = getFieldByPiece();
            highlightField(f);
            highlightPiece(f);
            // No field found
            if (f == null)
            {
                Piece p = getPieceByPiece();
                highlightPiece(p); // colour piece

                // for pawn promotion
                if (fieldPawnPromotion != null && Input.GetMouseButtonDown(0))
                {
                    if (p != null)
                    {
                        if (pawnPromotion(p))
                        {
                            restSendMove();
                        }
                    }
                }
            }
            // Field clicked
            if (f != null)
            {
                if (Input.GetMouseButtonDown(0) && turn == player)
                {
                    if (fieldPawnPromotion != null)
                    {
                        if (pawnPromotion(f))
                        {
                            restSendMove();
                        }
                    }
                    else
                    {
                        // Select field if it is from same player
                        if (f.player == player && fieldPawnPromotion == null)
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
            if (turn == player)
            {
                Field f = getFieldByField();
                highlight4Field(f);
                highlight4Piece(f);

                if (Input.GetMouseButtonDown(0))
                {
                    List<Field> allMoves = getMoves(fields, selectedField);
                    if (allMoves.Contains(f))
                    {
                        checkCastling(f);
                        movePiece(selectedField, f);
                        if (fieldPawnPromotion == null)
                        {
                            restSendMove();
                        }
                        selectField(null);
                    }
                    else
                    {
                        Field fPiece = getFieldByPiece();
                        selectField(null);
                        if (fPiece.player == player && fieldPawnPromotion == null)
                        {
                            selectField(fPiece);
                        }
                    }
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
