using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicCameraChangerController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    [SerializeField] private string tagMask;
    [SerializeField] private float transitionSpeed;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            GameManager.instance.mainCameraCMBrain.m_DefaultBlend.m_Time = transitionSpeed;

            vcam.gameObject.SetActive(true);
            if (GameManager.instance.currentPlayerCamera != null &&
                GameManager.instance.currentPlayerCamera != vcam)
                GameManager.instance.currentPlayerCamera.gameObject.SetActive(false);

            GameManager.instance.currentPlayerCamera = vcam;
        }
    }
}
