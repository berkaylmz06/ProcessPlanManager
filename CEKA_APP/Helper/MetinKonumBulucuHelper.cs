using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Generic;

namespace CEKA_APP.Helper
{
    public class MetinKonumBulucu : LocationTextExtractionStrategy
    {
        private readonly string _targetText;
        private readonly List<(double X, double Y)> _textLocations;

        public MetinKonumBulucu(string targetText)
        {
            _targetText = targetText;
            _textLocations = new List<(double X, double Y)>();
        }

        public override void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_TEXT && data is TextRenderInfo renderInfo)
            {
                string text = renderInfo.GetText();
                if (text.Contains(_targetText))
                {
                    var baseline = renderInfo.GetBaseline();
                    var startPoint = baseline.GetStartPoint();
                    _textLocations.Add((startPoint.Get(0), startPoint.Get(1)));
                }
            }
            base.EventOccurred(data, type);
        }

        public List<(double X, double Y)> GetTextLocations()
        {
            return _textLocations;
        }
    }
}
