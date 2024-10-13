using System.Collections.Generic;
[System.Serializable]
public class Player {
    public string playerName;
    public List<Card> playerHand;
    public bool isHuman;
   public Player(string playerName,bool isHuman)
    {
        this.playerName = playerName;
        this.playerHand = new List<Card>();
        this.isHuman = isHuman;
    }
    public void DrawCard(Card card)
    {
        playerHand.Add(card);
    }
    public void PlayCard(Card card)
    {
        playerHand.Remove(card);
    }

    public virtual void TakeTurn(Card card){}
}
