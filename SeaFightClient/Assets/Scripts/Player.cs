using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public NavMeshAgent agent;
    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }
    private string username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }
    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({username})";
        player.Id = id;
        player.username = username;
        list.Add(id, player);
    }

    private void playerMove(Vector3 position)
    {
        Debug.Log(name+"X:" + position.x + "   Y:" + position.y + "    Z:" + position.z);
        agent.destination = position;
        GameObject[] marker;
        marker = GameObject.FindGameObjectsWithTag("Marker");

        if (marker.Length != 0)
        {
            marker[0].GetComponent<Marker>().Player = gameObject;
            marker[0].transform.position = position + new Vector3(0,2,0);
            marker[0].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("WayPoint Marker existiert nicht");
        }
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMove)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.playerMove(message.GetVector3());
    }


}
