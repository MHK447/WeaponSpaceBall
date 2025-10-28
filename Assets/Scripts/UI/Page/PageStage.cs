using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BanpoFri;
using System.Collections.Generic;


[UIPath("UI/Page/PageStage")]
public class PageStage : UIBase
{
    [SerializeField]
    private TextMeshProUGUI StageGoalText;

    [SerializeField]
    private TextMeshProUGUI NextStageGoalText;

   


    public void Init()
    {
        int result = ((GameRoot.Instance.UserData.Stageidx.Value - 1) / 5) + 1;
        StageGoalText.text = $"Chapter {result}";
        NextStageGoalText.text = $"Chapter {result + 1}";


        int endgoal = result * 5;



    }

    public void Interaction(int stageidx , System.Action nextaction)
    {
        Init();
        GameRoot.Instance.WaitTimeAndCallback(1.5f , nextaction);
        GameRoot.Instance.WaitTimeAndCallback(2f , Hide);
    }
}
