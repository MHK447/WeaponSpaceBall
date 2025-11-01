using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
public class PlayerUnit : UnitBase
{


    private int UnitIdx = 0;



    public override void Set(int idx)
    {   
        base.Set(idx);

        UnitIdx = idx;

        var td = Tables.Instance.GetTable<PlayerUnitInfo>().GetData(idx);

        if(td != null)
        {
            //UnitImg.sprite = AtlasManager.Instance.GetSprite(Atlas.Atlas_UI_Common, td.image);

            Status.SetStatus(td.base_dmg, td.base_hp, td.atk_speed);

            foreach(var weaponidx in td.weapon_idx)
            {
                WeaponController.AddWeapon(weaponidx);
            }
        }
    }

}
