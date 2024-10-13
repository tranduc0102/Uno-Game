using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private Sprite sprite;
    private Sprite hintSprite;
    public CardColor color;
    public CardValue value;
    private Dictionary<CardColor, System.Action> colorActions;

    private Player own;
    private Card myCard;
    public Card MyCard
    {
        get => myCard;
        set => myCard = value;
    }

    public Player Owner => own;
    private void OnEnable()
    {
        image= GetComponent<Image>();
        InitializeColorActions();
        InitImageHint();
    }

    private void InitImageHint()
    {
        this.color = CardColor.NONE;
        this.value = CardValue.NONE;
        if (colorActions.ContainsKey(color))
        {
            colorActions[color].Invoke();
        }

        hintSprite = sprite;
    }

    public void SetCard(Card card,Player owner)
    {
        this.color = card.color;
        this.value = card.value;
        myCard = card;
        if (colorActions.ContainsKey(color))
        {
            colorActions[color].Invoke();
        }
        this.image.sprite = sprite;
        this.own = owner;
    }

    public void SetColor(CardColor cardColor)
    {
        color = cardColor;
        myCard.color = cardColor;
    }

    public void HintCard()
    {
        this.image.sprite = hintSprite;
    }

    public void ShowCard()
    {
        this.image.sprite = sprite;
    }
    
    private void InitializeColorActions()
    {
        colorActions = new Dictionary<CardColor, System.Action>
        {
            { CardColor.RED, SearchRedValue },
            { CardColor.GREEN, SearchGreenValue },
            { CardColor.BLUE, SearchBlueValue },
            { CardColor.YELLOW, SearchYellowValue },
            { CardColor.NONE, SearchNoneValue }
        };
    }

    private void SearchBlueValue()
    {
        SetSpriteFromResources("CardBlue");
    }

    private void SearchRedValue()
    {
        SetSpriteFromResources("CardRed");
    }

    private void SearchGreenValue()
    {
        SetSpriteFromResources("CardGreen");
    }

    private void SearchYellowValue()
    {
        SetSpriteFromResources("CardYellow");
    }

    private void SearchNoneValue()
    {
        Sprite[] cardSprite = Resources.LoadAll<Sprite>("Other");
        switch (value)
        {
            case CardValue.WILD:
                sprite = cardSprite[1];
                break;
            case CardValue.WILD_DRAW_FOUR:
                sprite = cardSprite[2];
                break;
            case CardValue.NONE:
                sprite = cardSprite[0];
                break;
        }
    }

    private void SetSpriteFromResources(string resourceName)
    {
        Sprite[] cardSprite = Resources.LoadAll<Sprite>(resourceName);
        switch (value)
        {
            case CardValue.ZERO:
                sprite = cardSprite[0];
                break;
            case CardValue.ONE:
                sprite = cardSprite[1];
                break;
            case CardValue.TWO:
                sprite = cardSprite[2];
                break;
            case CardValue.THREE:
                sprite = cardSprite[3];
                break;
            case CardValue.FOUR:
                sprite = cardSprite[4];
                break;
            case CardValue.FIVE:
                sprite = cardSprite[5];
                break;
            case CardValue.SIX:
                sprite = cardSprite[6];
                break;
            case CardValue.SEVEN:
                sprite = cardSprite[7];
                break;
            case CardValue.EIGHT:
                sprite = cardSprite[8];
                break;
            case CardValue.NINE:
                sprite = cardSprite[9];
                break;
            case CardValue.DRAW_TWO:
                sprite = cardSprite[10];
                break;
            case CardValue.REVERSE:
                sprite = cardSprite[11];
                break;
            case CardValue.SKIP:
                sprite = cardSprite[12];
                break;
        }
    }
}
