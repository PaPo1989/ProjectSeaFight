using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public Vector3 TargetPosition;


    //Muss noch mit OutlineShader ersetzt werden
    void OnMouseOver()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
