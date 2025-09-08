using Bricks.Geometry;

namespace Bricks;

public static class GeometryUtils
{
    public static bool DoSegmentsIntersect(Segment segmentA, Segment segmentB, double errorTolerance)
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

            return Overlaps(t0, t1, 0, aDot, errorTolerance);
        }

        // Segments are not parallel. Check intersection point
        double t = (double)startDiff.Cross(bVector) / crossAwithB;
        double u = (double)startDiff.Cross(aVector) / crossAwithB;

        return t >= -errorTolerance && t <= 1 + errorTolerance &&
               u >= -errorTolerance && u <= 1 + errorTolerance;
    }

    public static bool DoSegmentsIntersectWithRectangle(Segment segment, Rectangle rectangle, double errorTolerance)
    {
        return DoSegmentsIntersect(segment, rectangle.Top,    errorTolerance) ||
               DoSegmentsIntersect(segment, rectangle.Bottom, errorTolerance) ||
               DoSegmentsIntersect(segment, rectangle.Left,   errorTolerance) ||
               DoSegmentsIntersect(segment, rectangle.Right,  errorTolerance);
    }

    private static bool Overlaps(int aStart, int aEnd, int bStart, int bEnd, double errorTolerance)
    {
        return
            Math.Max(
                Math.Min(aStart, aEnd),
                Math.Min(bStart, bEnd)) - errorTolerance
            <=
            Math.Min(
                Math.Max(aStart, aEnd),
                Math.Max(bStart, bEnd)) + errorTolerance;
    }
}
