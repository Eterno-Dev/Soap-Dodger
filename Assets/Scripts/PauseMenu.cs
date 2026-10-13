using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Optional Button References")]
    [Tooltip("Boton de continuar (opcional, tambien se puede vincular por OnClick)")]
    [SerializeField] private Button continueButton;
    [Tooltip("Boton de volver al menu (opcional, tambien se puede vincular por OnClick)")]
    [SerializeField] private Button returnMenuButton;

    private playerMovement player;

    void Start()
    {
        player = FindAnyObjectByType<playerMovement>();

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
        }

        if (returnMenuButton != null)
        {
            returnMenuButton.onClick.AddListener(ReturnToMenu);
        }
    }

    public void ContinueGame()
    {
        if (player != null)
        {
            player.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

    public void ReturnToMenu()
    {
        if (player != null)
        {
            player.ReturnToMenuFromPause();
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuInicio");
        }
    }
}
