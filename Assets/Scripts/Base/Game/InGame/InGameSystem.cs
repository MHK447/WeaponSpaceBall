using System.Collections.Generic;
using UniRx;
using BanpoFri;
using UnityEngine;

[System.Serializable]
public enum GameType
{
    Main,
    Event,
    Travel,

    None = 99,
}

public class InGameSystem
{
    public InGameMode CurInGame { get; private set; } = null;

    private bool firstInit = false;
    public System.Action NextActionClear = null;
    public System.Action NextAction = null;

    public T GetInGame<T>() where T : InGameMode
    {
        return CurInGame as T;
    }

    public void RegisteInGame(InGameMode mode)
    {
        CurInGame = mode;
    }


    private void StartGame(GameType type, System.Action loadCallback = null, bool nextStage = false)
    {
        GameRoot.Instance.Loading.Show(true);
        GameRoot.Instance.SceneSystem.ChangeScene(type, loadCallback);
    }

    public void ChangeMode(GameType type, System.Action _action = null)
    {
        System.GC.Collect();
        firstInit = false;
        NextActionClear = null;
        StartGame(type, () =>
        {
            _action?.Invoke();
            SoundPlayer.Instance.PlayBGM("bgm");
        });
    }


    public void InitPopups()
    {
        GameRoot.Instance.InitCurrencyTop();
        GameRoot.Instance.UISystem.OpenUI<HUDTotal>(popup =>
        {
            GameRoot.Instance.Loading.Hide(false, () =>
            {
                GameRoot.Instance.ActionQueueSystem.OnFirstInitCall();
            });
        });
    }
}
