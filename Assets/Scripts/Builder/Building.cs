using UnityEngine;

namespace Builder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Vector2Int size = Vector2Int.one;
        [SerializeField] private Renderer mainRenderer;

        public Vector2Int Size => size;

        public void SetAvailableColor(bool state)
        {
            mainRenderer.material.color = state ? Color.white : Color.red;
        }

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
    }
}