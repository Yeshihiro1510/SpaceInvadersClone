using System;
using UnityEngine;

namespace Project.Source
{
    public class BackgroundFx : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _firstLayerMultiplier;
        [SerializeField, Min(0)] private float _secondLayerMultiplier;
        [SerializeField, Min(0)] private float _scrollingSpeed;

        [SerializeField] private PlayerController _player;
        [SerializeField] private Material _firstLayerMaterial;
        [SerializeField] private Material _secondLayerMaterial;

        private void Update()
        {
            var posX = _player.transform.position.x;
            var time = posX / GameFieldInfo.HalfWidth;
            var cycledTime = Mathf.Repeat(Time.time * _scrollingSpeed, 1) * 2 - 1;
            // var absPosX = Mathf.Abs(posX);
            // var time = absPosX / GameFieldInfo.HalfWidth;
            // var poweredTime = Mathf.Sqrt(time);
            // _firstLayerMaterial.SetFloat("_Progress", float.IsNaN(poweredTime) ? time : poweredTime * (posX / absPosX) * _firstLayerMultiplier);
            // _secondLayerMaterial.SetFloat("_Progress", float.IsNaN(poweredTime) ? time : poweredTime * (posX / absPosX) * _secondLayerMultiplier);
            _firstLayerMaterial.SetFloat("_Progress", time * _firstLayerMultiplier);
            _secondLayerMaterial.SetFloat("_Progress", time * _secondLayerMultiplier);
            _firstLayerMaterial.SetFloat("_ProgressY", cycledTime);
            _secondLayerMaterial.SetFloat("_ProgressY", cycledTime);
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            _firstLayerMaterial.SetFloat("_Progress", 0);
            _secondLayerMaterial.SetFloat("_Progress", 0);
            _firstLayerMaterial.SetFloat("_ProgressY", 0);
            _secondLayerMaterial.SetFloat("_ProgressY", 0);
        }
#endif
    }
}