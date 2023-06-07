using UnityEngine;

namespace Builder
{
    public class StructureBuilder : MonoBehaviour
    {
        [SerializeField] private BuildingGrid buildingGrid;

        [Header("Buildings prefabs")]
        [SerializeField] private Building farmHousePrefab;
        [SerializeField] private Building towerPrefab;
        [SerializeField] private Building housePrefab;

        public void BuildFarmHouse()
        {
            buildingGrid.StartPlacingBuilding(farmHousePrefab);
        }
        
        public void BuildTower()
        {
            buildingGrid.StartPlacingBuilding(towerPrefab);
        }
        
        public void BuildHouse()
        {
            buildingGrid.StartPlacingBuilding(housePrefab);
        }
    }
}