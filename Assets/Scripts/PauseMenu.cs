using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Reference")]
    [Tooltip("El GameObject contenedor del menu de pausa (si se deja vacio, toma este GameObject)")]
    [SerializeField] private GameObject pauseUI;

    [Header("Optional Button References")]
    [Tooltip("Boton de continuar (opcional, tambien se puede vincular por OnClick)")]
    [SerializeField] private Button continueButton;
    [Tooltip("Boton de volver al menu (opcional, tambien se puede vincular por OnClick)")]
    [SerializeField] private Button returnMenuButton;

    private PlayerControls controls;
    private playerMovement player;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        isPaused = false;
        player = FindAnyObjectByType<playerMovement>();

        // Si no se asigno manualmente, asume que este script esta en el propio objeto Pause
        if (pauseUI == null)
        {
            pauseUI = gameObject;
        }

        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
        }

        if (returnMenuButton != null)
        {
            returnMenuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    void OnEnable()
    {
        if (controls == null) controls = new PlayerControls();
        controls.Gameplay.Enable();
        controls.Gameplay.Return.performed += OnReturnPerformed;
        controls.Gameplay.Dash.performed += OnDashPerformed;
    }

    void OnDisable()
    {
        if (controls != null)
        {
            controls.Gameplay.Return.performed -= OnReturnPerformed;
            controls.Gameplay.Dash.performed -= OnDashPerformed;
            controls.Gameplay.Disable();
        }
    }

    private void OnReturnPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // No actuar si el jugador ha muerto (el menu de muerte se encarga de esa condicion)
        if (player != null && player.muerto) return;

        if (!isPaused)
        {
            // Escape durante la partida abre el menu de pausa
            PauseGame();
        }
        else
        {
            // Pulsar Return mientras esta en pausa regresa al menu principal
            ReturnToMenu();
        }
    }

    private void OnDashPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Si el juego esta pausado, pulsar Dash (Espacio / Boton A) continua la partida
        if (isPaused)
        {
            ContinueGame();
        }
    }

    public void PauseGame()
    {
        if (player != null && player.muerto) return;

        isPaused = true;
        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        isPaused = false;
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}
