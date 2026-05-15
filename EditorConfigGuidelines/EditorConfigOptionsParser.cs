using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Media;

namespace EditorConfigGuidelines
{
    internal static class EditorConfigOptionsParser
    {
        public static Guideline[] Parse(IEditorOptions options)
        {
            try
            {
                IReadOnlyDictionary<string, object> conventions =
                    options.GetOptionValue(DefaultOptions.RawCodingConventionsSnapshotOptionId);
                object guidelines;
                if (conventions != null &&
                    conventions.TryGetValue("guidelines", out guidelines) &&
                    guidelines is string)
                {
                    List<Guideline> result = new List<Guideline>();
                    foreach (string str in guidelines.ToString().Split([',', ';'], StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] tokens = str.Split(
                            [' ', '\t'],
                            StringSplitOptions.RemoveEmptyEntries);

                        int? column = null;
                        DoubleCollection? dashArray = null;

                        foreach (string token in tokens)
                        {
                            DoubleCollection? dc;
                            int num;

                            if (TryParseGuidelineStyle(token, out dc))
                            {
                                dashArray = dc;
                            }
                            else if (TryParseGuidelineColumn(token, out num))
                            {
                                column = num;
                            }
                        }

                        if (column.HasValue)
                        {
                            result.Add(new Guideline(column.Value, dashArray));
                        }
                    }

                    return result.ToArray();
                }

                return [];
            }
            catch (Exception)
            {
                return [];
            }
        }

        private static bool TryParseGuidelineStyle(string str,
            out DoubleCollection? dashArray)
        {
            if (str.Equals("solid", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = [];
                return true;
            }
            else if (str.Equals("dashed", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = [3, 3];
                return true;

            }
            else if (str.Equals("dotted", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = [1, 4];
                return true;
            }
            else
            {
                dashArray = null;
                return false;
            }
        }

        private static bool TryParseGuidelineColumn(string str, out int column)
        {
            return int.TryParse(str, out column);
        }
    }
}
