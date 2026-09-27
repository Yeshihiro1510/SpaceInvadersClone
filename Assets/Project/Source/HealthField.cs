using System.Collections.Generic;
using UnityEngine;

namespace Project.Source
{
    public class HealthField : MonoBehaviour
    {
        [SerializeField] private GameObject _healthIconPrefab;
        
        private readonly List<GameObject> _icons = new();
        
        public void SetHealth(int health)
        {
            if (health > _icons.Count)
            {
                for (var i = 0; i < health; i++)
                {
                    var icon = Instantiate(_healthIconPrefab, transform);
                    icon.SetActive(true);
                    _icons.Add(icon);
                }
                
                return;
            }

            for (var i = 0; i < _icons.Count; i++)
            {
                _icons[i].SetActive(i <= health - 1);
            }
        }
    }
}