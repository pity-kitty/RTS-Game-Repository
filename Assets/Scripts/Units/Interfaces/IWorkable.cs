using Units.Enums;

namespace Units.Interfaces
{
    public interface IWorkable
    {
        public bool StartWork(WorkType work);
        public void EndWork();
    }
}