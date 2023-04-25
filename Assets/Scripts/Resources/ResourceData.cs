using Resources.Enums;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceData : ScriptableObject
    {
        public ResourceType resourceType;
        public float timeToCollect;
        public int amountGained;
    }
}