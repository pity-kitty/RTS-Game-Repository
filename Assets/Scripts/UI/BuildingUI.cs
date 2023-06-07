using Builder;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] private StructureBuilder structureBuilder;
        
        [Header("Buildings buttons")]
        [SerializeField] private Button farmHouseButton;
        [SerializeField] private Button towerButton;
        [SerializeField] private Button houseButton;

        private void Start()
        {
            InitializeSubscriptions();
        }

        private void InitializeSubscriptions()
        {
            farmHouseButton.onClick.AddListener(structureBuilder.BuildFarmHouse);
            towerButton.onClick.AddListener(structureBuilder.BuildTower);
            houseButton.onClick.AddListener(structureBuilder.BuildHouse);
        }

        private void RemoveSubscriptions()
        {
            farmHouseButton.onClick.RemoveListener(structureBuilder.BuildFarmHouse);
            towerButton.onClick.RemoveListener(structureBuilder.BuildTower);
            houseButton.onClick.RemoveListener(structureBuilder.BuildHouse);
        }

        private void OnDestroy()
        {
            RemoveSubscriptions();
        }
    }
}