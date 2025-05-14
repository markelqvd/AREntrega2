using System.Collections.Generic;
using UnityEngine;

public class GemaSpawner : MonoBehaviour
{
    public GameObject gemaPrefab;
    public PlaneClassifier planeClassifier;

    public void GenerarGemas()
    {
        int gemasSuelo = GameManager.GemasSuelo;
        int gemasPared = GameManager.GemasPared;

        // Generar gemas en planos horizontales
        for (int i = 0; i < gemasSuelo && i < planeClassifier.horizontalPlaneHits.Count; i++)
        {
            Pose pose = planeClassifier.horizontalPlaneHits[i];
            Instantiate(gemaPrefab, pose.position + Vector3.up * 0.05f, Quaternion.identity);
        }

        // Generar gemas en planos verticales
        for (int i = 0; i < gemasPared && i < planeClassifier.verticalPlaneHits.Count; i++)
        {
            Pose pose = planeClassifier.verticalPlaneHits[i];
            Instantiate(gemaPrefab, pose.position + Vector3.forward * 0.05f, Quaternion.identity);
        }
    }
}
