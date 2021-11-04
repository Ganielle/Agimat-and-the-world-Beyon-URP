using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D characterCollider;
    [SerializeField] private CapsuleCollider2D characterBlocker;

    private void OnEnable()
    {
        Physics2D.IgnoreCollision(characterCollider, characterBlocker, true);
    }
}
