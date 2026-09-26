using DG.Tweening;
using UnityEngine;

namespace Project.Source
{
    public class BackgroundFx : MonoBehaviour
    {
        private static readonly int ProgressX = Shader.PropertyToID("_Progress");
        private static readonly int ProgressY = Shader.PropertyToID("_ProgressY");

        [field: SerializeField, Min(0)] public float FirstLayerMultiplier { get; private set; }
        [field: SerializeField, Min(0)] public float SecondLayerMultiplier { get; private set; }
        [field: SerializeField, Min(0)] public float ScrollingSpeed { get; private set; }
        [field: SerializeField, Min(0)] public float ScrollingStartAndStopDuration { get; private set; }
        [field: SerializeField] public Ease ScrollingStartAndStopEase { get; private set; }

        [SerializeField] private PlayerController _player;
        [SerializeField] private Material _firstLayerMaterial;
        [SerializeField] private Material _secondLayerMaterial;

        private float _runtimeScrollingSpeed;
        private float _time;

        private void Update()
        {
            var posX = _player.transform.position.x;
            _time += Time.deltaTime * _runtimeScrollingSpeed;
            var cycledTime = Mathf.Repeat(_time, 1) * 2 - 1;
            // var absPosX = Mathf.Abs(posX);
            // var time = absPosX / GameFieldInfo.HalfWidth;
            // var poweredTime = Mathf.Sqrt(time);
            // _firstLayerMaterial.SetFloat("_Progress", float.IsNaN(poweredTime) ? time : poweredTime * (posX / absPosX) * _firstLayerMultiplier);
            // _secondLayerMaterial.SetFloat("_Progress", float.IsNaN(poweredTime) ? time : poweredTime * (posX / absPosX) * _secondLayerMultiplier);
            var progress = posX / GameFieldInfo.HalfWidth;
            _firstLayerMaterial.SetFloat(ProgressX, progress * FirstLayerMultiplier);
            _secondLayerMaterial.SetFloat(ProgressX, progress * SecondLayerMultiplier);
            _firstLayerMaterial.SetFloat(ProgressY, cycledTime);
            _secondLayerMaterial.SetFloat(ProgressY, cycledTime);
        }

        public void StartScrolling()
        {
            DOVirtual.Float(0f, ScrollingSpeed, ScrollingStartAndStopDuration,
                v => _runtimeScrollingSpeed = v).SetEase(ScrollingStartAndStopEase);
        }

        public void StopScrolling()
        {
            DOVirtual.Float(ScrollingSpeed, 0f, ScrollingStartAndStopDuration,
                v => _runtimeScrollingSpeed = v).SetEase(ScrollingStartAndStopEase);
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            _firstLayerMaterial.SetFloat(ProgressX, 0);
            _secondLayerMaterial.SetFloat(ProgressX, 0);
            _firstLayerMaterial.SetFloat(ProgressY, 0);
            _secondLayerMaterial.SetFloat(ProgressY, 0);
        }
#endif
    }
}