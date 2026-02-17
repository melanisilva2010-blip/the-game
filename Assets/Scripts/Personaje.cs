using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    [Header("Estadísticas")]
    [SerializeField] private float velocidad;
    public float vida = 100f;
    public float fuerzarebote = 5f;

    private BarraDeVida scriptBarraVida;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spritePersonaje;

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
        scriptBarraVida = FindAnyObjectByType<BarraDeVida>();
    }

    private void Update()
    {
        if (muerto) return;

        Movimiento();

        if (Input.GetMouseButtonDown(0) && !atacando)
        {
            Atacando();
        }

        anim.SetBool("Quieto", horizontal == 0 && vertical == 0);
        anim.SetBool("recibeDano", recibiendoDano);
    }

    private void Movimiento()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // GIRAR TODO EL PERSONAJE (Incluye la espada)
        if (horizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void Atacando()
    {
        atacando = true;
        anim.SetTrigger("Ataca");
        Debug.Log("Personaje: Atacando...");
        // Red de seguridad si falla el evento de animación
        Invoke("NoAtaca", 0.5f);
    }

    public void NoAtaca()
    {
        atacando = false;
    }

    public void RecibeDano(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDano && !muerto)
        {
            vida -= cantDanio;
            if (vida <= 0) { vida = 0; Morir(); }
            else
            {
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
        if (scriptBarraVida != null) Invoke("LlamarGameOver", 2.0f);
    }

    private void LlamarGameOver() { scriptBarraVida.GameOver(); }
    public void DesactivaDano() { recibiendoDano = false; }

    private void FixedUpdate()
    {
        if (!muerto && !recibiendoDano)
        {
            rb.velocity = new Vector2(horizontal, vertical).normalized * velocidad * Time.fixedDeltaTime;
            anim.SetFloat("Camina", rb.velocity.magnitude);
        }
    }
}