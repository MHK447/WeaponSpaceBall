using UnityEngine;
using System.Collections.Generic;
using BanpoFri;
using UnityEngine.AddressableAssets;


public class WeaponController : MonoBehaviour
{
    [HideInInspector]
    public List<WeaponBase> WeaponList = new List<WeaponBase>();

    private UnitBase Unit = null;


    public void Set(UnitBase unit)
    {
        Unit = unit;

        foreach (var weapon in WeaponList)
        {
            ProjectUtility.SetActiveCheck(weapon.gameObject, false);
        }
    }


    public void AddWeapon(int weaponidx)
    {
        var td = Tables.Instance.GetTable<WeaponInfo>().GetData(weaponidx);

        if (td == null) return;

        var findweapon = WeaponList.Find(x => x.gameObject.activeSelf == false && x.GetWeaponIdx == weaponidx);

        if (findweapon != null)
        {
            findweapon.Set(weaponidx);
            ProjectUtility.SetActiveCheck(findweapon.gameObject, true);
        }
        else
        {
            var handle = Addressables.InstantiateAsync($"Weapon_{weaponidx}", transform);

            var result = handle.WaitForCompletion();

            if (result != null)
            {
                var weapon = handle.Result.GetComponent<WeaponBase>();
                weapon.Set(weaponidx);
                weapon.transform.SetParent(Unit.GetBodyTr);
                ProjectUtility.SetActiveCheck(weapon.gameObject, true);
                WeaponList.Add(weapon);
            }
            else
            {
                BpLog.LogError($"Weapon_{weaponidx} not found");
            }
        }
    }

}
