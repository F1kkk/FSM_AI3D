using UnityEngine;

public class EnemyInAirState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        // Menggunakan variabel jumpColor yang baru
        enemy.CharacterRenderer.material.color = enemy.jumpColor; 
        if (enemy.StatusText != null) enemy.StatusText.text = "Jumping";
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (enemy.Controller.isGrounded)
        {
            enemy.ChangeState(enemy.ChaseState);
            return;
        }

        Vector3 moveDirection = Vector3.zero;
        Vector3 targetPosition = new Vector3(enemy.player.position.x, enemy.transform.position.y, enemy.transform.position.z);
        
        Vector3 directionToTarget = (targetPosition - enemy.transform.position).normalized;
        moveDirection = directionToTarget * enemy.chaseSpeed;

        CombineMovementAndGravity(enemy, moveDirection);
    }

    public override void ExitState(EnemyStateMachine enemy) { }
}