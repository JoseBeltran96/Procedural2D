using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RuidoPerlin : MonoBehaviour
{
    [Header("Ancho y alto de la generación")]
    [SerializeField] int width;
    [Range(0, 100)]
    [SerializeField] float heightValue, smoothness;
    [Header("Referencias a los tile maps")]
    [SerializeField] Tilemap dirtTilemap;
    [SerializeField] Tilemap grassTilemap;
    [SerializeField] Tilemap stoneTilemap;
    [Header("Cantidad de cueva respecto a tierra")]
    [Range(0, 1)]
    [SerializeField] float cavePercentage;
    [Header("Referencias a los tile")]
    [SerializeField] Tile dirt;
    [SerializeField] Tile grass;
    [SerializeField] Tile stone;
    [Header("Semilla")]
    [SerializeField] float seed;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            seed = Random.Range(-1000000, 1000000);
            Generation();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            dirtTilemap.ClearAllTiles();
            grassTilemap.ClearAllTiles();
            stoneTilemap.ClearAllTiles();
        }
    }


   
    /* Para no repetir valores utilizaremos una semilla */
    void Generation()
    {
        for (int x = 0; x < width; x++)
        {
            // parámetro seed con Perlin noise
            int perlinHeight = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));

            int minStoneSpawnDistance = perlinHeight - 5;
            int maxStoneSpawnDistance = perlinHeight - 6;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);

            for (int y = 0; y < perlinHeight; y++)
                if (y < totalStoneSpawnDistance)
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), stone);
                else
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
            dirtTilemap.SetTile(new Vector3Int(x, perlinHeight, 0), grass);
        }
    }
}
