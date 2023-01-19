using UnityEngine;

namespace _Scripts.old
{
    /**
     * This class generates tiles on the x- and y-axis.
     */
    public class ProceduralGeneration : MonoBehaviour
    {
        [SerializeField] private Vector2Int resolution = new Vector2Int(256, 144);
        [SerializeField] private GameObject dirt;

        void Start()
        {
            Generation();
        }

        // The generation of the tiles
        private void Generation()
        {
            for (int x = 0; x < resolution.x; x++) // This will help to spawn a tile on the xAxis
            {
                for (int y = 0; y < resolution.y; y++) // This will help to spawn a tile on the yAxis
                {
                    Instantiate(dirt, new Vector2(x, y), Quaternion.identity);
                }
            }
        }
    }
}