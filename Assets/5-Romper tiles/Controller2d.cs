using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Controller2d : MonoBehaviour
{
    [SerializeField] LayerMask mascaraSuelo;
    [SerializeField] Tilemap tilemap;
    public float speed = 5f;
    private Rigidbody2D rb2d;
    private Vector2 moveAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movent();
        DestroyTileDetection();
    }

    void Movent()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveAmount = new Vector2(horizontal, vertical).normalized * speed;
        rb2d.MovePosition(rb2d.position + moveAmount * Time.fixedDeltaTime);
    }

    void DestroyTileDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1f, mascaraSuelo);
        Debug.DrawRay(transform.position, Vector2.right * 1f, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hit.collider)
            {
                // Tenemos que mover la detección un poco a la derecha porque queremos borrar el bloque
                // de la derecha a partir de la detección, la detección si la dejamos en el punto exacto
                // en el que se cruza el raycast y el tile dará resultados inconsistentes ya que
                // está justo en mitad de dos posiciones del tile. Por lo tanto se le suma 0.1 a la derecha
                Vector3Int positionTile = tilemap.WorldToCell(hit.point + new Vector2(0.1f, 0));
                tilemap.SetTile(positionTile, null);
                Debug.Log($"Borrando tile: {positionTile}");
            }
        }
    }
}
