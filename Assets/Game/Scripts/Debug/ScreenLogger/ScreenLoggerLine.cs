namespace DebugTools
{
    [System.Serializable]
    class ScreenLoggerLine
    {
        private string _label;
        private string _text;
        private string _line;

        private int _labelLength;
        private int _textLength;
        private int _length;

        public int Length { get => _length; }
        public string Line { get => _line; }

        public ScreenLoggerLine(string lineLabel, string lineText)
        {
            _label = "<b>" + lineLabel + "</b>: ";
            _text = lineText;

            _labelLength = _label.Length;
            _textLength = _text.Length;

            UpdateLength();
            UpdateLine();
        }

        public void UpdateLineText(string newText)
        {
            _text = newText;
            _textLength = _text.Length;

            UpdateLength();
            UpdateLine();
        }

        private void UpdateLength()
        {
            _length = _labelLength + _textLength;
        }

        private void UpdateLine()
        {
            _line = _label + _text;
        }
    }
}