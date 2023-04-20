using Units.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public abstract class Unit : MonoBehaviour, IMovable
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float health;
        [SerializeField] private float speed;

        protected virtual void Start()
        {
            navMeshAgent.speed = speed;
        }

        public virtual bool Move(Vector3 position)
        {
            return navMeshAgent.SetDestination(position);
        }
    }
}