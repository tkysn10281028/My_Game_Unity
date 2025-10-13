using System.Threading.Tasks;
using UnityEngine;

public interface IPlayerObstacleHandler
{
    public Vector3 CheckObstacle(Vector3 origin, Vector3 target);
}