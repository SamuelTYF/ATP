using System;
namespace ATP
{
    public interface ITerm:IComparable<ITerm>
    {
        int Deep { get; }
    }
}
