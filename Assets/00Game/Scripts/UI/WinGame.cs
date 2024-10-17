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
    private void Reset()
    {
        LoadCompoment();
    }
    private void Start()
    {
        LoadCompoment();
        btnHome.onClick.AddListener(() => Home());
        btnRePlay.onClick.AddListener(() => RePlay());
    }
    private void LoadCompoment()
    {
        txtWin = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        btnHome = transform.GetChild(1).GetComponent<Button>();
        btnRePlay = transform.GetChild(2).GetComponent<Button>();
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
    public void Message(string txt)
    {
        txtWin.text = txt;
    }
}
