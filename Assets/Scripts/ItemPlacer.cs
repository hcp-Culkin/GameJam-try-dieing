using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    public static ItemPlacer Instance;
    [SerializeField] ItemPlacerSpriteHelper[] itemSprites;
    [SerializeField] ScrollRect scroll;
    [SerializeField] ItemTypesEnum[] tmpItems;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetUpDisplay(tmpItems);
    }

    public void SetUpDisplay(ItemTypesEnum[] items)
    {
        int index;
        for (index = 0; index < items.Length; index++)
        {
            GetItemTransform(index).GetComponent<ItemButton>().Initalize(items[index], GetSprite(items[index]).sprite);
        }

        for(; index< scroll.content.childCount; index++)
        {
            scroll.content.GetChild(index).gameObject.SetActive(false);
        }
    }

    Transform GetItemTransform(int index)
    {
        Transform child;
        if (index < scroll.content.childCount)
        {
            child = scroll.content.GetChild(index);
        }
        else
        {
            child = Instantiate(scroll.content.GetChild(0), scroll.content);
        }

        child.gameObject.SetActive(true);
        return child;
    }

    public void OnPlaceButtonClicked(GameObject clickedObject)
    {
        if (clickedObject == null) return;
        if (ItemButton.SelectedButton == null) return;


    }

    ItemPlacerSpriteHelper GetSprite(ItemTypesEnum itemType)
    {
        foreach (var itm in itemSprites)
        {
            if (itemType == itm.itemType) return itm;
        }

        return null;
    }
}

[System.Serializable]
public class ItemPlacerSpriteHelper
{
    public ItemTypesEnum itemType;
    public Sprite sprite;
    public GameObject prefab;
}

public enum ItemTypesEnum
{
    Truck, StartPoint
}