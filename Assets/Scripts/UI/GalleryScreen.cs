using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject
{
    namespace UI
    {
        public class GalleryScreen : MonoBehaviour
        {
            [SerializeField] private ImageContainer _imageContainerPrefab;
            [SerializeField] private Transform _imagesSpawnParent;
            [SerializeField] private DataManagment.DataLoader _dataLoader;

            [Space]

            [SerializeField] private RectTransform _viewport;
            [SerializeField] private GridLayoutGroup _imagesContainerLayout;

            [Space]

            [SerializeField] private ProgressBar _progressBar;

            [Space]

            [SerializeField] private ScrollRect _scrollRect;

            [Space]

            [SerializeField] private FullScreenImageView _fullScreenImageView;

            private int _galleryImagesCount = 0;
            private List<ImageContainer> _spawnedImages = new List<ImageContainer>();

            private int _possiblyVisibleImagesCount = 0;

            private List<string> _galleryFilesNames;

            private int _nextImageIndexToSpawn = 0;
            private int _nextImagesLoadedCount = 0;
            private bool _nextImagesSpawned = true;


            private void Awake()
            {
                _galleryFilesNames = _dataLoader.GetFilesNamesList(_dataLoader.ImagesURL);
                _galleryImagesCount = _galleryFilesNames.Count;

                _possiblyVisibleImagesCount = Mathf.Clamp(GetPossiblyVisibleImagesCount(), 0, _galleryImagesCount);
                _progressBar.Init(_possiblyVisibleImagesCount);

                SpawnVisibleGalleryImages();

                _scrollRect.onValueChanged.AddListener(OnScroll);
            }

            private void OnScroll(Vector2 position)
            {
                if (_scrollRect.velocity.y > 0f)
                {
                    if (_scrollRect.verticalNormalizedPosition <= 0f)
                    {
                        if (_nextImagesSpawned)
                        {
                            SpawnNextImagesRow();
                        }
                    }
                }
            }

            private void SpawnNextImagesRow()
            {
                _nextImagesSpawned = false;

                int columsCount = _imagesContainerLayout.constraintCount;

                for (int i = _nextImageIndexToSpawn; i < _nextImageIndexToSpawn + columsCount && i < _galleryFilesNames.Count; i++)
                {
                    string imageFileName = _galleryFilesNames[i];

                    LoadGalleryImage(imageFileName, OnSpawnNextImages, OnSpawnNextImages);
                }
            }

            private void OnSpawnNextImages()
            {
                _nextImagesLoadedCount++;

                int columsCount = _imagesContainerLayout.constraintCount;

                if (_nextImagesLoadedCount >= columsCount)
                {
                    _nextImagesLoadedCount = 0;
                    _nextImagesSpawned = true;
                }
            }

            private void SpawnVisibleGalleryImages()
            {
                for (int i = 0; i < _possiblyVisibleImagesCount; i++)
                {
                    string imageFileName = _galleryFilesNames[i];

                    LoadGalleryImage
                    (
                        imageFileName, 
                        () =>
                        {
                            OnSpawnVisibleImageHandler();
                        },
                        () =>
                        {
                            OnSpawnVisibleImageHandler();
                        }
                    );
                }
            }

            private void OnSpawnVisibleImageHandler()
            {
                _progressBar.Fill();
            }

            private void LoadGalleryImage(string imageFileName, Action onSpawnImage, Action onFailSpawn = null)
            {
                _dataLoader.LoadTexture(imageFileName, (loadedTexture) =>
                {
                    if (loadedTexture != null)
                    {
                        SpawnGalleryImage(loadedTexture);
                        onSpawnImage?.Invoke();
                    }
                    else
                        onFailSpawn?.Invoke();
                });
            }

            private void SpawnGalleryImage(Texture2D tex2D)
            {
                ImageContainer newImage = Instantiate(_imageContainerPrefab, _imagesSpawnParent);
                newImage.Setup(tex2D);
                newImage.OnClick += () => _fullScreenImageView.Show(newImage.CurrentTexture);
                _spawnedImages.Add(newImage);

                _nextImageIndexToSpawn++;
            }

            private int GetPossiblyVisibleImagesCount()
            {
                float viewportHeight = _viewport.rect.height;
                viewportHeight -= _imagesContainerLayout.padding.top;

                float imageHeight = _imageContainerPrefab.Rect.rect.height + _imagesContainerLayout.spacing.y;

                return Mathf.RoundToInt(viewportHeight / imageHeight) * _imagesContainerLayout.constraintCount;
            }
        }
    }
}
