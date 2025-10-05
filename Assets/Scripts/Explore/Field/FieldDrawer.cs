using UnityEngine;

public class FieldDrawer : MonoBehaviour
{
    [SerializeField] private MonoBehaviour drawFieldHandlerComponent;
    private IDrawFieldHandler drawFieldHandler;
    void Awake()
    {
        drawFieldHandler = drawFieldHandlerComponent as IDrawFieldHandler;
    }
    void Start()
    {
        drawFieldHandler.Draw();
    }
}
