
using System.Collections.Generic;
using UnityEngine;
namespace Boot
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public List<MapObject> mapObjectList;
        public List<StatusObject> statusObjectList;
        public bool IsPlayerLocked { get; private set; }

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
        public void LockPlayer()
        {
            IsPlayerLocked = true;
        }

        public void UnlockPlayer()
        {
            IsPlayerLocked = false;
        }
    }
}
