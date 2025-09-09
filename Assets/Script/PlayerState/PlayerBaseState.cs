using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateMachine player);
    public abstract void UpdateState(PlayerStateMachine player);
    public abstract void ExitState(PlayerStateMachine player);

    // --- FUNGSI BARU YANG DITAMBAHKAN ---
    protected void CombineMovementAndGravity(PlayerStateMachine player, Vector3 horizontalMovement)
    {
        // Terapkan gravitasi
        if (player.Controller.isGrounded && player.Velocity.y < 0)
        {
            player.Velocity = new Vector3(player.Velocity.x, -2f, player.Velocity.z);
        }
        
        Vector3 currentVelocity = player.Velocity;
        currentVelocity.y += player.gravity * Time.deltaTime;
        player.Velocity = currentVelocity;
        
        // Gabungkan pergerakan horizontal dan vertikal
        Vector3 finalMovement = horizontalMovement + new Vector3(0, player.Velocity.y, 0);

        // Gerakkan controller satu kali
        player.Controller.Move(finalMovement * Time.deltaTime);
    }
}