using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Paneles")]
    public GameObject panelEscaneo;
    public GameObject panelHUD;
    public GameObject panelFin;

    [Header("Textos")]
    public Text textoPlanosDetectados;
    public Text textoTiempo;
    public Text textoGemas;
    public Text textoFin;

    [Header("Botones")]
    public Button botonComenzar;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ActualizarContadorPlanos(int actuales, int requeridos)
    {
        textoPlanosDetectados.text = $"Planos detectados: {actuales} / {requeridos}";
        botonComenzar.interactable = actuales >= requeridos;
    }

    public void MostrarHUD()
    {
        panelEscaneo.SetActive(false);
        panelHUD.SetActive(true);
    }

    public void ActualizarTiempo(int segundos)
    {
        textoTiempo.text = $"Tiempo: {segundos}s";
    }

    public void ActualizarContadorGemas(int recogidas, int total)
    {
        textoGemas.text = $"Gemas: {recogidas} / {total}";
    }

    public void MostrarFin(bool victoria)
    {
        panelHUD.SetActive(false);
        panelFin.SetActive(true);
        textoFin.text = victoria ? "¡Victoria!" : "Tiempo agotado";
    }

    public void OnVolverAlMenu()
    {
        GameManager.Instance.VolverAlMenu();
    }

    public void OnComenzarJuego()
    {
        GameManager.Instance.ComenzarJuego();
    }
}
