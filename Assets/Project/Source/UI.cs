using TMPro;
using UnityEngine;

namespace Project.Source
{
    public class UI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text PointsText { get; private set; }
        [field: SerializeField] public HealthField HealthField { get; private set; }
        [field: SerializeField] public DialogField DialogField { get; private set; }

        private static UI _instance;

        public static UI Instance
        {
            get
            {
                if (_instance == null) _instance = FindAnyObjectByType<UI>();
                return _instance;
            }
        }
    }
}