using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoEnemigo : MonoBehaviour
{
    [Header("Disc Movement Settings")]
    public float speed = 5f; // Velocidad del disco
    public float delayBetweenPasses = 3f; // Tiempo de espera antes de reaparecer
    public float rotationSpeed = 360f; // Velocidad de rotación del disco (grados por segundo)

    [Header("Appearance Settings")]
    public bool randomizeDirection = true; // Si el disco puede aparecer en dirección horizontal o vertical

    private Vector2 screenBounds;
    private float spriteWidth;  // Ancho del sprite
    private float spriteHeight; // Alto del sprite

    private void Start()
    {
        // Calcular los límites de la pantalla en coordenadas del mundo
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Obtener el tamaño del Sprite Renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteWidth = spriteRenderer.bounds.extents.x;  // Mitad del ancho del sprite
            spriteHeight = spriteRenderer.bounds.extents.y; // Mitad del alto del sprite
        }

        // Iniciar el comportamiento del disco
        StartCoroutine(DiscoMovimiento());
    }

    private IEnumerator DiscoMovimiento()
    {
        while (true)
        {
            // Determinar si el movimiento será horizontal o vertical
            bool isHorizontal = randomizeDirection ? Random.value > 0.5f : true;

            // Determinar si el disco empieza desde un lado positivo o negativo del eje
            bool startsFromPositive = Random.value > 0.5f;

            // Posición inicial y final
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;

            if (isHorizontal)
            {
                // Movimiento horizontal
                startPos = startsFromPositive
                    ? new Vector3(-screenBounds.x - spriteWidth, Random.Range(-screenBounds.y, screenBounds.y), 0f)
                    : new Vector3(screenBounds.x + spriteWidth, Random.Range(-screenBounds.y, screenBounds.y), 0f);

                endPos = startsFromPositive
                    ? new Vector3(screenBounds.x + spriteWidth, startPos.y, 0f)
                    : new Vector3(-screenBounds.x - spriteWidth, startPos.y, 0f);
            }
            else
            {
                // Movimiento vertical
                startPos = startsFromPositive
                    ? new Vector3(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spriteHeight, 0f)
                    : new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spriteHeight, 0f);

                endPos = startsFromPositive
                    ? new Vector3(startPos.x, screenBounds.y + spriteHeight, 0f)
                    : new Vector3(startPos.x, -screenBounds.y - spriteHeight, 0f);
            }

            // Mover el disco a la posición inicial
            transform.position = startPos;

            // Movimiento del disco
            while (Vector3.Distance(transform.position, endPos) > 0.1f)
            {
                // Rotar el sprite mientras se mueve
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

                // Mover el disco hacia su posición final
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

                yield return null;
            }

            // Esperar antes del próximo movimiento
            yield return new WaitForSeconds(delayBetweenPasses);
        }
    }
}
