using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using BanpoFri;


public class ActionQueueSystem
{
    #region System
    private readonly LinkedList<Action> actions = new();

    public void Append(Action action)
    {
        actions.AddLast(action);
    }

    public void InsertNext(Action action)
    {
        actions.AddFirst(action);
    }

    public void NextAction()
    {
        if (actions.Count == 0)
        {
            BpLog.Log($"ActionQueue finished");
            return;
        }

        var action = actions.First.Value;
        actions.RemoveFirst();
        action?.Invoke();

        BpLog.Log($"ActionQueue left : {actions.Count}");
    }

    public void ClearActions()
    {
        actions.Clear();
    }
    #endregion

    #region Actions

    public struct GameFinishContext
    {
        public bool isStageClear;
        public bool shouldRestoreSkill;

        public int passKeyCount;
    }

    public void OnFirstInitCall()
    {
        ClearActions();
        
        // AttendancePopupCheck();
        // TowerRacePopupCheck();
        // StarterPackageRewardPopupCheck();
        // NoAdsPopupCheck();
        // GoldGrabPopupCheck();
        // FirstTreasureHunterPopupCheck();

        NextAction();
    }

    public void OnGameFinishCall(GameFinishContext context)
    {
        // ShowTouchLock();
        // TowerRacePopupCheck();
        // BounceBallTutorialCheck();
        // BlockUnlockPopupCheck();
        // BowlingBallUnlockPopupCheck();
        // ReviewPopupCheck();
        // SkillPackagePopupCheck();
        // NoAdsPopupCheck();
        // StarterPackagePopupCheck();
        // LobbyIconThrowCheck(context.isStageClear, context.passKeyCount);
        // GoldGrabPopupCheck();
        // HideTouchLock();
    }

    #endregion

    // private void ShowTouchLock()
    // {
    //     Append(() =>
    //     {
    //         GameRoot.Instance.UISystem.OpenUI<PopupTouchLock>(x => NextAction(), NextAction);
    //     });
    // }

    // private void HideTouchLock()
    // {
    //     Append(() =>
    //     {
    //         var touchlock = GameRoot.Instance.UISystem.GetUI<PopupTouchLock>();
    //         if (touchlock == null) { NextAction(); return; }
    //         touchlock.Hide();
    //     });
    // }

    // private void BounceBallTutorialCheck()
    // {
    //     //스테이지 보상 바운스볼 획득 튜토리얼
    //     if (!GameRoot.Instance.TutorialSystem.IsClearTuto(TutorialSystem.Tuto_8)
    //         && GameRoot.Instance.UserData.Chapteridx == 1 && GameRoot.Instance.UserData.Stageidx.Value == 2)
    //     {
    //         Append(() =>
    //          {
    //              Debug.Log("b all");
    //              GameRoot.Instance.TutorialSystem.StartTutorial(TutorialSystem.Tuto_8);
    //              GameRoot.Instance.TutorialSystem.OnActiveTutoEnd = NextAction;
    //          });
    //     }
    // }

    // private void BlockUnlockPopupCheck()
    // {
    //     var stageChallengeCount = GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.ChallengeStageCount);
    //     var adblockidx = GameRoot.Instance.BlockSystem.CheckAddAdBlock();
    //     if (adblockidx > 0 && stageChallengeCount % 2 == 0 && GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.PopupCharacterReward))
    //     {
    //         Append(() =>
    //         {
    //             GameRoot.Instance.UISystem.OpenUI<PopupCharacterReward>(page => page.Set(adblockidx, NextAction));
    //         });
    //     }
    // }

