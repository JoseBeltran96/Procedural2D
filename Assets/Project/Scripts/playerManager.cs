using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // velocidad de movimiento
    [SerializeField] private float jumpForce = 7f; // fuerza de salto
    private Rigidbody2D rb; // componente Rigidbody2D del objeto
    public bool isGrounded = false; // indicador de si el objeto está en contacto con el suelo
    public bool jump = false;
    private Animator animator;
    //Caja debajo del personaje
    public float boxPositionX = 0f;
    public float boxPositionY = 0f;
    public float boxSizeX = 10f;
    public float boxSizeY = 10f;
    public LayerMask Ground;
    //public GameObject menuFinal;
    private bool p_moving;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // obtener el componente Rigidbody2D
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // verificar si el objeto está en contacto con el suelo
        isGrounded = p_groundedCheck();

        if (isGrounded == false)
        {
            animator.SetBool("grounded", false);
        }

        // permitir al jugador saltar si está en el suelo y presiona la tecla de espacio
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            animator.SetBool("grounded", isGrounded);
            jump = true;
        }

    }

    private void FixedUpdate()
    {
        // obtener la entrada del jugador en el eje horizontal
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        // crear un vector de movimiento basado en la entrada del jugador y la velocidad
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);


        if (rb.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            p_moving = true;
        }
        else if (rb.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            p_moving = true;
        }
        else if (rb.velocity.x == 0)
        {
            p_moving = false;
        }

        if (p_moving)
        {
            //Run
            animator.SetInteger("moving", 1);
        }
        else
        {
            //Idle
            animator.SetInteger("moving", 0);
        }

        if (jump)
        {
            if (isGrounded == true)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                jump = false;
            }

        }
    }

    //Comprueba si el personaje esta en el suelo
    private bool p_groundedCheck()
    {
        BoxCast(new Vector2(transform.position.x - boxPositionX, transform.position.y - boxPositionY), new Vector2(boxSizeX, boxSizeY), 0f, new Vector2(0, 0), 0f, Ground);

        bool p_floorCollider;
        p_floorCollider = Physics2D.BoxCast(new Vector2(transform.position.x - boxPositionX, transform.position.y - boxPositionY), new Vector2(boxSizeX, boxSizeY), 0f, new Vector2(0, 0), 0f, Ground);

        animator.SetBool("grounded", true);

        return p_floorCollider;
    }

    /**
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("muerte"))
        {
            Time.timeScale = 0f;
            menuFinal.SetActive(true);
        }
    } */

    static public RaycastHit2D BoxCast(Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);

        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.blue;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;
    }
}
