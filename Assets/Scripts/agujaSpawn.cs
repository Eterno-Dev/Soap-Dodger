using UnityEngine;

public class agujaSpawn : MonoBehaviour
{
    public GameObject proyectilPrefab; // Prefab del proyectil
    public AudioClip sonidoAguja;
    private AudioSource audioSource;
    public float tiempoEntreSpawns = 3f; // Tiempo entre la generación de proyectiles

    private Camera camara;

    RoundManager RM;
    int i;

    void Start()
    {
        RM = FindAnyObjectByType<RoundManager>();
        camara = Camera.main;
        if (camara == null)
        {
            Debug.LogError("No se encontró la cámara principal.");
        }

        // Genera un proyectil cada cierto tiempo
        InvokeRepeating(nameof(SpawnProyectil), 1f, tiempoEntreSpawns);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoAguja;
    }

    void SpawnProyectil()
    {
        // Genera una posición dentro de la cámara
        Vector3 posicionAleatoria = GenerarPosicionDentroDePantalla();

        // Instancia el proyectil
        Instantiate(proyectilPrefab, posicionAleatoria, Quaternion.identity);
        audioSource.Play();
    }

    Vector3 GenerarPosicionDentroDePantalla()
    {
        // Calcula el límite de la cámara en el espacio mundial
        Vector3 pantallaMin = camara.ViewportToWorldPoint(new Vector3(0, 0, camara.nearClipPlane));
        Vector3 pantallaMax = camara.ViewportToWorldPoint(new Vector3(1, 1, camara.nearClipPlane));

        // Genera una posición aleatoria dentro de los límites de la cámara
        float x = Random.Range(pantallaMin.x, pantallaMax.x);
        float y = Random.Range(pantallaMin.y, pantallaMax.y);

        return new Vector3(x, y, 0); // Devuelve una posición dentro de la cámara
    }
}
