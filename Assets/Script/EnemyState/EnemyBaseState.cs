using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateMachine enemy);
    public abstract void UpdateState(EnemyStateMachine enemy);
    public abstract void ExitState(EnemyStateMachine enemy);

    // --- FUNGSI BARU YANG DITAMBAHKAN ---
    // Fungsi ini menggabungkan pergerakan horizontal dan vertikal (gravitasi)
    protected void CombineMovementAndGravity(EnemyStateMachine enemy, Vector3 horizontalMovement)
    {
        // Terapkan gravitasi pada kecepatan vertikal
        if (enemy.Controller.isGrounded && enemy.Velocity.y < 0)
        {
            enemy.Velocity = new Vector3(enemy.Velocity.x, -2f, enemy.Velocity.z);
        }
        
        Vector3 currentVelocity = enemy.Velocity;
        currentVelocity.y += enemy.gravity * Time.deltaTime;
        enemy.Velocity = currentVelocity;
        
        // Gabungkan pergerakan horizontal dari state (patrol/chase) dengan kecepatan vertikal (gravitasi)
        Vector3 finalMovement = horizontalMovement + new Vector3(0, enemy.Velocity.y, 0);

        // Panggil Controller.Move() hanya satu kali dengan gerakan gabungan
        enemy.Controller.Move(finalMovement * Time.deltaTime);
    }
}