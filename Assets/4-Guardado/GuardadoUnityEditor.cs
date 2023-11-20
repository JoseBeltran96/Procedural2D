using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

// Video tutorial: https://www.youtube.com/watch?v=TeEWLC-QKjw

public class GuardadoUnityEditor : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap1, tilemap2;
    [SerializeField] private Tile tile0, tile1;
    [SerializeField] private int levelIndex;

    public void SaveMap()
    {
        // Instancia de los objetos a guardar
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.levelIndex = levelIndex; // Se guarda el nivel

        // GetTilesFromMap() declarado más abajo como IEnumerable<SavedTile> GetTilesFromMap()
        newLevel.tilemap1 = GetTilesFromMap(tilemap1).ToList();
        newLevel.tilemap2 = GetTilesFromMap(tilemap2).ToList();

        // Guardado en assets
        ScriptableObjectUtility.SaveLevelFile(newLevel);

        // Devuele un IEnumerable de todos los tiles de un tilemap
        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    //var levelTile = map.GetTile<LevelTile>(pos);
                    yield return new SavedTile()
                    {
                        // vamos devolviendo posiciones y tipo de tile, en este caso se ha 
                        // identificado con un número (0). Podrían identificarse varios tipos
                        // de tile en el mismo tilemap mediante números diferentes
                        Position = pos,
                        Tile = 0
                    };
                }
            }
        }
    }

    public void LoadMap()
    {
        // Escojemos el nivel indicado en el entorno (no olvida cambiar)
        var level = Resources.Load<ScriptableLevel>($"Levels/Level{levelIndex}");
        if (level == null)
        {
            Debug.LogError($"Assets/4-Guardado/Level{levelIndex} does not exist.");
            return;
        }

        ClearMap();

        // Tendremos un pequeño error al cargar, ya que el tilemap de tierra usa un ruletile  
        // y no un tile normal, ¿cómo se podriá arreglar? Pista: TileBase y cambiar código
        foreach (var savedTile in level.tilemap1)
        {
            switch (savedTile.Tile)
            {
                case 0:
                    tilemap1.SetTile(savedTile.Position, tile0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var savedTile in level.tilemap2)
        {
            switch (savedTile.Tile)
            {
                case 0:
                    tilemap2.SetTile(savedTile.Position, tile1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /* Borrado de todos los tilemap de la escena */
    public void ClearMap()
    {
        var maps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in maps)
        {
            tilemap.ClearAllTiles();
        }
    }

    // Condicional compilation https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
#if UNITY_EDITOR
    // Esta forma de guardar nos servirá si nosotros como desarrolladores queremos generar mapas,  
    // pero no que sea el jugador el que los genere
    public static class ScriptableObjectUtility
    {
        public static void SaveLevelFile(ScriptableLevel level)
        {
            AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/Level{level.levelIndex}.asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
#endif

}
