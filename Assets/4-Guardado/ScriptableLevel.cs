using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ScriptableLevel : ScriptableObject {
    public int levelIndex; // Número que identifica al nivel
    // Listas para guardar tilemaps diferentes
    public List<SavedTile> tilemap1;
    public List<SavedTile> tilemap2;
}

/*
 * Se guarda un número que identifique al tile y posición
 * Para una mejor implementación usar enum: https://www.youtube.com/watch?v=TeEWLC-QKjw
 */
[Serializable]
public class SavedTile {
    public Vector3Int Position;
    public int Tile;
}