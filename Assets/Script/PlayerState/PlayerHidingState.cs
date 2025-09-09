using UnityEngine;

public class PlayerHidingState : PlayerBaseState
{
    public override void EnterState(PlayerStateMachine player)
    {
        player.IsHidden = true;
        
        // Nonaktifkan komponen fisik dan visual agar musuh bisa lewat
        player.Controller.enabled = false;
        player.CharacterRenderer.enabled = false;
        
        // Perbarui teks status dan pastikan tetap terlihat
        if (player.StatusText != null)
        {
            player.StatusText.enabled = true; // Pastikan teks selalu aktif
            player.StatusText.text = "Hiding";
        }
    }

    public override void UpdateState(PlayerStateMachine player)
    {
        // Jika pemain menekan tombol sembunyi lagi, keluar dari state Hiding
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(player.IdleState);
        }
        
        // Tidak ada pergerakan atau gravitasi yang diterapkan saat bersembunyi
    }

    public override void ExitState(PlayerStateMachine player)
    {
        player.IsHidden = false;
        
        // Aktifkan kembali komponen fisik dan visual
        player.Controller.enabled = true;
        player.CharacterRenderer.enabled = true;

        // Teks akan diatur oleh state selanjutnya, jadi tidak perlu diubah di sini.
    }
}