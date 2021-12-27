using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventDisabler : MonoBehaviour
{
    public void AnimationDisable() => gameObject.SetActive(false);
}
