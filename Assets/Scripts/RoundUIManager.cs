using UnityEngine;

public class RoundUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rondaUI; // Referencia a la imagen / texto UI de la ronda
    [SerializeField] private float tiempoVisible = 5f; // Tiempo que la UI permanecera visible

    private float tiempoRestante = 0f;
    private bool isBannerActive = false;
    private bool isPausedBanner = false;

    void Start()
    {
        if (rondaUI != null)
        {
            rondaUI.SetActive(false); // Asegurar que la UI este oculta al inicio
        }
    }

    void Update()
    {
        if (isBannerActive && !isPausedBanner)
        {
            tiempoRestante -= Time.unscaledDeltaTime;
            if (tiempoRestante <= 0f)
            {
                OcultarRondaUI();
            }
        }
    }

    public void ActivarRondaUI()
    {
        if (rondaUI != null)
        {
            isBannerActive = true;
            isPausedBanner = false;
            tiempoRestante = tiempoVisible;
            rondaUI.SetActive(true); // Activa la UI
        }
    }

    public void PausarRondaUI()
    {
        if (isBannerActive)
        {
            isPausedBanner = true;
            if (rondaUI != null)
            {
                rondaUI.SetActive(false); // Ocultar mientras el juego este en pausa
            }
        }
    }

    public void ReanudarRondaUI()
    {
        if (isBannerActive && isPausedBanner)
        {
            isPausedBanner = false;
            if (rondaUI != null)
            {
                rondaUI.SetActive(true); // Volver a mostrar con el tiempo restante
            }
        }
    }

    public void OcultarPorMuerte()
    {
        isBannerActive = false;
        isPausedBanner = false;
        tiempoRestante = 0f;
        if (rondaUI != null)
        {
            rondaUI.SetActive(false); // Ocultar al morir para no solaparse con el menu de muerte
        }
    }

    public void OcultarRondaUI()
    {
        isBannerActive = false;
        isPausedBanner = false;
        tiempoRestante = 0f;
        if (rondaUI != null)
        {
            rondaUI.SetActive(false); // Oculta la UI
        }
    }
}