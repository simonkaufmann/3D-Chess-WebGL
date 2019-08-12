using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{

    public GameObject wQueen;

    string tagChessBoard = "Table";

    // Start is called before the first frame update
    void Start()
    {
        var rend = wQueen.GetComponent<Renderer>();
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
                    wQueen.transform.localPosition = hit.point;
                }
            }
        }
    }
}
