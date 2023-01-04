using UnityEngine;

namespace _Scripts.Helper
{
    /*
     * This class displays the map.
     */
    public class MapDisplay
    {
        private GameObject _root;
        private static Texture2D _texture;
        private static Rect _rect;
        private static SpriteRenderer _renderer;

        // Constructor
        public MapDisplay(Vector3 position, Vector2Int resolution, GameObject root)
        {
            // Set properties
            _root = root;

            // Create texture and rect for Sprite
            _texture = new Texture2D(resolution.x, resolution.y, TextureFormat.RGBA32, -1, true);
            _texture.wrapMode = TextureWrapMode.Clamp;
            
            // FilterMode.Point to get checkerboard pattern
            _texture.filterMode = FilterMode.Point;
            _rect = new Rect(position.x, position.y, resolution.x, resolution.y);

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    _texture.SetPixel(x, y, Color.magenta);
                }
            }

            // Apply color changes
            _texture.Apply();

            // Create Sprite
            var sprite = Sprite.Create(_texture, _rect, Vector2.up);

            // Add Sprite to SpriteRenderer on root
            _renderer = _root.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            if (_renderer == null)
            {
                Debug.LogError("SpriteRenderer is missing.");
                return;
            }

            _renderer.sprite = sprite;
            _renderer.enabled = true;
        }

        /*
         * Update the map
         */
        public void UpdateMapDisplay(int[,] valueMap)
        {
            int mapWidth = valueMap.GetLength(0);
            int mapHeight = valueMap.GetLength(1);

            // Check valueMap and Sprite dimensions
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Color color = valueMap[x, y] == 1 ? Color.black : Color.white;
                    _texture.SetPixel(x, y, color);
                }
            }

            _renderer.sprite.texture.Apply();
        }
    }
}