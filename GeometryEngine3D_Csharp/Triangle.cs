namespace GeometryEngine3D_Csharp
{
    public class Triangle
    {
        public int M1 { get; }
        public int M2 { get; }
        public int M3 { get; }
        public Point Normal { get; }

        public Triangle(int m1, int m2, int m3, Point normal = null)
        {
            M1 = m1; M2 = m2; M3 = m3; Normal = normal ?? new Point();
        }
    }
}
