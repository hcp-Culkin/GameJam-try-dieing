using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileFunction : MonoBehaviour
{
    // Start is called before the first frame update



    public Sprite[] SpritesToRandom;

    private void Start()
    {
        GetComponent<Image>().sprite = SpritesToRandom[Random.Range(0, SpritesToRandom.Length)];


    }

    public void OnItemPlacer()
    {
        ItemPlacer.Instance.OnPlaceButtonClicked(this.gameObject);
    }

    

}
