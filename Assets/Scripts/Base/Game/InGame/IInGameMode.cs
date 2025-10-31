using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BanpoFri;

public interface IInGameMode
{
    void Load();
    void UnLoad();
    void SetCameraBoundMinY(float value);
}

public abstract class InGameMode : MonoBehaviour, IInGameMode
{
    [SerializeField]
    private PanAndZoom MainCam;

    public PanAndZoom GetMainCam { get { return MainCam; } }

    public float CamPixelWidth { get; private set; }
    public float CamPixelHeight { get; private set; }


    private void Awake()
    {
        CamPixelWidth = MainCam.cam.pixelWidth;
        CamPixelHeight = MainCam.cam.pixelHeight;
        if (GameRoot.IsInit())
        {
            GameRoot.Instance.InGameSystem.RegisteInGame(this);
          
            Load();
        }
        else
        {
            GameRoot.Load();
        }
    }

    protected virtual void Update()
    {
    }

    public virtual void Load()
    {
        LoadUI();
    }
    public virtual void UnLoad()
    {
        UnLoadUI();
    }

    protected virtual void LoadUI() { }
    protected virtual void UnLoadUI()
    {
        //GameRoot.Instance.UISystem.UnLoadUIAll();
    }

    public virtual void SetCameraBoundMinY(float value)
    {
    }
}