using Units.Enums;

namespace Units.Interfaces
{
    public interface ISelectable
    {
        public UnitType UnitType { get; }
        public void SetHighlight(bool state);
    }
}