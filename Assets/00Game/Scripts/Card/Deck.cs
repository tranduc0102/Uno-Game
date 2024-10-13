using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour,IPointerClickHandler
{
    public List<Card> cards = new List<Card>();
    public void InitDeck()
    {
        cards.Clear();
        foreach (CardColor color in System.Enum.GetValues(typeof(CardColor))){
            foreach (CardValue cardValue in System.Enum.GetValues(typeof(CardValue))){
                if (color != CardColor.NONE && cardValue != CardValue.NONE && cardValue!=CardValue.WILD && cardValue!=CardValue.WILD_DRAW_FOUR)
                {
                    cards.Add(new Card(color,cardValue));
                    cards.Add(new Card(color, cardValue));
                }
            }
        }
        for (int i = 0; i < 4; i++) { 
            cards.Add(new Card(CardColor.NONE,CardValue.WILD));
            cards.Add(new Card(CardColor.NONE,CardValue.WILD_DRAW_FOUR));
        }
    }
    public void ShuffleDeck()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            int newIndex = Random.Range(0, cards.Count - 1);
            cards[i] = cards[newIndex];
            cards[newIndex] = card;
        }
    }
    public Card DrawCard()
    {
        if(cards.Count == 0) return null;
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.Instance.CheckAnyCardPlay())
        {
            GameManager.Instance.DrawCardInDeck();
            GameManager.Instance.SwitchTurn();
        }
    }
}
