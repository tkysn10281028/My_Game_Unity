using UnityEngine;
namespace Explore.Player
{
    public class PlayerKeyboardInputHandler : MonoBehaviour, IPlayerMoveInputHandler
    {
        public Vector3 GetPlayerMoveVector()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            return new Vector3(moveX, moveY, 0f).normalized;
        }
    }
}