using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.CharacterRenderer.material.color = enemy.patrolColor;
        if (enemy.StatusText != null) enemy.StatusText.text = "Patrolling";
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        // Cek apakah pemain bisa dilihat (dalam jangkauan DAN tidak bersembunyi)
        // Ini adalah SATU-SATUNYA transisi dari state Patrol
        bool canSeePlayer = Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRadius 
                            && !enemy.player.GetComponent<PlayerStateMachine>().IsHidden;

        if (canSeePlayer)
        {
            enemy.ChangeState(enemy.ChaseState);
            return;
        }

        // --- Logika Patroli ---
        // Logika ini hanya berjalan jika transisi di atas tidak terjadi.
        Vector3 moveDirection = Vector3.zero;
        if (enemy.waypoints.Length > 0)
        {
            Transform targetWaypoint = enemy.waypoints[enemy.CurrentWaypointIndex];
            Vector3 targetPosition = new Vector3(targetWaypoint.position.x, enemy.transform.position.y, enemy.transform.position.z);
            
            Vector3 directionToTarget = (targetPosition - enemy.transform.position).normalized;
            moveDirection = directionToTarget * enemy.moveSpeed;

            if (targetWaypoint.position.x > enemy.transform.position.x && !enemy.IsFacingRight) enemy.Flip();
            else if (targetWaypoint.position.x < enemy.transform.position.x && enemy.IsFacingRight) enemy.Flip();

            if (Vector3.Distance(enemy.transform.position, targetPosition) < 0.1f)
            {
                enemy.CurrentWaypointIndex = (enemy.CurrentWaypointIndex + 1) % enemy.waypoints.Length;
            }
        }
        
        CombineMovementAndGravity(enemy, moveDirection);
    }

    public override void ExitState(EnemyStateMachine enemy) {}
}