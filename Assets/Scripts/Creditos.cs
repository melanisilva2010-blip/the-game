using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelCreditos; // Le cambié el nombre para que no se confunda con la clase
    [SerializeField] private GameObject canvasFade; // Asumo que es para el efecto visual

    private void Start()
    {
        // Al empezar, nos aseguramos que los créditos estén ocultos
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asegurar de que el personaje tenga el Tag "Personaje"
        if (collision.CompareTag("Personaje"))
        {
            MostrarCreditos(); // <--- Ahora llamamos a una función real
        }
    }

    // Esta es la función nueva que hace el trabajo
    public void MostrarCreditos()
    {
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(true); // Encendemos el panel
            // Opcional: Si quieres que el juego se pause al ver los créditos, descomenta la siguiente línea:
            // Time.timeScale = 0f; 
        }
    }

    public void volverMenu()
    {
        Time.timeScale = 1f; // Importante: Reactivar el tiempo por si estaba pausado
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego..."); // Para ver que funciona en el editor
        Application.Quit();
    }

    // He mantenido tu corrutina por si la usas en otro lado, 
    // pero recuerda llamarla con StartCoroutine(DesvanecerPantalla()) si la necesitas.
    IEnumerator DesvanecerPantalla()
    {
        if (canvasFade != null)
        {
            canvasFade.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            // Esto hará que se vuelva transparente de golpe. 
            // Si quieres que sea gradual, necesitas un bucle (for/while).
            if (canvasFade.GetComponent<Image>() != null)
                canvasFade.GetComponent<Image>().color = Color.clear;

            yield return new WaitForSeconds(0.6f);
        }
    }
}