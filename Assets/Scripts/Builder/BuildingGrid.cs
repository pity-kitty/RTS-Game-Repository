using UnityEngine;

namespace Builder
{
    public class BuildingGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float rayDistance = 20f;

        private Camera mainCamera;
        private Building placingBuilding;

        private void Start()
        {
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
                //TODO: Move to some player control script
                if (Input.GetMouseButtonDown(0)) 
                    PlaceBuilding();
            }
        }

        public void StartPlacingBuilding(Building building)
        {
            if (placingBuilding != null) Destroy(placingBuilding.gameObject);
            placingBuilding = Instantiate(building);
        }

        public void PlaceBuilding()
        {
            placingBuilding = null;
        }
    }
}