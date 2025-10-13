using Common;
using Common.Enum;
using UnityEngine;

namespace Boot
{
    public class BootLoader : MonoBehaviour
    {
        void Start()
        {
            SceneLoader.Load(Scenes.ExploreScene);
        }
    }
}
