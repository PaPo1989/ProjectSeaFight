using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) // Wenn der linke Mausklick erfolgt
        {
            Debug.Log("Klick");
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.MovePosition);
                    message.AddVector3(hit.point);
                    NetworkManager.Singleton.Client.Send(message);
                    Debug.Log("Send");
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Market"))
                {
                    Debug.Log("Market klick");
                    Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.MovePosition);
                    message.AddVector3(hit.transform.gameObject.GetComponent<Market>().TargetPosition);
                    NetworkManager.Singleton.Client.Send(message);
                    Debug.Log("Send");
                }

                
            }
            else
                Debug.Log("MisKlick");
        }
        
    }




    
}
