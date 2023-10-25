using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Marker : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        if(Player != null && Vector3.Distance(Player.transform.position,transform.position) < 3f)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);   
        }
    }
}
