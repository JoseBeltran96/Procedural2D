using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AleatorioScript1 : MonoBehaviour
{
    [Header("Ancho y alto de la generación")]
    [SerializeField] int width;
    [SerializeField] int height;
    [Header("Referencias a los tile maps")]
    [SerializeField] Tilemap dirtTilemap;
    [SerializeField] Tilemap grassTilemap;
    [SerializeField] Tilemap stoneTilemap;
    [Header("Referencias a los tile")]
    [SerializeField] Tile dirt;
    [SerializeField] Tile grass;
    [SerializeField] Tile stone;
    // Start is called before the first frame update
    void Start()
    {
        //Generation1();
        //Generation2();
        Generation3();
        //Generation4();
        //Generation5();
    }

    /* Ejemplo de inserción de una fila en el tile map */
    void Generation1()
    {
        for (int x = 0; x < width; x++)
        {
            // Necesario un Vector3 de enteros con la posición y el tile a insertar
            dirtTilemap.SetTile(new Vector3Int(x, 0, 0), dirt);
        }
    }

    /* Lo mismo que el anterior pero un cuadrado, centrándolo y sudando grass */
    void Generation2()
    {
        // offsets para cambiar el punto de inicio
        int offsetX = -10;
        int offsetY = -5;

        for (int x = 0 + offsetX; x < width + offsetX; x++)
        {
            for (int y = 0 + offsetY; y < height + offsetY; y++)
            {
                dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
            }
            dirtTilemap.SetTile(new Vector3Int(x, 0, 0), grass); // uno en cada columna
        }
    }
    
    /* Gradualmente cambiando el min y max de la altura y generando un valor aleatorio en el rango */
    void Generation3()
    {
        for (int x = 0; x < width; x++)
        {
            // Rango permitido de la altura -1 y +2 de la altura inicial, que irá cambiando 
            int minHeight = height - 1;
            int maxHeight = height + 2;

            // Aleatorio entre el rango [altura actual -1, altura actual + 2]
            // y cambiamos el valor de la altura actual <----- importante
            height = Random.Range(minHeight, maxHeight);

            for (int y = 0; y < height; y++)
                dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);

            dirtTilemap.SetTile(new Vector3Int(x, height, 0), grass);
        }
    }

    /* Igual que el anterior pero generando stones */
    void Generation4()
    {
        for (int x = 0; x < width; x++)
        {
            int minHeight = height - 1;
            int maxHeight = height + 2;

            // Las piedras aparecerán entre el nivel -5 y -6 de la altura
            int minStoneSpawnDistance = height - 5;
            int maxStoneSpawnDistance = height - 6;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            height = Random.Range(minHeight, maxHeight);

            for (int y = 0; y < height; y++)
                // si la distancia es la de este momento de stone, spawn stone
                if (y < totalStoneSpawnDistance)
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), stone);
                else
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
            dirtTilemap.SetTile(new Vector3Int(x, height, 0), grass);
        }
    }
    /* Compara uno totalmente aleatorio */
    void Generation5()
    {
        for (int x = 0; x < width; x++)
        {
            // ya no se modifica la altura el cada columna
            int h = Random.Range(1, height);

            for (int y = 0; y < h; y++)
                dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
            dirtTilemap.SetTile(new Vector3Int(x, h, 0), grass);
        }
    }
}