    // private void ReviewPopupCheck()
    // {
    //     if (GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.PopupReview)
    //     && GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.ReviewPopup) == 0)
    //     {
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.ReviewPopup, 1);
    //              GameRoot.Instance.UISystem.OpenUI<PopupReview>(null, NextAction);
    //          });
    //     }
    // }

    // private void SkillPackagePopupCheck()
    // {
    //     //튜토리얼 중복 방지
    //     if (!GameRoot.Instance.TutorialSystem.IsClearTuto(TutorialSystem.Tuto_2)
    //         && GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.CardUpgrade)) return;

    //     int packageType = (int)PackageType.ItemPackage_10001;
    //     int stageChallengeCount = GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.ChallengeStageCount);

    //     var td = Tables.Instance.GetTable<ShopProduct>().GetData(packageType);
    //     bool canShowPackage = GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck((ContentsOpenSystem.ContentsOpenType)td.contents_open_idx)
    //         && !GameRoot.Instance.UserData.BuyInappIds.Contains(td.product_id);

    //     int totalItemCount = 0;
    //     var skillInfos = Tables.Instance.GetTable<BlockSkillInfo>().DataList.ToList();
    //     foreach (var skillInfo in skillInfos)
    //     {
    //         var skillData = GameRoot.Instance.SkillSystem.GetSkillData(skillInfo.idx);
    //         if (skillData == null) continue;
    //         totalItemCount += skillData.Count.Value;
    //     }

    //     if (stageChallengeCount > 1 && stageChallengeCount % 3 == 0 && canShowPackage && totalItemCount <= 3)
    //     {
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UISystem.OpenUI<PopupPackageSkillItem>(popup => popup.Set(packageType), NextAction);
    //          });
    //     }
    // }

    // private void NoAdsPopupCheck()
    // {
    //     if (!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.NoAds)) return;
    //     if (GameRoot.Instance.ShopSystem.NoInterstitialAds.Value) return;

    //     if (GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.NoAdsPopupShown) == 0)
    //     {
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.NoAdsPopupShown, 1);
    //              GameRoot.Instance.UISystem.OpenUI<PopupNoAds>(null, NextAction);
    //          });
    //     }
    //     else
    //     {
    //         int stageChallengeCount = GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.ChallengeStageCount);
    //         if (stageChallengeCount > 1 && stageChallengeCount % 4 == 0)
    //         {
    //             Append(() =>
    //              {
    //                  GameRoot.Instance.UISystem.OpenUI<PopupNoAdsPackages>(null, NextAction);
    //              });
    //         }
    //     }
    // }

    // private void StarterPackagePopupCheck()
    // {
    //     if (!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.StarterPackage)) return;
    //     if (GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.FirstStarterPackagePopup) > 0) return;
    //     if (GameRoot.Instance.UserData.Starterpackdata.Isbuy.Value) return;

    //     Append(() =>
    //      {
    //          GameRoot.Instance.UISystem.OpenUI<PopupPackageStarter>(null, NextAction);
    //          GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.FirstStarterPackagePopup, 1);
    //      });
    // }

    // private void StarterPackageRewardPopupCheck()
    // {
    //     if (!GameRoot.Instance.UserData.Starterpackdata.Isbuy.Value) return;
    //     if (!GameRoot.Instance.DailyResetSystem.StarterPackageRewardCheck()) return;

    //     Append(() =>
    //     {
    //         GameRoot.Instance.UISystem.OpenUI<PopupPackageStarter>(null, NextAction);
    //     });
    // }

    // private void AttendancePopupCheck()
    // {
    //     if (!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.AttendanceReward)) return;
    //     if (!GameRoot.Instance.AttendanceSystem.IsAttendanceRewardCheck()) return;

    //     Append(() =>
    //     {
    //         GameRoot.Instance.UISystem.OpenUI<PopupAttendance>(popup => popup.Init(), NextAction);
    //     });
    // }



    // private void TowerRacePopupCheck()
    // {
    //     if (GameRoot.Instance.TowerRaceSystem.TowerStartPopupOpenCheck())
    //     {
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UISystem.OpenUI<PopupTowerRaceStart>();
    //          });
    //     }
    //     else if (GameRoot.Instance.UserData.Towerracedata.TowerRaceStateProperty.Value == TowerRaceState.Reward || GameRoot.Instance.UserData.Towerracedata.TowerRaceStateProperty.Value == TowerRaceState.End)
    //     {
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UISystem.OpenUI<PopupTowerRace>();
    //          });
    //     }
    // }

    // private void GoldGrabPopupCheck()
    // {
    //     if (!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.GoldGrab)) return;

    //     //first popup
    //     if (!GameRoot.Instance.TutorialSystem.IsClearTuto(TutorialSystem.Tuto_GoldGrab))
    //     {
    //         Append(() => GameRoot.Instance.UISystem.OpenUI<PopupGoldGrabEventStart>());
    //         return;
    //     }

    //     //finish popup
    //     GameRoot.Instance.GoldGrabSystem.CheckState();
    //     if (GameRoot.Instance.GoldGrabSystem.CurrentState != GoldGrab.Finish) return;
    //     Append(() =>
    //         {
    //             GameRoot.Instance.UISystem.OpenUI<PopupGoldGrabEnd>();
    //         });
    // }

    // private void FirstTreasureHunterPopupCheck()
    // {
    //     if (!GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.TreasureHunter)) return;

    //     if (GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.FirstTreasureHunterPopup) < 1)
    //     {
    //         Append(() =>
    //         {
    //             // first popup
    //             GameRoot.Instance.UISystem.OpenUI<PopupTreasureHunterStart>();
    //             GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.FirstTreasureHunterPopup, 1);
    //             return;
    //         });
    //     }
    // }

    // private void BowlingBallUnlockPopupCheck()
    // {
    //     var getblockcount = GameRoot.Instance.UserData.GetRecordCount(Config.RecordCountKeys.AddAdRockBlock);

    //     var findblockdata = GameRoot.Instance.BlockSystem.FindBlockData((int)Config.WeaponBlock.BowlingBallBlock);

    //     if (getblockcount == 0 && findblockdata == null && GameRoot.Instance.UserData.Stageidx.Value == 5)
    //     {
    //         GameRoot.Instance.UserData.AddRecordCount(Config.RecordCountKeys.AddAdRockBlock, 1);
    //         Append(() =>
    //          {
    //              GameRoot.Instance.UISystem.OpenUI<PopupCharacterReward>(page => page.Set((int)Config.WeaponBlock.BowlingBallBlock, NextAction));
    //          });
    //     }
    // }

    // private void RestoreSkillsCheck()
    // {
    //     Append(() =>
    //      {
    //          GameRoot.Instance.UISystem.OpenUI<PopupRestoreSkills>(null, NextAction);
    //      });
    // }
}