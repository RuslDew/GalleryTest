using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestProject
{
    namespace Scenes
    {
        public class SceneLoader : MonoBehaviour
        {
            public void LoadScene(string sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
