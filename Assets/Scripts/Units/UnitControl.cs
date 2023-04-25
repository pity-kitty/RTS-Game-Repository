using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitControl : MonoBehaviour
    {
        [SerializeField] private List<Unit> units;
        [SerializeField] private LayerMask unitAndGroundLayer;
        [SerializeField] private LayerMask unitLayer;
        [SerializeField] private float raycastDistance = 100f;
        [SerializeField] private float unitOffset = 0.5f;
        [SerializeField] private int countOfUnitsInOneLine = 5;

        private List<Unit> selectedUnits = new ();
        private Camera mainCamera;
        private int[] unitsStandPattern;
        private bool isCtrlHold;
        private bool needSelectArea;
        private Vector3 initialPosition;
        private Vector3 initialMousePosition;

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
            }
            if (Input.GetMouseButtonDown(1))
            {
                MoveUnits();
            }
            if (needSelectArea && Input.GetMouseButtonUp(0))
            {
                SelectUnitsInArea();
            }
            if (Input.GetKeyDown(KeyCode.LeftControl)) isCtrlHold = true;
            if (Input.GetKeyUp(KeyCode.LeftControl)) isCtrlHold = false;
        }

        private void SelectUnits()
        {
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, raycastDistance,
                    unitAndGroundLayer)) return;
            var isUnit = hitInfo.transform.gameObject.TryGetComponent<Unit>(out var unit);
            if (!isUnit)
            {
                if (selectedUnits.Count != 0) SetHighLightForAllUnits(false);
                selectedUnits = new List<Unit>();
                initialPosition = hitInfo.point;
                initialMousePosition = Input.mousePosition;
                needSelectArea = true;
                return;
            }
            needSelectArea = false;
            switch (isCtrlHold)
            {
                case true when selectedUnits.Contains(unit):
                    selectedUnits.Remove(unit);
                    unit.SetHighlight(false);
                    return;
                case true:
                    selectedUnits.Add(unit);
                    break;
                default:
                    SetHighLightForAllUnits(false);
                    selectedUnits = new List<Unit> { unit };
                    break;
            }
            unit.SetHighlight(true);
        }

        private void SetHighLightForAllUnits(bool state)
        {
            foreach (var selectedUnit in selectedUnits)
                selectedUnit.SetHighlight(state);
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

        private void SelectUnitsInArea()
        {
            needSelectArea = false;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo)) return;
            var endPosition = hitInfo.point;
            var scale = initialPosition - endPosition;
            scale.x = Mathf.Abs(scale.x);
            scale.y = Mathf.Abs(scale.y);
            scale.z = Mathf.Abs(scale.z);
            var center = (initialPosition + endPosition) / 2;
            var raycastHits = Physics.BoxCastAll(center, scale / 2, Vector3.up, Quaternion.identity,
                float.MaxValue, unitLayer);
            foreach (var hitResult in raycastHits)
            {
                var unit = hitResult.transform.gameObject.GetComponent<Unit>();
                selectedUnits.Add(unit);
            }
            SetHighLightForAllUnits(true);
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

        private void OnGUI()
        {
            if (!needSelectArea) return;
            var rect = Utils.GetScreenRect(initialMousePosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}