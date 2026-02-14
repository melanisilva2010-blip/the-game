using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSalto : MonoBehaviour
{
    public float forceJump;
    private bool isground;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, 0.7f))
        {
            isground=true;
        }else isground=false;

        if (Input.GetKeyDown(KeyCode.Space)&& isground)
        {
            Saltar();
        }
    }
    private void Saltar()
    {
        rb.AddForce(Vector2.up*forceJump,ForceMode2D.Impulse);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,Vector2.down*0.7f);
    }
}
