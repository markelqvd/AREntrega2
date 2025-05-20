using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class PlaneTracker : MonoBehaviour
{
    [Header("AR Components")]
    public ARPlaneManager planeManager;

    [Header("UI")]
    public Text verticalCountText;
    public Text horizontalCountText;
    public Button startGameButton;

    [Header("Game Parameters")]
    public int requiredVerticalPlanes = 2;
    public int requiredHorizontalPlanes = 2;

    private int currentVerticalPlanes = 0;
    private int currentHorizontalPlanes = 0;

    private HashSet<TrackableId> trackedVerticals = new();
    private HashSet<TrackableId> trackedHorizontals = new();

    private bool trackingComplete = false;

    void Start()
    {
        startGameButton.gameObject.SetActive(false);
        planeManager.planesChanged += OnPlanesChanged;

        // Opcional: Obtén parámetros de la escena 0
        requiredVerticalPlanes = PlayerPrefs.GetInt("VerticalPlanes", 2);
        requiredHorizontalPlanes = PlayerPrefs.GetInt("HorizontalPlanes", 2);

        UpdateUI();
    }

    void OnDestroy()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (trackingComplete) return;

        foreach (var plane in args.added)
        {
            ClassifyPlane(plane);
        }

        foreach (var plane in args.updated)
        {
            ClassifyPlane(plane);
        }

        UpdateUI();

        if (currentVerticalPlanes >= requiredVerticalPlanes &&
            currentHorizontalPlanes >= requiredHorizontalPlanes)
        {
            TrackingComplete();
        }
    }

    void ClassifyPlane(ARPlane plane)
    {
        if (plane.alignment == PlaneAlignment.Vertical &&
            !trackedVerticals.Contains(plane.trackableId))
        {
            trackedVerticals.Add(plane.trackableId);
            currentVerticalPlanes++;
        }

        else if ((plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown) &&
                 !trackedHorizontals.Contains(plane.trackableId))
        {
            trackedHorizontals.Add(plane.trackableId);
            currentHorizontalPlanes++;
        }
    }

    void UpdateUI()
    {
        if (verticalCountText != null)
            verticalCountText.text = $"Verticales: {currentVerticalPlanes}/{requiredVerticalPlanes}";
        if (horizontalCountText != null)
            horizontalCountText.text = $"Horizontales: {currentHorizontalPlanes}/{requiredHorizontalPlanes}";
    }

    void TrackingComplete()
    {
        trackingComplete = true;
        Debug.Log("¡Se han detectado todos los planos necesarios!");

        // Opcional: desactivar el ARPlaneManager si no quieres más planos visibles
        // planeManager.enabled = false;

        // Mostrar botón para empezar
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(true);
    }

    public void OnBotonComenzar()
    {
        // Desactivar detección de planos para que no se siga generando
        planeManager.enabled = false;

        // Ocultar planos visibles (por ejemplo desactivar todos los ARPlanes o el MeshRenderer)
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        // Llama a GameManager para comenzar el juego
        GameManager.Instance.ComenzarJuego();

        // También oculta panel escaneo y muestra HUD
        UIManager.Instance.MostrarHUD();

        // Lanza el spawn de gemas
        GemaSpawner.Instance.SpawnGemas();
    }

}
