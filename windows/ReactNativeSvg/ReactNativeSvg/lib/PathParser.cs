using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace ReactNativeSvg.svg
{
    class PathElement
    {
        public ElementType type;
        public Point[] points;
        public PathElement(ElementType type, Point[] points)
        {
            this.type = type;
            this.points = points;
        }
    }

    class PathParser
    {
        private Regex PATH_REG_EXP = new Regex("[a-df-z]|[\\-+]?(?:[\\d.]e[\\-+]?|[^\\s\\-+,a-z])+", RegexOptions.IgnoreCase);
        private Regex DECIMAL_REG_EXP = new Regex("(\\.\\d+)(?=\\-?\\.)");
        private string mString;
        private double mScale;
        private PathGeometry mPath;
        private PathFigure mFigure;
        private bool mValid = true;
        private Match mMatcher;
        private string mLastValue;
        private string mLastCommand;

        private double mPenX = 0f;
        private double mPenY = 0f;
        private double mPenDownX;
        private double mPenDownY;
        private double mPivotX = 0f;
        private double mPivotY = 0f;
        private bool mPendDownSet = false;
        public PathParser(string d, double scale = 1)
        {
            mString = d;
            mScale = 1;
        }

        public PathGeometry getPath()
        {
            mPath = new PathGeometry();
            mPath.Figures = new PathFigureCollection();
            mMatcher = PATH_REG_EXP.Match(DECIMAL_REG_EXP.Replace(mString, "$1,"));
            while (mMatcher.Success && mValid)
            {
                ExecuteCommand(mMatcher.Value);
                mMatcher = mMatcher.NextMatch();
            }
            return mPath;
        }

        private double getNextdouble()
        {
            if (mLastValue != "" && mLastValue != null)
            {
                String lastValue = mLastValue;
                mLastValue = null;
                return Convert.ToDouble(lastValue);
            }
            else if (mMatcher.Success)
            {
                mMatcher = mMatcher.NextMatch();
                return Convert.ToDouble(mMatcher.Value);
            }
            else
            {
                mValid = false;
                mPath = new PathGeometry();
                return 0;
            }
        }

        private bool getNextbool()
        {
            if (mMatcher.Success)
            {
                mMatcher = mMatcher.NextMatch();
                return mMatcher.Value == "1";
            }
            else
            {
                mValid = false;
                mPath = new PathGeometry();
                return false;
            }
        }

        private void ExecuteCommand(string command)
        {
            switch (command)
            {
                // moveTo command
                case "m":
                    MoveCommand(getNextdouble(), getNextdouble());
                    break;
                case "M":
                    MoveToCommand(getNextdouble(), getNextdouble());
                    break;

                // lineTo command
                case "l":
                    LineCommand(getNextdouble(), getNextdouble());
                    break;
                case "L":
                    LineToCommand(getNextdouble(), getNextdouble());
                    break;

                // horizontalTo command
                case "h":
                    LineCommand(getNextdouble(), 0);
                    break;
                case "H":
                    LineToCommand(getNextdouble(), mPenY);
                    break;

                // verticalTo command
                case "v":
                    LineCommand(0, getNextdouble());
                    break;
                case "V":
                    LineToCommand(mPenX, getNextdouble());
                    break;

                // curveTo command
                case "c":
                    CurveCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;
                case "C":
                    CurveToCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;

                // smoothCurveTo command
                case "s":
                    SmoothCurveCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;
                case "S":
                    SmoothCurveToCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;

                // quadraticBezierCurveTo command
                case "q":
                    QuadraticBezierCurveCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;
                case "Q":
                    QuadraticBezierCurveToCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextdouble());
                    break;

                // smoothQuadraticBezierCurveTo command
                case "t":
                    SmoothQuadraticBezierCurveCommand(getNextdouble(), getNextdouble());
                    break;
                case "T":
                    SmoothQuadraticBezierCurveToCommand(getNextdouble(), getNextdouble());
                    break;

                // arcTo command
                case "a":
                    ArcCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextbool(), getNextbool(), getNextdouble(), getNextdouble());
                    break;
                case "A":
                    ArcToCommand(getNextdouble(), getNextdouble(), getNextdouble(), getNextbool(), getNextbool(), getNextdouble(), getNextdouble());
                    break;
                // close command
                case "Z":
                case "z":
                    CloseCommand();
                    break;
                default:
                    mLastValue = command;
                    ExecuteCommand(mLastCommand);
                    return;
            }

            mLastCommand = command;

            if (command == "m")
            {
                mLastCommand = "l";
            }
            else if (command == "M")
            {
                mLastCommand = "L";
            }
        }

        private void MoveCommand(double x, double y)
        {
            MoveToCommand(x + mPenX, y + mPenY);
        }

        private void MoveToCommand(double x, double y)
        {
            mPivotX = mPenX = x;
            mPivotY = mPenY = y;

            mFigure = new PathFigure();
            mPath.Figures.Add(mFigure);
            mFigure.StartPoint = new Point(x * mScale, y * mScale);
            mFigure.Segments = new PathSegmentCollection();
        }

        private void LineCommand(double x, double y)
        {
            LineToCommand(x + mPenX, y + mPenY);
        }

        private void LineToCommand(double x, double y)
        {
            setPenDown();
            mPivotX = mPenX = x;
            mPivotY = mPenY = y;
            mFigure.Segments.Add(new LineSegment() { Point = new Point(x * mScale, y * mScale) });
        }

        private void CurveCommand(double c1x, double c1y, double c2x, double c2y, double ex, double ey)
        {
            CurveToCommand(c1x + mPenX, c1y + mPenY, c2x + mPenX, c2y + mPenY, ex + mPenX, ey + mPenY);
        }

        private void CurveToCommand(double c1x, double c1y, double c2x, double c2y, double ex, double ey)
        {
            setPenDown();
            mPivotX = c2x;
            mPivotY = c2y;
            mPenX = ex;
            mPenY = ey;
            mFigure.Segments.Add(new BezierSegment()
            {
                Point1 = new Point(c1x * mScale, c1y * mScale),
                Point2 = new Point(c2x * mScale, c2y * mScale),
                Point3 = new Point(ex * mScale, ey * mScale),
            });
        }

        private void SmoothCurveCommand(double c1x, double c1y, double ex, double ey)
        {
            SmoothCurveToCommand(c1x + mPenX, c1y + mPenY, ex + mPenX, ey + mPenY);
        }

        private void SmoothCurveToCommand(double c1x, double c1y, double ex, double ey)
        {
            setPenDown();
            double c2x = c1x;
            double c2y = c1y;
            c1x = (mPenX * 2) - mPivotX;
            c1y = (mPenY * 2) - mPivotY;
            mPivotX = c2x;
            mPivotY = c2y;
            mPenX = ex;
            mPenY = ey;
            mFigure.Segments.Add(new BezierSegment()
            {
                Point1 = new Point(c1x * mScale, c1y * mScale),
                Point2 = new Point(c2x * mScale, c2y * mScale),
                Point3 = new Point(ex * mScale, ey * mScale),
            });
        }

        private void QuadraticBezierCurveCommand(double c1x, double c1y, double c2x, double c2y)
        {
            QuadraticBezierCurveToCommand(c1x + mPenX, c1y + mPenY, c2x + mPenX, c2y + mPenY);
        }

        private void QuadraticBezierCurveToCommand(double c1x, double c1y, double c2x, double c2y)
        {
            setPenDown();
            mPivotX = c1x;
            mPivotY = c1y;
            double ex = c2x;
            double ey = c2y;
            c2x = (ex + c1x * 2) / 3;
            c2y = (ey + c1y * 2) / 3;
            c1x = (mPenX + c1x * 2) / 3;
            c1y = (mPenY + c1y * 2) / 3;
            mPenX = ex;
            mPenY = ey;
            mFigure.Segments.Add(new BezierSegment()
            {
                Point1 = new Point(c1x * mScale, c1y * mScale),
                Point2 = new Point(c2x * mScale, c2y * mScale),
                Point3 = new Point(ex * mScale, ey * mScale),
            });
        }

        private void SmoothQuadraticBezierCurveCommand(double c1x, double c1y)
        {
            SmoothQuadraticBezierCurveToCommand(c1x + mPenX, c1y + mPenY);
        }

        private void SmoothQuadraticBezierCurveToCommand(double c1x, double c1y)
        {
            double c2x = c1x;
            double c2y = c1y;
            c1x = (mPenX * 2) - mPivotX;
            c1y = (mPenY * 2) - mPivotY;
            QuadraticBezierCurveToCommand(c1x, c1y, c2x, c2y);
        }

        private void ArcCommand(double rx, double ry, double rotation, bool outer, bool clockwise, double x, double y)
        {
            ArcToCommand(rx, ry, rotation, outer, clockwise, x + mPenX, y + mPenY);
        }

        private void ArcToCommand(double rx, double ry, double rotation, bool outer, bool clockwise, double x, double y)
        {
            setPenDown();
            mPenX = mPivotX = x;
            mPenY = mPivotY = y;

            mFigure.Segments.Add(new ArcSegment()
            {
                RotationAngle = rotation,
                Point = new Point(x * mScale, y * mScale),
                Size = new Size(rx * mScale, ry * mScale),
                SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                IsLargeArc = outer
            });
        }
        private void CloseCommand()
        {
            if (mPendDownSet)
            {
                mPenX = mPenDownX;
                mPenY = mPenDownY;
                mPendDownSet = false;
                mFigure.IsClosed = true;
            }
        }

        private void setPenDown()
        {
            if (!mPendDownSet)
            {
                mPenDownX = mPenX;
                mPenDownY = mPenY;
                mPendDownSet = true;
            }
        }
    }
}
