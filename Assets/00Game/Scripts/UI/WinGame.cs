using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWin;
    [SerializeField] private Button btnRePlay;
    [SerializeField] private Button btnHome;
    private void Start()
    {
        btnHome.onClick.AddListener(() => Home());
        btnRePlay.onClick.AddListener(() => RePlay());
    }
    private void RePlay()
    {
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("InGame");
    }
    private void Home()
    {
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("Lobby");
    }
}
