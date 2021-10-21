using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour
{
    [Header("AUDIO CLIP")]
    public AudioClip landJumpClip;
    public AudioClip wallJumpClip;

    [Header("AUDIO SOURCE")]
    public AudioSource footAS;

    public void PlaySFX(AudioSource source, AudioClip clip) => source.PlayOneShot(clip);
}
