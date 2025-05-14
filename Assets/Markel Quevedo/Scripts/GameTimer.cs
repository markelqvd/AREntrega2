using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public GameObject victoriaPanel;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = GameManager.TiempoJuego;
    }

    void Update()
    {
        tiempoRestante -= Time.deltaTime;
        timerText.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString();

        if (tiempoRestante <= 0)
        {
            victoriaPanel.SetActive(true);
            enabled = false; // Detener temporizador
        }
    }
}
