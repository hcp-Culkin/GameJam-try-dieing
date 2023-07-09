using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFunction : MonoBehaviour
{
    // Start is called before the first frame update


    public void OnItemPlacer()
    {
        ItemPlacer.Instance.OnPlaceButtonClicked(this.gameObject);
    }

    

}
