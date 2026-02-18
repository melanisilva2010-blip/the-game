using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float velocidad;
    [SerializeField] private BoxCollider2D colEspada; // La mantuve como pediste

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spritePersonaje;

    private float horizontal;
    private float vertical;

    // Estados
    private bool atacando;
    private bool muerto;
    private bool recibiendoDano;

    public float fuerzarebote = 5f;
    public float vida = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spritePersonaje = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Si estamos atacando o recibiendo daño, no leemos input de movimiento
        if (atacando || recibiendoDano) return;

        Movimiento();

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Ataca");
            Atacando(); // Activa el bool atacando
        }

        // Animaciones
        // "Quieto" es true si no nos movemos ni en X ni en Y
        bool estaQuieto = horizontal == 0 && vertical == 0;
        anim.SetBool("Quieto", estaQuieto);
        anim.SetBool("recibeDano", recibiendoDano);
    }

    // OJO: En el script del enemigo, llama a esto pasando "transform.position" del enemigo
    public void RecibeDano(Vector2 posicionEnemigo, int cantDanio)
    {
        if (!recibiendoDano)
        {
            Debug.Log("Golpeado");
            recibiendoDano = true;
            vida -= cantDanio;

            // Frenamos para que el rebote sea limpio
            rb.velocity = Vector2.zero;

            // CALCULO DEL REBOTE TOP-DOWN (ZELDA)
            // (Tu posicion - Posicion Enemigo) nos da la dirección contraria
            Vector2 direccionRebote = (transform.position - (Vector3)posicionEnemigo).normalized;

            rb.AddForce(direccionRebote * fuerzarebote, ForceMode2D.Impulse);
        }
    }

    public void DesactivaDano()
    {
        recibiendoDano = false;
        rb.velocity = Vector2.zero;
    }

    private void Atacando()
    {
        atacando = true;
        rb.velocity = Vector2.zero; // Frenamos al personaje al atacar
    }

    // IMPORTANTE: Llama a esta función desde un Evento en la Animación de Atacar
    public void NoAtaca()
    {
        atacando = false;
    }

    private void Movimiento()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // Usé Raw para que sea más responsivo, puedes quitarlo si prefieres GetAxis normal
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal < 0)
        {
            spritePersonaje.flipX = true;
        }
        else if (horizontal > 0)
        {
            spritePersonaje.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        if (recibiendoDano || atacando) return;

        // Quité Time.deltaTime aquí porque rb.velocity ya maneja el tiempo físico.
        // Si lo dejas, el personaje se moverá leeeento.
        rb.velocity = new Vector2(horizontal, vertical).normalized * velocidad;

        // Usamos magnitude para que la animación de caminar funcione también hacia arriba/abajo
        anim.SetFloat("Camina", rb.velocity.magnitude);
    }
}