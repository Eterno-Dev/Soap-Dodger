using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [Header("Player Movement Variables")]
    [SerializeField] float playerMoveSpeed = 5f;
    [SerializeField] float tiempoLento = 0.3f;

    [Header("Dash Variables")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 0.2f;

    private bool isDashing = false;
    private float dashTime = 0f;

    private Vector2 movement;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;

    RoundManager RM;
    float timer;
    public bool muerto;
    public bool escudo;
    public bool canDash;
    public bool tiempo;

    public GameObject escudoBubble;
    public GameObject dashBubble;

    [SerializeField] SpriteRenderer sp;
    Animator anim;
    CameraZoom camZo;

    public GameObject uiTiempo;
    public GameObject uiMuerte;

    private PlayerControls controls;

    // Agrega una referencia al AudioSource
    private AudioSource audioSource;

    // Agrega el clip de audio para el sonido de muerte
    [SerializeField] private AudioClip deathSound;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        controls = new PlayerControls();
        playerCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>(); // Obtén el AudioSource
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Dash.performed += ctx => TryDash();
        controls.Gameplay.Dash.performed += ctx => ReloadSceneIfDead();
        controls.Gameplay.Return.performed += ctx => ReturnToMenu();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        camZo = FindAnyObjectByType<CameraZoom>();
        playerRB = GetComponent<Rigidbody2D>();
        RM = FindAnyObjectByType<RoundManager>();

        muerto = false;
        escudo = false;
        tiempo = false;
        canDash = false;

        if (uiTiempo) uiTiempo.SetActive(false);
        if (uiMuerte) uiMuerte.SetActive(false);
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;

        movement = controls.Gameplay.Move.ReadValue<Vector2>();

        if (tiempo)
        {
            if (uiTiempo) uiTiempo.SetActive(true);
        }
        else
        {
            if (uiTiempo) uiTiempo.SetActive(false);
        }

        if (muerto)
        {
            Time.timeScale = 0f;
            uiMuerte?.SetActive(true);
            PlayDeathSound();
            return;
        }

        if (tiempo && movement == Vector2.zero)
        {
            Time.timeScale = tiempoLento;
        }
        else
        {
            Time.timeScale = 1f;
        }

        timer = RM.timer;

        if (movement != Vector2.zero)
        {
            sp.flipX = movement.x > 0;
            anim.SetBool("andar", true);
        }
        else
        {
            anim.SetBool("andar", false);
        }
    }

    void FixedUpdate()
    {
        if (muerto || PauseMenu.isPaused) return;

        if (isDashing)
        {
            Dash();
        }
        else
        {
            playerRB.MovePosition(playerRB.position + movement * playerMoveSpeed * Time.fixedDeltaTime);
        }
    }

    private void TryDash()
    {
        if (!isDashing && canDash && !PauseMenu.isPaused)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = dashDuration;
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0.5f); // Reduce transparencia a la mitad
        playerCollider.enabled = false; // Desactivar colisión mientras es semitransparente
    }

    private void Dash()
    {
        Vector2 dashDirection = movement.normalized;
        if (dashDirection.magnitude > 0f)
        {
            playerRB.linearVelocity = dashDirection * dashSpeed;
        }

        dashTime -= Time.deltaTime;
        if (dashTime <= 0f)
        {
            isDashing = false;
            playerRB.linearVelocity = Vector2.zero;
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1f); // Restaurar transparencia
            playerCollider.enabled = true; // Reactivar colisión
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!escudo && !isDashing)
            {
                anim.SetBool("deat", true);
                muerto = true;
            }
            else if (isDashing)
            {
                Debug.Log("Evitado daño por Dash");
            }
            else
            {
                Debug.Log("Escudo roto");
                Destroy(collision.gameObject);
                escudo = false;
                escudoBubble.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Escudo"))
        {
            escudo = true;
            escudoBubble.SetActive(true);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Dash"))
        {
            canDash = true;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Tiempo"))
        {
            tiempo = true;
            if (uiTiempo) uiTiempo.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    public void ResetDash()
    {
        canDash = false;
    }

    private void ReturnToMenu()
    {
        if (muerto)
        {
            SceneManager.LoadScene("MenuInicio");
        }
    }

    private void ReloadSceneIfDead()
    {
        if (muerto)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Método para reproducir el sonido de muerte
    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound); // Reproduce el sonido de muerte
        }
    }
}
