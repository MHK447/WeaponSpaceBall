using System.Collections;
using System.Collections.Generic;
using TMPro;
using BanpoFri;
using UnityEngine;
using UnityEngine.UI;

public enum HudBottomBtnType
{
    Shop = 0,
    Blocks = 1,
    TRAINING = 2,
    CARD = 3,
    BATTLE = 4,

    Done,
}



[UIPath("UI/Page/HUDTotal", true)]
public class HUDTotal : UIBase
{
    [SerializeField]
    private Button ShopBtn;

    [SerializeField]
    private GameObject ShopReddotObj;

    [SerializeField]
    private Image ReddotImg;

    [SerializeField]
    private TextMeshProUGUI FreeTextObj;

    [SerializeField]
    private List<HudBottomBtnComponent> HudBottomBtnList = new List<HudBottomBtnComponent>();

    public Queue<System.Action> ActionQueue = new Queue<System.Action>();
    private Coroutine waitQueue = null;

    private List<UIBase> UIList = new List<UIBase>();

    public HudBottomBtnType CurrentlyOpenPage;

    protected override void Awake()
    {
        base.Awake();

        if (UIList.Count == 0)
        {
            for (int i = 0; i < (int)HudBottomBtnType.Done; ++i)
            {
                switch (i)
                {
                    case (int)HudBottomBtnType.Shop:
                        {

                        }
                        break;
                    case (int)HudBottomBtnType.CARD:
                        {
                        }
                        break;
                    case (int)HudBottomBtnType.TRAINING:
                        {

                        }
                        break;
                    case (int)HudBottomBtnType.Blocks:
                        {

                        }
                        break;
                    case (int)HudBottomBtnType.BATTLE:
                        {
                            GameRoot.Instance.UISystem.PreLoadUI(typeof(PageLobbyBattle), ui =>
                             {
                                 UIList.Add(ui);
                                 OpenPage(HudBottomBtnType.BATTLE);
                                 UpdateButtonLock();
                             });
                        }
                        break;
                }
            }
        }


        RegisterContentsOpen();
    }

    public override void OnShowAfter()
    {
        base.OnShowAfter();

        // var getui = GameRoot.Instance.UISystem.GetUI<PageLobbyTraining>();

        // if (getui != null)
        // {
        //     // getui.SetScroll();
        // }


    }


    public override void OnShowBefore()
    {
        base.OnShowBefore();

        foreach (var item in HudBottomBtnList)
        {
            item.Set(OnClickHudBottomBtn);
        }

        GameRoot.Instance.ActionQueueSystem.NextAction();
    }


    public void RegisterContentsOpen()
    {
        // GameRoot.Instance.ContentsOpenSystem.RegisterOpenWaitContentByStage(ContentsOpenSystem.ContentsOpenType.Pass, CheckContentsOpen_Pass);
        // GameRoot.Instance.ContentsOpenSystem.RegisterOpenWaitContentByStage(ContentsOpenSystem.ContentsOpenType.AttendanceReward, CheckContentsOpen_Attendance);
        // GameRoot.Instance.ContentsOpenSystem.RegisterOpenWaitContentByStage(ContentsOpenSystem.ContentsOpenType.BlockOpen, CheckContentsOpen_Block);
    }

    public void EnqueueTutorialContentsOpen()
    {
        // CheckContentsOpen_Training(true);
        // CheckContentsOpen_Card(true);
        // CheckContentsOpen_Shop(true);
    }

    public void UpdateButtonLock()
    {
        // HudBottomBtnList[(int)HudBottomBtnType.TRAINING].SetLocked(!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.TrainingRoomOpen));
        // HudBottomBtnList[(int)HudBottomBtnType.CARD].SetLocked(!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.CardUpgrade));
        // HudBottomBtnList[(int)HudBottomBtnType.Blocks].SetLocked(!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.BlockOpen));
        // HudBottomBtnList[(int)HudBottomBtnType.Shop].SetLocked(!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.ShopOpen));
    }


    public RectTransform GetWorldPosByContentsOpen(ContentsOpenSystem.ContentsOpenType opentype)
    {
        RectTransform targettr = null;


        switch (opentype)
        {
            case ContentsOpenSystem.ContentsOpenType.TrainingRoomOpen:
                targettr = HudBottomBtnList[(int)HudBottomBtnType.TRAINING].GetLockObj.transform as RectTransform;
                break;
            case ContentsOpenSystem.ContentsOpenType.CardUpgrade:
                targettr = HudBottomBtnList[(int)HudBottomBtnType.CARD].GetLockObj.transform as RectTransform;
                break;
            case ContentsOpenSystem.ContentsOpenType.BlockOpen:
                targettr = HudBottomBtnList[(int)HudBottomBtnType.Blocks].GetLockObj.transform as RectTransform;
                break;
            case ContentsOpenSystem.ContentsOpenType.Pass:
                break;
        }

        return targettr;
    }

