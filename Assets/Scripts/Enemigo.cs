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
    private bool recibiendoDano;
    private bool estaMuerto = false;
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            // Giro del enemigo respetando su tamaño original
            if (direction.x < 0) transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            else if (direction.x > 0) transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }

    // DETECCIÓN DE LA ESPADA (TRIGGER)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENEMIGO: He sido tocado por " + collision.gameObject.name + " con Tag: " + collision.tag);

        if (!estaMuerto && collision.CompareTag("Espada"))
        {
            Debug.Log("ENEMIGO: ¡Recibí daño de la espada!");
            TomarDano(collision.transform.position, 10);
        }
    }

    public void TomarDano(Vector2 posAtaque, int cantidad)
    {
        if (recibiendoDano) return;

        vidaActual -= cantidad;
        recibiendoDano = true;

        Vector2 rebote = new Vector2(transform.position.x - posAtaque.x, 1).normalized;
        rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);

        StartCoroutine(DesactivaDano());

        if (vidaActual <= 0) Morir();
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
        yield return new WaitForSeconds(0.4f);
        recibiendoDano = false;
        if (!estaMuerto) rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMuerto) return;
        if (collision.gameObject.CompareTag("Personaje"))
        {
            Personaje p = collision.gameObject.GetComponent<Personaje>();
            if (p != null) p.RecibeDano(transform.position, danoAlJugador);
        }
    }
}