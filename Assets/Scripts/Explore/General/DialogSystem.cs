using UnityEngine;
using System;
using System.Linq;
using UniRx;
using TMPro;
using System.Collections.Generic;
using Boot;

public class DialogSystem : MonoBehaviour
{
    private static DialogSystem instance;
    private DialogView view;

    private readonly float charInterval = 0.05f;
    private readonly float messageInterval = 0f;
    private bool isShowing = false;
    private int currentChoiceIndex = 0;

    // --- 購読管理 ---
    private readonly CompositeDisposable disposables = new();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        view = new GameObject("DialogView").AddComponent<DialogView>();
        view.Initialize();
    }

    private void CleanupSubscriptions()
    {
        disposables.Clear();
    }

    public static void ShowAsync(params string[] messages)
    {
        GameManager.Instance.LockPlayer();
        instance.ShowAsyncCore(messages, null);
    }

    public static void ShowAsync(Action onCompleted, params string[] messages)
    {
        GameManager.Instance.LockPlayer();
        instance.ShowAsyncCore(messages, onCompleted);
    }

    private void ShowAsyncCore(IEnumerable<string> messages, Action onCompleted)
    {
        if (isShowing)
            return;

        isShowing = true;
        CleanupSubscriptions();
        view.Panel.SetActive(true);

        messages
            .Select(message =>
                Observable.Defer(() =>
                    Observable.Return(message)
                        .Do(m => TypeTextObservable(m))
                        .SelectMany(_ => WaitForKeyDown(KeyCode.Space))
                        .Concat(Observable.Timer(TimeSpan.FromSeconds(messageInterval)).AsUnitObservable())
                )
            )
            .Concat()
            .Subscribe(
                _ => { },
                () =>
                {
                    view.Panel.SetActive(false);
                    GameManager.Instance.UnlockPlayer();
                    isShowing = false;
                    onCompleted?.Invoke();
                }
            )
            .AddTo(disposables);
    }

    public static void ShowWithChoices(Action onCompleted, string message, string[] choices, Action<int> onSelected)
    {
        GameManager.Instance.LockPlayer();
        instance.ShowChoiceAsync(message, choices, onSelected, onCompleted);
    }

    public static void ShowWithChoices(string message, string[] choices, Action<int> onSelected)
    {
        GameManager.Instance.LockPlayer();
        instance.ShowChoiceAsync(message, choices, onSelected, null);
    }

    private void ShowChoiceAsync(string message, string[] choices, Action<int> onSelected, Action onCompleted)
    {
        if (isShowing)
            return;

        isShowing = true;
        CleanupSubscriptions();

        view.Panel.SetActive(true);
        view.ChoiceContainer.SetActive(true);

        TypeTextObservable(message);
        view.CreateChoices(choices, view.TextArea.font);

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveChoice(-1);
                else if (Input.GetKeyDown(KeyCode.RightArrow)) MoveChoice(+1);
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    int selected = currentChoiceIndex;
                    onSelected?.Invoke(selected);
                    EndDialog();
                    onCompleted?.Invoke();
                }
            })
            .AddTo(disposables);
    }

    private void MoveChoice(int delta)
    {
        view.ChoiceTexts[currentChoiceIndex].color = Color.white;
        currentChoiceIndex = (currentChoiceIndex + delta + view.ChoiceTexts.Count) % view.ChoiceTexts.Count;
        view.ChoiceTexts[currentChoiceIndex].color = Color.yellow;
    }

    private void EndDialog()
    {
        view.Panel.SetActive(false);
        view.ChoiceContainer.SetActive(false);
        GameManager.Instance.UnlockPlayer();
        isShowing = false;
        CleanupSubscriptions();
    }

    private void TypeTextObservable(string message)
    {
        view.TextArea.text = "";
        message.ToCharArray()
            .Select(chr =>
                Observable.Defer(() =>
                    Observable.Return(chr)
                        .Do(c => view.TextArea.text += c)
                        .Concat(Observable.Timer(TimeSpan.FromSeconds(charInterval)).Select(_ => default(char)))
                )
            )
            .Concat()
            .Subscribe(_ => { })
            .AddTo(disposables);
    }

    private IObservable<Unit> WaitForKeyDown(KeyCode key)
    {
        return Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(key))
            .Take(1)
            .AsUnitObservable();
    }
}
