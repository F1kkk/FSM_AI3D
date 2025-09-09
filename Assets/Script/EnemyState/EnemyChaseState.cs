using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.CharacterRenderer.material.color = enemy.chaseColor;
        if (enemy.StatusText != null) enemy.StatusText.text = "Chasing";
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        // Transisi kembali ke Patrol jika pemain hilang atau bersembunyi
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) > enemy.detectionRadius || enemy.player.GetComponent<PlayerStateMachine>().IsHidden)
        {
            enemy.ChangeState(enemy.PatrolState);
            return;
        }

        // Transisi ke Attack jika dalam jangkauan
        if (Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.attackRange)
        {
            enemy.ChangeState(enemy.AttackState);
            return;
        }

        // --- Pemicu Lompatan yang Diperbarui ---
        // Cek kondisi pemain dan musuh untuk memicu lompatan
        bool isPlayerInAir = !enemy.player.GetComponent<CharacterController>().isGrounded;
        if (isPlayerInAir && enemy.Controller.isGrounded)
        {
            enemy.Jump();
            return; // Penting: Keluar dari Update agar state InAir bisa diproses
        }
        
        // --- Logika Mengejar ---
        // Hanya berjalan jika tidak ada transisi lain yang terjadi
        Vector3 moveDirection = Vector3.zero;
        Vector3 targetPosition = new Vector3(enemy.player.position.x, enemy.transform.position.y, enemy.transform.position.z);
        
        Vector3 directionToTarget = (targetPosition - enemy.transform.position).normalized;
        moveDirection = directionToTarget * enemy.chaseSpeed;

        if (enemy.player.position.x > enemy.transform.position.x && !enemy.IsFacingRight) enemy.Flip();
        else if (enemy.player.position.x < enemy.transform.position.x && enemy.IsFacingRight) enemy.Flip();
        
        CombineMovementAndGravity(enemy, moveDirection);
    }

    public override void ExitState(EnemyStateMachine enemy) {}
}