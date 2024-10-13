using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panel Choice Color")]
    public GameObject panelColor;
    public Button btnRed;
    public Button btnYellow;
    public Button btnGreen;
    public Button btnBlue;

    [Header("TagBar color")]
    public Image Mid; //dùng để thay đổi đổi thanh điều hướng của game
    public Image Left;
    public Image Right;

    [Header("Current Player")]
    public List<Image> list;


    void Start()
    {
        panelColor.SetActive(false);
        btnRed.onClick.AddListener((() => ClickButtonColor("RED")));
        btnYellow.onClick.AddListener((() => ClickButtonColor("YELLOW")));
        btnGreen.onClick.AddListener((() => ClickButtonColor("GREEN")));
        btnBlue.onClick.AddListener(()=>ClickButtonColor("BLUE"));
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
        list[oldPlayer].color = Color.black;
        list[currentPlayer].color = Color.green;
    }
 /*   public void UpdateTextCard(int currendPlayer)
    {
        list[currentPlayer].GetComponentInChildren<TextMeshProUGUI>().text = cardInHand.ToString();
    }*/
    
}
