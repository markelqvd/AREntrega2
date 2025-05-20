using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int tiempoRestante;
    public int gemasTotales;
    public int gemasRecogidas;

    private bool juegoActivo = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Cargar config
        tiempoRestante = GameConfig.Instance.tiempoJuego;
        gemasTotales = GameConfig.Instance.gemasPared + GameConfig.Instance.gemasSuelo;

        // Aquí podrías activar/desactivar oclusión según GameConfig.Instance.oclusionActiva
        // Y pasar esos datos a un spawner o script de oclusión
    }

    public void ComenzarJuego()
    {
        juegoActivo = true;
        UIManager.Instance.MostrarHUD();
        InvokeRepeating(nameof(ActualizarTiempo), 1f, 1f);
    }

    void ActualizarTiempo()
    {
        if (!juegoActivo) return;

        tiempoRestante--;
        UIManager.Instance.ActualizarTiempo(tiempoRestante);

        if (tiempoRestante <= 0)
        {
            TerminarJuego(false);
        }
    }

    public void RecogerGema()
    {
        gemasRecogidas++;
        UIManager.Instance.ActualizarContadorGemas(gemasRecogidas, gemasTotales);

        if (gemasRecogidas >= gemasTotales)
        {
            TerminarJuego(true);
        }
    }

    void TerminarJuego(bool victoria)
    {
        juegoActivo = false;
        CancelInvoke(nameof(ActualizarTiempo));
        UIManager.Instance.MostrarFin(victoria);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuConfiguracion");
    }
}
