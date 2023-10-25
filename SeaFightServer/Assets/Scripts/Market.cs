using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            ushort toClientId = other.gameObject.GetComponent<Player>().Id;

            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.handel);
            message.AddString("Baum");
            NetworkManager.Singleton.Server.Send(message, toClientId);

        }
    }
}
