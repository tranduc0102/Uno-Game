using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    // Constructor của AIPlayer gọi constructor của lớp cha Player
    public AIPlayer(string name, bool isHuman) : base(name, isHuman)
    {
        // AIPlayer specific initialization
    }

    // Logic khi đến lượt AI chơi
    public override void TakeTurn(Card topCard)
    {
        // AI logic ở đây
        List<Card> listCard = GetListBestCard(topCard);
        Card cardToPlay = null;

        if (listCard.Count > 0)
        {
            cardToPlay = ChoiceBestCard(listCard);
            GameManager.Instance.PlayCard(null,cardToPlay);
            if (cardToPlay.color == CardColor.NONE)
            {
                GameManager.Instance.TopColor(ChoiceBestColor(listCard));
                GameManager.Instance.SwitchTurn();
            }
        }
        else
        {
            GameManager.Instance.DrawCardInDeck();
            GameManager.Instance.SwitchTurn();
        }
    }

    // Tìm lá bài tốt nhất trong danh sách
    private Card ChoiceBestCard(List<Card> list)
    {
        int CountCardNextPlayer = GameManager.Instance.GetCountCardNextPlayer();
        Card bestCard = null;

        foreach (Card card in list)
        {
            // Ưu tiên các lá hành động khi người chơi tiếp theo còn ít bài
            if (CountCardNextPlayer <= 2)
            {
                if (card.value == CardValue.WILD_DRAW_FOUR ||
                    card.value == CardValue.DRAW_TWO ||
                    card.value == CardValue.SKIP ||
                    card.value == CardValue.REVERSE ||
                    card.value == CardValue.WILD)
                {
                    return card; // Ưu tiên lá hành động ngay
                }
            }

            // So sánh giá trị của các lá bài không phải hành động
            if (bestCard == null || card.value > bestCard.value)
            {
                bestCard = card;
            }
        }

        return bestCard ?? list[0]; // Trả về lá bài tốt nhất, hoặc lá bài đầu tiên nếu không có
    }

    // Chọn màu tốt nhất dựa trên số lượng lá bài của mỗi màu
    private CardColor ChoiceBestColor(List<Card> cards)
    {
        Dictionary<CardColor, int> cardsColor = new Dictionary<CardColor, int>()
        {
            { CardColor.RED, 0 },
            { CardColor.BLUE, 0 },
            { CardColor.GREEN, 0 },
            { CardColor.YELLOW, 0 }
        };

        foreach (Card card in cards)
        {
            if (card.color != CardColor.NONE)
            {
                cardsColor[card.color]++;
            }
        }

        // Chọn màu có nhiều lá bài nhất
        CardColor bestColor = CardColor.RED;
        int maxCount = -1;

        foreach (var cardColor in cardsColor)
        {
            if (cardColor.Value > maxCount)
            {
                maxCount = cardColor.Value;
                bestColor = cardColor.Key;
            }
        }

        return bestColor;
    }

    // Lấy danh sách các lá bài có thể chơi
    private List<Card> GetListBestCard(Card topCard)
    {
        List<Card> bestList = new List<Card>();

        foreach (Card card in playerHand)
        {
            if (card.color == topCard.color || card.value == topCard.value || card.color == CardColor.NONE)
            {
                bestList.Add(card);
            }
        }

        return bestList;
    }
}
