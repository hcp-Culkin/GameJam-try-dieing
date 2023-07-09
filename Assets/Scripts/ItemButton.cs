using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerClickHandler
{
    public static ItemButton SelectedButton;

    [SerializeField] GameObject selectedIndicator;
    [SerializeField] Image itemView;

    public ItemTypesEnum ItemType { get; private set; }

    public void Initalize(ItemTypesEnum itemType, Sprite img)
    {
        selectedIndicator.SetActive(false);
        ItemType = itemType;
        itemView.sprite = img;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SelectedButton == this) return;
        SelectedButton?.Deselect();
        Select();
    }

    public void Select()
    {
        selectedIndicator?.SetActive(true);
        SelectedButton = this;
    }

    public void Deselect()
    {
        selectedIndicator.SetActive(false);
    }
}
