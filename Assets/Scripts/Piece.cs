﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public GameObject gameObject;

    private bool _highlight1;
    private bool _highlight2;
    private bool _highlight3;
    private bool _highlight4;

    public bool highlight1
    {
        get
        {
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
    
    public Piece()
    {
        gameObject = new GameObject();
    }

    void updateHighlight()
    {
        var rend = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        if (_highlight4 && _highlight2)
        {
            rend.material.color = Field.AMBER_NONTRANSPARENT;
        }
        else if (_highlight1 || _highlight2)
        {
            rend.material.color = Color.red;
        }
        else if (_highlight3)
        {
            rend.material.color = Color.green;
        }
        else
        {
            rend.material.color = Color.white;
        }
    }
}
