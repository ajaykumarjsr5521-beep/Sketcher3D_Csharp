using System.Collections.Generic;

namespace GeometryEngine3D_Csharp
{
    /// <summary>
    /// Provides common 3D transformation utilities such as
    /// translation, scaling, and rotation.
    /// Works on vertex data using homogeneous coordinates.
    /// </summary>
    public static class Transformations
    {
        /// <summary>
        /// Calculates the centroid (pivot point) of a list of vertices.
        /// Used as rotation and scaling center.
        /// </summary>
        public static Point CalculatePivot(IReadOnlyList<Point> vertices)
        {
            // Return origin if input is invalid
            if (vertices == null || vertices.Count == 0)
                return new Point(0, 0, 0);

            double cx = 0, cy = 0, cz = 0;

            // Sum all coordinates
            for (int i = 0; i < vertices.Count; i++)
            {
                cx += vertices[i].X;
                cy += vertices[i].Y;
                cz += vertices[i].Z;
            }

            // Average to get centroid
            double n = vertices.Count;
            return new Point(cx / n, cy / n, cz / n);
        }

        /// <summary>
        /// Applies a transformation matrix to a list of points.
        /// Uses homogeneous coordinates (4x1 vector).
        /// </summary>
        public static List<Point> ApplyTransform(List<Point> vertices, Matrix matrix)
        {
            List<Point> outPts = new List<Point>(vertices.Count);

            for (int i = 0; i < vertices.Count; i++)
            {
                Point p = vertices[i];

                // Convert point to homogeneous vector
                Matrix v = new Matrix(4, 1);
                v[0, 0] = p.X;
                v[1, 0] = p.Y;
                v[2, 0] = p.Z;
                v[3, 0] = 1.0;

                // Apply transformation
                Matrix r = matrix * v;

                // Convert back to Point
                outPts.Add(new Point(r[0, 0], r[1, 0], r[2, 0]));
            }

            return outPts;
        }

        /// <summary>
        /// Translates vertices by given offsets.
        /// Appends transformed coordinates to the same float list.
        /// </summary>
        public static List<float> Translate(
            List<float> vec,
            double tx = 0,
            double ty = 0,
            double tz = 0)
        {
            List<Point> verts = FloatsToPoints(vec);

            Matrix m = Matrix.GetTranslationMatrix(tx, ty, tz);

            List<Point> t = ApplyTransform(verts, m);

            AppendPoints(vec, t);
            return vec;
        }

        /// <summary>
        /// Scales vertices around their centroid.
        /// </summary>
        public static List<float> Scale(
            List<float> vec,
            double sx = 1,
            double sy = 1,
            double sz = 1)
        {
            List<Point> verts = FloatsToPoints(vec);

            // Compute pivot for uniform scaling
            Point pivot = CalculatePivot(verts);

            // Translate → Scale → Translate back
            Matrix m =
                Matrix.GetTranslationMatrix(pivot.X, pivot.Y, pivot.Z) *
                Matrix.GetScalingMatrix(sx, sy, sz) *
                Matrix.GetTranslationMatrix(-pivot.X, -pivot.Y, -pivot.Z);

            List<Point> t = ApplyTransform(verts, m);

            AppendPoints(vec, t);
            return vec;
        }

        /// <summary>
        /// Rotates vertices around X-axis using centroid as pivot.
        /// </summary>
        public static List<float> RotationX(List<float> vec, double degX = 0)
        {
            List<Point> verts = FloatsToPoints(vec);
            Point pivot = CalculatePivot(verts);

            Matrix m =
                Matrix.GetTranslationMatrix(pivot.X, pivot.Y, pivot.Z) *
                Matrix.GetRotationXMatrix(degX) *
                Matrix.GetTranslationMatrix(-pivot.X, -pivot.Y, -pivot.Z);

            List<Point> t = ApplyTransform(verts, m);

            AppendPoints(vec, t);
            return vec;
        }

        /// <summary>
        /// Rotates vertices around Y-axis using centroid as pivot.
        /// </summary>
        public static List<float> RotationY(List<float> vec, double degY = 0)
        {
            List<Point> verts = FloatsToPoints(vec);
            Point pivot = CalculatePivot(verts);

            Matrix m =
                Matrix.GetTranslationMatrix(pivot.X, pivot.Y, pivot.Z) *
                Matrix.GetRotationYMatrix(degY) *
                Matrix.GetTranslationMatrix(-pivot.X, -pivot.Y, -pivot.Z);

            List<Point> t = ApplyTransform(verts, m);

            AppendPoints(vec, t);
            return vec;
        }

        /// <summary>
        /// Rotates vertices around Z-axis using centroid as pivot.
        /// </summary>
        public static List<float> RotationZ(List<float> vec, double degZ = 0)
        {
            List<Point> verts = FloatsToPoints(vec);
            Point pivot = CalculatePivot(verts);

            Matrix m =
                Matrix.GetTranslationMatrix(pivot.X, pivot.Y, pivot.Z) *
                Matrix.GetRotationZMatrix(degZ) *
                Matrix.GetTranslationMatrix(-pivot.X, -pivot.Y, -pivot.Z);

            List<Point> t = ApplyTransform(verts, m);

            AppendPoints(vec, t);
            return vec;
        }

        /// <summary>
        /// Converts a flat float list (x,y,z) into Point objects.
        /// </summary>
        private static List<Point> FloatsToPoints(List<float> vec)
        {
            List<Point> pts = new List<Point>(vec.Count / 3);

            for (int i = 0; i + 2 < vec.Count; i += 3)
                pts.Add(new Point(vec[i], vec[i + 1], vec[i + 2]));

            return pts;
        }

        /// <summary>
        /// Appends transformed Point coordinates back into a float list.
        /// </summary>
        private static void AppendPoints(List<float> vec, List<Point> pts)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                vec.Add((float)pts[i].X);
                vec.Add((float)pts[i].Y);
                vec.Add((float)pts[i].Z);
            }
        }
    }
}
