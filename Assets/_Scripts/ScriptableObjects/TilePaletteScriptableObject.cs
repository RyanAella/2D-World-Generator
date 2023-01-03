using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    [Serializable]
    public class TilePaletteScriptableObject : ScriptableObject
    {
        public List<Tile> tiles;
    }
}
