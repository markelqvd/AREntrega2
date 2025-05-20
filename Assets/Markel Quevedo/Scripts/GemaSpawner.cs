using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GemaSpawner : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public GameObject gemaPrefab;

    public int maxGemasPared = 5;   // gemas en planos verticales
    public int maxGemasSuelo = 5;   // gemas en planos horizontales

    private int gemasParedGeneradas = 0;
    private int gemasSueloGeneradas = 0;

    public static GemaSpawner Instance;

    private List<ARPlane> planosDetectados = new List<ARPlane>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SpawnGemas()
    {
        int gemasPared = GameConfig.Instance.gemasPared;
        int gemasSuelo = GameConfig.Instance.gemasSuelo;

        int gemasParedGeneradas = 0;
        int gemasSueloGeneradas = 0;

        foreach (var plano in planeManager.trackables)
        {
            if (plano.alignment == PlaneAlignment.Vertical && gemasParedGeneradas < gemasPared)
            {
                SpawnGemaEnPlano(plano);
                gemasParedGeneradas++;
            }
            else if ((plano.alignment == PlaneAlignment.HorizontalUp || plano.alignment == PlaneAlignment.HorizontalDown) && gemasSueloGeneradas < gemasSuelo)
            {
                SpawnGemaEnPlano(plano);
                gemasSueloGeneradas++;
            }
        }
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plano in args.added)
        {
            if (plano.alignment == PlaneAlignment.Vertical && gemasParedGeneradas < maxGemasPared)
            {
                SpawnGemaEnPlano(plano);
                gemasParedGeneradas++;
            }
            else if ((plano.alignment == PlaneAlignment.HorizontalUp || plano.alignment == PlaneAlignment.HorizontalDown) && gemasSueloGeneradas < maxGemasSuelo)
            {
                SpawnGemaEnPlano(plano);
                gemasSueloGeneradas++;
            }
        }
    }

    void SpawnGemaEnPlano(ARPlane plano)
    {
        // Posición aleatoria dentro del plano detectado (usar el bounds del plano)
        Vector3 center = plano.center;
        Vector3 extents = plano.size * 0.5f;

        float randomX = Random.Range(center.x - extents.x, center.x + extents.x);
        float randomZ = Random.Range(center.z - extents.z, center.z + extents.z);
        Vector3 spawnPos = new Vector3(randomX, center.y, randomZ);

        Instantiate(gemaPrefab, spawnPos, Quaternion.identity);
    }
}
