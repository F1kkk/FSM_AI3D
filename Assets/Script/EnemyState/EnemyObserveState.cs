using UnityEngine;

public class EnemyObserveState : EnemyBaseState
{
    private float observeTimer;

    public override void EnterState(EnemyStateMachine enemy)
    {
        // Set timer acak untuk durasi mengamati
        observeTimer = Random.Range(enemy.minObserveTime, enemy.maxObserveTime);
        
        // Perbarui visual
        enemy.CharacterRenderer.material.color = enemy.chaseColor;
        if (enemy.StatusText != null) enemy.StatusText.text = "Observing";
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        // --- PERBAIKAN LOGIKA ---
        // Cek apakah pemain masih bisa dilihat (dalam jangkauan DAN tidak bersembunyi)
        bool canStillSeePlayer = Vector3.Distance(enemy.transform.position, enemy.player.position) < enemy.detectionRadius 
                                 && !enemy.player.GetComponent<PlayerStateMachine>().IsHidden;

        // Jika pemain sudah tidak terlihat, langsung kembali ke state Patrol
        if (!canStillSeePlayer)
        {
            enemy.ChangeState(enemy.PatrolState);
            return;
        }

        // Hitung mundur timer
        observeTimer -= Time.deltaTime;

        // Jika timer habis, buat keputusan berdasarkan probabilitas
        if (observeTimer <= 0)
        {
            DecideAction(enemy);
        }
        else
        {
            // Saat mengamati, musuh diam tapi tetap terkena gravitasi agar tidak melayang
            CombineMovementAndGravity(enemy, Vector3.zero);
        }
    }

    private void DecideAction(EnemyStateMachine enemy)
    {
        // Hasilkan angka acak antara 0.0 dan 1.0
        float randomValue = Random.value; 

        // Jika angka acak lebih kecil dari probabilitas mengejar...
        if (randomValue < enemy.chaseProbability)
        {
            // ...maka kejar pemain.
            enemy.ChangeState(enemy.ChaseState);
        }
        else
        {
            // ...jika tidak, kembali patroli.
            enemy.ChangeState(enemy.PatrolState);
        }
    }

    public override void ExitState(EnemyStateMachine enemy) { }
}