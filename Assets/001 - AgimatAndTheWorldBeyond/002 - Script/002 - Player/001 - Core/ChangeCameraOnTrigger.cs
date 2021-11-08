using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameraOnTrigger : MonoBehaviour
{
    [SerializeField] private string tagMask;
    [SerializeField] private float transitionSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            GameManager.instance.mainCameraCMBrain.m_DefaultBlend.m_Time = transitionSpeed;
            collision.gameObject.GetComponent<CinematicCameraChangerController>().vcam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagMask))
        {
            GameManager.instance.mainCameraCMBrain.m_DefaultBlend.m_Time = 1f;
            collision.gameObject.GetComponent<CinematicCameraChangerController>().vcam.SetActive(false);
        }
    }
}
