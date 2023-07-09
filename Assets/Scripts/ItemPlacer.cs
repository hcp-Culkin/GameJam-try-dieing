using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{

    private void Start()
    {
        
    }

    public void OnPlaceButtonClicked(GameObject clickedObject)
    {

    }
}

public class ItemPlacerSpriteHelper
{
    public ItemTypesEnum itemType;
    public Sprite sprite;
}

public enum ItemTypesEnum
{
    Truck, StartPoint
}