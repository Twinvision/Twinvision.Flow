namespace Twinvision.Flow
{
    public interface IAttribute
    {
        string Name { get; set; }
        string Value { get; set; }

        string ToString();
        string ToString(bool enforceProperCase);
    }
}