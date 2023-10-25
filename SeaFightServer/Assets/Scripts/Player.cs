using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using UnityEditor;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }


    private void OnDestroy()
    {
        list.Remove(Id);
    }



    public static void Spawn(ushort id, string username)
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        list.Add(id, player);
    }

    private void MovePlayer(ushort id, Vector3 position)
    {
        Debug.Log(id + ") X:" + position.x + "   Y:" + position.y + "    Z:" + position.z);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerMove);
        message.AddUShort(id);
        message.AddVector3(position);
        NetworkManager.Singleton.Server.SendToAll(message);
        
        gameObject.GetComponent<NavMeshAgent>().destination = position;
    }

    [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }

    [MessageHandler((ushort)ClientToServerId.MovePosition)]
    private static void MovePlayer(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
            player.MovePlayer(fromClientId, message.GetVector3());

    }

    private void SendSpawned(ushort toClientId)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned);
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        NetworkManager.Singleton.Server.Send(message, toClientId);
    }

    private void SendSpawned()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.playerSpawned);
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        NetworkManager.Singleton.Server.SendToAll(message);
    }


}
