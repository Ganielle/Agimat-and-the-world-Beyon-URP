using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerParticleController : MonoBehaviour
{
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private List<SpriteRenderer> effectList;
    [SerializeField] private Renderer distortion;
    [SerializeField] private float speed;
    [SerializeField] private float speedDistortFadeIn;
    [SerializeField] private float speedDistortFadeOut;

    [SerializeField] private float activeTime = 2.1f;

    [Header("Debugger")] 
    [ReadOnly] [SerializeField] private Transform player;
    [ReadOnly] [SerializeField] private float timeActivated;
    [ReadOnly] public bool isCanceled;
    [ReadOnly] [SerializeField] bool firstInstance;

    Coroutine currentFadeDistort;

    private void Awake()
    {
        firstInstance = true;
    }

    private void OnEnable()
    {
        SetSettings();
        currentFadeDistort = StartCoroutine(StartDistortion());
    }

    private void PlayerObjChange(object sender, EventArgs e)
    {
        SetSettings();
    }

    private void SetSettings()
    {
        gameObject.GetComponent<Animator>().enabled = true;

        if (!firstInstance)
        {
            GameManager.instance.PlayerStats.onPlayerCharacterObjChange += PlayerObjChange;

            player = GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform;

            transform.position = player.position;
            transform.rotation = player.rotation;

            foreach (SpriteRenderer renderer in effectList)
                renderer.material.SetFloat("_AlphaClipThreshold", 0.5f);

            timeActivated = Time.time;
        }

        if (firstInstance)
            firstInstance = false;
    }

    private void Update()
    {
        if (Time.time >= (timeActivated + activeTime) && !isCanceled)
            GameManager.instance.effectManager.switchEffectPooler.AddToPool(gameObject);
    }

    public IEnumerator StopParticles()
    {
        gameObject.GetComponent<Animator>().enabled = false;

        StopCoroutine(currentFadeDistort);

        float time = 0f;
        float distortamount = distortion.material.GetFloat("_DistortionAmount");

        foreach (SpriteRenderer renderer in effectList)
        {
            LeanTween.alpha(renderer.gameObject, 0f, speed).setEase(easeType);
        }

        while (time < speed)
        {
            distortion.material.SetFloat("_DistortionAmount", Mathf.Lerp(distortamount, 0f, time / speed));

            time += Time.deltaTime;
            yield return null;
        }

        isCanceled = false;
        GameManager.instance.effectManager.switchEffectPooler.AddToPool(gameObject);
    }

    IEnumerator StartDistortion()
    {
        float time = 0f, fadeoutTime = 0f;
        float distortamount = distortion.material.GetFloat("_DistortionAmount");

        while (time < speedDistortFadeIn)
        {
            distortion.material.SetFloat("_DistortionAmount", Mathf.Lerp(distortamount, 0.5f, time / speedDistortFadeIn));

            time += Time.deltaTime;
            yield return null;
        }

        distortamount = distortion.material.GetFloat("_DistortionAmount");

        while (fadeoutTime < speedDistortFadeOut)
        {
            distortion.material.SetFloat("_DistortionAmount", Mathf.Lerp(distortamount, 0f, fadeoutTime / speedDistortFadeOut));

            Debug.Log("hello");

            fadeoutTime += Time.deltaTime;
            yield return null;
        }
    }
}
