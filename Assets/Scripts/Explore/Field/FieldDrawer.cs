using UnityEngine;

public class FieldDrawer : MonoBehaviour
{
    private IDrawFieldHandler drawFieldHandler;
    void Awake()
    {
        drawFieldHandler = GetComponent<IDrawFieldHandler>();
    }
    void Start()
    {
        drawFieldHandler.Draw();
    }
}
