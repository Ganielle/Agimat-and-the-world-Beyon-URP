using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomerDebug : MonoBehaviour
{
    public bool Zooming
    {
        get => isZooming;
        private set => isZooming = value;
    }


    [SerializeField] private CinemachineCameraOffset vcam;
    [SerializeField] private float resetZoomValue;

    [SerializeField] private bool isZooming;
    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            vcam.m_Offset.z = resetZoomValue;
            Zooming = false;
            return;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            vcam.m_Offset.z += Input.mouseScrollDelta.y;
            Zooming = true;
        }

    }
}
