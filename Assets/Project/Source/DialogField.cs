using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Project.Source
{
    public class DialogField : MonoBehaviour
    {
        [field: SerializeField] public float TextDuration { get; private set; }
        [field: SerializeField] public float FieldDuration { get; private set; }
        [field: SerializeField] public Ease TextEase { get; private set; } = Ease.Linear;
        [field: SerializeField] public Ease FieldEase { get; private set; } = Ease.Linear;

        [SerializeField] private TMP_Text _text;

        private void Awake()
        {
            transform.localScale = new Vector3(1, 0, 1);
            _text.text = string.Empty;
        }

        public void DOText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                DOVirtual.Int(_text.text.Length, 0, TextDuration, v => _text.maxVisibleCharacters = v)
                    .SetEase(TextEase)
                    .OnComplete(() =>
                    {
                        _text.text = string.Empty;
                        transform.DOScaleY(0, FieldDuration).SetEase(FieldEase);
                    });
            }
            else
            {
                _text.text = text;
                _text.maxVisibleCharacters = 0;
                transform.DOScaleY(1, FieldDuration).SetEase(FieldEase)
                    .OnComplete(() =>
                        DOVirtual.Int(0, _text.text.Length, TextDuration, v => _text.maxVisibleCharacters = v)
                            .SetEase(TextEase));
            }
        }
    }
}