using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System.Security.Cryptography.X509Certificates;

public class JuegoUIManager : MonoBehaviour
{
    public TextMeshProUGUI tiempoRestanteText;
    public TextMeshProUGUI infoVerticalesText;
    public TextMeshProUGUI infoHorizontalesText;
    public TextMeshProUGUI mensajeResultadoText;
    public Button instanciarGemasButton;
    public GameObject prefabGema;

    private int tiempoRestante;
    private int gemasVerticalesMinimas;
    private int gemasHorizontalesMinimas;
    private int gemasTotalesRecolectadas = 0;
    private int gemasTotales;

    private int planosVerticalesDetectados = 0;
    private int planosHorizontalesDetectados = 0;

    private float tiempoTranscurrido;
    private bool juegoIniciado = false;

    private ARPlaneManager planeManager;

    void Start()
    {
        if (GameConfig.Instance == null)
        {
            Debug.LogError("GameConfig no encontrado en la escena.");
            return;
        }

        // Obtener valores de configuración
        tiempoRestante = GameConfig.Instance.tiempoJuego;
        gemasVerticalesMinimas = GameConfig.Instance.gemasPared;
        gemasHorizontalesMinimas = GameConfig.Instance.gemasSuelo;
        gemasTotales = gemasVerticalesMinimas + gemasHorizontalesMinimas;

        bool oclusionActiva = GameConfig.Instance.oclusionActiva;

        // Inicializar el ARPlaneManager
        planeManager = FindObjectOfType<ARPlaneManager>();

        // Ocultar el botón y mensaje al inicio
        instanciarGemasButton.gameObject.SetActive(false);
        mensajeResultadoText.gameObject.SetActive(false);

        // Añadir listener al botón
        instanciarGemasButton.onClick.AddListener(InstanciarGemas);

        // Actualizar textos iniciales
        ActualizarUI();
    }

    void Update()
    {
        // Reducir el tiempo restante
        tiempoTranscurrido += Time.deltaTime;
        tiempoRestante = Mathf.Max(0, PlayerPrefs.GetInt("TiempoDeBusqueda") - Mathf.FloorToInt(tiempoTranscurrido));

        // Actualizar la UI
        ActualizarUI();

        // Mostrar el botón si se alcanzan los mínimos
        if (planosVerticalesDetectados >= gemasVerticalesMinimas && planosHorizontalesDetectados >= gemasHorizontalesMinimas)
        {
            instanciarGemasButton.gameObject.SetActive(true);
        }

        // Verificar estado del juego
        if (tiempoRestante <= 0 && gemasTotalesRecolectadas < gemasTotales)
        {
            FinDelJuego(false); // Derrota si el tiempo se termina
        }
        else if (gemasTotalesRecolectadas >= gemasTotales)
        {
            FinDelJuego(true); // Victoria si recolecta todas las gemas
        }
    }

    public void ActualizarPlanosDetectados(int verticales, int horizontales)
    {
        planosVerticalesDetectados = verticales;
        planosHorizontalesDetectados = horizontales;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        tiempoRestanteText.text = $"Tiempo restante: {tiempoRestante} s";
        infoVerticalesText.text = $"Planos Verticales: {planosVerticalesDetectados}/{gemasVerticalesMinimas}";
        infoHorizontalesText.text = $"Planos Horizontales: {planosHorizontalesDetectados}/{gemasHorizontalesMinimas}";
    }

    private void InstanciarGemas()
    {
        int verticalesColocados = 0;
        int horizontalesColocados = 0;

        foreach (var plane in planeManager.trackables)
        {
            if (verticalesColocados < gemasVerticalesMinimas && IsVerticalPlane(plane))
            {
                InstanciarGemaEnPlano(plane);
                verticalesColocados++;
            }
            else if (horizontalesColocados < gemasHorizontalesMinimas && IsHorizontalPlane(plane))
            {
                InstanciarGemaEnPlano(plane);
                horizontalesColocados++;
            }

            // Si ya se alcanzó el mínimo, detener el bucle
            if (verticalesColocados >= gemasVerticalesMinimas && horizontalesColocados >= gemasHorizontalesMinimas)
                break;
        }

        // Desactivar el botón para evitar múltiples clicks
        instanciarGemasButton.gameObject.SetActive(false);
    }

    private void InstanciarGemaEnPlano(ARPlane plane)
    {
        Vector3 posicionAleatoria = ObtenerPosicionAleatoriaEnPlano(plane);
        GameObject gema = Instantiate(prefabGema, posicionAleatoria, Quaternion.identity);
        gema.GetComponent<Gema>().SetUIManager(this);
    }

    private Vector3 ObtenerPosicionAleatoriaEnPlano(ARPlane plane)
    {
        Bounds bounds = plane.GetComponent<MeshRenderer>().bounds;
        Vector3 centro = bounds.center;
        Vector3 extents = bounds.extents;

        float x = Random.Range(centro.x - extents.x, centro.x + extents.x);
        float z = Random.Range(centro.z - extents.z, centro.z + extents.z);

        return new Vector3(x, plane.transform.position.y, z);
    }

    private bool IsVerticalPlane(ARPlane plane)
    {
        Vector3 normal = plane.transform.up;
        return Mathf.Abs(Vector3.Dot(normal, Vector3.forward)) > 0.9f; // Vertical si la normal es cercana a Vector3.forward
    }

    private bool IsHorizontalPlane(ARPlane plane)
    {
        Vector3 normal = plane.transform.up;
        return Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.9f; // Horizontal si la normal es cercana a Vector3.up
    }

    public void RecolectarGema()
    {
        gemasTotalesRecolectadas++;
    }

    public void FinDelJuego(bool exito)
    {
        mensajeResultadoText.gameObject.SetActive(true);
        mensajeResultadoText.text = exito ? "¡VICTORIA!" : "DERROTA";

        // Deshabilitar interacción
        enabled = false;

        // Opcional: Regresar al menú principal después de un retraso
        Invoke(nameof(Salir), 3f);
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }
}
