using UnityEngine;
using System.Collections.Generic;
using BanpoFri;
using UnityEngine.AddressableAssets;



public class PlayerUnitGroup : MonoBehaviour
{
    public HashSet<PlayerUnit> ActiveBlocks = new HashSet<PlayerUnit>();

    public bool IsAllDeadCheck { get { return ActiveBlocks.Count == 0; } }

    public void Init()
    {
        ActiveBlocks.Clear();


    }

    public void AddBlock(int index)
    {
        var handle = Addressables.InstantiateAsync($"PlayerUnit_{index}", transform);

        var result = handle.WaitForCompletion();

        PlayerUnit instance = result.GetComponent<PlayerUnit>();

        instance.transform.localPosition = Vector3.zero;
        ActiveBlocks.Add(instance);
        instance.Set(index);

        ProjectUtility.SetActiveCheck(instance.gameObject, true);
    }

    public void ClearData()
    {
        foreach (var block in ActiveBlocks)
        {
            block.Clear();
            Destroy(block.gameObject);
        }
        ActiveBlocks.Clear();
    }
}
