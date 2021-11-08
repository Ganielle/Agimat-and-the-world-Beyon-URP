using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangePlayerCameraCollider : MonoBehaviour
{
    [SerializeField] private string tagMask;
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private float damp;

    Coroutine currentCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            currentCoroutine = StartCoroutine(ChangeDamp());
            GameManager.instance.gameplayConfiner.m_BoundingShape2D = polygonCollider;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            GameManager.instance.gameplayConfiner.m_Damping = 0f;
            StopCoroutine(currentCoroutine);
        }
    }

    IEnumerator ChangeDamp()
    {
        GameManager.instance.gameplayConfiner.m_Damping = damp;

        yield return new WaitForSeconds(damp);

        GameManager.instance.gameplayConfiner.m_Damping = 0f;
    }
}
