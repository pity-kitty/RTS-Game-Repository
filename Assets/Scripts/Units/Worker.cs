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
            StopCoroutine(workRoutine);
            currentResource = null;
            currentWork = WorkType.None;
        }

        private IEnumerator Work()
        {
            while (true)
            {
                yield return new WaitForSeconds(currentResource.TimeToCollect);
                var resourceAmount = currentResource.GetResources();
                resourceHandler.AddResource(currentResource.ResourceType, resourceAmount);
            }
        }
    }
}