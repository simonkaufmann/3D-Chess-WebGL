using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Field
{
    public static float BOARD_X_MIN = -50.2f;
    public static float BOARD_X_MAX = 48.5f; //43.3f;
    public static float BOARD_Y_MIN = -55.5f;
    public static float BOARD_Y_MAX = 43.3f;
    public static int FIELDS_X = 8;
    public static float FIELDS_Y = 8;
    public static float FIELD_X = (BOARD_X_MAX - BOARD_X_MIN) / (float) FIELDS_X;
    public static float FIELD_Y = (BOARD_Y_MAX - BOARD_Y_MIN) / (float) FIELDS_Y;

    public static int EMPTY = 0;
    public static int WHITE = 1;
    public static int BLACK = 2;

    public static Color AMBER = new Color(255.0f / 255.0f, 50.0f / 255.0f, 0, 150.0f/255.0f);
    public static Color AMBER_NONTRANSPARENT = new Color(255.0f / 255.0f, 50.0f / 255.0f, 0);
    public static Color GREEN = new Color(0 / 255.0f, 200.0f / 255.0f, 0, 150.0f / 255.0f);
    public static Color RED = new Color(255.0f / 255.0f, 0 / 255.0f, 0, 150.0f / 255.0f);

    public int no;
    public int player;

    public int row, col;

    private bool _highlight1;
    private bool _highlight2;
    private bool _highlight3;
    private bool _highlight4;

    public bool highlight1
    {
        get {
            return _highlight1;
        }

        set
        {
            _highlight1 = value;
            updateHighlight();
        }

    }

    public bool highlight2
    {
        get
        {
            return _highlight2;
        }

        set
        {
            _highlight2 = value;
            updateHighlight();
        }
    }

    public bool highlight3
    {
        get
        {
            return _highlight3;
        }

        set
        {
            _highlight3 = value;
            updateHighlight();
        }
    }

    public bool highlight4
    {
        get
        {
            return _highlight4;
        }

        set
        {
            _highlight4 = value;
            updateHighlight();
        }
    }

    GameObject fieldHighlighter;

    void updateHighlight()
    {
        var rend = fieldHighlighter.GetComponent<Renderer>();
        if (_highlight1 || _highlight2 || _highlight3)
        {
            rend.enabled = true;
        } else
        {
            rend.enabled = false;
        }

        if (_highlight4 && _highlight2)
        {
            rend.material.color = Field.AMBER;
        }
        else if (_highlight1 || _highlight2)
        {
            rend.material.color = RED;
        }
        else if (_highlight3)
        {
            rend.material.color = GREEN;
        }
        else
        {
            rend.material.color = RED;
        }
    }

    public static Vector2Int? getField(Vector2 coord)
    {
        coord.x = (coord.x - BOARD_X_MIN) / FIELD_X;
        coord.y = (coord.y - BOARD_Y_MIN) / FIELD_Y;
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        if (x >= 0 && x < FIELDS_X && y >= 0 && y < FIELDS_Y)
        {
            return new Vector2Int(x, y);
        }

        return null;
    }

    public static Vector2 getFieldPos(Vector2Int field)
    {
        Vector2 fieldPos = new Vector2();
        fieldPos.x = BOARD_X_MIN + FIELD_X / 2 + field.x * FIELD_X;
        fieldPos.y = BOARD_Y_MIN + FIELD_Y / 2 + field.y * FIELD_Y;
        return fieldPos;
    }

    public Field(int col, int row)
    {
        this.row = row;
        this.col = col;

        no = 0;
        player = EMPTY;

        var blueprintFieldHighlighter = GameObject.Find("blueprintFieldHighlighter");

        fieldHighlighter = GameObject.Instantiate(blueprintFieldHighlighter);

        var rend = fieldHighlighter.GetComponent<Renderer>();
        rend.enabled = false;

        Vector2 fieldPos = getFieldPos(new Vector2Int(col, row));
        Vector3 fieldPos3 = new Vector3(fieldPos.x, fieldHighlighter.transform.position.y, fieldPos.y);
        fieldHighlighter.transform.position = new Vector3(fieldPos.x, fieldHighlighter.transform.position.y, fieldPos.y);
    }

    public Field Clone()
    {
        Field new_field = new Field(col, row);
        new_field.player = this.player;
        new_field.no = this.no;
        //new_field.highlight1 = this._highlight1;
        //new_field.highlight2 = this._highlight2;
        //new_field.highlight3 = this._highlight3;
        return new_field;
    }
}
