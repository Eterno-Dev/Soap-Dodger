using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referencia al VideoPlayer

    void Start()
    {
        // Asegurar que el video comience automáticamente
        videoPlayer.Play();

        // Suscribirse al evento de finalización del video
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Cambiar a la escena "MenuInicio" cuando el video termine
        SceneManager.LoadScene("MenuInicio");
    }
}
