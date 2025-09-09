using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        // Atur visual saat memasuki state di udara
        player.CharacterRenderer.material.color = player.jumpColor;
        if (player.StatusText != null) player.StatusText.text = "Jumping";
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        // --- LOGIKA MENDARAT YANG DIPERBARUI ---
        // Kembali ke Idle HANYA jika sudah di darat DAN tidak sedang bergerak ke atas.
        if (player.Controller.isGrounded && player.Velocity.y <= 0)
        {
            player.ChangeState(player.IdleState);
            return;
        }

        // Izinkan pergerakan di udara
        Vector3 horizontalMovement = player.transform.right * player.HorizontalInput * player.moveSpeed;
        CombineMovementAndGravity(player, horizontalMovement);
    }

    public override void ExitState(PlayerStateMachine player) { }
}