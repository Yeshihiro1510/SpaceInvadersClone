using DG.Tweening;
using UnityEngine;

namespace Project.Source
{
    public class BackgroundFx : MonoBehaviour
    {
        private static readonly int ProgressX = Shader.PropertyToID("_ProgressX");
        private static readonly int ProgressY = Shader.PropertyToID("_ProgressY");

        [field: SerializeField, Min(0)] public float FirstLayerMultiplier { get; private set; }
        [field: SerializeField, Min(0)] public float SecondLayerMultiplier { get; private set; }
        [field: SerializeField, Min(0)] public float StarterScrollingSpeed { get; private set; }
        [field: SerializeField, Min(0)] public float ScrollingStartAndStopDuration { get; private set; }
        [field: SerializeField] public Ease ScrollingStartAndStopEase { get; private set; }

        [SerializeField] private Material _firstLayerMaterial;
        [SerializeField] private Material _secondLayerMaterial;
        
        public void Tick(float playerPosX)
        {
            var offset = playerPosX / GameFieldInfo.HalfWidth;
            _firstLayerMaterial.SetFloat(ProgressX, offset * FirstLayerMultiplier);
            _secondLayerMaterial.SetFloat(ProgressX, offset * SecondLayerMultiplier);
        }

        public void StartScrolling()
        {
            // DOVirtual.Float(0f, StarterScrollingSpeed, ScrollingStartAndStopDuration,
            //     v => _runtimeScrollingSpeed = v).SetEase(ScrollingStartAndStopEase);
        }

        public void StopScrolling()
        {
            // DOVirtual.Float(StarterScrollingSpeed, 0f, ScrollingStartAndStopDuration,
            //     v => _runtimeScrollingSpeed = v).SetEase(ScrollingStartAndStopEase);
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