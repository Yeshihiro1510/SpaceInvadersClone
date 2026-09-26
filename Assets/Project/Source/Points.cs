using TMPro;

namespace Project.Source
{
    public class Points
    {
        private int _value;
        private readonly TMP_Text _text;
        
        public Points(TMP_Text text)
        {
            _text = text;
            Value = 0;
        }
            
        public int Value
        {
            get => _value;
            set
            {
                if (value < 0) value = 0;
                _value = value;
                _text.text = $"Points: {_value}";
            }
        }
    }
}