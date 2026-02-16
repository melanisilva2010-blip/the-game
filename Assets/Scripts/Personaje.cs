using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Estadísticas")]
    [SerializeField] private float velocidad;
    public float vida = 100f;
    public float fuerzarebote = 5f;

    [Header("Referencias")]
    [SerializeField] private BoxCollider2D colEspada;

    // YA NO ES NECESARIO ARRASTRARLO (Quitamos el [SerializeField])
    private BarraDeVida scriptBarraVida;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spritePersonaje;

    // Variables de control
    private float horizontal;
    private float vertical;
    private bool atacando;
    private bool recibiendoDano;
    private bool muerto = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spritePersonaje = GetComponentInChildren<SpriteRenderer>();

        // AQUÍ ESTÁ EL CAMBIO MÁGICO:
        // Buscamos el script dentro de este mismo objeto
        scriptBarraVida = GetComponent<BarraDeVida>();

        // Si no está en el personaje, intentamos buscarlo en el Canvas por si acaso
        if (scriptBarraVida == null)
        {
            scriptBarraVida = FindAnyObjectByType<BarraDeVida>();
        }
    }

    private void Update()
    {
        if (muerto) return;

        Movimiento();

        if (Input.GetMouseButtonDown(0) && !atacando)
        {
            anim.SetTrigger("Ataca");
            Atacando();
        }
        else
        {
            // OJO: Si usas triggers, cuidado con llamar a esto en cada frame
            // Pero lo dejamos como en tu original.
            // anim.SetTrigger("NoAtaca"); // Comentado por seguridad, mejor usar Bool o Evento
        }

        anim.SetBool("Quieto", horizontal == 0);
        anim.SetBool("recibeDano", recibiendoDano);
    }

    public void RecibeDano(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDano && !muerto)
        {
            vida -= cantDanio;

            if (vida <= 0)
            {
                vida = 0;
                Morir();
            }
            else
            {
                Debug.Log("Golpeado");
                recibiendoDano = true;
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
                rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);
                Invoke("DesactivaDano", 0.5f);
            }
        }
    }

    private void Morir()
    {
        muerto = true;
        anim.SetTrigger("Muerte");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (scriptBarraVida != null)
        {
            Invoke("LlamarGameOver", 2.0f);
        }
        else
        {
            Debug.LogError("¡No encuentro el script BarraDeVida! Revisa que esté en la escena.");
        }
    }

    private void LlamarGameOver()
    {
        scriptBarraVida.GameOver();
    }

    public void DesactivaDano()
    {
        recibiendoDano = false;
        rb.velocity = Vector2.zero;
    }

    private void Atacando()
    {
        atacando = true;
    }

    // Funciones de movimiento
    private void Movimiento()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (horizontal < 0) spritePersonaje.flipX = true;
        else if (horizontal > 0) spritePersonaje.flipX = false;
    }

    private void FixedUpdate()
    {
        if (!muerto)
        {
            rb.velocity = new Vector2(horizontal, vertical).normalized * velocidad * Time.deltaTime;
            anim.SetFloat("Camina", Mathf.Abs(rb.velocity.x));
        }
    }
}