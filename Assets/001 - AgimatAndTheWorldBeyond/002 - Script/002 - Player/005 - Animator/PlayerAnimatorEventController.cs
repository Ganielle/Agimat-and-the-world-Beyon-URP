using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEventController : MonoBehaviour
{
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private PlayerStateMachinesController movementController;
    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private SpriteRenderer sr;

    public void AnimationTrigger() => movementController.statemachineChanger.CurrentState.AnimationTrigger();

    public void AnimationFinishTrigger() => movementController.statemachineChanger.CurrentState.AnimationFinishTrigger();

    public void DisableEnvironmentCollider(bool stats) => playerHitbox.enabled = stats;

    public void ResetVelocity() => playerCore.SetVelocityZero();

    #region ABILITY STATES

    public void SwitchCharacter()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LILY;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LUKAS;
    }
    public void DodgeState(Sprite sprite) => sr.sprite = sprite;

    #endregion

    #region ATTACK STATES

    public void GroundAttackMovementVelocity(float value) => playerCore.playerRB.AddForce(Vector2.right * value * playerCore.GetFacingDirection, ForceMode2D.Impulse);

    public void OnLastComboAttackIndex() => playerCore.attackController.onLastAttackIndex = true;

    public void CannotNextAttack() => playerCore.attackController.canNextAttack = false;

    public void CanChangeDirectionWhenAttacking() => playerCore.attackController.canChangeDirectionWhenAttacking = true;

    public void CannotChangeDirectionWhenAttacking() => playerCore.attackController.canChangeDirectionWhenAttacking = false;

    public void CanAnimationCancel() => playerCore.attackController.canAnimationCancel = true;

    public void CannotAnimationCancel() => playerCore.attackController.canAnimationCancel = false;

    #endregion
}
