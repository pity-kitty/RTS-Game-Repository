using System.Collections.Generic;
using System.Linq;
using Resources;
using Units.Enums;
using UnityEngine;

namespace Units
{
    public class UnitControl : MonoBehaviour
    {
        [SerializeField] private List<Unit> units;
        [SerializeField] private LayerMask unitAndGroundLayer;
        [SerializeField] private LayerMask unitLayer;
        [SerializeField] private LayerMask groundAndResourceLayer;
        [SerializeField] private float raycastDistance = 100f;
        [SerializeField] private float unitOffset = 0.5f;
        [SerializeField] private int countOfUnitsInOneLine = 5;

        private Dictionary<string, Unit> selectedUnits = new ();
        private Camera mainCamera;
        private int[] unitsStandPattern;
        private bool isCtrlHold;
        private bool needSelectArea;
        private Vector3 initialPosition;
        private Vector3 initialMousePosition;
        private UnitType currentUnitType;

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
                selectedUnits = new Dictionary<string, Unit>();
                initialPosition = hitInfo.point;
                initialMousePosition = Input.mousePosition;
                needSelectArea = true;
                currentUnitType = UnitType.None;
                return;
            }
            needSelectArea = false;
            switch (isCtrlHold)
            {
                case true when selectedUnits.ContainsKey(unit.Guid):
                    selectedUnits.Remove(unit.Guid);
                    unit.SetHighlight(false);
                    return;
                case true:
                    if (unit.UnitType == currentUnitType) selectedUnits.Add(unit.Guid, unit);
                    else if (selectedUnits.Count == 0)
                    {
                        selectedUnits.Add(unit.Guid, unit);
                        currentUnitType = unit.UnitType;
                    }
                    else return;
                    break;
                default:
                    SetHighLightForAllUnits(false);
                    selectedUnits = new Dictionary<string, Unit>() {{ unit.Guid, unit }};
                    currentUnitType = unit.UnitType;
                    break;
            }
            unit.SetHighlight(true);
        }

        private void SetHighLightForAllUnits(bool state)
        {
            foreach (var selectedUnit in selectedUnits.Values)
                selectedUnit.SetHighlight(state);
        }

        private void MoveUnits()
        {
            if (selectedUnits.Count == 0) return;
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hitInfo, float.MaxValue,
                    groundAndResourceLayer)) return;
            var isResource = hitInfo.transform.gameObject.TryGetComponent<Resource>(out var resource);
            var targetPosition = hitInfo.point;
            var selectedUnitsCount = selectedUnits.Count;
            if (!isResource || currentUnitType != UnitType.Worker) RegularMove(selectedUnitsCount, targetPosition);
            else WorkMove(selectedUnitsCount, targetPosition, resource);
        }

        private void WorkMove(int selectedUnitsCount, Vector3 targetPosition, Resource resource)
        {
            var unitsArray = selectedUnits.Values.ToArray();
            var worker = (Worker)unitsArray[0];
            worker.NeedWork = true;
            worker.MoveToWork(targetPosition, WorkType.None, resource);
            for (int i = 1; i < selectedUnitsCount; i++)
            {
                var patternIndex = i % countOfUnitsInOneLine;
                worker = (Worker)unitsArray[i];
                worker.NeedWork = true;
                if (patternIndex == 0)
                {
                    targetPosition.z -= unitOffset * i / countOfUnitsInOneLine;
                    worker.MoveToWork(targetPosition, WorkType.None, resource);
                }
                else
                {
                    var moveOffset = new Vector3(unitOffset * unitsStandPattern[patternIndex], 0f, 0f);
                    worker.MoveToWork(targetPosition + moveOffset, WorkType.None, resource);
                }
            }
        }

        private void RegularMove(int selectedUnitsCount, Vector3 targetPosition)
        {
            var unitsArray = selectedUnits.Values.ToArray();
            unitsArray[0].Move(targetPosition);
            for (int i = 1; i < selectedUnitsCount; i++)
            {
                var patternIndex = i % countOfUnitsInOneLine;
                if (patternIndex == 0)
                {
                    targetPosition.z -= unitOffset * i / countOfUnitsInOneLine;
                    unitsArray[i].Move(targetPosition);
                }
                else
                {
                    var moveOffset = new Vector3(unitOffset * unitsStandPattern[patternIndex], 0f, 0f);
                    unitsArray[i].Move(targetPosition + moveOffset);
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
            var unitsCount = raycastHits.Length;
            if (unitsCount == 0) return;
            var unit = raycastHits[0].transform.GetComponent<Unit>();
            currentUnitType = unit.UnitType;
            selectedUnits.Add(unit.Guid, unit);
            for (var i = 1; i < unitsCount; i++)
            {
                unit = raycastHits[i].transform.gameObject.GetComponent<Unit>();
                if (unit.UnitType == currentUnitType) selectedUnits.Add(unit.Guid, unit);
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