public enum CardColor
{
    NONE,
    RED,
    GREEN,
    BLUE,
    YELLOW,
}

public enum CardValue
{
    NONE,
    ZERO,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    NINE,
    DRAW_TWO,
    REVERSE,
    SKIP,
    WILD,
    WILD_DRAW_FOUR
}

[System.Serializable]
public class Card
{
    public CardColor color;
    public CardValue value;
    public Card(CardColor color, CardValue value)
    {
        this.color = color;
        this.value = value;
    }
}
