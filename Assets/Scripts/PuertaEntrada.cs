using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaEntrada : MonoBehaviour
{
    [Header("Arrastra aquí tu Panel")]
    public GameObject panelPregunta;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si entra el personaje, ACTIVAMOS el panel
        if (collision.CompareTag("Personaje"))
        {
            panelPregunta.SetActive(true); // ¡Aquí se hace visible!
            Time.timeScale = 0f;           // Pausa el juego
        }
    }

    // --- BOTÓN SÍ (Ir a la escena 8) ---
    public void Entrar()
    {
        Time.timeScale = 1f;       // Despausar siempre antes de cambiar
        SceneManager.LoadScene(8); // Carga la escena 8
    }

    // --- BOTÓN NO (Quedarse) ---
    public void Cancelar()
    {
        panelPregunta.SetActive(false); // Oculta el panel
        Time.timeScale = 1f;            // Sigue el juego
    }
}