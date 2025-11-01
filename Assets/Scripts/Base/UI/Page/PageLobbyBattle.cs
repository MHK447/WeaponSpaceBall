using UnityEngine;
using BanpoFri;
using UnityEngine.UI;
using TMPro;

[UIPath("UI/Page/PageLobbyBattle")]
public class PageLobbyBattle : CommonUIBase
{
    [SerializeField]
    private Button StartBtn;


    override protected void Awake()
    {
        base.Awake();
        StartBtn.onClick.AddListener(OnStartBtnClick);
    }

    public void OnStartBtnClick()
    {
        Hide();

        StartBtn.interactable = false;
        GameRoot.Instance.InGameSystem.GetInGame<InGameBase>().Stage.StartBattle();
    }

    public override void OnShowBefore()
    {
        base.OnShowBefore();
        Set();
    }

    public void Set()
    {
        StartBtn.interactable = true;

    }

}
