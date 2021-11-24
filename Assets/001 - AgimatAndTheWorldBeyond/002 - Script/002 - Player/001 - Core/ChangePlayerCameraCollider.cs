using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangePlayerCameraCollider : MonoBehaviour
{
    [SerializeField] private string tagMask;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private float transitionSpeed;

    //Coroutine currentCoroutine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            GameManager.instance.mainCameraCMBrain.m_DefaultBlend.m_Time = transitionSpeed;

            playerCamera.gameObject.SetActive(true);

            if (GameManager.instance.currentPlayerCamera != null && 
                GameManager.instance.currentPlayerCamera != playerCamera)
                GameManager.instance.currentPlayerCamera.gameObject.SetActive(false);

            GameManager.instance.currentPlayerCamera = playerCamera;
        }
    }
}
