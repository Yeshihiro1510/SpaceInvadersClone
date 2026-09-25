using UnityEngine;

namespace Project.Source
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerPrefab;

        private void Awake()
        {
            var player = Instantiate(_playerPrefab);
            player.transform.position = new Vector3(0, GameFieldInfo.BottomBound, 0);
        }
    }
}