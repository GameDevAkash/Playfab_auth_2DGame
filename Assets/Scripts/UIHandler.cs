using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] public static UIHandler Singleton;
    private const string GameScence = "Game";
    private const string MenuScence = "PlayMenu";
    [SerializeField] private GameObject PlayMenuBG;
    [SerializeField] private GameObject GamePlayCanvas;
    [SerializeField] private GameObject DieCanvas;
    [SerializeField] public TextMeshProUGUI CoinCount;
    [SerializeField] public TextMeshProUGUI Time;
    [SerializeField] public TextMeshProUGUI JetPowerUp;


    private void Awake()
    {
        if(Singleton == null) { Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.Singleton != null)
        {
            Time.text = $"{string.Format("{0:0}", GameManager.Singleton.GameTime)}"+"/60";
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(GameScence);
        PlayMenuBG.SetActive(false);
        GamePlayCanvas.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void OnDie()
    {
        DieCanvas.SetActive(true);
    }

    public void Replay()
    {
        GameManager.Singleton.isGamePlaying = true;
        DieCanvas.SetActive(false);
        GameManager.Singleton.inGamePlayer.GetComponent<Player>().moveSpeed = 4;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(MenuScence);
        PlayMenuBG.SetActive(true);
        GamePlayCanvas.SetActive(false); 
        DieCanvas.SetActive(false);
    }

    public void OnBoostClick()
    {
        if (GameManager.Singleton == null) { return; }
        GameManager.Singleton.inGamePlayer.GetComponent<Player>().OnBoostClick();
    }public void OnJumpClick()
    {
        if (GameManager.Singleton == null) { return; }
        GameManager.Singleton.inGamePlayer.GetComponent<Player>().Jump();
    }
}
