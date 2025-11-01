using UnityEngine;

public partial class InGameBaseStage : MonoBehaviour
{
    public PlayerUnitGroup PlayerUnitGroup;

    public EnemyUnitGroup EnemyUnitGroup;

    public void InitStage()
    {
        PlayerUnitGroup.Init();
        EnemyUnitGroup.Init();
    }

    public void StartBattle()
    {
        SoundPlayer.Instance.SetBGMVolume(0f);
        GameRoot.Instance.UISystem.GetUI<HUDTotal>()?.Hide();


        GameRoot.Instance.UISystem.OpenUI<PopupInGame>(x =>
        {
            x.Init();
        });

        PlayerUnitGroup.AddBlock(1);


    }


    public void StageClear()
    {
        PlayerUnitGroup.ClearData();
        EnemyUnitGroup.ClearData();

    }
}
