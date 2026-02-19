using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Configuración")]
    public float detectionRadius = 5f;
    public float speed = 200f; // Sube este valor (ej. 200-400) porque usamos FixedDeltaTime con velocidad
    public int danoAlJugador = 10;
    public float fuerzarebote = 5f;

    [Header("Vida del Enemigo")]
    public int vidaMax = 30;
    [SerializeField]private int vidaActual;

    [Header("Referencias")]
    public Transform personaje;
    private Rigidbody2D rb;
    private Animator anim;
    private bool recibiendoDano;
    private bool estaMuerto = false;
    private Vector3 escalaOriginal;

    // Variable para guardar la dirección y usarla en FixedUpdate
    private Vector2 direccionMovimiento;

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
        bool moviendose = false;

        // Solo calculamos la dirección en el Update
        if (distanceToPlayer < detectionRadius && !recibiendoDano)
        {
            moviendose = true;
            // Usamos normalized para que la velocidad sea constante como en tu Personaje
            direccionMovimiento = (personaje.position - transform.position).normalized;

            // Giro del enemigo
            if (direccionMovimiento.x < 0) transform.localScale = new Vector3(-escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
            else if (direccionMovimiento.x > 0) transform.localScale = new Vector3(escalaOriginal.x, escalaOriginal.y, escalaOriginal.z);
        }
        else
        {
            direccionMovimiento = Vector2.zero;
        }

        if (anim != null)
        {
            anim.SetBool("enMovimiento", moviendose);
        }
        anim.SetBool("EsGolpeado",recibiendoDano);
    }

    private void FixedUpdate()
    {
        if (estaMuerto || recibiendoDano) return;

        // Aplicamos la velocidad igual que en tu script de Personaje
        rb.velocity = direccionMovimiento * speed * Time.fixedDeltaTime;
    }

    public void TomarDano(Vector2 posAtaque, int cantidad)
    {
        if (recibiendoDano || estaMuerto) return;
        Debug.Log("ME PEGASTE");
        vidaActual -= cantidad;
        recibiendoDano = true;

        if (anim != null) anim.SetBool("enMovimiento", false);

        // Aplicamos rebote
        Vector2 rebote = ((Vector2)transform.position - posAtaque).normalized;
        rb.velocity = Vector2.zero; // Limpiamos velocidad antes del impulso
        rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);

        StartCoroutine(DesactivaDano());

        if (vidaActual <= 0) Morir();
    }

    void Morir()
    {
        estaMuerto = true;
        if (anim != null)
        {
            //anim.SetBool("enMovimiento", false);
            anim.SetBool("EstaMuerto", estaMuerto);
        }

        GetComponent<Collider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Igual que tu personaje al morir
        Debug.Log("Me mori");
        Destroy(gameObject, 1.0f);
    }

    IEnumerator DesactivaDano()
    {
        yield return new WaitForSeconds(0.4f);
        recibiendoDano = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!estaMuerto && collision.CompareTag("Espada"))
        {
            TomarDano(collision.transform.position, 10);
        }
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