using Resources;
using Units.Enums;

namespace Units.Interfaces
{
    public interface IWorkable
    {
        public bool StartWork(Resource resource);
        public void EndWork();
    }
}