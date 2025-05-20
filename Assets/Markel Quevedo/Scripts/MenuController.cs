using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Slider tiempoSlider;
    public Slider paredesSlider;
    public Slider sueloSlider;
    public Toggle oclusionToggle;

    public Text tiempoTexto;
    public Text paredesTexto;
    public Text sueloTexto;

    void Start()
    {
        // Actualizar textos iniciales
        ActualizarTextos();
    }

    void Update()
    {
        ActualizarTextos();
    }

    void ActualizarTextos()
    {
        tiempoTexto.text = tiempoSlider.value.ToString("F0") + "s";
        paredesTexto.text = paredesSlider.value.ToString("F0");
        sueloTexto.text = sueloSlider.value.ToString("F0");
    }

    public void OnComenzarJuego()
    {
        if (GameConfig.Instance == null)
        {
            Debug.LogError("GameConfig no está en la escena.");
            return;
        }

        GameConfig.Instance.tiempoJuego = Mathf.RoundToInt(tiempoSlider.value);
        GameConfig.Instance.gemasPared = Mathf.RoundToInt(paredesSlider.value);
        GameConfig.Instance.gemasSuelo = Mathf.RoundToInt(sueloSlider.value);
        GameConfig.Instance.oclusionActiva = oclusionToggle.isOn;

        SceneManager.LoadScene("Game");
    }
}
