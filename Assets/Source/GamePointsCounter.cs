using System;
using TMPro;
using UnityEngine;

namespace Source
{
    public class GamePointsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public int Points
        {
            get => _points;
            set
            {
                if (value < 0) value = 0;
                _points = value;
                _text.text = value.ToString();
            }
        }
    
        private int _points;

        private void Awake()
        {
            Points = 0;
        }

        public void AddPoints(int amount) => Points += amount;
    }
}