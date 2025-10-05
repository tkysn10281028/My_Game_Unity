using UnityEngine;
using UniRx;
using UniRx.Triggers;
namespace Explore.Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 30f;
        [SerializeField] private MonoBehaviour inputHandlerComponent;
        [SerializeField] private MonoBehaviour obstacleHandlerComponent;
        private IPlayerMoveInputHandler inputHandler;
        private IPlayerObstacleHandler obstacleHandler;

        void Awake()
        {
            inputHandler = inputHandlerComponent as IPlayerMoveInputHandler;
            obstacleHandler = obstacleHandlerComponent as IPlayerObstacleHandler;
        }

        void Start()
        {
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    Vector3 moveDirection = inputHandler.GetPlayerMoveVector();
                    if (moveDirection == Vector3.zero) return;
                    var origin = transform.position;
                    var distance = moveSpeed * Time.deltaTime;
                    transform.position = obstacleHandler.CheckObstacle(origin, origin + moveDirection * distance);
                    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                })
                .AddTo(this);
        }
    }
}