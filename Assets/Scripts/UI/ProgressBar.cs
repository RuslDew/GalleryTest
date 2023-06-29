using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace TestProject
{
    namespace UI
    {
        public class ProgressBar : MonoBehaviour
        {
            [SerializeField] private Image _fillImage;
            [SerializeField] private float _fillSpeed = 5f;

            private Tweener _fillTweener;

            public UnityEvent OnCompleteFill;

            private int _stagesCount;
            private int _completedStagesCount;
            private float _fillAmountStep;

            private bool _isActive = true;


            public void Init(int stagesCount)
            {
                _isActive = true;

                if (_fillImage == null)
                {
                    OnCompleteFill?.Invoke();
                    return;
                }

                _fillImage.fillAmount = 0f;
                _stagesCount = stagesCount;
                _completedStagesCount = 0;
                _fillAmountStep = 1f / (float)_stagesCount;
            }

            public void Fill()
            {
                if (_fillImage == null || !_isActive)
                {
                    OnCompleteFill?.Invoke();
                    return;
                }

                if (_fillTweener.IsActive())
                {
                    IncreaseCompletedStagesCount();
                    _fillTweener.Kill();
                }

                float currentFillAmount = (float)_completedStagesCount * _fillAmountStep;
                float fillAmountEndValue = currentFillAmount + _fillAmountStep;

                _fillTweener = _fillImage.DOFillAmount(fillAmountEndValue, _fillSpeed).OnComplete(() =>
                {
                    if (_fillImage.fillAmount >= 1f)
                    {
                        OnCompleteFill?.Invoke();
                        Deactivate();
                    }

                    IncreaseCompletedStagesCount();
                }).SetEase(Ease.Linear).SetSpeedBased();
            }

            private void IncreaseCompletedStagesCount()
            {
                _completedStagesCount++;
            }

            private void Deactivate()
            {
                gameObject.SetActive(false);
                _isActive = false;
            }

            private void OnDestroy()
            {
                if (_fillTweener != null)
                    _fillTweener.Kill();
            }
        }
    }
}
