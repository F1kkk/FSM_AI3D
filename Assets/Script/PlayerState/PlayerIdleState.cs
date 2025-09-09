using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        player.CharacterRenderer.material.color = player.idleColor;
        if (player.StatusText != null) player.StatusText.text = "Idle";
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(player.HidingState);
            return;
        }

        // Panggil fungsi Jump() sebagai pemicu
        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
            return;
        }

        if (player.HorizontalInput != 0)
        {
            player.ChangeState(player.WalkingState);
            return;
        }
        
        CombineMovementAndGravity(player, Vector3.zero);
    }

    public override void ExitState(PlayerStateMachine player) {}
}