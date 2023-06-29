using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace TestProject
{
    namespace UI
    {
        public class FullScreenImageView : MonoBehaviour
        {
            [SerializeField] private Transform _imageTransform;
            [SerializeField] private GameObject _backgroundVisual;
            [SerializeField] private Image _image;
            [SerializeField] private float _showDuration;

            private Tweener _showAndHideTweener;

            private bool _isHidden = true;


            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Hide();
                }
            }

            public void Show(Texture2D imageTexture)
            {
                AllowOrientationChange();

                if (!_isHidden)
                    return;

                if (_showAndHideTweener != null)
                    _showAndHideTweener.Kill();

                _backgroundVisual.SetActive(true);

                Sprite sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));
                _image.sprite = sprite;
                _image.preserveAspect = true;

                _showAndHideTweener = _imageTransform.DOScale(new Vector3(1f, 1f, 1f), _showDuration);

                _isHidden = false;
            }

            public void Hide()
            {
                if (_isHidden)
                    return;

                if (_showAndHideTweener != null)
                    _showAndHideTweener.Kill();

                _showAndHideTweener = _imageTransform.DOScale(new Vector3(0f, 0f, 0f), _showDuration).OnComplete(() =>
                {
                    _backgroundVisual.SetActive(false);
                    SetStatusAsHidden();
                    DisallowOrientationChange();
                });
            }

            private void SetStatusAsHidden()
            {
                _isHidden = true;
            }

            private void AllowOrientationChange()
            {
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToLandscapeLeft = true;

                Screen.orientation = ScreenOrientation.AutoRotation;
            }

            private void DisallowOrientationChange()
            {
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeRight = false;
                Screen.autorotateToLandscapeLeft = false;

                Screen.orientation = ScreenOrientation.Portrait;
            }
        }
    }
}
