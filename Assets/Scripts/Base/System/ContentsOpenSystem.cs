using System;
using System.Collections.Generic;
using BanpoFri;
using UniRx;

public class ContentsOpenSystem
{
    public enum ContentsOpenType
    {
        CardUpgrade = 1,
        TrainingRoomOpen = 2,
        InGameAddBlockAd = 3,
        SwapBlock = 4,
        AddBouceBall = 5,

        PopupRewardReroll = 6,
        BlockOpen = 7,

        ShopOpen = 8,
        RevivalOpen = 9,

        PackageOpen_10001 = 10,

        InterstitialAdsOpen = 11,

        PopupReview = 12,
        LuckyChoice = 13,
        AdPass = 14,
        Energy = 15,

        PopupCharacterReward = 16,
        PopupBeforeBattleSkill = 17,
        NoAds = 18,
        LuckySpin = 19,
        Pass = 20,
        Race = 21,
        RestoreSkill = 22,
        GoldGrab = 23,

        StarterPackage = 24,

        CrossPromotion = 25,
        TreasureHunter = 26,
        AttendanceReward = 27,
        AttendanceAdReward = 28,
    
        
    }

    private Dictionary<ContentsOpenType, Action<bool>> OpenWaitCallbackDic = new();
    private CompositeDisposable disposables = new CompositeDisposable();
    public int revival_open_enemy_count = 0;

    public void Create()
    {
        disposables.Clear();
        //revival_open_enemy_count = Tables.Instance.GetTable<Define>().GetData("revival_open_enemy_count").value;
        GameRoot.Instance.UserData.Stageidx.SkipLatestValueOnSubscribe().Subscribe(x => RefreshContentsOpen()).AddTo(disposables);
    }

    //특수 조건은 사용하면 안됨
    public void RegisterOpenWaitContentByStage(ContentsOpenType opentype, Action<bool> openCallback)
    {
        var result = ContentsOpenCheck(opentype);
        if (!result)
        {
            if (openCallback != null)
            {
                if (!OpenWaitCallbackDic.ContainsKey(opentype))
                {
                    OpenWaitCallbackDic.Add(opentype, openCallback);
                }
            }
        }
        else
        {
            openCallback?.Invoke(false);
        }
    }

    public void UnLoad()
    {
        OpenWaitCallbackDic.Clear();
    }

    public void UnRegisterOpenWaitContentByStage(ContentsOpenType opentype, Action<bool> openCallback)
    {
        if (openCallback != null)
        {
            if (OpenWaitCallbackDic.ContainsKey(opentype))
            {
                OpenWaitCallbackDic[opentype] -= openCallback;
            }
        }
    }

    public void RefreshContentsOpen()
    {
        var listCompleteKey = new List<ContentsOpenType>();
        foreach (var callback in OpenWaitCallbackDic)
        {
            var result = ContentsOpenCheck(callback.Key);
            if (result && OpenWaitCallbackDic[callback.Key] != null)
            {
                callback.Value?.Invoke(true);
                listCompleteKey.Add(callback.Key);
            }
        }

        foreach (var key in listCompleteKey)
            OpenWaitCallbackDic.Remove(key);
    }


    public bool ContentsOpenCheck(ContentsOpenType opentype)
    {

        switch (opentype)
        {
            case ContentsOpenType.AddBouceBall:
                {
                    if (GameRoot.Instance.UserData.Stageidx.Value >= 3)
                    {
                        return true;
                    }
                }
                break;
    
        }

        var td = Tables.Instance.GetTable<ContentsOpenCheck>().GetData((int)opentype);

        if (td != null)
        {
            var stageidx = GameRoot.Instance.UserData.Stageidx.Value;

            
            return stageidx >= td.stage_idx;
        }


        return false;
    }
}
