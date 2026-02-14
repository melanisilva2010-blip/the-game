using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform personaje;
    public float detectionRadius = 5f;  
    public float speed = 2f;
    public int dano = 5;
    private Rigidbody2D rb;
    private Animation animator;
    private Vector2 movement;
    private bool recibiendoDano;
    public float fuerzarebote = 5f;
    private bool enMovimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, personaje.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (personaje.position - transform.position).normalized;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            movement = direction;

            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }
        if(!recibiendoDano)
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Personaje"))
        {
            Vector2 direccionDano = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<Personaje>().RecibeDano(direccionDano, 4);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada"))
        {
            Vector2 direccionDano = new Vector2(collision.gameObject.transform.position.x, 0);
            RecibeDano(direccionDano, 4);
        }
    }

    public void RecibeDano(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDano)
        {
            Debug.Log("Golpeado");
            recibiendoDano = true;
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
            rb.AddForce(rebote * fuerzarebote, ForceMode2D.Impulse);
            StartCoroutine(DesactivaDano());
        }
    }

    IEnumerator DesactivaDano()
    {
        yield return new WaitForSeconds(0.5f);
        recibiendoDano = false;
        rb.velocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius-2);
    }
}

