using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Texto UI para mostrar la puntuación
    public TextMeshProUGUI maxScoreText; // Texto UI para mostrar la puntuación
    public PlayerScoreManager playerScoreManager; // Referencia al script del jugador

    playerMovement PM;

    private void Start()
    {
        PM = FindAnyObjectByType<playerMovement>();
    }

    void Update()
    {
        if (playerScoreManager != null)
        {
            scoreText.text = $"Score: {playerScoreManager.currentScore}";
           
        }

        if (PM.muerto == true)
        {
            maxScoreText.text = $"Max Score: {playerScoreManager.highScore}";
        }
        else
        {
            maxScoreText.text = "";
        }


        /*if(PM.muerto == true)
        {
            scoreText.enabled = false ; 
            maxScoreText.enabled = false;  
        }*/

    }
}
