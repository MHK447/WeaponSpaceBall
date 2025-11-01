using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    private int WeaponIdx = 0;

    public int GetWeaponIdx {get { return WeaponIdx; }}

    public void Set(int idx)
    {
        WeaponIdx = idx;




    }


}
