using UnityEngine;

public class spawnBubble : MonoBehaviour
{
    [Header("Spawn Config")]
    public GameObject[] objectsToSpawn; // Lista de objetos a spawnear
    public float spawnInterval = 5f;    // Tiempo entre cada spawn
    public float spawnRadius = 0.5f;    // Radio para verificar si el área está libre

    private float spawnTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Obtener la cámara principal
        spawnTimer = spawnInterval; // Inicializar el temporizador
    }

    void Update()
    {
        // Contador del tiempo
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObject();
            spawnTimer = spawnInterval; // Reiniciar el temporizador
        }
    }

    void SpawnObject()
    {
        // Asegurarse de que hay objetos configurados
        if (objectsToSpawn.Length == 0)
        {
            Debug.LogWarning("No hay objetos configurados para spawnear.");
            return;
        }

        // Seleccionar un objeto aleatorio de la lista
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        GameObject objectToSpawn = objectsToSpawn[randomIndex];

        // Calcular los límites de la cámara
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Intentar spawnear hasta que encontremos una posición válida
        Vector2 spawnPosition = Vector2.zero;
        int maxAttempts = 10; // Número máximo de intentos para encontrar una posición válida
        bool validPositionFound = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Generar una posición aleatoria dentro de los límites de la cámara
            float spawnX = Random.Range(mainCamera.transform.position.x - camWidth / 2f, mainCamera.transform.position.x + camWidth / 2f);
            float spawnY = Random.Range(mainCamera.transform.position.y - camHeight / 2f, mainCamera.transform.position.y + camHeight / 2f);

            spawnPosition = new Vector2(spawnX, spawnY);

            // Comprobar si hay un objeto con el tag "lado", "arriba" o "abajo" en la posición
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, spawnRadius);
            bool isOverlappingInvalidTag = false;

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("lado") || collider.CompareTag("arriba/abajo"))
                {
                    isOverlappingInvalidTag = true;
                    break;
                }
            }

            if (!isOverlappingInvalidTag)
            {
                validPositionFound = true; // La posición es válida
                break;
            }
        }

        if (validPositionFound)
        {
            // Instanciar el objeto
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No se encontró una posición válida para spawnear después de varios intentos.");
        }
    }
}
