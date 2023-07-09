using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TruckControls : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class InteractionEvent : UnityEvent { }
    [Space(10)]
    [Header("Give the Functions to be executed")]
    public InteractionEvent onInteraction = new InteractionEvent();
    public Gamemanager gamemanager;
    public float Delay;
    public Transform FromMarker;
    public Transform ToMarker;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            gamemanager.DieChickenDie();
        }
    }


    void Start()
    {
    }


    public void SetInMotion()
    {
        transform.DOKill();
        transform.localPosition = FromMarker.localPosition;
        transform.DOLocalMove(ToMarker.localPosition, 2f).SetEase(Ease.Linear).SetDelay(Delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
