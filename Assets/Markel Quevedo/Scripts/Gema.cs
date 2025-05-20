using UnityEngine;

public class Gema : MonoBehaviour
{
    private UIManager uiManager;
    public AudioClip sonidoRecoger; // Asigna en el Inspector

    private void Start()
    {
        // Agrega un AudioSource si no lo tiene
        if (GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();
    }

    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.RecolectarGema();

            // Reproduce el sonido de recoger
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(sonidoRecoger);

            // Destruir el objeto después de que suene el clip
            Destroy(gameObject, sonidoRecoger.length);
        }
    }
}
