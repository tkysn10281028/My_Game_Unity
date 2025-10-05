using Common.Enum;
using UnityEngine.SceneManagement;

namespace Common
{
    public static class SceneLoader
    {
        public static void Load(Scenes scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}
