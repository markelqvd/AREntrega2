using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARRaycastManager))]
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaceOnPlane : PressInputBase
    {
        [SerializeField]
        private List<GameObject> m_PrefabOptions;

        private GameObject m_PlacedPrefab;
        private List<GameObject> spawnedObjects = new List<GameObject>();
        private ARRaycastManager m_RaycastManager;
        private ARPlaneManager m_PlaneManager;

        private int horizontalPlanes = 0;
        private int verticalPlanes = 0;

        private UIManager juegoUIManager;

        protected override void Awake()
        {
            base.Awake();
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_PlaneManager = GetComponent<ARPlaneManager>();

            if (m_PrefabOptions.Count > 0)
            {
                m_PlacedPrefab = m_PrefabOptions[0];
            }

            // Encontrar el script JuegoUIManager
            juegoUIManager = FindObjectOfType<UIManager>();

            m_PlaneManager.planesChanged += OnPlanesChanged;
        }

        private void OnPlanesChanged(ARPlanesChangedEventArgs args)
        {
            // Recalcular los planos detectados
            horizontalPlanes = 0;
            verticalPlanes = 0;

            foreach (var plane in m_PlaneManager.trackables)
            {
                if (IsHorizontalPlane(plane))
                {
                    horizontalPlanes++;
                }
                else
                {
                    verticalPlanes++;
                }
            }

            // Enviar datos actualizados al UI Manager
            if (juegoUIManager != null)
            {
                juegoUIManager.ActualizarPlanosDetectados(verticalPlanes, horizontalPlanes);
            }
        }

        private bool IsHorizontalPlane(ARPlane plane)
        {
            Vector3 normal = plane.transform.up;
            return Mathf.Abs(Vector3.Dot(normal, Vector3.up)) > 0.9f;
        }
    }
}
