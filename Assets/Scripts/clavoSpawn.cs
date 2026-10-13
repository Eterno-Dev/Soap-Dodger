using UnityEngine;
using System.Collections;

public class SpawnerDeProyectiles : MonoBehaviour
{
    public GameObject proyectilPrefab; // Prefab del proyectil
    public AudioClip sonidoClavo;
    private AudioSource audioSource;
    
   
    public float distanciaFueraDePantalla = 10f; // Distancia a la que se generará el proyectil
    public float velocidadProyectil = 10f; // Velocidad del proyectil
    public float offsetRotacion = 10f; // Offset de rotación en grados
    GameObject linea;

    private Camera camara;

    void Start()
    {
        camara = Camera.main;
        if (camara == null)
        {
            Debug.LogError("No se encontró la cámara principal.");
        }

        // Genera un proyectil cada 2 segundos
        InvokeRepeating(nameof(SpawnProyectil), 1f, 2f);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoClavo;

    }



    void SpawnProyectil()
    {
        // Genera una posición aleatoria fuera de la pantalla
        Vector3 posicionAleatoria = GenerarPosicionFueraDePantalla();
        // Instancia el proyectil
        GameObject proyectil = Instantiate(proyectilPrefab, posicionAleatoria, Quaternion.identity);

        // Obtiene la dirección hacia el centro de la pantalla (cámara)
        Vector3 centroDeLaPantalla = camara.transform.position;
        Vector3 direccion = (centroDeLaPantalla - posicionAleatoria).normalized;

        // Calcula el ángulo hacia el centro con el offset
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        angulo += offsetRotacion; // Aplica el offset de rotación

        // Ajusta la rotación del proyectil
        proyectil.transform.rotation = Quaternion.Euler(0, 0, angulo);
        linea = proyectil.transform.GetChild(0).gameObject;
        linea.SetActive(true);
        // Configura el movimiento del proyectil
        Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
        //Reproduce el sonido.
        audioSource.Play();
        if (rb != null)
        {
            // Mueve el proyectil en la dirección en la que está mirando, con la velocidad ajustada
            rb.linearVelocity = proyectil.transform.up * velocidadProyectil;
        }
        linea.SetActive(false);
    }

    Vector3 GenerarPosicionFueraDePantalla()
    {
        Vector3 pantallaMin = camara.ViewportToWorldPoint(new Vector3(0, 0, camara.nearClipPlane));
        Vector3 pantallaMax = camara.ViewportToWorldPoint(new Vector3(1, 1, camara.nearClipPlane));

        Vector3 posicionAleatoria = Vector3.zero;
        int lado = Random.Range(0, 4); // 0: arriba, 1: abajo, 2: izquierda, 3: derecha

        switch (lado)
        {
            case 0: // Arriba
                posicionAleatoria = new Vector3(Random.Range(pantallaMin.x, pantallaMax.x), pantallaMax.y + distanciaFueraDePantalla, 0);
                break;
            case 1: // Abajo
                posicionAleatoria = new Vector3(Random.Range(pantallaMin.x, pantallaMax.x), pantallaMin.y - distanciaFueraDePantalla, 0);
                break;
            case 2: // Izquierda
                posicionAleatoria = new Vector3(pantallaMin.x - distanciaFueraDePantalla, Random.Range(pantallaMin.y, pantallaMax.y), 0);
                break;
            case 3: // Derecha
                posicionAleatoria = new Vector3(pantallaMax.x + distanciaFueraDePantalla, Random.Range(pantallaMin.y, pantallaMax.y), 0);
                break;
        }

        return posicionAleatoria;
    }
}
