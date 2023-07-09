using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ItemMover : MonoBehaviour
{
    [SerializeField] float moveWait = 1f;
    [SerializeField] float moveDuration = 0.25f;
    [SerializeField] int size = 2;

    int position = 0;
    Coroutine movingRoutine;
    Transform initialParent;
    public void Initialize(Transform parent)
    {
        initialParent = parent;
        transform.position = parent.position;
        position = initialParent.GetSiblingIndex();
    }

    public void StartMoving()
    {
        movingRoutine = StartCoroutine(MovingLoop());
    }

    public void ResetMovement()
    {
        transform.DOKill();
        if (movingRoutine != null)
        {
            StopCoroutine(movingRoutine);
        }
        transform.position = initialParent.position;
        position = initialParent.GetSiblingIndex();
    }

    IEnumerator MovingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveWait);
            position++;
            var move = transform.DOMove(GetIndexTransform().position, moveDuration);
            yield return new WaitWhile(move.IsPlaying);
        }
    }

    Transform GetIndexTransform()
    {
        if (position < 0)
        {
            position = initialParent.parent.childCount - 1;
            transform.position = initialParent.parent.GetChild(position).position;
            position--;
        }
        else if (position >= initialParent.parent.childCount)
        {
            position = 0;
            transform.position = initialParent.parent.GetChild(position).position;
            position++;
        }

        return initialParent.parent.GetChild(position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            FindObjectOfType<Gamemanager>().DieChickenDie();
        }
    }
}