    public HudBottomBtnComponent GetHudBottomBtn(HudBottomBtnType type)
    {
        return HudBottomBtnList[(int)type];
    }


    // private void CheckContentsOpen_Shop(bool action)
    // {
    //     if (!action) return;

    //     if (GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.ShopOpen) &&
    //     GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.FirstShopOpen) == 0)
    //     {
    //         GameRoot.Instance.ActionQueueSystem.Append(() =>
    //         {
    //             GameObject target = HudBottomBtnList[(int)HudBottomBtnType.Shop].GetLockObj;
    //             GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //             {
    //                 popup.Set(ContentsOpenSystem.ContentsOpenType.ShopOpen, target.transform as RectTransform, () =>
    //                 {
    //                     HudBottomBtnList[(int)HudBottomBtnType.Shop].SetLocked(false);

    //                     GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.FirstShopOpen, 1);

    //                     NextAction();
    //                 });
    //             });
    //         });
    //     }
    // }



    // private void CheckContentsOpen_Training(bool action)
    // {
    //     if (!action) return;

    //     if (!GameRoot.Instance.TutorialSystem.IsClearTuto(TutorialSystem.Tuto_1)
    //         && GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.TrainingRoomOpen))
    //     {
    //         GameRoot.Instance.ActionQueueSystem.Append(() =>
    //         {
    //             GameObject target = HudBottomBtnList[(int)HudBottomBtnType.TRAINING].GetLockObj;
    //             GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //             {
    //                 popup.Set(ContentsOpenSystem.ContentsOpenType.TrainingRoomOpen, target.transform as RectTransform, () =>
    //                 {
    //                     HudBottomBtnList[(int)HudBottomBtnType.TRAINING].SetLocked(false);
    //                     GameRoot.Instance.UserData.SetReward((int)Config.RewardType.Currency, (int)Config.CurrencyID.SilverCoin, 50);
    //                     GameRoot.Instance.TutorialSystem.StartTutorial(TutorialSystem.Tuto_1);

    //                     GameRoot.Instance.TutorialSystem.OnActiveTutoEnd = () =>
    //                     {
    //                         NextAction();
    //                     };
    //                 });
    //             });
    //         });
    //     }
    // }

    // private void CheckContentsOpen_Card(bool action)
    // {
    //     if (!action) return;

    //     GameObject target = HudBottomBtnList[(int)HudBottomBtnType.CARD].GetLockObj;
    //     //card
    //     if (!GameRoot.Instance.TutorialSystem.IsClearTuto(TutorialSystem.Tuto_2)
    //         && GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.CardUpgrade))
    //     {
    //         GameRoot.Instance.ActionQueueSystem.Append(() =>
    //         {
    //             GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //             {
    //                 popup.Set(ContentsOpenSystem.ContentsOpenType.CardUpgrade, target.transform as RectTransform, () =>
    //                 {
    //                     HudBottomBtnList[(int)HudBottomBtnType.CARD].SetLocked(false);
    //                     GameRoot.Instance.TutorialSystem.StartTutorial(TutorialSystem.Tuto_2);

    //                     GameRoot.Instance.TutorialSystem.OnActiveTutoEnd = () =>
    //                     {
    //                         NextAction();
    //                     };
    //                 });
    //             });
    //         });
    //     }
    // }

    // private void CheckContentsOpen_Pass(bool action)
    // {
    //     if (!action) return;
    //     GameRoot.Instance.ActionQueueSystem.Append(() =>
    //     {

    //         GameRoot.Instance.PassSystem.FirstStartCheck();
    //         GameObject target = null;
    //         OpenPage(HudBottomBtnType.BATTLE);
    //         var getui = GameRoot.Instance.UISystem.GetUI<PageLobbyBattle>();
    //         if (getui != null)
    //         {
    //             var getpassicon = getui.GetIconComponent(PageIconType.Pass) as PassIconComponent;
    //             target = getpassicon.gameObject;
    //             getpassicon.IsWaitShow = true;
    //             TpUtility.SetActiveCheck(target, false);
    //         }
    //         //pass
    //         GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //         {
    //             popup.Set(ContentsOpenSystem.ContentsOpenType.Pass, target.transform as RectTransform, () =>
    //             {
    //                 var getui = GameRoot.Instance.UISystem.GetUI<PageLobbyBattle>();
    //                 var getpassicon = getui.GetIconComponent(PageIconType.Pass) as PassIconComponent;
    //                 getpassicon.IsWaitShow = false;
    //                 TpUtility.SetActiveCheck(getpassicon.gameObject, true);

