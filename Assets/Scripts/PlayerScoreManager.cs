using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public int currentScore = 0; // Puntuación actual
    public int highScore = 0;    // Puntuación máxima
    [SerializeField] float tiempoLento = 0.3f;

    void Start()
    {
        // Cargar la puntuación máxima guardada al inicio
        highScore = PlayerPrefs.GetInt("HighScore", 0); // 0 es el valor predeterminado si no hay datos guardados
        Debug.Log($"Puntuación máxima cargada: {highScore}");
    }

    // Método para añadir puntos
    public void AddScore(int points)
    {
        
        if(Time.timeScale == tiempoLento)
        {
            currentScore += points;
        }
        else
        {
            currentScore += points*2;
        }

        // Si la puntuación actual supera la máxima, actualiza el récord
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore); // Guardar la puntuación máxima
            PlayerPrefs.Save();
            Debug.Log($"¡Nuevo récord! Puntuación máxima: {highScore}");
        }
    }
}
