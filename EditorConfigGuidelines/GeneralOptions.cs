using Community.VisualStudio.Toolkit;

namespace EditorConfigGuidelines
{
    public class GeneralOptions : BaseOptionModel<GeneralOptions>, IRatingConfig
    {
        public int RatingRequests { get; set; }
    }
}
