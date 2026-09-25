using TMPro;
using UnityEngine;

namespace Project.Source
{
    public class PointsController : MonoBehaviour
    {
        [field: SerializeField] public int RewardPerEnemy { get; private set; }
            
        [SerializeField] private TMP_Text _text;
        
        private Points _points;

        private void Awake()
        {
            _points = new Points(_text);
            _points.Value = 0;
        }

        public void AddPoints() => _points.Value += RewardPerEnemy;

        private class Points
        {
            public Points(TMP_Text text)
            {
                _text = text;
            }
            
            public int Value
            {
                get => _value;
                set
                {
                    if (value < 0) value = 0;
                    _value = value;
                    _text.text = value.ToString();
                }
            }

            private int _value;
            private readonly TMP_Text _text;
        }
    }
}