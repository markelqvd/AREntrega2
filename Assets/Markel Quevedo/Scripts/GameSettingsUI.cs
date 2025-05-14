using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{
    public Slider tiempoSlider;
    public Slider gemasParedSlider;
    public Slider gemasSueloSlider;
    public Toggle oclusionToggle;

    public GameObject menuPanel;
    public GameObject escaneoPanel;

    public void ComenzarEscaneo()
    {
        GameManager.TiempoJuego = (int)tiempoSlider.value;
        GameManager.GemasPared = (int)gemasParedSlider.value;
        GameManager.GemasSuelo = (int)gemasSueloSlider.value;
        GameManager.OclusionActiva = oclusionToggle.isOn;

        menuPanel.SetActive(false);
        escaneoPanel.SetActive(true);
    }
}