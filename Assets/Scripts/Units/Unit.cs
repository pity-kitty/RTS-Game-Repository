using Units.Enums;
using Units.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public abstract class Unit : MonoBehaviour, IMovable, ISelectable
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] private UnitType unitType;
        [SerializeField] private float health;
        [SerializeField] private float speed;
        [SerializeField] private Renderer mainRenderer;

        private string guid = System.Guid.NewGuid().ToString();
        private Color defaultColor;

        public string Guid => guid;
        public UnitType UnitType => unitType;
        
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