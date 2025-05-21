using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotObject : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] public ItemScriptableObject itemInfo;
    [SerializeField] public bool isItemExist;
    [SerializeField] public bool isDragged;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected Draggeditem dragged;
    [SerializeField] protected TMP_Text numbText;
    [Space]
    [Header("Color")]
    [SerializeField] protected Color DefaultColor;
    [SerializeField] protected Color HighlightColor;

    public virtual void Equip()
    {
        isItemExist = true;
        itemImage.sprite = itemInfo.itemImage;
        backgroundImage.sprite = itemInfo.itemImage;

        itemImage.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);
    }

    public virtual void UnEqup()
    {
        isItemExist = false;
        itemInfo = null; // 중복 생성 방지!!
        itemImage.sprite = null;
        backgroundImage.sprite = null;

        itemImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }

    public virtual  void OnBeginDrag(PointerEventData eventData)
    {
      
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnDrop(PointerEventData eventData)
    {

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = HighlightColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = DefaultColor;
    }
}