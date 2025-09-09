using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        player.CharacterRenderer.material.color = player.walkColor;
        if (player.StatusText != null) player.StatusText.text = "Walking";
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        // Panggil fungsi Jump() sebagai pemicu
        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
            return;
        }

        if (player.HorizontalInput == 0)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        Vector3 horizontalMovement = player.transform.right * player.HorizontalInput * player.moveSpeed;
        CombineMovementAndGravity(player, horizontalMovement);
    }

    public override void ExitState(PlayerStateMachine player) { }
}