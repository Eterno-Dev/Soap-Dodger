using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RoundManager : MonoBehaviour
{
    public float roundDuration = 20f; // Duración de cada ronda en segundos
    public TextMeshProUGUI roundText;           // Referencia a un texto UI para mostrar la ronda actual
    public TextMeshProUGUI timerText;           // Referencia a un texto UI para mostrar el tiempo restante

    public float timer;             // Temporizador interno
    [HideInInspector] public int currentRound = 0;    // Contador de rondas
    private bool isRoundActive = false; // Estado de la ronda

    ProyectilDeSeguimiento aguja;
    agujaSpawn AS;
    playerMovement pj;

    [SerializeField] Image timerImg;

    public PlayerScoreManager SM; // Referencia al script del jugador

    void Start()
    {
        pj = FindAnyObjectByType<playerMovement>();  
        AS = FindAnyObjectByType<agujaSpawn>();
        SM = FindAnyObjectByType<PlayerScoreManager>();
        StartNextRound(); // Inicia la primera ronda
    }

    void Update()
    {
        if (isRoundActive && (pj == null || !pj.enPausa))
        {
            // Decrementar el temporizador
            Debug.Log("Sube puntuacion");
            if (pj != null && pj.muerto == false)
            {
                SM.AddScore(1);
            }
            
            timer -= Time.deltaTime;
            UpdateTimerUI();

            // Verificar si el tiempo de la ronda terminó
            if (timer <= 0)
            {
                EndRound();
            }
        }
    }

    public void UpdateHB(float health)
    {
        timerImg.fillAmount = health / roundDuration;
    }

    public void StartNextRound()
    {
        currentRound++;                 // Incrementar el número de ronda
        if (AS.tiempoEntreSpawns > 0.5f)
        {
            AS.tiempoEntreSpawns -= 0.5f;
        }
        timer = roundDuration;          // Reiniciar el temporizador
        isRoundActive = true;           // Activar la ronda
        UpdateRoundUI();                // Actualizar el texto de la ronda
        Debug.Log($"Comienza la ronda {currentRound}"); // Mensaje en consola

        // Aquí puedes añadir lógica específica para el inicio de cada ronda
        SpawnEnemiesForRound(currentRound);
    }

    private void EndRound()
    {
        isRoundActive = false;          // Terminar la ronda
        Debug.Log($"Ronda {currentRound} terminada");

        // Desactivar las burbujas al terminar la ronda
        pj.escudo = false;
        pj.escudoBubble.SetActive(false);   
        pj.tiempo = false;  
        pj.canDash = false;

        FindAnyObjectByType<RoundUIManager>().ActivarRondaUI(); // Activa la UI cada ronda

        // Aquí puedes añadir lógica para el final de la ronda
        // Ejemplo: Mostrar resultados o preparar la siguiente ronda
        Invoke("StartNextRound", 3f);   // Esperar 3 segundos antes de iniciar la siguiente ronda
    }

    private void UpdateRoundUI()
    {
        if (roundText != null)
        {
            roundText.text = $"Round: {currentRound}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"{Mathf.Ceil(timer)}"; // Mostrar tiempo restante redondeado
            UpdateHB(timer);
            //Debug.Log($"Timer: {timer}.");
        }
        if ((pj.muerto == true))
        {
            timerImg.enabled = false;
        }
        
    }

    private void SpawnEnemiesForRound(int round)
    {
        // Ejemplo: Spawn de enemigos basado en la ronda actual
        Debug.Log($"Spawneando enemigos para la ronda {round}");
        // Aquí puedes implementar tu sistema de spawn según la lógica de tu juego.
    }
}
