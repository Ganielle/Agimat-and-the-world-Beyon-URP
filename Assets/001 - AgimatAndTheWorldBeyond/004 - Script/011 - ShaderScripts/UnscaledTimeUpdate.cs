using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeUpdate : MonoBehaviour
{
    [SerializeField] private bool isImage;
    [ConditionalField("isImage")] [SerializeField] private Image imageRenderer;
    [ConditionalField("isImage", true)] [SerializeField] private Renderer graphicsRenderer;

    private void Update()
    {
        if (!isImage)
            graphicsRenderer.material.SetFloat("_UnscaledTime", Time.unscaledTime);
        else
            imageRenderer.material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
