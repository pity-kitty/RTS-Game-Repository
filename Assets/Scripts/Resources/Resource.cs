using System.Collections;
using Extensions;
using Resources.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private ResourceData resourceData;
        [SerializeField] private int overallAmount;
        [SerializeField] private float timeToRecoverInMinutes;

        [Header("Visual resource states")]
        [SerializeField] private GameObject normalState;
        [SerializeField] private GameObject emptyState;

        private float timeToRecoverInSeconds;

        public ResourceType ResourceType => resourceData.resourceType;
        public float TimeToCollect => resourceData.timeToCollect;
        public int AmountGained => resourceData.amountGained;
        public int OverallAmount => overallAmount;
        public float TimeToRecoverInMinutes => timeToRecoverInMinutes;
        public int CurrentAmount { get; private set; }

        private void Start()
        {
            CurrentAmount = overallAmount;
            timeToRecoverInSeconds = timeToRecoverInMinutes.MinutesToSeconds();
        }

        public int GetResources()
        {
            var collectedResources = resourceData.amountGained;
            CurrentAmount -= collectedResources;
            if (CurrentAmount >= 0)
                return resourceData.amountGained;
            collectedResources = CurrentAmount;
            CurrentAmount = 0;
            return resourceData.amountGained + collectedResources;
        }

        public void SetVisualState(bool isNormal)
        {
            if (isNormal)
            {
                normalState.SetActive(true);
                emptyState.SetActive(false);
                return;
            }
            
            emptyState.SetActive(true);
            normalState.SetActive(false);
        }

        public void StartRecover()
        {
            StartCoroutine(Recover());
        }

        private IEnumerator Recover()
        {
            yield return new WaitForSeconds(timeToRecoverInSeconds);
            CurrentAmount = overallAmount;
        }
    }
}