using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private BoxCollider2D colEspada;
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private float vertical;
    private bool atacando;
    private bool muerto;
    private SpriteRenderer spritePersonaje;
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
        Movimiento();

        if (Input.GetMouseButtonDown(0) && !atacando )
        {
            anim.SetTrigger("Ataca");
            Atacando();
        }
        else anim.SetTrigger("NoAtaca");
        anim.SetBool("Quieto", horizontal == 0);
        anim.SetBool("recibeDano", recibiendoDano);
    }

    public void RecibeDano(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDano)
        {
            Debug.Log("Golpeado");
            recibiendoDano = true;
            vida -= cantDanio;
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
            rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);
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
    }
    private void NoAtaca()
    {
        atacando = false;
    }

    private void Movimiento()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
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
        rb.velocity = new Vector2(horizontal, vertical).normalized * velocidad*Time.deltaTime;
        anim.SetFloat("Camina", Mathf.Abs(rb.velocity.x));
    }

}
