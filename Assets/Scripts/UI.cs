using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public void toggleWhiteValueChanged()
    {
        GameObject toggleWhite = GameObject.Find("toggleWhite");
        Toggle tWhite = toggleWhite.GetComponent<Toggle>();

        GameObject toggleBlack = GameObject.Find("toggleBlack");
        Toggle tBlack = toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();
        p.myTurn = !p.myTurn;

        if (tWhite.isOn)
        {
            tBlack.isOn = false;
            p.player = Field.WHITE;
        } else
        {
            tBlack.isOn = true;
            p.player = Field.BLACK;
        }
    }
    
    public void toggleBlackValueChanged()
    {
        GameObject toggleWhite = GameObject.Find("toggleWhite");
        Toggle tWhite = toggleWhite.GetComponent<Toggle>();

        GameObject toggleBlack = GameObject.Find("toggleBlack");
        Toggle tBlack = toggleBlack.GetComponent<Toggle>();

        Pieces p = gameObject.GetComponent<Pieces>();
        p.myTurn = !p.myTurn;

        if (tBlack.isOn)
        {
            p.player = Field.BLACK;
            tWhite.isOn = false;
        }
        else
        {
            p.player = Field.WHITE;
            tWhite.isOn = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
