using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Necesario para usar el New Input System

public class TutorialManager : MonoBehaviour
{
        [SerializeField] private Transform imagesParent; // Parent que contiene las imágenes
        [SerializeField] private InputActionReference dashAction; // Acción del Input System (Dash)

        private GameObject[] images; // Array de imágenes dentro del parent
        private int currentIndex = 0; // Índice de la imagen actual

        private void Start()
        {
            // Obtener todas las imágenes del Parent
            images = new GameObject[imagesParent.childCount];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = imagesParent.GetChild(i).gameObject;
                images[i].SetActive(false); // Desactivar todas las imágenes al inicio
            }

            // Activar la primera imagen si hay imágenes en el parent
            if (images.Length > 0)
                images[0].SetActive(true);
        }

        private void OnEnable()
        {
            dashAction.action.Enable();
            dashAction.action.performed += OnDashPressed;
        }

        private void OnDisable()
        {
            dashAction.action.performed -= OnDashPressed;
            dashAction.action.Disable();
        }

        private void OnDashPressed(InputAction.CallbackContext context)
        {
            // Desactivar la imagen actual
            if (currentIndex < images.Length)
                images[currentIndex].SetActive(false);

            // Avanzar a la siguiente imagen
            currentIndex++;

            // Si hay más imágenes, activar la siguiente
            if (currentIndex < images.Length)
            {
                images[currentIndex].SetActive(true);
            }
            else
            {
                // Si llegamos al final, desactivar todas y reiniciar
                ResetImages();
            }
        }

        private void ResetImages()
        {
            foreach (var img in images)
                img.SetActive(false);

            currentIndex = 0; // Reiniciar secuencia
        }
}
