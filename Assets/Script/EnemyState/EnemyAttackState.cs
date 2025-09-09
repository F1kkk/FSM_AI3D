using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.CharacterRenderer.material.color = enemy.attackColor;
        if (enemy.StatusText != null) enemy.StatusText.text = "Attacking!";
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        // Transisi kembali ke Chase jika pemain di luar jangkauan serangan
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) > enemy.attackRange)
        {
            enemy.ChangeState(enemy.ChaseState);
            return;
        }

        // Balikkan arah hadap jika diperlukan
        if (enemy.player.position.x > enemy.transform.position.x && !enemy.IsFacingRight) enemy.Flip();
        else if (enemy.player.position.x < enemy.transform.position.x && enemy.IsFacingRight) enemy.Flip();
        
        // Saat menyerang, musuh tidak bergerak secara horizontal
        Vector3 horizontalMovement = Vector3.zero;

        // Terapkan gravitasi agar tetap di tanah
        CombineMovementAndGravity(enemy, horizontalMovement);
    }

    public override void ExitState(EnemyStateMachine enemy) {}
}