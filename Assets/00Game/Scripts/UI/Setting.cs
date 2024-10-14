using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private Button btnSetting;
    [SerializeField] private Transform holder;
    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnClose;
    private void Reset()
    {
        LoadCompoment();
    }
    void Start()
    {
        btnSetting.onClick.AddListener(() =>
        {
            ActivePanel();
        });
        btnHome.onClick.AddListener(() => { Home();});

        btnClose.onClick.AddListener(() => { Close();});

    }
    private void LoadCompoment()
    {
        btnSetting = GetComponent<Button>();
        holder = transform.GetChild(0);
        btnHome = holder.GetChild(1).GetComponent<Button>();
        btnClose = holder.GetChild(2).GetComponent<Button>();
    }
    private void ActivePanel()
    {
        Time.timeScale = 0;
        holder.gameObject.SetActive(true);
    }
    private void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Lobby");
    }
    private void Close()
    {
        Time.timeScale = 1;
        holder.gameObject.SetActive(false);
    }
}
