using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace ReactNativeSvg.svg
{

    enum RNSVGMarkerType
    {
        kStartMarker,
        kMidMarker,
        kEndMarker
    }

    enum ElementType
    {
        kCGPathElementAddCurveToPoint,
        kCGPathElementAddQuadCurveToPoint,
        kCGPathElementMoveToPoint,
        kCGPathElementAddLineToPoint,
        kCGPathElementCloseSubpath
    }

    class SegmentData
    {
        public Point start_tangent;  // Tangent in the start point of the segment.
        public Point end_tangent;    // Tangent in the end point of the segment.
        public Point position;      // The end point of the segment.
    }

    class RNSVGMarkerPosition
    {

        static private List<RNSVGMarkerPosition> positions_;
        static private int element_index_;
        static private Point origin_;
        static private Point subpath_start_;
        static private Point in_slope_;
        static private Point out_slope_;

        static private bool auto_start_reverse_; // TODO

        public RNSVGMarkerType type;
        public Point origin;
        public double angle;

        private RNSVGMarkerPosition(RNSVGMarkerType type, Point origin, double angle)
        {
            this.type = type;
            this.origin = origin;
            this.angle = angle;
        }

        static List<RNSVGMarkerPosition> fromPath(List<PathElement> elements)
        {
            positions_ = new List<RNSVGMarkerPosition>();
            element_index_ = 0;
            origin_ = new Point(0, 0);
            subpath_start_ = new Point(0, 0);
            foreach (PathElement e in elements)
            {
                UpdateFromPathElement(e);
            }
            PathIsDone();
            return positions_;
        }

        private static void PathIsDone()
        {
            double angle = CurrentAngle(RNSVGMarkerType.kEndMarker);
            positions_.Add(new RNSVGMarkerPosition(RNSVGMarkerType.kEndMarker, origin_, angle));
        }

        private static double BisectingAngle(double in_angle, double out_angle)
        {
            // WK193015: Prevent bugs due to angles being non-continuous.
            if (Math.Abs(in_angle - out_angle) > 180)
                in_angle += 360;
            return (in_angle + out_angle) / 2;
        }

        private static double rad2deg(double rad)
        {
            double RNSVG_radToDeg = 180 / Math.PI;
            return rad * RNSVG_radToDeg;
        }

        private static double SlopeAngleRadians(Point p)
        {
            return Math.Atan2(p.Y, p.X);
        }

        private static double CurrentAngle(RNSVGMarkerType type)
        {
            // For details of this calculation, see:
            // http://www.w3.org/TR/SVG/single-page.html#painting-MarkerElement
            double in_angle = rad2deg(SlopeAngleRadians(in_slope_));
            double out_angle = rad2deg(SlopeAngleRadians(out_slope_));
            switch (type)
            {
                case RNSVGMarkerType.kStartMarker:
                    if (auto_start_reverse_)
                        out_angle += 180;
                    return out_angle;
                case RNSVGMarkerType.kMidMarker:
                    return BisectingAngle(in_angle, out_angle);
                case RNSVGMarkerType.kEndMarker:
                    return in_angle;
            }
            return 0;
        }

        private static Point subtract(Point p1, Point p2)
        {
            return new Point(p2.X - p1.X, p2.Y - p1.Y);
        }

        private static bool isZero(Point p)
        {
            return p.X == 0 && p.Y == 0;
        }

        private static void ComputeQuadTangents(SegmentData data,
                                                Point start,
                                                Point control,
                                                Point end)
        {
            data.start_tangent = subtract(control, start);
            data.end_tangent = subtract(end, control);
            if (isZero(data.start_tangent))
                data.start_tangent = data.end_tangent;
            else if (isZero(data.end_tangent))
                data.end_tangent = data.start_tangent;
        }

        private static SegmentData ExtractPathElementFeatures(PathElement element)
        {
            SegmentData data = new SegmentData();
            Point[] points = element.points;
            switch (element.type)
            {
                case ElementType.kCGPathElementAddCurveToPoint:
                    data.position = points[2];
                    data.start_tangent = subtract(points[0], origin_);
                    data.end_tangent = subtract(points[2], points[1]);
                    if (isZero(data.start_tangent))
                        ComputeQuadTangents(data, points[0], points[1], points[2]);
                    else if (isZero(data.end_tangent))
                        ComputeQuadTangents(data, origin_, points[0], points[1]);
                    break;
                case ElementType.kCGPathElementAddQuadCurveToPoint:
                    data.position = points[1];
                    ComputeQuadTangents(data, origin_, points[0], points[1]);
                    break;
                case ElementType.kCGPathElementMoveToPoint:
                case ElementType.kCGPathElementAddLineToPoint:
                    data.position = points[0];
                    data.start_tangent = subtract(data.position, origin_);
                    data.end_tangent = subtract(data.position, origin_);
                    break;
                case ElementType.kCGPathElementCloseSubpath:
                    data.position = subpath_start_;
                    data.start_tangent = subtract(data.position, origin_);
                    data.end_tangent = subtract(data.position, origin_);
                    break;
            }
            return data;
        }

        private static void UpdateFromPathElement(PathElement element)
        {
            SegmentData segment_data = ExtractPathElementFeatures(element);
            // First update the outgoing slope for the previous element.
            out_slope_ = segment_data.start_tangent;
            // Record the marker for the previous element.
            if (element_index_ > 0)
            {
                RNSVGMarkerType marker_type =
                        element_index_ == 1 ? RNSVGMarkerType.kStartMarker : RNSVGMarkerType.kMidMarker;
                double angle = CurrentAngle(marker_type);
                positions_.Add(new RNSVGMarkerPosition(marker_type, origin_, angle));
            }
            // Update the incoming slope for this marker position.
            in_slope_ = segment_data.end_tangent;
            // Update marker position.
            origin_ = segment_data.position;
            // If this is a 'move to' segment, save the point for use with 'close'.
            if (element.type == ElementType.kCGPathElementMoveToPoint)
                subpath_start_ = element.points[0];
            else if (element.type == ElementType.kCGPathElementCloseSubpath)
                subpath_start_ = new Point(0, 0);
            ++element_index_;
        }
    }
}
