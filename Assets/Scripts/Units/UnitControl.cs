using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitControl : MonoBehaviour
    {
        [SerializeField] private List<Unit> units;
        [SerializeField] private LayerMask unitLayer;
        [SerializeField] private float raycastDistance = 100f;
        [SerializeField] private float unitOffset = 0.5f;
        [SerializeField] private int countOfUnitsInOneLine = 5;

        private List<Unit> selectedUnits = new ();
        private Camera mainCamera;
        private int[] unitsStandPattern;

        private void Start()
        {
            mainCamera = Camera.main;
            unitsStandPattern = GenerateStandPattern(countOfUnitsInOneLine);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectUnits();
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                MoveUnits();
            }
        }

        private void SelectUnits()
        {
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, raycastDistance,
                    unitLayer)) return;
            var unit = hitInfo.transform.gameObject.GetComponent<Unit>();
            if (selectedUnits.Contains(unit))
            {
                selectedUnits.Remove(unit);
                unit.SetHighlight(false);
                return;
            }
            selectedUnits.Add(unit);
            unit.SetHighlight(true);
        }

        private void MoveUnits()
        {
            if (selectedUnits.Count == 0) return;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo)) return;
            var targetPosition = hitInfo.point;
            selectedUnits[0].Move(targetPosition);
            var selectedUnitsCount = selectedUnits.Count;
            for (int i = 1; i < selectedUnitsCount; i++)
            {
                var patternIndex = i % countOfUnitsInOneLine;
                if (patternIndex == 0)
                {
                    targetPosition.z -= unitOffset * i / countOfUnitsInOneLine;
                    selectedUnits[i].Move(targetPosition);
                }
                else
                {
                    var moveOffset = new Vector3(unitOffset * unitsStandPattern[patternIndex], 0f, 0f);
                    selectedUnits[i].Move(targetPosition + moveOffset);
                }
            }
        }

        private int[] GenerateStandPattern(int unitsCount)
        {
            var standPattern = new int[unitsCount];
            var patternValue = 1;
            for (int i = 1; i < unitsCount; i += 2)
            {
                standPattern[i] = patternValue;
                standPattern[i + 1] = -patternValue;
                patternValue++;
            }

            return standPattern;
        }
    }
}