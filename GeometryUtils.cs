using Bricks.Geometry;

namespace Bricks;

public static class GeometryUtils
{
    public static bool DoSegmentsIntersect(Segment segmentA, Segment segmentB)
    {
        var aStart = segmentA.Start;
        var aVector = segmentA.End - segmentA.Start;

        var bStart = segmentB.Start;
        var bVector = segmentB.End - segmentB.Start;

        var startDiff = bStart - aStart;

        int crossAwithB = aVector.Cross(bVector);
        int crossStartDiffWithA = startDiff.Cross(aVector);

        // Segments are parallel
        if (crossAwithB == 0)
        {
            // Not colinear
            if (crossStartDiffWithA != 0)
            {
                return false;
            }

            // Segments are colinear. Check for overlap
            int aDot = aVector.Dot(aVector);
            int t0 = startDiff.Dot(aVector);
            int t1 = t0 + bVector.Dot(aVector);

            return Overlaps(t0, t1, 0, aDot);
        }

        // Segments are not parallel. Check intersection point
        double t = (double)startDiff.Cross(bVector) / crossAwithB;
        double u = (double)startDiff.Cross(aVector) / crossAwithB;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

    public static bool DoSegmentsIntersectWithRectangle(Segment segment, Rectangle rectangle)
    {
        return DoSegmentsIntersect(segment, rectangle.Top) ||
               DoSegmentsIntersect(segment, rectangle.Bottom) ||
               DoSegmentsIntersect(segment, rectangle.Left) ||
               DoSegmentsIntersect(segment, rectangle.Right);
    }

    private static bool Overlaps(int aStart, int aEnd, int bStart, int bEnd)
    {
        return
            Math.Max(
                Math.Min(aStart, aEnd),
                Math.Min(bStart, bEnd))
            <=
            Math.Min(
                Math.Max(aStart, aEnd),
                Math.Max(bStart, bEnd));
    }
}
