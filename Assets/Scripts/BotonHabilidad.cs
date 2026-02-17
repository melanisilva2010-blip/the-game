using UnityEngine;
using UnityEngine.UI; // Necesario para manejar el botón e imágenes
using TMPro; // Si usas TextMeshPro para el texto del contador

public class BotonHabilidad : MonoBehaviour
{
    [Header("Configuración")]
    public Personaje personaje; // Arrastra al personaje aquí
    public float tiempoCooldown = 30f;
    private float tiempoRestante = 0f;
    private bool enCooldown = false;

    [Header("Componentes UI")]
    private Button miBoton;
    public Image imagenSombra; // Una imagen encima para el efecto de carga
    public TextMeshProUGUI textoContador; // Texto para ver los segundos

    void Start()
    {
        miBoton = GetComponent<Button>();
        if (imagenSombra != null) imagenSombra.fillAmount = 0;
        if (textoContador != null) textoContador.text = "";
    }

    void Update()
    {
        if (enCooldown)
        {
            tiempoRestante -= Time.deltaTime;

            // Actualizamos la barra visual de carga
            if (imagenSombra != null)
                imagenSombra.fillAmount = tiempoRestante / tiempoCooldown;

            // Actualizamos el texto
            if (textoContador != null)
                textoContador.text = Mathf.Ceil(tiempoRestante).ToString();

            if (tiempoRestante <= 0)
            {
                TerminarCooldown();
            }
        }
    }

    // Esta función la llamará el botón al hacer click (On Click)
    public void ClickHabilidad()
    {
        // Solo funciona si no está en cooldown y el personaje necesita vida
        if (!enCooldown && personaje.vida < 100)
        {
            personaje.UsarHabilidadRegeneracion(); // Llama a la función del personaje
            IniciarCooldown();
        }
    }

    void IniciarCooldown()
    {
        enCooldown = true;
        miBoton.interactable = false; // Desactiva el botón
        tiempoRestante = tiempoCooldown;
    }

    void TerminarCooldown()
    {
        enCooldown = false;
        miBoton.interactable = true; // Activa el botón
        if (imagenSombra != null) imagenSombra.fillAmount = 0;
        if (textoContador != null) textoContador.text = "";
    }
}