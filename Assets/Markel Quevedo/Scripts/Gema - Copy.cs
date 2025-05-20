using UnityEngine;

public class Gema : MonoBehaviour
{
    private JuegoUIManager uiManager;

    public void SetUIManager(JuegoUIManager manager)
    {
        uiManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la etiqueta "Player"
        {
            uiManager.RecolectarGema();
            Destroy(gameObject);
        }
    }
}
