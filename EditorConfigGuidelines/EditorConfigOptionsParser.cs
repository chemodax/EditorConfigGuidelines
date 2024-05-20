using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    foreach (string str in guidelines.ToString().Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] tokens = str.Split(
                            new char[] { ' ', '\t' },
                            StringSplitOptions.RemoveEmptyEntries);

                        int? column = null;
                        DoubleCollection dashArray = default;

                        foreach (string token in tokens)
                        {
                            DoubleCollection dc;
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

                return new Guideline[] { };
            }
            catch (Exception)
            {
                return new Guideline[] { };
            }
        }

        private static bool TryParseGuidelineStyle(string str,
            out DoubleCollection dashArray)
        {
            if (str.Equals("solid", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = new DoubleCollection();
                return true;
            }
            else if (str.Equals("dashed", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = new DoubleCollection() { 3, 3 };
                return true;

            }
            else if (str.Equals("dotted", StringComparison.InvariantCultureIgnoreCase))
            {
                dashArray = new DoubleCollection() { 1, 4 };
                return true;
            }
            else
            {
                dashArray = default;
                return false;
            }
        }

        private static bool TryParseGuidelineColumn(string str, out int column)
        {
            return int.TryParse(str, out column);
        }
    }
}
