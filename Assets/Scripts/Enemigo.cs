using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Configuración")]
    public float detectionRadius = 5f;
    public float speed = 2f;
    public int danoAlJugador = 10;
    public float fuerzarebote = 5f;

    [Header("Vida del Enemigo")]
    public int vidaMax = 30;
    private int vidaActual;

    [Header("Referencias")]
    public Transform personaje;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private bool recibiendoDano;
    private bool estaMuerto = false;

    // Variable para guardar el tamaño original del Inspector
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // GUARDAMOS EL TAMAÑO QUE TIENE EN EL INSPECTOR AL EMPEZAR
        escalaOriginal = transform.localScale;

        vidaActual = vidaMax;

        if (personaje == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Personaje");
            if (playerObj != null) personaje = playerObj.transform;
        }
    }

    void Update()
    {
        if (personaje == null || estaMuerto) return;

        float distanceToPlayer = Vector2.Distance(transform.position, personaje.position);

        if (distanceToPlayer < detectionRadius && !recibiendoDano)
        {
            Vector2 direction = (personaje.position - transform.position).normalized;

            // CORRECCIÓN DEL TAMAÑO:
            // Usamos escalaOriginal.x para que mantenga el tamaño que tú le pusiste
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            }

            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }

        if (!recibiendoDano)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMuerto) return;

        if (collision.gameObject.CompareTag("Personaje"))
        {
            Personaje scriptPersonaje = collision.gameObject.GetComponent<Personaje>();
            if (scriptPersonaje != null)
            {
                Vector2 direccionDelGolpe = transform.position;
                scriptPersonaje.RecibeDano(direccionDelGolpe, danoAlJugador);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaMuerto) return;

        if (collision.CompareTag("Espada"))
        {
            int danoEspada = 10;
            Vector2 direccionAtaque = collision.transform.position;
            TomarDano(direccionAtaque, danoEspada);
        }
    }

    public void TomarDano(Vector2 direccion, int cantidad)
    {
        if (!estaMuerto && !recibiendoDano)
        {
            vidaActual -= cantidad;
            recibiendoDano = true;
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
            rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);
            StartCoroutine(DesactivaDano());

            if (vidaActual <= 0)
            {
                Morir();
            }
        }
    }

    void Morir()
    {
        estaMuerto = true;
        if (anim != null) anim.SetTrigger("Muerte");
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1.0f);
    }

    IEnumerator DesactivaDano()
    {
        yield return new WaitForSeconds(0.5f);
        recibiendoDano = false;
        if (!estaMuerto) rb.velocity = Vector2.zero;
    }
}
