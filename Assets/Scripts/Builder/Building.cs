using UnityEngine;

namespace Builder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize = Vector2Int.one;

        private void OnDrawGizmosSelected()
        {
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    Gizmos.color = (x + y) % 2 == 0 ? new Color(1f, 0f, 1f) : Color.black;
                    Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
                }
            }
        }
    }
}