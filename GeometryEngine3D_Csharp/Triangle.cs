namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Represents a single triangle in an indexed mesh.
    /// Stores indices to vertex list and an optional normal vector.
    /// </summary>
    public class Triangle
    {
        // Index of first vertex in triangulation point list
        public int M1 { get; }

        // Index of second vertex in triangulation point list
        public int M2 { get; }

        // Index of third vertex in triangulation point list
        public int M3 { get; }

        // Normal vector of the triangle (optional)
        public Point Normal { get; }

        /// <summary>
        /// Creates a triangle using indices into a vertex list.
        /// Normal is optional and defaults to (0,0,0).
        /// </summary>
        public Triangle(int m1, int m2, int m3, Point normal = null)
        {
            M1 = m1;
            M2 = m2;
            M3 = m3;

            // Assign provided normal or default to zero vector
            Normal = normal ?? new Point();
        }
    }
}
