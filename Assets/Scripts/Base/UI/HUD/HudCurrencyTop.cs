using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine;
using BanpoFri;
using UnityEngine.UI;
using UniRx;
using TMPro;


[UIPath("HudCurrencyTop")]
public class HudCurrencyTop : UIBase
{
    [SerializeField]
    private bool IsBuffValueCheck;


    [Header("Material")]
    [SerializeField]
    private TextMeshProUGUI MaterialText;
    [SerializeField]
    private Transform MaterialIconTr;

    public Transform GetMaterialIconTr { get { return MaterialIconTr; } }

    [Header("MoneyCoin")]
    [SerializeField]
    private TextMeshProUGUI MoneyText;

    [SerializeField]
    private Transform SilverCoinIconTr;

    public Transform GetSilverCoinIconTr { get { return SilverCoinIconTr; } }





    [Header("Reminder")]

    [SerializeField]
    private Button OptionBtn;
    [SerializeField]
    private Button ProfileBtn;


    [SerializeField]
    private GameObject MoneyRoot;

    public Transform GetMoneyRoot { get { return MoneyRoot.transform; } }

    [FormerlySerializedAs("StoneRoot")]
    [SerializeField]
    private GameObject MaterialRoot;



    private readonly bool[] RootStates = new bool[] { true, true, true, true };
    private readonly int[] CurrencyValues = new int[3];
    private Tweener[] Tweeners = new Tweener[3];


    private int MaxEnergy = 0;


    protected override void Awake()
    {
        base.Awake();
        SetDataHook();

        if (OptionBtn != null)
            OptionBtn.onClick.AddListener(OnClickOption);

        //if (BuyCoinBtn != null) BuyCoinBtn.onClick.AddListener(OnClickCoin);


    }

    private void Update()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        if (MoneyText) MoneyText.text = ProjectUtility.CalculateMoneyToString((System.Numerics.BigInteger)CurrencyValues[0]);
        if (MaterialText) MaterialText.text = ProjectUtility.CalculateMoneyToString((System.Numerics.BigInteger)CurrencyValues[1]);
    }

    public void RewardCheck()
    {

    }

    public void SyncReward()
    {
        CurrencyValues[0] = (int)GameRoot.Instance.UserData.Money.Value;
        CurrencyValues[1] = (int)GameRoot.Instance.UserData.Cash.Value;
        if (MoneyText) MoneyText.text = ProjectUtility.CalculateMoneyToString((System.Numerics.BigInteger)CurrencyValues[0]);
        if (MaterialText) MaterialText.text = ProjectUtility.CalculateMoneyToString((System.Numerics.BigInteger)CurrencyValues[1]);
    }

    private void SetDataHook()
    {
        if (MoneyText != null)
        {
            MoneyText.text = ProjectUtility.CalculateMoneyToString(GameRoot.Instance.UserData.Money.Value);

            GameRoot.Instance.UserData.Money.Subscribe(x =>
            {
                if (!gameObject.activeInHierarchy)
                {
                    CurrencyValues[0] = (int)x;
                    return;
                }

                if (Tweeners[0] != null)
                {
                    Tweeners[0].Kill();
                    Tweeners[0] = null;
                }

                Tweeners[0] = DOTween.To(() => CurrencyValues[0],
                (int v) => CurrencyValues[0] = v,
                (int)x,
                 0.5f)
      .SetEase(Ease.Linear)
      .SetUpdate(true)
      .OnComplete(() =>
      {
          Tweeners[0] = null;
      });

            }).AddTo(this);
        }

        if (MaterialText != null)
        {
            MaterialText.text = GameRoot.Instance.UserData.Cash.Value.ToString();

            GameRoot.Instance.UserData.Cash.Subscribe(x =>
            {
                if (!gameObject.activeInHierarchy)
                {
                    CurrencyValues[1] = (int)x;
                    return;
                }
                Tweeners[1].Kill();
                Tweeners[1] = DOVirtual.Int(CurrencyValues[1], (int)x, 0.2f, v =>
                {
                    CurrencyValues[1] = v;
                })
                    .SetEase(Ease.Linear)
                    .SetUpdate(true)
                    .SetTarget(this);
            }).AddTo(this);
        }
    }


    private void OnClickOption()
    {
        GameRoot.Instance.UISystem.OpenUI<PopupOption>();
    }

    private void OnClickCoin()
    {
        //BuyCoinBtn.interactable = false;
        // GameRoot.Instance.UISystem.OpenUI<PopupCoinInsufficent>(x =>
        // {
        //     BuyCoinBtn.interactable = true;
        // });
    }


    public void SetCurrencyState(ShowFlag currencyShow)
    {
        RootStates[0] = ShowRoot(MoneyRoot.transform, RootStates[0], currencyShow.HasFlag(ShowFlag.Coin));
        RootStates[1] = ShowRoot(MaterialRoot.transform, RootStates[1], currencyShow.HasFlag(ShowFlag.Material) && GameRoot.Instance.ContentsOpenSystem.ContentsOpenCheck(ContentsOpenSystem.ContentsOpenType.CardUpgrade));
        OptionBtn.gameObject.SetActive(currencyShow.HasFlag(ShowFlag.Setting));
        ProfileBtn.gameObject.SetActive(currencyShow.HasFlag(ShowFlag.Profile));
    }

    private bool ShowRoot(Transform root, bool currentState, bool value)
    {
        if (currentState == value) return currentState;
        root.DOKill();
        if (value)
        {
            root.DOScale(1, 0.2f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    ProjectUtility.SetActiveCheck(root.gameObject, true);
                });
            return true;
        }
        else
        {
            root.DOScale(0, 0.2f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    ProjectUtility.SetActiveCheck(root.gameObject, false);
                });
            return false;
        }
    }

    public Vector3 GetRewardEndPos(int rewardType, int rewardIdx)
    {
        switch ((Config.RewardType)rewardType)
        {
            case Config.RewardType.Currency:
                {
                    switch ((Config.CurrencyID)rewardIdx)
                    {
                        case Config.CurrencyID.Money:
                            return GetMoneyRoot.position;
                    }
                    break;
                }
        }


        return Vector3.zero;
    }


}