    //                 GameRoot.Instance.UISystem.OpenUI<PopupPackagePassStart>(null, NextAction);
    //             });
    //         });
    //     });
    // }


    // private void CheckContentsOpen_Attendance(bool action)
    // {
    //     if (!action) return;
    //     GameRoot.Instance.ActionQueueSystem.Append(() =>
    //     {

    //         GameRoot.Instance.AttendanceSystem.FirstStartCheck();
    //         GameObject target = null;
    //         OpenPage(HudBottomBtnType.BATTLE);
    //         var getui = GameRoot.Instance.UISystem.GetUI<PageLobbyBattle>();
    //         if (getui != null)
    //         {
    //             var getpassicon = getui.GetIconComponent(PageIconType.Attendance) as AttendanceIconComponent;
    //             target = getpassicon.gameObject;
    //             getpassicon.IsWaitShow = true;
    //             TpUtility.SetActiveCheck(target, false);
    //         }

    //         GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //         {
    //             popup.Set(ContentsOpenSystem.ContentsOpenType.AttendanceReward, target.transform as RectTransform, () =>
    //             {
    //                 var getui = GameRoot.Instance.UISystem.GetUI<PageLobbyBattle>();
    //                 var getpassicon = getui.GetIconComponent(PageIconType.Attendance) as AttendanceIconComponent;
    //                 getpassicon.IsWaitShow = false;
    //                 TpUtility.SetActiveCheck(getpassicon.gameObject, true);
    //             });
    //         });
    //     });
    // }

    // private void CheckContentsOpen_Block(bool action)
    // {
    //     if (!action) return;

    //     GameRoot.Instance.ActionQueueSystem.Append(() =>
    //     {
    //         HudBottomBtnList[(int)HudBottomBtnType.Blocks].SetLocked(true);
    //         GameObject target = HudBottomBtnList[(int)HudBottomBtnType.Blocks].GetLockObj;
    //         GameRoot.Instance.UISystem.OpenUI<PopupContentsOpen>(popup =>
    //         {
    //             popup.Set(ContentsOpenSystem.ContentsOpenType.BlockOpen, target.transform as RectTransform, null);
    //         }, () =>
    //         {
    //             HudBottomBtnList[(int)HudBottomBtnType.Blocks].SetLocked(false);
    //             GameRoot.Instance.WaitRealTimeAndCallback(0.05f, NextAction);
    //         });
    //     });
    // }



    public void NextAction()
    {
        GameRoot.Instance.ActionQueueSystem.NextAction();
    }



    public void OnClickHudBottomBtn(HudBottomBtnType type, bool isopen)
    {
        OpenPage(type);
    }

    public void OpenPage(HudBottomBtnType type, bool forceOpen = false)
    {
        if (type == CurrentlyOpenPage && !forceOpen) return;

        foreach (var ui in UIList)
        {
            ui.Hide();
        }
        for (int i = 0; i < HudBottomBtnList.Count; ++i)
        {
            HudBottomBtnList[i].SetActive(HudBottomBtnList[i].CurBtnType != type);
        }

        switch (type)
        {
            case HudBottomBtnType.Blocks:
                //GameRoot.Instance.UISystem.OpenUI<PageLobbyBlock>(x => x.Init());
                break;
            case HudBottomBtnType.TRAINING:
                // GameRoot.Instance.UISystem.OpenUI<PageLobbyTraining>(x => x.Init());
                break;
            case HudBottomBtnType.CARD:
                //GameRoot.Instance.UISystem.OpenUI<PageLobbyCards>(x => x.Init());
                break;
            case HudBottomBtnType.BATTLE:
                GameRoot.Instance.UISystem.OpenUI<PageLobbyBattle>();
                break;
            case HudBottomBtnType.Shop:
                //GameRoot.Instance.UISystem.OpenUI<PageLobbyShop>(x => x.Init());
                break;
        }

        CurrentlyOpenPage = type;
    }

}
