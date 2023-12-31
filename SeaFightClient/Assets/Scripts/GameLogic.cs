using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        Singleton = this;
    }

    public GameObject LocalPlayerPrefab => localPlayerPrefab;
    public GameObject PlayerPrefab => playerPrefab;


    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;
}
