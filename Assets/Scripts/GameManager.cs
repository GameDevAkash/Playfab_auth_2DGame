using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public static GameManager Singleton;
    [SerializeField] public Transform PlayerSpawnPoint;
    [SerializeField] private GameObject Player;
    [SerializeField] public GameObject inGamePlayer;

    [Header("Values")]
    public Vector2 initPlayerPos;
    public Vector3 initCameraPos;
    [SerializeField] public float GameTime;
    [SerializeField] public bool isGamePlaying;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;

            //DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        isGamePlaying = true;
        inGamePlayer = Instantiate(Player, PlayerSpawnPoint.position, Quaternion.identity);
        CameraFollow.Singleton.player = inGamePlayer;
        initPlayerPos = inGamePlayer.transform.position;
        initCameraPos = Camera.main.transform.position;
    }
    private void Update()
    {
        if (!isGamePlaying) { return; }
        GameTime += Time.deltaTime;
        if(inGamePlayer.transform.position.y < -20f || GameTime >= 60)
        {
            Die();
        }
    }
    private void Die()
    {
        UIHandler.Singleton.OnDie();
        inGamePlayer.GetComponent<Player>().moveSpeed = 0;
        inGamePlayer.GetComponent<Player>().TotalBoostCount = 2;
        inGamePlayer.GetComponent<CoinWallet>().TotalCoins = 0;
        UIHandler.Singleton.CoinCount.text = inGamePlayer.GetComponent<CoinWallet>().TotalCoins.ToString();
        inGamePlayer.transform.position = initPlayerPos;
        Camera.main.transform.position = initCameraPos;
        isGamePlaying = false;
        GameTime = 0;
    }


}
