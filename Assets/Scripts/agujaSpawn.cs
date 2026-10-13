using UnityEngine;

public class agujaSpawn : MonoBehaviour
{
    public GameObject proyectilPrefab; // Prefab del proyectil
    public AudioClip sonidoAguja;
    private AudioSource audioSource;
    public float tiempoEntreSpawns = 3f; // Tiempo entre la generación de proyectiles
    [SerializeField] private float distanciaSeguraJugador = 2.5f; // Distancia mínima para no spawnear encima del jugador

    private Camera camara;
    private Transform jugador;
    RoundManager RM;

    void Start()
    {
        RM = FindAnyObjectByType<RoundManager>();
        camara = Camera.main;
        if (camara == null)
        {
            Debug.LogError("No se encontró la cámara principal.");
        }

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
        {
            jugador = pObj.transform;
        }

        // Genera un proyectil cada cierto tiempo
        InvokeRepeating(nameof(SpawnProyectil), 1f, tiempoEntreSpawns);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoAguja;
    }

    void SpawnProyectil()
    {
        Vector3 posicionAleatoria = GenerarPosicionDentroDePantalla();
        Instantiate(proyectilPrefab, posicionAleatoria, Quaternion.identity);
        if (audioSource != null && sonidoAguja != null)
        {
            audioSource.Play();
        }
    }

    Vector3 GenerarPosicionDentroDePantalla()
    {
        Vector3 pantallaMin = camara.ViewportToWorldPoint(new Vector3(0, 0, camara.nearClipPlane));
        Vector3 pantallaMax = camara.ViewportToWorldPoint(new Vector3(1, 1, camara.nearClipPlane));

        Vector3 posicion = Vector3.zero;
        int maxIntentos = 10;

        for (int i = 0; i < maxIntentos; i++)
        {
            float x = Random.Range(pantallaMin.x + 0.5f, pantallaMax.x - 0.5f);
            float y = Random.Range(pantallaMin.y + 0.5f, pantallaMax.y - 0.5f);
            posicion = new Vector3(x, y, 0);

            // Verificar distancia de seguridad respecto al jugador
            if (jugador != null)
            {
                if (Vector2.Distance(posicion, jugador.position) >= distanciaSeguraJugador)
                {
                    return posicion;
                }
            }
            else
            {
                return posicion;
            }
        }

        return posicion;
    }
}
