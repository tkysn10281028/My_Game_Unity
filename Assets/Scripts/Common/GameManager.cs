
using UnityEngine;
namespace Common
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public string[] json;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
