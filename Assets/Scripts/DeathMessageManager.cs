using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DeathMessageController : MonoBehaviour
{
    public TextMeshProUGUI deathTextUI; // Referencia al componente TextMeshPro en el Canvas
    public float timerChanger = 30f; // Intervalo de tiempo en segundos

    private List<string> deathMessages = new List<string>
    {
        "¡Cuando te arrastre el oleaje, iza proa y suelta anclajes!",
        "No te rindas, esquiva más caídas.",
        "Los héroes nunca mueren... pero a veces desaparecen.",
        "No te ahondes todavía, tiempo queda en este día.",
        "¿Otra vez? ¿Dónde está tu rapidez?",
        "Pocas pompas soportan muchas rondas.",
        "¡Pon tu guardia a flote, que las pompas no exploten!",
        "Enjabona tus desdichos malpensares, y recuerda los viejos cantares.",
        "Que no fluya la amargura, la esperanza siempre perdura.",
        "No temas caer, levanta el alma y vuelve a correr."
    };


    private void Start()
    {
        StartCoroutine(ChangeDeathMessage()); // Inicia la coroutine al principio
    }

    private IEnumerator ChangeDeathMessage()
    {
        while (true) // Bucle infinito
        {
            ShowDeathMessage(); // Llama al método para mostrar un mensaje
            yield return new WaitForSeconds(timerChanger); // Espera el tiempo especificado
        }
    }

    // Método para mostrar un mensaje aleatorio
    public void ShowDeathMessage()
    {
        if (deathTextUI != null)
        {
            string randomMessage = deathMessages[Random.Range(0, deathMessages.Count)];
            deathTextUI.text = randomMessage;
        }
    }
}
