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
        bool canSeePlayer = Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRadius 
                            && !enemy.player.GetComponent<PlayerStateMachine>().IsHidden;

        if (canSeePlayer)
        {
            enemy.ChangeState(enemy.ObserveState);
            return;
        }

        // --- Logika Patroli yang Diperbarui untuk Mencegah Glitch ---
        if (enemy.waypoints.Length > 0)
        {
            Transform targetWaypoint = enemy.waypoints[enemy.CurrentWaypointIndex];
            Vector3 targetPosition = new Vector3(targetWaypoint.position.x, enemy.transform.position.y, enemy.transform.position.z);
            
            // 1. Hitung posisi frame berikutnya dengan aman menggunakan MoveTowards
            Vector3 nextPosition = Vector3.MoveTowards(enemy.transform.position, targetPosition, enemy.moveSpeed * Time.deltaTime);
            
            // 2. Hitung kecepatan horizontal yang diperlukan untuk mencapai posisi tersebut
            // Ini mencegah overshoot dan glitch.
            Vector3 horizontalVelocity = (nextPosition - enemy.transform.position) / Time.deltaTime;

            // 3. Gabungkan kecepatan horizontal dengan gravitasi
            CombineMovementAndGravity(enemy, horizontalVelocity);

            // Balikkan arah hadap jika diperlukan
            if (targetWaypoint.position.x > enemy.transform.position.x && !enemy.IsFacingRight) enemy.Flip();
            else if (targetWaypoint.position.x < enemy.transform.position.x && enemy.IsFacingRight) enemy.Flip();

            // Cek jika sudah sampai di waypoint untuk beralih
            if (Vector3.Distance(enemy.transform.position, targetPosition) < 0.01f)
            {
                enemy.CurrentWaypointIndex = (enemy.CurrentWaypointIndex + 1) % enemy.waypoints.Length;
            }
        }
        else
        {
            // Jika tidak ada waypoint, pastikan gravitasi tetap diterapkan
            CombineMovementAndGravity(enemy, Vector3.zero);
        }
    }

    public override void ExitState(EnemyStateMachine enemy) {}
}