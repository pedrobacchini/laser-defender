public class Boundary
{
    public float XMin { get; }
    public float XMax { get; }
    public float YMin { get; }
    public float YMax { get; }

    public Boundary(float xMin, float xMax, float yMin, float yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }
}