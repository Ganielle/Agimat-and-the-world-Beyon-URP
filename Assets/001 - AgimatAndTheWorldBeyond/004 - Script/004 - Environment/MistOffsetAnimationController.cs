using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistOffsetAnimationController : MonoBehaviour
{
    [SerializeField] private bool isSpriteRenderer;
    [ConditionalField("isSpriteRenderer")] [SerializeField] private SpriteRenderer sr;
    [ConditionalField("isSpriteRenderer", true)] [SerializeField] private MeshRenderer mr;
    [SerializeField] private Vector4 directionSpeed;

    private void OnEnable()
    {
        if (isSpriteRenderer)
            sr.material.SetVector("_DirectionVert", directionSpeed);
        else
            mr.material.SetVector("_DirectionVert", directionSpeed);
    }
}
