using Common.Interfaces;

namespace Common.Components
{
    public struct CullingComponent: IComponent
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
    }
}
