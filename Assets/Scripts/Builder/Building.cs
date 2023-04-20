using UnityEngine;
using UnityEngine.AI;

namespace Builder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Vector2Int size = Vector2Int.one;
        [SerializeField] private Renderer mainRenderer;
        [SerializeField] private NavMeshObstacle navMeshObstacle;

        public Vector2Int Size => size;

        private void OnDrawGizmos()
        {
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    Gizmos.color = (x + y) % 2 == 0 ? new Color(1f, 0f, 1f) : Color.black;
                    Gizmos.DrawCube(transform.position + new Vector3(x + 0.5f, 0, y + 0.5f), 
                        new Vector3(1, 0.1f, 1));
                }
            }
        }

        public void SetAvailableColor(bool state)
        {
            mainRenderer.material.color = state ? Color.white : Color.red;
        }

        public void EnableNavMeshObstacle()
        {
            navMeshObstacle.enabled = true;
        }
    }
}