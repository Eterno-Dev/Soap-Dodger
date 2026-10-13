# Soap Dodger 🧼🫧

**Soap Dodger** es un juego de supervivencia arcade 2D desarrollado originalmente para una Game Jam con **Unity 6**.

El jugador controla una pastilla de jabón que debe sobrevivir en una bañera esquivando peligros crecientes ronda tras ronda, recolectando burbujas con habilidades especiales.

---

## 🎮 Mecánicas y Jugabilidad

* **Sistema de Rondas:** Cada ronda dura 20 segundos. Al superar cada ronda, el ritmo del juego se intensifica y aparecen nuevos obstáculos y enemigos.
* **Peligros y Obstáculos:**
  * **Agujas teledirigidas:** Siguen al jugador durante unos segundos antes de lanzarse a toda velocidad.
  * **Clavos:** Proyectiles disparados desde los bordes de la pantalla.
  * **Motosierras:** Se activan a partir de la ronda 2, rebotando por el escenario.
  * **Shurikens:** Aparecen a partir de la ronda 3.
* **Burbujas de Poder (Power-ups):**
  * 🛡️ **Escudo:** Protege al jugador de un impacto letal.
  * 💨 **Dash:** Permite un impulso rápido e invulnerable a través de los peligros.
  * ⏳ **Ralentización temporal:** Al detener el movimiento, el tiempo fluye más lento, permitiendo planificar esquivas con precisión.

---

## ⌨️ Controles

* **Moverse:** Flechas de dirección / WASD / Stick izquierdo (Gamepad)
* **Dash:** Botón de Dash / Barra espaciadora (teclado)
* **Reiniciar / Menú:** Al morir, pulsa Dash para reiniciar la partida o Escape para volver al menú.

---

## 🛠️ Tecnologías y Requisitos

* **Motor:** Unity 6 (`6000.5.8f1` o compatible)
* **Sistema de Input:** Unity Input System (nuevo sistema de eventos)
* **Renderizado:** 2D / Sprites
* **UI:** TextMeshPro & Unity UI

---

## 🚀 Cómo abrir el proyecto

1. Clona este repositorio:
   ```bash
   git clone https://github.com/TU_USUARIO/SoapDodger_Final_Proyect_Unity.git
   ```
2. Abre **Unity Hub**.
3. Selecciona **Add project from disk** y elige la carpeta raíz del repositorio.
4. Asegúrate de abrirlo con la versión de Unity recomendada (`6000.x` o superior).
5. Abre la escena principal ubicada en `Assets/Scenes/`.
