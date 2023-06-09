using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cuevas : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [Header("Ancho y alto de la generaci�n")]
    [SerializeField] int width;
    [SerializeField] int height;
    [Header("Smoothness")]
    [SerializeField] float smoothness;
    [Header("Cave Gen")]
    [Range(0,1)]
    [SerializeField] float modifier;
    [Header("Cantidad de cueva respecto a tierra")]
    [Range(0, 1)]
    [SerializeField] float cavePercentage;
    [Header("Semilla")]
    [SerializeField] float seed;
    [Header("Referencia al tilemap")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap fondo;
    private int alturaSpawn;

    [Header("Referencia al tilerule")]
    [SerializeField] Tile dirt; // La clase cambia para admitir el tile rule
    [SerializeField] Tile stone;
    [SerializeField] Tile snow;
    [SerializeField] Tile ice;
    [SerializeField] Tile grass;
    [SerializeField] Tile cactus;
    [SerializeField] Tile flower;
    [SerializeField] Tile arbusto;
    [SerializeField] Tile nube;
    [SerializeField] Tile fondoCueva;
    [Header("vegetacion gen")]
    [Range(0, 1)]
    [SerializeField] float vegetacionGen;
    [Header("vegetacion Porcentaje")]
    [Range(0, 1)]
    [SerializeField] float vegetacionPorcentaje;
    [Header("nube gen")]
    [Range(0, 1)]
    [SerializeField] float nubeGen;
    [Header("nube Porcentaje")]
    [Range(0, 1)]
    [SerializeField] float nubePorcentaje;



    private int offset = 0;
    private bool biomaHielo = false;
    private bool biomaBosque = false;
    int ultimaAltura = 0;

    int[,] map;
    // Start is called before the first frame update
    void Start()
    {
        map = new int[width, height];
        map = GenerateArray(width, height, true);
        map = TerrenGeneration(map);
        RenderMap(map, tilemap);
        spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Reset();
            seed = Random.Range(-1000000, 1000000);
            Generation();         
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            tilemap.ClearAllTiles();
            fondo.ClearAllTiles();
        }
    }

    /* Generar un array de todos 0 y tama�o width,height */
    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Si empty es true 0, si es false 1
                map[x, y] = (empty) ? 0 : 1;
            }
        }
        return map;
    }
    
    /* Pinta los tile seg�n el array map con valor en [x,y] 1 , si el valor es 0 no hay tile */
    public void RenderMap(int[,] map, Tilemap tilemap)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), stone);
                }
                else if (map[x, y] == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }
                else if (map[x, y] == 3)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), snow);
                }
                else if (map[x, y] == 4)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), ice);
                }
                else if (map[x, y] == 5)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), grass);
                }
                else if (map[x, y] == 6)
                {
                    fondo.SetTile(new Vector3Int(x, y, 0), cactus);
                }
                else if (map[x, y] == 7)
                {
                    fondo.SetTile(new Vector3Int(x, y, 0), arbusto);
                }
                else if (map[x, y] == 8)
                {
                    fondo.SetTile(new Vector3Int(x, y, 0), flower);
                }
                else if (map[x, y] == 9)
                {
                    fondo.SetTile(new Vector3Int(x, y, 0), nube);
                }
                else if (map[x, y] == 10)
                {
                    fondo.SetTile(new Vector3Int(x, y, 0), fondoCueva);
                }
            }
        }
    }

    /* Generamos la altura con perlin noise del terreno */
    public int[,] TerrenGeneration(int[,] map)
    {
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            if (x < 100)
            {
                smoothness = 250;
            }
            else if (x >= 100 && x < 200)
            {
                smoothness = 50;
            }
            else if (x >= 200)
            {
                smoothness = 150;
            }

            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height);

            generarOffset(perlinHeight, ultimaAltura);

            int altura = perlinHeight + offset;

            if (altura >= height)
            {
                altura = height;
            }

            for (int y = 0; y < altura; y++)
            {
                if (x < 100 && y > altura - 4)
                {
                    map[x, y] = 2;

                }
                else if (x >= 100 && x < 200 && y > altura - 4)
                {
                    map[x, y] = 3;
                }
                else if (x >= 200 && y > altura - 4)
                {
                    map[x, y] = 5;
                }


                //map[x, y] = 1; // solo terreno sin cuevas

                /* En estas 4 l�neas est� toda la clave de la generaci�n:
                    1. Se genera ruido perlin de 2 dimensiones, con los valors x e y,
                    con semilla para permitir variedad
                    2. Nos devolver� un valor entre 0 y 1, por lo que si queremos que la proporci�n
                    entre tierra y cueva se modique, tenemos que definir un umbral (cavePercentage) 
                */
                else if (Mathf.PerlinNoise(x * modifier + seed, y * modifier + seed) > cavePercentage)
                {
                    map[x, y] = 1;

                    if (x >= 100 && x < 200)
                    {
                        map[x, y] = 4;
                    }
                }
                else
                    map[x, y] = 10;
            }


            if (x == width * 0.5)
            {
                alturaSpawn = perlinHeight + offset + 2;
            }

            ultimaAltura = perlinHeight + offset;
            generarVegetacion((int) smoothness, x, perlinHeight);
            generarNubes(x, ultimaAltura, map);
        }
        return map;
    }

    void Generation()
    {
        map = GenerateArray(width, height, true);
        map = TerrenGeneration(map);
        RenderMap(map, tilemap);
    }

    void generarOffset(int perlin, int altura)
    {
        if (smoothness == 50 && biomaHielo == false)
        {
            offset = altura - perlin;
            biomaHielo = true;
            Debug.Log(offset);
        }

        if (smoothness == 150 && biomaBosque == false)
        {
            offset = altura - perlin;
            biomaBosque = true;
            Debug.Log(offset);
        }
    }

    private void generarVegetacion(int zona, int x, int perlin)
    {
        switch (zona)
        {
            case 250:
                if (perlin + offset < height)
                {
                    if (Mathf.PerlinNoise(x / vegetacionGen, seed) < vegetacionPorcentaje)
                        map[x, perlin + offset] = 6;
                }
                break;
            case 50:
                if (perlin + offset < height)
                {
                    if (Mathf.PerlinNoise(x / vegetacionGen, seed) < vegetacionPorcentaje)
                        map[x, perlin + offset] = 7;
                }
                break;
            case 150:
                if (perlin + offset < height)
                {
                    if (Mathf.PerlinNoise(x / vegetacionGen, seed) < vegetacionPorcentaje)
                        map[x, perlin + offset] = 8;
                }
                break;
        }
    }

    private void generarNubes(int x, int perlin, int[,] map)
    {
        for (int y = perlin + 10; y < height ; y++)
        {
            if (Mathf.PerlinNoise(x * nubeGen + seed, y * nubeGen + seed) > nubePorcentaje)
            {
                map[x, y] = 0;

            }
            else
                map[x, y] = 9;
        }
    }

    private void Reset()
    {
        offset = 0;
        ultimaAltura = 0;
        biomaBosque = false;
        biomaHielo = false;
    }

    private void spawn()
    {
        Vector3Int v = new Vector3Int(width / 2, alturaSpawn, 0);
        Instantiate(Player, tilemap.CellToWorld(v), Quaternion.identity);
    }
}
