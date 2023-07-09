using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    public static ItemPlacer Instance;
    public GameObject PlacementUI;
    [SerializeField] ItemPlacerSpriteHelper[] itemSprites;
    [SerializeField] ScrollRect scroll;
    [SerializeField] Transform placementTransform;
    [SerializeField] Button playButton;
    List<ItemTypesEnum> itemsToPlace = new List<ItemTypesEnum>();
    private void Awake()
    {
        Instance = this;
    }

    public void SetUpDisplay(ItemTypesEnum[] items)
    {
        if (FindObjectOfType<Gamemanager>().Level == 0)
        {
            playButton.interactable = true;
            PlacementUI.SetActive(false);
            return;
        }

        PlacementUI.SetActive(true);
        itemsToPlace = new List<ItemTypesEnum>(items);
        clearList = new Dictionary<GameObject, ItemTypesEnum>();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        ItemButton.SelectedButton = null;
        int index;
        for (index = 0; index < itemsToPlace.Count; index++)
        {
            GetItemTransform(index).GetComponent<ItemButton>().Initalize(itemsToPlace[index], GetSprite(itemsToPlace[index]).sprite);
        }

        for (; index < scroll.content.childCount; index++)
        {
            scroll.content.GetChild(index).gameObject.SetActive(false);
        }
        playButton.interactable = itemsToPlace.Count == 0;
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

        var t = ItemButton.SelectedButton.ItemType;
        GameObject item = Instantiate(GetSprite(ItemButton.SelectedButton.ItemType).prefab, placementTransform);
        item.GetComponent<ItemMover>().Initialize(clickedObject.transform);
        clearList.Add(item, t);
        itemsToPlace.Remove(t);
        UpdateDisplay();
    }

    ItemPlacerSpriteHelper GetSprite(ItemTypesEnum itemType)
    {
        foreach (var itm in itemSprites)
        {
            if (itemType == itm.itemType) return itm;
        }

        return null;
    }

    public void BeginMovement()
    {
        for (int i = 0; i < placementTransform.childCount; i++)
        {
            placementTransform.GetChild(i).GetComponent<ItemMover>().StartMoving();
        }
    }
    public void StopMovement()
    {
        for (int i = 0; i < placementTransform.childCount; i++)
        {
            placementTransform.GetChild(i).GetComponent<ItemMover>().ResetMovement();
        }

       // SetUpDisplay(tmpItems); //TODO: Pass in from game manager instead of here
    }

    Dictionary<GameObject, ItemTypesEnum> clearList;
    public void ClearButton()
    {
        foreach (var item in clearList)
        {
            Destroy(item.Key);
            itemsToPlace.Add(item.Value);
        }
        clearList.Clear();
        UpdateDisplay();
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