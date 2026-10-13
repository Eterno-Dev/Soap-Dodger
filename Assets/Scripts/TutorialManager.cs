using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform imagesParent; // Parent que contiene las imagenes
    [SerializeField] private InputActionReference dashAction; // Accion del Input System (Dash / A / Espacio)

    private GameObject[] images; // Array de imagenes dentro del parent
    private int currentIndex = 0; // Indice de la imagen actual
    private float openTime = 0f;
    private float lastAdvanceTime = 0f;
    private bool stickWasMoved = false;
    private const float OPEN_COOLDOWN = 0.35f; // Evita que la pulsacion que abrio el menu pase de diapositiva
    private const float CLOSE_COOLDOWN = 0.45f; // Evita volver a abrir "Como jugar" al cerrar creditos

    private void Awake()
    {
        InitImagesArray();
    }

    private void Start()
    {
        InitImagesArray();
    }

    private void OnEnable()
    {
        openTime = Time.unscaledTime;
        lastAdvanceTime = Time.unscaledTime;
        stickWasMoved = false;

        if (dashAction != null && dashAction.action != null)
        {
            dashAction.action.Enable();
            dashAction.action.performed += OnDashPressed;
        }

        // Al abrir el tutorial, mostrar siempre la primera imagen
        InitImagesArray();
        ResetImages();
        if (images != null && images.Length > 0)
        {
            images[0].SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (dashAction != null && dashAction.action != null)
        {
            dashAction.action.performed -= OnDashPressed;
            dashAction.action.Disable();
        }
    }

    private void InitImagesArray()
    {
        if (imagesParent == null)
        {
            imagesParent = transform;
        }

        if (images == null || images.Length != imagesParent.childCount)
        {
            images = new GameObject[imagesParent.childCount];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = imagesParent.GetChild(i).gameObject;
            }
        }
    }

    private void Update()
    {
        if (WasAnyAdvanceInputPressed())
        {
            AdvanceSlide();
        }
    }

    private bool WasAnyAdvanceInputPressed()
    {
        // 1. Teclado: WASD, Flechas, Espacio, Enter
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.wKey.wasPressedThisFrame ||
                keyboard.aKey.wasPressedThisFrame ||
                keyboard.sKey.wasPressedThisFrame ||
                keyboard.dKey.wasPressedThisFrame ||
                keyboard.upArrowKey.wasPressedThisFrame ||
                keyboard.downArrowKey.wasPressedThisFrame ||
                keyboard.leftArrowKey.wasPressedThisFrame ||
                keyboard.rightArrowKey.wasPressedThisFrame ||
                keyboard.spaceKey.wasPressedThisFrame ||
                keyboard.enterKey.wasPressedThisFrame ||
                keyboard.numpadEnterKey.wasPressedThisFrame)
            {
                return true;
            }
        }

        // 2. Mando: Botones Norte, Sur, Este, Oeste (Xbox: Y/A/B/X; PS: Triangulo/Cruz/Circulo/Cuadrado), Cruceta (D-Pad) y Stick
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame || // A / Cruz
                gamepad.buttonNorth.wasPressedThisFrame || // Y / Triangulo
                gamepad.buttonEast.wasPressedThisFrame ||  // B / Circulo
                gamepad.buttonWest.wasPressedThisFrame ||  // X / Cuadrado
                gamepad.dpad.up.wasPressedThisFrame ||
                gamepad.dpad.down.wasPressedThisFrame ||
                gamepad.dpad.left.wasPressedThisFrame ||
                gamepad.dpad.right.wasPressedThisFrame ||
                gamepad.leftShoulder.wasPressedThisFrame ||
                gamepad.rightShoulder.wasPressedThisFrame)
            {
                return true;
            }

            // Movimiento del stick izquierdo
            Vector2 stick = gamepad.leftStick.ReadValue();
            bool isTilted = stick.magnitude > 0.5f;
            if (isTilted && !stickWasMoved)
            {
                stickWasMoved = true;
                return true;
            }
            else if (!isTilted)
            {
                stickWasMoved = false;
            }
        }

        // 3. Raton: Clic izquierdo
        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    private void OnDashPressed(InputAction.CallbackContext context)
    {
        AdvanceSlide();
    }

    public void AdvanceSlide()
    {
        // Ignorar interacciones durante el periodo inicial tras abrir el tutorial
        if (Time.unscaledTime - openTime < OPEN_COOLDOWN)
        {
            return;
        }

        // Evitar que multiples inputs en el mismo frame avancen mas de una diapositiva
        if (Time.unscaledTime - lastAdvanceTime < 0.15f)
        {
            return;
        }
        lastAdvanceTime = Time.unscaledTime;

        if (images == null || images.Length == 0) return;

        // Desactivar la imagen actual
        if (currentIndex >= 0 && currentIndex < images.Length)
        {
            images[currentIndex].SetActive(false);
        }

        // Avanzar a la siguiente imagen
        currentIndex++;

        // Si hay mas imagenes, activar la siguiente
        if (currentIndex < images.Length)
        {
            images[currentIndex].SetActive(true);
        }
        else
        {
            // Bloquear reactivacion inmediata accidental de "Como jugar" al cerrar
            PanelControl.SetTutorialCooldown(CLOSE_COOLDOWN);

            // Si llegamos al final, cerrar el panel y reiniciar secuencia
            ResetImages();
            gameObject.SetActive(false);
        }
    }

    private void ResetImages()
    {
        if (images != null)
        {
            foreach (var img in images)
            {
                if (img != null) img.SetActive(false);
            }
        }

        currentIndex = 0; // Reiniciar secuencia
    }
}