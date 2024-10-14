using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class UIManager : Singleton<UIManager>
{
    [Header("Panel Choice Color")]
    [HideInInspector] public GameObject panelColor;
    [SerializeField] private Button btnRed;
    [SerializeField] private Button btnYellow;
    [SerializeField] private Button btnGreen;
    [SerializeField] private Button btnBlue;

    [Header("TagBar color")]
    [SerializeField] private Image Mid; //dùng để thay đổi đổi thanh điều hướng của game
    [SerializeField] private Image Left;
    [SerializeField] private Image Right;

    [Header("Current Player")]
    [SerializeField] private List<Image> listColorTurn;
    [SerializeField] private List<TextMeshProUGUI> txtCountCardInHand;


    private void Reset()
    {
        LoadCompoment();
    }
    private void Start()
    {
        LoadCompoment();
        panelColor.SetActive(false);
        btnRed.onClick.AddListener((() => ClickButtonColor("RED")));
        btnYellow.onClick.AddListener((() => ClickButtonColor("YELLOW")));
        btnGreen.onClick.AddListener((() => ClickButtonColor("GREEN")));
        btnBlue.onClick.AddListener(()=>ClickButtonColor("BLUE"));
    }

    private void LoadCompoment()
    {
        LoadPanelColor();
        TagBarColor();
        LoadColorTurn();
        LoadText();
    }
    private void LoadPanelColor()
    {
        if (panelColor == null)
        {
            panelColor = transform.GetChild(2).gameObject;
        }
        if (btnBlue == null || btnRed==null || btnGreen == null || btnYellow ==null) {
            btnRed = panelColor.transform.GetChild(0).GetComponent<Button>();
            btnYellow = panelColor.transform.GetChild(1).GetComponent<Button>();
            btnGreen = panelColor.transform.GetChild(2).GetComponent<Button>();
            btnBlue = panelColor.transform.GetChild(3).GetComponent<Button>();
        }

    }
    private void TagBarColor()
    {
        if (Mid == null || Left == null || Right == null)
        {
            Mid = transform.GetChild(3).GetChild(0).GetComponent<Image>();
            Left = transform.GetChild(3).GetChild(1).GetComponent<Image>();
            Right = transform.GetChild(3).GetChild(2).GetComponent<Image>();
        }
    }
    private void LoadColorTurn()
    {
        listColorTurn.Clear();
        listColorTurn = transform.GetChild(4).GetComponentsInChildren<Image>().ToList();
    }
    private void LoadText()
    {
        if (txtCountCardInHand.Count != 0) return;
        foreach(var obj in listColorTurn)
        {
            txtCountCardInHand.Add(obj.transform.GetComponentInChildren<TextMeshProUGUI>());
        }
    }
    private void ClickButtonColor(string color)
    {
        switch (color)
        {
            case "RED":
                GameManager.Instance.TopColor(CardColor.RED);
                break;
            case "YELLOW":
                GameManager.Instance.TopColor(CardColor.YELLOW);
                break;
            case "GREEN":
                GameManager.Instance.TopColor(CardColor.GREEN);
                break;
            case "BLUE":
                GameManager.Instance.TopColor(CardColor.BLUE);
                break;
        }
        panelColor.SetActive(false);
        GameManager.Instance.SwitchTurn();
    }
    public void SetColor(CardColor color)
    {
        switch (color)
        {
            case CardColor.RED:
                Mid.color = Color.red;
                Right.color = Color.red;
                Left.color = Color.red;
                break;
            case CardColor.YELLOW:
                Mid.color = Color.yellow;
                Right.color = Color.yellow;
                Left.color = Color.yellow;
                break;
            case CardColor.GREEN:
                Mid.color = Color.green;
                Left.color = Color.green;
                Right.color = Color.green;
                break;
            case CardColor.BLUE:
                Mid.color = Color.blue;
                Left.color = Color.blue;
                Right.color = Color.blue;
                break;
        }
    }
    public void SetActiveTurn()
    {
        if(Right.IsActive() == false)
        {
            Right.gameObject.SetActive(true);
            Left.gameObject.SetActive(false);
        }
        else
        {
            Right.gameObject.SetActive(false);
            Left.gameObject.SetActive(true);
        }
    }

    public void UITurnPlayer(int oldPlayer,int currentPlayer)
    {
        listColorTurn[oldPlayer].color = Color.black;
        listColorTurn[currentPlayer].color = Color.green;
    }
    public void UpdateTextCard(List<Player> players)
    {
        foreach (Player player in players) {
            int playerIndex = players.IndexOf(player); // Lấy vị trí của player trong danh sách
            txtCountCardInHand[playerIndex].text = player.playerHand.Count.ToString();
        }
    }
    
}
