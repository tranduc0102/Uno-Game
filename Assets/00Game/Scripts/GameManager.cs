using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Player> Players = new List<Player>();

    [SerializeField] private Deck Deck;
    [SerializeField] private Transform playerHandTransform; // Vị trí của người chơi trên Scence Game
    [SerializeField] private RectTransform discardPileTransform; // Vị trí của AI Player
    [SerializeField] private CardDisplay topCard; // Xác định thẻ đầu tiền ở trên cùng mỗi lượt đánh
    [SerializeField] private List<Transform> aiHandTransforms = new List<Transform>(); 
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int numberOfAIPlayer = 3; //Số lượng AI có ở trong phòng
    [SerializeField] private int startingHandSize = 7; // Số lượng bài khi bắt đầu
    [SerializeField] private int currentPlayer = 0; //người chơi hiện tại
    [SerializeField] private bool turnPlayer;
    private int nextCountTurn = 1;

    public CardDisplay TopCard
    {
        get => topCard;
        set => topCard = value;
    }
    
    public bool TurnPlayer
    {
        get => turnPlayer;
    }
    private void Start()
    {
        Deck.InitDeck();
        Deck.ShuffleDeck();
        InitPlayer();
        StartCoroutine(DealStartingCards());
    }

    private void InitPlayer()
    {
        Players.Clear();
        Players.Add(new Player("Player", true)); // thêm 1 người chơi
        for (int i = 0; i < numberOfAIPlayer; i++)
        {
            Players.Add(new AIPlayer("AI " + (i + 1), false));
        }
    }

    //Hàm này dùng để thêm thẻ vào vị trí của người chơi lúc bắt đầu
    private IEnumerator DealStartingCards()
    {
        for (int i = 0; i < startingHandSize; i++)
        {
            foreach (Player player in Players)
            {
                Card drawCard = Deck.DrawCard();
                AddCardToPlayerHand(drawCard, player);
            }
            AudioGame.Instance.PlayAudioClip();
            UIManager.Instance.UpdateTextCard(Players);
            yield return new WaitForSeconds(0.1f);
        }
        SetupFirstCard();
        StartPlayerTurn();
    }

    //Hàm này để thêm thẻ vô tay người chơi ví dụ như bốc bài hoặc bị Draw
    private void AddCardToPlayerHand(Card drawCard, Player player)
    {
        Transform handTransform = player.isHuman ? playerHandTransform : aiHandTransforms[Players.IndexOf(player) - 1];
        GameObject card = Instantiate(cardPrefab, handTransform, false);
        CardDisplay cardDisplay = card.GetComponentInChildren<CardDisplay>();
        cardDisplay.SetCard(drawCard, player);
        player.playerHand.Add(drawCard);

        if (!player.isHuman)
        {
            cardDisplay.HintCard(); // Display AI hand hint
        }
    }

    //Hàm này để Tạo ra một thẻ đầu tiên để chơi theo màu thẻ đó
    private void SetupFirstCard()
    {
        Card firstCard = Deck.DrawCard();
        GameObject card = Instantiate(cardPrefab, discardPileTransform, false);
        topCard = card.GetComponentInChildren<CardDisplay>();
        topCard.SetCard(firstCard, null);
        if (topCard.MyCard.color == CardColor.NONE)
        {
            topCard.SetColor(RandomColor());
        }
        UIManager.Instance.SetColor(topCard.MyCard.color);
        topCard.GetComponent<CardInteraction>().enabled = false;
    }
    //Nếu màu thẻ đầu tiên là None thì sẽ RanDom
    private CardColor RandomColor()
    {
        int index = Random.Range(0,4);
        CardColor color = CardColor.NONE;
        switch (index)
        {
            case 0:
                color = CardColor.RED;
                break;
            case 1:
                color = CardColor.YELLOW;
                break;
            case 2:
                color = CardColor.BLUE;
                break;
            case 3:
                color = CardColor.GREEN;
                break;
        }
        return color;
    }

    // Hàm này dùng khi bốc bài
    public void DrawCardInDeck()
    {
        Card card = Deck.DrawCard();
        if (card != null)
        {
            Player player = Players[currentPlayer];
            AddCardToPlayerHand(card, player);
        }
    }
    //Hàm này khi người chơi hoặc AI dùng thẻ trên tay 
    public void PlayCard(CardDisplay cardDisplay = null,Card card = null)
    {
        Card cardToPlay = cardDisplay?.MyCard??card;
        if (cardDisplay == null && card != null)
        {
            cardDisplay = FindCardDiplayForCard(card);
        }
        if(!CheckCard(cardToPlay))return;
        if (cardDisplay != null)
        {
            cardDisplay.ShowCard();
            Players[currentPlayer].PlayCard(cardToPlay);
            MoveCardToPile(cardDisplay.transform.parent.gameObject, cardToPlay);
            UpdateTopCard(cardDisplay);
        }
    }
    //Hàm này tìm thẻ trên tay của AI ứng với phần hiện thị của Thẻ 
    private CardDisplay FindCardDiplayForCard(Card card)
    {
        Player player = Players[currentPlayer];
        Transform hand = player.isHuman ? playerHandTransform : aiHandTransforms[Players.IndexOf(player) - 1];
        foreach (Transform cardTransform in hand)
        {
            CardDisplay temDisplay = cardTransform.GetComponentInChildren<CardDisplay>();
            if (temDisplay.MyCard == card)
            {
                return temDisplay;
            }
        }
        return null;
    }

    //Di chuyển thẻ đến với nơi chứa những thẻ đánh rồi
    private void MoveCardToPile(GameObject currentCard,Card card)
    {
        currentCard.GetComponentInChildren<CardInteraction>().enabled = false;
        currentCard.transform.SetParent(discardPileTransform);
        AudioGame.Instance.PlayAudioClip();
        currentCard.transform.DOMove(discardPileTransform.transform.position, 0.4f).OnComplete(() =>
        {
            currentCard.transform.localPosition = Vector3.zero;
        });
        Quaternion pileQuater = discardPileTransform.rotation;
        float randomZRot = UnityEngine.Random.Range(-10f, 10f);
        Quaternion randomRot = Quaternion.Euler(0f,0f,randomZRot);
        currentCard.transform.rotation = pileQuater * randomRot;
        EffectCard(card);
        if (card.value != CardValue.SKIP && card.value!=CardValue.WILD_DRAW_FOUR && card.value!=CardValue.WILD)
        {
            SwitchTurn();
        }
    }
    //Đổi lượt
    public void SwitchTurn(bool isSkip = false)
    {
        int numberOfPlayer = Players.Count;
        int old = currentPlayer;
        if (isSkip)
        {
            currentPlayer = (currentPlayer + 2 * nextCountTurn + numberOfPlayer) % numberOfPlayer;
        }
        else
        {
            currentPlayer = (currentPlayer + nextCountTurn + numberOfPlayer) % numberOfPlayer;
        }
    
       
        if (currentPlayer > Players.Count - 1)
        {
            currentPlayer = 0;
        }else if (currentPlayer < 0)
        {
            currentPlayer = Players.Count - 1;
        }
        turnPlayer = Players[currentPlayer].isHuman; // Only allow human player to have tur
        UIManager.Instance.UpdateTextCard(Players);
        UIManager.Instance.UITurnPlayer(old, currentPlayer);
        if (!turnPlayer)
        {
            StartCoroutine(TurnAI());
        }
    }

    IEnumerator TurnAI()
    {
        yield return new WaitForSeconds(2f);
        Players[currentPlayer].TakeTurn(topCard.MyCard);
    }
    //Lấy số lượng thẻ của người chơi tiếp theo
    public int GetCountCardNextPlayer()
    {
        int numberOfPlayer = Players.Count;
        int nextcurrentPlayer = (currentPlayer + nextCountTurn + numberOfPlayer) % numberOfPlayer;
        return Players[nextcurrentPlayer].playerHand.Count;
    }

    private void StartPlayerTurn()
    {
        UIManager.Instance.UITurnPlayer(0,0);
        if(Players[currentPlayer].isHuman) turnPlayer = true;
    }

    private void UpdateTopCard(CardDisplay card)
    {
        // Logic to update the top card on discard pile
        topCard = card;
        UIManager.Instance.SetColor(topCard.color);
    }

    public bool CheckAnyCardPlay() 
    {
        foreach (Card card in Players[currentPlayer].playerHand)
        {
            if (CheckCard(card))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckCard(Card card)
    {
        return topCard.MyCard.color == card.color || topCard.MyCard.value == card.value || card.color == CardColor.NONE;
    }

    private void EffectCard(Card card)
    {
        switch (card.value)
        {
            case CardValue.SKIP:
                SkipPlayer();
                break;
            case CardValue.REVERSE:
                Reveser();
                break;
            case CardValue.WILD:
                ChoiceColor();
                break;
            case CardValue.DRAW_TWO:
                DrawAmount(2);
                break;
            case CardValue.WILD_DRAW_FOUR:
                ChoiceColor();
                DrawAmount(4);
                break;
        }
    }

    private void SkipPlayer()
    {
        SwitchTurn(true);
    }

    private void ChoiceColor()
    {
        if (turnPlayer)
        {
            UIManager.Instance.panelColor.SetActive(true);   
        }
    }

    public void TopColor(CardColor color)
    {
        topCard.SetColor(color);
        UIManager.Instance.SetColor(color);
    }

    private void DrawAmount(int amount)
    {
        Player player;
        Card card;
        int numberOfPlayer = Players.Count;
        int nextcurrentPlayer = (currentPlayer + nextCountTurn + numberOfPlayer) % numberOfPlayer;
        for (int i = 0; i < amount; i++)
        {
           player = Players[nextcurrentPlayer];
           card = Deck.DrawCard();
           AddCardToPlayerHand(card, player);
        }
    }

    private void Reveser()
    {
        nextCountTurn = -nextCountTurn;
        UIManager.Instance.SetActiveTurn();
    }
}
