using Common;
using Common.Enum;
using UnityEngine;

namespace Boot
{
    public class BootLoader : MonoBehaviour
    {
        void Start()
        {
            Debug.Log(Scenes.ExploreScene);
            SceneLoader.Load(Scenes.ExploreScene);
        }
    }
}
