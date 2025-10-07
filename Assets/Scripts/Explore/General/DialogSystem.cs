using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;

    private Canvas canvas;
    private GameObject panel;
    private TMP_Text textArea;
    private Coroutine typingCoroutine;

    private float charInterval = 0.03f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CreateDialogUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateDialogUI()
    {
        // Canvas
        var canvasObj = new GameObject("DialogCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        DontDestroyOnLoad(canvasObj);

        // Panel (background)
        panel = new GameObject("DialogPanel");
        panel.transform.SetParent(canvas.transform, false);

        var panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0.25f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.7f);

        // Text
        var textObj = new GameObject("DialogText");
        textObj.transform.SetParent(panel.transform, false);

        textArea = textObj.AddComponent<TextMeshProUGUI>();
        var textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.05f, 0.05f);
        textRect.anchorMax = new Vector2(0.95f, 0.95f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        textArea.fontSize = 32;
        textArea.color = Color.white;
        textArea.text = "";
        textArea.font = Resources.Load<TMP_FontAsset>("Fonts/ヒラギノ角ゴシック W3 SDF");
    }

    // 公開メソッド
    public static void Show(string message)
    {
        if (instance == null)
        {
            var go = new GameObject("DialogSystem");
            instance = go.AddComponent<DialogSystem>();
        }

        instance.ShowInternal(message);
    }

    private void ShowInternal(string message)
    {
        panel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    public static void Hide()
    {
        if (instance != null)
            instance.panel.SetActive(false);
    }

    private IEnumerator TypeText(string message)
    {
        textArea.text = "";
        foreach (char c in message)
        {
            textArea.text += c;
            yield return new WaitForSeconds(charInterval);
        }
    }
}
