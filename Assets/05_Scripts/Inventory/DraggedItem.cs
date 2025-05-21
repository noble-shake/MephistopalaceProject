using UnityEngine;

public class Draggeditem : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject dragged;

    private void Start()
    {
        dragged = GetComponentInParent<SlotObject>().itemInfo;  
    }

    public void SetItem(ItemScriptableObject _item)
    { 
        dragged = _item;
    }
}