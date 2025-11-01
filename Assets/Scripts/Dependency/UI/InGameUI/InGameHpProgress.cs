using TMPro;
using BanpoFri;
using UnityEngine;

[FloatUIPath("UI/InGame/InGameHpProgress")]
[FloatingDepth((int)Config.FloatingUIDepth.HpProgress)]
public class InGameHpProgress : InGameFloatingUI
{

    [SerializeField]
    private TextMeshProUGUI HpText;

    public float updatespeed = 0.5f;


    private double CurHp;

    private double MaxHp;

    private Coroutine Col;

    private void Start()
    {

    }

    public void SetHpText(double curhp, double maxhp)
    {
        CurHp = curhp < 1 ? System.Math.Ceiling(curhp) : System.Math.Round(curhp);

        MaxHp = curhp < 1 ? System.Math.Ceiling(curhp) : System.Math.Round(maxhp);

        HpText.text = ProjectUtility.CalculateMoneyToString(CurHp);

        if (this.gameObject.activeSelf)
        {
            if (Col != null)
                StopCoroutine(Col);
        }
    }

    public override void Init(Transform parent)
    {
        base.Init(parent);
    }
}
