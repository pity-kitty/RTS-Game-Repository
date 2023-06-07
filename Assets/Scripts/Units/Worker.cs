using System.Collections;
using Resources;
using Units.Enums;
using Units.Interfaces;
using UnityEngine;

namespace Units
{
    public class Worker : Unit, IWorkable
    {
        [SerializeField] private ResourceHandler resourceHandler;

        private WorkType currentWork = WorkType.None;
        private Resource currentResource;
        private Coroutine workRoutine;
        private bool isWorking;

        public bool NeedWork { get; set; }

        public override bool Move(Vector3 position)
        {
            if (isWorking) EndWork();
            return base.Move(position);
        }

        public bool MoveToWork(Vector3 position, WorkType work, Resource resource)
        {
            var moveResult = base.Move(position);
            if (resource.CurrentAmount == 0) return false;
            if (moveResult) StartCoroutine(WaitForMovement(work, resource));
            return moveResult;
        }

        private IEnumerator WaitForMovement(WorkType work, Resource resource)
        {
            var isWaiting = true;
            while (isWaiting)
            {
                yield return null;
                if (!NeedWork) yield break;
                if (navMeshAgent.pathPending) continue;
                if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) continue;
                if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0f) continue;
                isWaiting = false;
            }

            StartWork(work, resource);
        }

        public bool StartWork(WorkType work, Resource resource)
        {
            if (resource.CurrentAmount == 0) return false;
            currentResource = resource;
            currentWork = work;
            workRoutine = StartCoroutine(Work());
            return true;
        }

        public void EndWork()
        {
            if (workRoutine == null) return;
            NeedWork = false;
            StopCoroutine(workRoutine);
            currentResource = null;
            currentWork = WorkType.None;
            isWorking = false;
        }

        private IEnumerator Work()
        {
            isWorking = true;
            while (true)
            {
                yield return new WaitForSeconds(currentResource.TimeToCollect);
                var resourceAmount = currentResource.GetResources();
                resourceHandler.AddResource(currentResource.ResourceType, resourceAmount);
                if (currentResource.CurrentAmount != 0) continue;
                EndWork();
                yield break;
            }
        }
    }
}