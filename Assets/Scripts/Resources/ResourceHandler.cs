using Resources.Enums;
using UnityEngine;

namespace Resources
{
    public class ResourceHandler : MonoBehaviour
    {
        [SerializeField] private int food;
        [SerializeField] private int wood;
        [SerializeField] private int stone;

        public void AddResource(ResourceType resource, int amount)
        {
            switch (resource)
            {
                case ResourceType.Food:
                    food += amount;
                    break;
                case ResourceType.Wood:
                    wood += amount;
                    break;
                case ResourceType.Stone:
                    stone += amount;
                    break;
            }
        }

        public void RemoveResource(ResourceType resource, int amount)
        {
            AddResource(resource, -amount);
        }
    }
}