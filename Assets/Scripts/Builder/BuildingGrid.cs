using UnityEngine;

namespace Builder
{
    public class BuildingGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float rayDistance = 50f;

        private Building[,] grid;
        private Camera mainCamera;
        private Building placingBuilding;

        private void Start()
        {
            grid = new Building[gridSize.x, gridSize.y];
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (placingBuilding == null) return;
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, rayDistance, groundLayer))
            {
                var x = Mathf.RoundToInt(hitInfo.point.x);
                var z = Mathf.RoundToInt(hitInfo.point.z);
                placingBuilding.transform.position = new Vector3(x, 0, z);
                var canPlace = !(x < 0 || x > gridSize.x - placingBuilding.Size.x 
                                       || z < 0 || z > gridSize.y - placingBuilding.Size.y);
                if (canPlace) canPlace = IsSpaceAvailable(x, z);
                placingBuilding.SetAvailableColor(canPlace);
                if (canPlace && Input.GetMouseButtonDown(0)) 
                    PlaceBuilding(x, z);
            }
        }

        private bool IsSpaceAvailable(int xPosition, int zPosition)
        {
            for (var x = 0; x < placingBuilding.Size.x; x++)
            {
                for (var y = 0; y < placingBuilding.Size.y; y++)
                {
                    if (grid[xPosition + x, zPosition + y] != null) return false;
                }
            }

            return true;
        }

        public void StartPlacingBuilding(Building building)
        {
            if (placingBuilding != null) Destroy(placingBuilding.gameObject);
            placingBuilding = Instantiate(building);
        }

        private void PlaceBuilding(int xPosition, int zPosition)
        {
            for (var x = 0; x < placingBuilding.Size.x; x++)
            {
                for (var y = 0; y < placingBuilding.Size.y; y++)
                {
                    grid[xPosition + x, zPosition + y] = placingBuilding;
                }
            }
            placingBuilding.EnableNavMeshObstacle();
            placingBuilding = null;
        }
    }
}