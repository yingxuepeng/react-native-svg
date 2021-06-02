using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;

namespace ReactNativeSvg.svg
{
    enum UnitType
    {
        UNKNOWN,
        NUMBER,
        PERCENTAGE,
        EMS,
        EXS,
        PX,
        CM,
        MM,
        IN,
        PT,
        PC,
    }
    class SVGLength
    {
        // https://www.w3.org/TR/SVG/types.html#InterfaceSVGLength


        public readonly double value;
        public readonly UnitType unit;
        private SVGLength()
        {
            value = 0;
            unit = UnitType.UNKNOWN;
        }
        SVGLength(double number)
        {
            value = number;
            unit = UnitType.NUMBER;
        }
        private SVGLength(String length)
        {
            length = length.Trim();
            int stringLength = length.Length;
            int percentIndex = stringLength - 1;
            if (stringLength == 0 || length.Equals("normal"))
            {
                unit = UnitType.UNKNOWN;
                value = 0;
            }
            else if (length[percentIndex] == '%')
            {
                unit = UnitType.PERCENTAGE;
                value = Double.Parse(length.Substring(0, percentIndex));
            }
            else
            {
                int twoLetterUnitIndex = stringLength - 2;
                if (twoLetterUnitIndex > 0)
                {
                    String lastTwo = length.Substring(twoLetterUnitIndex);
                    int end = twoLetterUnitIndex;
                    switch (lastTwo)
                    {
                        case "px":
                            unit = UnitType.NUMBER;
                            break;

                        case "em":
                            unit = UnitType.EMS;
                            break;
                        case "ex":
                            unit = UnitType.EXS;
                            break;

                        case "pt":
                            unit = UnitType.PT;
                            break;

                        case "pc":
                            unit = UnitType.PC;
                            break;

                        case "mm":
                            unit = UnitType.MM;
                            break;

                        case "cm":
                            unit = UnitType.CM;
                            break;

                        case "in":
                            unit = UnitType.IN;
                            break;

                        default:
                            unit = UnitType.NUMBER;
                            end = stringLength;
                            break;
                    }
                    value = Double.Parse(length.Substring(0, end));
                }
                else
                {
                    unit = UnitType.NUMBER;
                    value = Double.Parse(length);
                }
            }
        }

        public static SVGLength from(JSValue dynamic)
        {
            switch (dynamic.Type)
            {
                case JSValueType.Double:
                case JSValueType.Int64:
                    return new SVGLength(dynamic.AsDouble());
                case JSValueType.String:
                    return new SVGLength(dynamic.AsString());
                default:
                    return new SVGLength();
            }
        }

        static String toString(JSValue dynamic)
        {
            switch (dynamic.Type)
            {
                case JSValueType.Double:
                case JSValueType.Int64:
                    return dynamic.AsDouble().ToString();
                case JSValueType.String:
                    return dynamic.AsString();
                default:
                    return null;
            }
        }

        static List<SVGLength> arrayFrom(JSValue dynamic)
        {
            switch (dynamic.Type)
            {
                case JSValueType.Double:
                case JSValueType.Int64:
                    {
                        List<SVGLength> list = new List<SVGLength>(1);
                        list.Add(new SVGLength(dynamic.AsDouble()));
                        return list;
                    }
                case JSValueType.Array:
                    {
                        JSValueArray arr = dynamic.AsArray() as JSValueArray;
                        int size = arr.Count;
                        List<SVGLength> list = new List<SVGLength>(size);
                        for (int i = 0; i < size; i++)
                        {
                            JSValue val = arr[i];
                            list.Add(from(val));
                        }
                        return list;
                    }
                case JSValueType.String:
                    {
                        List<SVGLength> list = new List<SVGLength>(1);
                        list.Add(new SVGLength(dynamic.AsString()));
                        return list;
                    }
                default:
                    return null;
            }
        }
    }
}