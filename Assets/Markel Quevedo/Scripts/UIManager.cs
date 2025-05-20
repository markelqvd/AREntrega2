using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tiempoRestanteText;
    public TextMeshProUGUI infoVerticalesText;
    public TextMeshProUGUI infoHorizontalesText;
    public TextMeshProUGUI mensajeResultadoText;
    public TextMeshProUGUI contadorGemasText;
    public Button instanciarGemasButton;
    public Button volverMenuButton;
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

        // Inicializar el ARPlaneManager
        planeManager = FindObjectOfType<ARPlaneManager>();

        // Ocultar elementos al inicio
        instanciarGemasButton.gameObject.SetActive(false);
        mensajeResultadoText.gameObject.SetActive(false);

        // Añadir listeners
        instanciarGemasButton.onClick.AddListener(InstanciarGemas);
        volverMenuButton.onClick.AddListener(Salir);

        ActualizarUI();
    }

    void Update()
    {
        // Actualizar UI de planos detectados
        ActualizarUI();

        // Mostrar botón si se alcanzan los mínimos antes de iniciar el juego
        if (!juegoIniciado && planosVerticalesDetectados >= gemasVerticalesMinimas && planosHorizontalesDetectados >= gemasHorizontalesMinimas)
        {
            instanciarGemasButton.gameObject.SetActive(true);
        }

        if (juegoIniciado)
        {
            tiempoTranscurrido += Time.deltaTime;
            tiempoRestante = Mathf.Max(0, GameConfig.Instance.tiempoJuego - Mathf.FloorToInt(tiempoTranscurrido));

            contadorGemasText.text = $"Gemas: {gemasTotalesRecolectadas}/{gemasTotales}";
            tiempoRestanteText.text = $"Tiempo: {tiempoRestante}s";

            if (tiempoRestante <= 0 && gemasTotalesRecolectadas < gemasTotales)
            {
                FinDelJuego(false);
            }
            else if (gemasTotalesRecolectadas >= gemasTotales)
            {
                FinDelJuego(true);
            }
        }
    }

    public void ActualizarPlanosDetectados(int verticales, int horizontales)
    {
        planosVerticalesDetectados = verticales;
        planosHorizontalesDetectados = horizontales;
    }

    private void ActualizarUI()
    {
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

            if (verticalesColocados >= gemasVerticalesMinimas && horizontalesColocados >= gemasHorizontalesMinimas)
                break;
        }

        juegoIniciado = true;
        tiempoTranscurrido = 0f;
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
        return Mathf.Abs(Vector3.Dot(normal, Vector3.forward)) > 0.9f;
    }

    private bool IsHorizontalPlane(ARPlane plane)
    {
        Vector3 normal = plane.transform.up;
        return Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.9f;
    }

    public void RecolectarGema()
    {
        gemasTotalesRecolectadas++;
    }

    public void FinDelJuego(bool exito)
    {
        mensajeResultadoText.gameObject.SetActive(true);
        mensajeResultadoText.text = exito ? "¡VICTORIA!" : "DERROTA";
        enabled = false;
    }

    public void Salir()
    {
        SceneManager.LoadScene("Menu");
    }
}
