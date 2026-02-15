using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaBloqueada : MonoBehaviour
{
    [Header("Arrastra aquí el Panel del mensaje")]
    public GameObject cartelAviso;

    private void Start()
    {
        if (cartelAviso != null)
        {
            cartelAviso.SetActive(false);
        }
    }

    // 1. Detecta cuando el personaje entra
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asegúrar de que el personaje tenga el Tag "Personaje"
        if (collision.CompareTag("Personaje"))
        {
            PausarYMostrar();
        }
    }

    // 2. Mostramos el cartel y congelar el tiempo 
    void PausarYMostrar()
    {
        cartelAviso.SetActive(true);
        Time.timeScale = 0; //detiene el juego
    }

    // 3. LA FUNCIÓN PARA EL BOTÓN (Igual que tu Reanudar)
    public void CerrarCartel()
    {
        Debug.Log("¡CLICK RECIBIDO! Intentando cerrar..."); // Esto escribe en la consola
        cartelAviso.SetActive(false);
        Time.timeScale = 1f;
    }
}