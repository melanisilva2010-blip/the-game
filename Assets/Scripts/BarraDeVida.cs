using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para la UI (Imagen)
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class BarraDeVida : MonoBehaviour
{
    [Header("Configuración UI")]
    public Image imagenBarraVida; // Aquí arrastras la imagen ROJA (la que se llena)

    [Header("Referencias (Automáticas)")]
    private Personaje scriptPersonaje;
    private float vidaMaxima;

    void Start()
    {
        // 1. Buscamos al Personaje automáticamente en la escena
        scriptPersonaje = FindAnyObjectByType<Personaje>();

        // 2. Guardamos la vida inicial como la "Vida Máxima" (el 100%)
        if (scriptPersonaje != null)
        {
            vidaMaxima = scriptPersonaje.vida;
        }
        else
        {
            Debug.LogError("¡ERROR! No encuentro al Personaje en la escena.");
        }
    }

    void Update()
    {
        // Solo actualizamos si encontramos al personaje
        if (scriptPersonaje != null)
        {
            // LA FÓRMULA DEL VIDEO:
            // Vida Actual / Vida Máxima = Porcentaje (entre 0 y 1)
            // Ejemplo: 50 / 100 = 0.5 (Mitad de barra)
            imagenBarraVida.fillAmount = scriptPersonaje.vida / vidaMaxima;
        }
    }

    // Esta función la llama el Personaje cuando muere
    public void GameOver()
    {
        // Asegúrate de tener una escena llamada "GameOver" en Build Settings
        SceneManager.LoadScene("GameOver");
    }
}