using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace TestProject
{
    namespace DataManagment
    {
        public class DataLoader : MonoBehaviour
        {
            [SerializeField] private string _imagesURL;
            public string ImagesURL => _imagesURL;

            
            public void LoadTexture(string fileName, Action<Texture2D> onTextureLoaded)
            {
                StartCoroutine(DownloadTexture(fileName, onTextureLoaded));
            }

            private IEnumerator DownloadTexture(string fileName, Action<Texture2D> onTextureLoaded)
            {
                UnityWebRequest request = UnityWebRequestTexture.GetTexture($"{_imagesURL}{fileName}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
                    onTextureLoaded?.Invoke(loadedTexture);
                }
                else
                    onTextureLoaded?.Invoke(null);
            }

            private string GetDirectoryListingRegexForUrl(string url)
            {
                if (url.Equals(_imagesURL))
                {
                    return "<a href=\".*\">(?<name>.*.jpg)</a>";
                }
                throw new NotSupportedException();
            }

            public List<string> GetFilesNamesList(string url)
            {
                List<string> _filesNamesList = new List<string>();

                HttpWebRequest request = null;

                try
                {
                    request = (HttpWebRequest)WebRequest.Create(url);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string html = reader.ReadToEnd();
                        Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));
                        MatchCollection matches = regex.Matches(html);
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (match.Success)
                                    _filesNamesList.Add(match.Groups["name"].Value);
                            }
                        }
                    }
                }

                return _filesNamesList;
            }
        }
    }
}
