using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISelectionKeeper : MonoBehaviour
{
    private static UISelectionKeeper instance;
    private GameObject lastSelected;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoInitialize()
    {
        if (instance == null && FindAnyObjectByType<UISelectionKeeper>() == null)
        {
            GameObject keeperObj = new GameObject("UISelectionKeeper");
            instance = keeperObj.AddComponent<UISelectionKeeper>();
            DontDestroyOnLoad(keeperObj);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (EventSystem.current == null) return;

        // 1. Detectar si el puntero del raton esta sobre un boton seleccionable y seleccionarlo para que se anime
        if (TryGetHoveredSelectable(out GameObject hoveredSelectable))
        {
            if (EventSystem.current.currentSelectedGameObject != hoveredSelectable)
            {
                EventSystem.current.SetSelectedGameObject(hoveredSelectable);
                lastSelected = hoveredSelectable;
            }
        }

        // 2. Mantener siempre un boton seleccionado para evitar que se desanime al hacer clic fuera
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null && current.activeInHierarchy)
        {
            lastSelected = current;
        }
        else
        {
            // Si el foco se perdio (ej: clic fuera en el panel o fondo), restaurar el ultimo boton valido
            if (lastSelected != null && lastSelected.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else
            {
                // Si el ultimo ya no esta disponible, seleccionar el primer Selectable activo
                Selectable[] selectables = FindObjectsByType<Selectable>(FindObjectsSortMode.None);
                foreach (var sel in selectables)
                {
                    if (sel.gameObject.activeInHierarchy && sel.interactable)
                    {
                        lastSelected = sel.gameObject;
                        EventSystem.current.SetSelectedGameObject(sel.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private bool TryGetHoveredSelectable(out GameObject selectableObj)
    {
        selectableObj = null;
        if (EventSystem.current == null) return false;

        Vector2 mousePosition;
        if (Mouse.current != null)
        {
            mousePosition = Mouse.current.position.ReadValue();
        }
        else
        {
            mousePosition = Input.mousePosition;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Selectable selectable = result.gameObject.GetComponentInParent<Selectable>();
            if (selectable != null && selectable.interactable && selectable.gameObject.activeInHierarchy)
            {
                selectableObj = selectable.gameObject;
                return true;
            }
        }

        return false;
    }
}
