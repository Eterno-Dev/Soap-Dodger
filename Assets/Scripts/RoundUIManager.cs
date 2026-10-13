using UnityEngine;
using UnityEngine.UI;

public class RoundUIManager : MonoBehaviour
{
    [SerializeField] private GameObject rondaUI; // Referencia a la imagen UI
    [SerializeField] private float tiempoVisible = 5f; // Tiempo que la UI permanecerá visible

    void Start()
    {
        if (rondaUI != null)
            rondaUI.SetActive(false); // Asegurar que la UI está oculta al inicio

    }

    public void ActivarRondaUI()
    {
        if (rondaUI != null)
        {
            rondaUI.SetActive(true); // Activa la UI

            Invoke("OcultarRondaUI", tiempoVisible); // La oculta después de 5s
        }
    }

    private void OcultarRondaUI()
    {
        if (rondaUI != null)
        {
            rondaUI.SetActive(false); // Oculta la UI
        }
    }
}
