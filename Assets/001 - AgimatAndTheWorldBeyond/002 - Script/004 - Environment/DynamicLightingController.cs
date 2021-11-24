using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLightingController : MonoBehaviour
{
    [SerializeField] private string whoIsPlayer;

    [Header("ENVIRONMENT LIGHTING")]
    [SerializeField] private Color skyColor;
    [SerializeField] private Color equatorColor;
    [SerializeField] private Color groundColor;

    [Header("DIRECTIONAL LIGHTING")]
    [SerializeField] private Color lightColor;
    [SerializeField] private float intensity;

    [Header("TRANSITION")]
    [SerializeField] private float transitionDuration;

    Coroutine currentCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(whoIsPlayer))
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine("TransitionAnimation");
        }
    }

    IEnumerator TransitionAnimation()
    {
        float time = 0f;
        Color startSkyColor = RenderSettings.ambientSkyColor;
        Color startEquatorColor = RenderSettings.ambientEquatorColor;
        Color startGroundColor = RenderSettings.ambientGroundColor;
        Color startLightColor = GameManager.instance.directionalLight.color;
        float startIntensity = GameManager.instance.directionalLight.intensity;

        while (time < transitionDuration)
        {
            RenderSettings.ambientSkyColor = Color.Lerp(startSkyColor, skyColor, time / transitionDuration);
            RenderSettings.ambientEquatorColor = Color.Lerp(startEquatorColor, equatorColor, time / transitionDuration);
            RenderSettings.ambientGroundColor = Color.Lerp(startGroundColor, groundColor, time / transitionDuration);
            GameManager.instance.directionalLight.color = Color.Lerp(startLightColor, lightColor, time / transitionDuration);
            GameManager.instance.directionalLight.intensity = Mathf.Lerp(startIntensity, intensity, time / transitionDuration);

            time += Time.deltaTime;

            yield return null;
        }
    }
}
