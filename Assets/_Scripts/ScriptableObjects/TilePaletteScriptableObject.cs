using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class TilePaletteScriptableObject : ScriptableObject
    {
        public List<Tile> tilePalette;
    }
}
