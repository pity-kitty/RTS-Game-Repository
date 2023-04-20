using Units.Enums;
using Units.Interfaces;

namespace Units
{
    public class Worker : Unit, IWorkable
    {
        private WorkType currentWork = WorkType.None;

        public bool StartWork(WorkType work)
        {
            return true;
        }

        public void EndWork()
        {
            
        }
    }
}