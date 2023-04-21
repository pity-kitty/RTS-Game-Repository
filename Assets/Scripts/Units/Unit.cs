using Units.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public abstract class Unit : MonoBehaviour, IMovable, ISelectable
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float health;
        [SerializeField] private float speed;
        [SerializeField] private Renderer mainRenderer;

        private Color defaultColor;
        
        protected virtual void Start()
        {
            navMeshAgent.speed = speed;
            defaultColor = mainRenderer.material.color;
        }

        public virtual bool Move(Vector3 position)
        {
            return navMeshAgent.SetDestination(position);
        }

        public virtual void SetHighlight(bool state)
        {
            mainRenderer.material.color = state ? Color.yellow : defaultColor;
        }
    }
}