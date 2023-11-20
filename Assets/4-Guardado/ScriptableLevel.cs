using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ScriptableLevel : ScriptableObject {
    public int levelIndex; // N�mero que identifica al nivel
    // Listas para guardar tilemaps diferentes
    public List<SavedTile> tilemap1;
    public List<SavedTile> tilemap2;
}

/*
 * Se guarda un n�mero que identifique al tile y posici�n
 * Para una mejor implementaci�n usar enum: https://www.youtube.com/watch?v=TeEWLC-QKjw
 */
[Serializable]
public class SavedTile {
    public Vector3Int Position;
    public int Tile;
}