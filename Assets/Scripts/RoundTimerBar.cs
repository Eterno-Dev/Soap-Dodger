using UnityEngine;
using UnityEngine.UI;

public class RoundTimerBar : MonoBehaviour
{
    private RoundManager roundManager;
    private Image timerBar;

    void Start()
    {
        // Encuentra el RoundManager en la escena
        roundManager = FindAnyObjectByType<RoundManager>();
        timerBar = GetComponent<Image>();

        if (roundManager == null)
        {
            Debug.LogError("RoundManager no encontrado en la escena.");
            return;
        }

        // Inicializa la barra llena
        ResetTimerBar();
    }

    void Update()
    {
        if (roundManager != null && timerBar != null)
        {
            // Actualiza el fillAmount de la imagen radial en base al tiempo restante
            timerBar.fillAmount = Mathf.Clamp01(roundManager.timer / roundManager.roundDuration);
        }
    }

    // Método para reiniciar la barra al inicio de cada ronda
    public void ResetTimerBar()
    {
        if (timerBar != null)
        {
            timerBar.fillAmount = 1f;
        }
    }
}
