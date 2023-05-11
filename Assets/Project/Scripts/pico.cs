using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class pico : MonoBehaviour
{
    private Vector3 objetivo;

    private Camera camara;

    [SerializeField] LayerMask mascaraSuelo;
    private Tilemap tilemap;
    private Tilemap tilemapFondo;
    [SerializeField] private TileBase tileFondo;
    [SerializeField] GameObject puntaPico;
    [SerializeField] GameObject taladro;

    // Start is called before the first frame update
    void Start()
    {
        camara = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tilemap = GameObject.FindGameObjectWithTag("Mapa").GetComponent<Tilemap>();
        tilemapFondo = GameObject.FindGameObjectWithTag("Fondo").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        objetivo = camara.ScreenToWorldPoint(Input.mousePosition);
        float anguloRad = Mathf.Atan2(objetivo.y - transform.position.y, objetivo.x - transform.position.x);
        float anguloGrad = (180 / Mathf.PI) * anguloRad - 135;
        transform.rotation = Quaternion.Euler(0, 0, anguloGrad);
        DestroyTileDetection();
    }

    void DestroyTileDetection()
    {


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3Int positionTile = tilemap.WorldToCell(puntaPico.transform.position);
            TileBase til = tilemap.GetTile(positionTile);
            if (til != null)
            {
                tilemap.SetTile(positionTile, null);
                tilemapFondo.SetTile(positionTile, tileFondo);
                Debug.Log($"Borrando tile: {positionTile}");
            }
        }
    }
}
