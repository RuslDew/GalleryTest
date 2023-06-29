using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject
{
    namespace UI
    {
        public class ImageContainer : MonoBehaviour
        {
            [SerializeField] private Image _loadedImage;
            [SerializeField] private RectTransform _selfRect;
            [SerializeField] private Button _button;

            public Texture2D CurrentTexture { get; private set; }

            public RectTransform Rect => _selfRect;

            public event Action OnClick;


            private void Awake()
            {
                if (_button != null)
                {
                    _button.onClick.AddListener(() => OnClick?.Invoke());
                }
            }

            public void Setup(Texture2D texture)
            {
                if (_loadedImage == null || texture == null)
                    return;

                CurrentTexture = texture;

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                _loadedImage.sprite = sprite;
                _loadedImage.preserveAspect = true;
            }

            private void OnDestroy()
            {
                if (CurrentTexture != null)
                    Destroy(CurrentTexture);
            }
        }
    }
}
