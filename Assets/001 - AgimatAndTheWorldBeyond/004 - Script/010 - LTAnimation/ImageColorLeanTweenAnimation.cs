using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorLeanTweenAnimation : MonoBehaviour
{
    [SerializeField] private Image image;

    [ReadOnly] [SerializeField] List<int> imageTweenAnimation;

    public void ImageColorTweenAnimation(LeanTweenType easeType, float animSpeed,
        float animDelay, float r, float g, float b, float a, [Optional] Action actionAfterAnimation)
    {

        imageTweenAnimation.Add(LeanTween.value(image.gameObject, color => image.color = color,
            new Color(image.color.r, image.color.g, image.color.b, image.color.a), new Color(r, g, b, a), animSpeed).
            setDelay(animDelay).setEase(easeType).setOnComplete(actionAfterAnimation).id);
    }

    public void ImageFadeTweenAnimation(LeanTweenType easeType, float animSpeed,
        float animDelay, float alphaTo, [Optional] Action actionAfterAnimation)
    {
        imageTweenAnimation.Add(LeanTween.value(image.gameObject, color => image.color = color,
            new Color(image.color.r, image.color.g, image.color.b, image.color.a), 
            new Color(image.color.r, image.color.g, image.color.b, alphaTo), animSpeed).
            setDelay(animDelay).setEase(easeType).setOnComplete(actionAfterAnimation).id);
    }

    public void CancelAnimation()
    {
        foreach (int id in imageTweenAnimation)
            LeanTween.cancel(gameObject, id);

        imageTweenAnimation.Clear();
    }
}
