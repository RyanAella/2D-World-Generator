using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.ScriptableObjects.TilePalettes
{
    /**
     * ScriptableObject to serialize a list of tiles.
     */
    [CreateAssetMenu]
    [Serializable]
    public class TilePaletteScriptableObject : ScriptableObject
    {
        public List<Tile> tiles;
    }
}
