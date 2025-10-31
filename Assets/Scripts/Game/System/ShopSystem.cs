using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using BanpoFri;


public enum InAppPurchaseLocation
{
    none = -1,
    shop = 0,
    popup = 1,
    hud,
    nomoney,
    other,
    banner,
}


public class ShopSystem
{
    public System.DateTime ResetTime { get; private set; }
    public System.DateTime ResetStartTime { get; private set; }
    public enum ProductShopType
    {
        ShopCurrencyGem_01 = 101,
        ShopCurrencyGem_02 = 102,
        ShopCurrencyGem_03 = 103,
        ShopCurrencyGem_04 = 104,
        ShopCurrencyGem_05 = 105,
        ShopCurrencyGem_06 = 106,

        FreeGem = 1,
        AdGem = 2,

        GemRush_01 = 1001,
        GemRush_02 = 1002,
        GemRush_03 = 1003,
    }

    public ReactiveProperty<bool> IsVipProperty = new ReactiveProperty<bool>(false);

    public IReactiveProperty<int> FreeAdRemindTime = new ReactiveProperty<int>(-1);

    private float curdeltatime = 0f;

    private float InterAdTime = 240f; // 기본값 4분
    private float currentInterAdTimer = 0f;
    private bool isInterAdReady = false;

    public int stage_energy_consume = 0;

    public int daily_reward_reset_time = 0;

    public ReactiveProperty<bool> NoInterstitialAds = new ReactiveProperty<bool>(false);

    public ReactiveProperty<bool> NoRewardedAds = new ReactiveProperty<bool>(false);


    public InAppPurchaseLocation curLocation = InAppPurchaseLocation.none;

    public void Create()
    {
        // 타이머 초기화
        currentInterAdTimer = 0f;

        // VIP 상태에 따라 광고 표시 여부 설정
        IsVipProperty.Subscribe(isVip =>
        {
            // VIP 사용자는 광고 표시 안함
            isInterAdReady = !isVip;
        });

        DayInitTime();
    }

    public void UpdateOneTimeSecond()
    {
        // VIP가 아닐 때만 광고 타이머 증가
        if (isInterAdReady)
        {
            currentInterAdTimer += 1f;

           
        }
    }



    // 광고 표시 시간 설정 (초 단위)
    public void SetInterAdTime(float seconds)
    {
        InterAdTime = seconds;
    }

    // 현재 타이머 리셋
    public void ResetInterAdTimer()
    {
        currentInterAdTimer = 0f;
    }

    // 광고 표시 강제 활성화/비활성화
    public void SetInterAdEnabled(bool enabled)
    {
        isInterAdReady = enabled;

        // 비활성화 시 타이머도 리셋
        if (!enabled)
        {
            currentInterAdTimer = 0f;
        }
    }

    public void UpdateOneSecond()
    {
        if (GameRoot.Instance.UserData.Dayinitialtime != default(System.DateTime))
        {
            var CurTime = TimeSystem.GetCurTime();

            var diff = GameRoot.Instance.UserData.Dayinitialtime.Subtract(CurTime);
            FreeAdRemindTime.Value = (int)diff.TotalSeconds;
            if (diff.TotalSeconds < 0)
            {
                DayInitTime();
                //TestMinuteInitTime();
            }
        }
    }


    public void RewardPay(int rewardtype, int rewardidx, int rewardvalue)
    {
        switch (rewardtype)
        {
            case (int)Config.RewardType.Currency:
                {
                    switch (rewardidx)
                    {
                        case (int)Config.CurrencyID.Cash:
                            {
                                GameRoot.Instance.UserData.SetReward(rewardtype, rewardidx, rewardvalue);
                            }
                            break;
                        case (int)Config.CurrencyID.EnergyMoney:
                            {
                                GameRoot.Instance.UserData.Energycoin.Value += rewardvalue;
                            }
                            break;
                    }

                }
                break;
        }
    }



    public void DayInitTime()
    {
        var CurTime = TimeSystem.GetCurTime();


        ResetStartTime = ResetTime = new System.DateTime(CurTime.Year, CurTime.Month, CurTime.Day, daily_reward_reset_time, 0, 0);

        if (CurTime.Hour >= daily_reward_reset_time)
        {
            ResetTime = ResetTime.AddDays(1);
        }
        else
        {
            ResetStartTime = ResetTime.AddDays(-1);
        }


        if (GameRoot.Instance.UserData.Dayinitialtime == default(System.DateTime))
        {
            Reset();
        }
        else
        {
            var diff = ResetStartTime.Subtract(GameRoot.Instance.UserData.Dayinitialtime);
            if (diff.TotalSeconds >= 0)
            {
                Reset();
            }
        }
    }


    public void Reset()
    {

        GameRoot.Instance.UserData.Dayinitialtime = ResetTime;
        GameRoot.Instance.UserData.ResetRecordCount(Config.RecordCountKeys.AdGemCount, 0);
        GameRoot.Instance.UserData.ResetRecordCount(Config.RecordCountKeys.FreeGemCount, 0);
    }

}
