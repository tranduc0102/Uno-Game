using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField]private CardDisplay cardDisplay;
    private Vector3 orginalPos;
    private float liftAmount=30f;
    private void Start()
    {
        cardDisplay = GetComponent<CardDisplay>();
        orginalPos = transform.localPosition; // localPos dùng trong UI
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cái này kiểu như xác định sự kiện vẫn còn giữ thẻ
        LiftCard(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // cái này xác định sự kiện khi thả thẻ ra
        LiftCard(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // cái này xác định sự kiện khi ấn vào thẻ
        if (GameManager.Instance.TurnPlayer && cardDisplay.Owner.isHuman)
        {
            GameManager.Instance.PlayCard(cardDisplay);
            Debug.Log("Click Card: "+ cardDisplay.MyCard.color.ToString() + cardDisplay.MyCard.value.ToString());
        }
    }

    private void LiftCard(bool lift)
    {
        if (lift && cardDisplay.Owner.isHuman)
        {
            transform.localPosition = orginalPos + new Vector3(0,liftAmount,0);
        }
        else
        {
            transform.localPosition = orginalPos;
        }
    }
}
