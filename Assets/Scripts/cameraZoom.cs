using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera; // Cámara principal
    public Transform player; // Transform del personaje
    public float zoomSpeed = 2f; // Velocidad del zoom
    public float targetZoom = 3f; // Tamaño objetivo de la cámara
    private float originalZoom; // Tamaño original de la cámara

    playerMovement pj;

    void Start()
    {
        pj = FindAnyObjectByType<playerMovement>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Busca automáticamente la cámara principal
        }

        originalZoom = mainCamera.orthographicSize; // Guarda el zoom original
    }

    void Update()
    {
        // Lerp para hacer zoom de forma suave
        if (pj.muerto == true) // Por ejemplo, al presionar "Z"
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.unscaledDeltaTime * zoomSpeed);
            // Asegurarse de que la cámara siempre sigue al jugador
            if (player != null)
            {
                mainCamera.transform.position = new Vector3(player.position.x, player.position.y, mainCamera.transform.position.z);
            }
        }
        else
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, originalZoom, Time.unscaledDeltaTime * zoomSpeed);
        }

        
    }
}
