using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitControl : MonoBehaviour
    {
        [SerializeField] private List<Unit> units;

        private List<Unit> selectedUnits = new ();
        private Camera mainCamera;

        private void Start()
        {
            selectedUnits = units; //Temp
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var unit in selectedUnits)
                {
                    if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
                    {
                        unit.Move(hitInfo.point);
                    }
                }
            }
        }
    }
}