using TMPro;
using UnityEngine;

namespace Project.Source
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _pointsText;
        [SerializeField] private HealthField _healthField;
        [SerializeField] private DialogField _dialogField;
        
        public static TMP_Text PointsText { get; private set; }
        public static HealthField HealthField { get; private set; }
        public static DialogField DialogField { get; private set; }
        
        private void OnValidate()
        {
            PointsText = _pointsText;
            HealthField = _healthField;
            DialogField = _dialogField;
        }
    }
}