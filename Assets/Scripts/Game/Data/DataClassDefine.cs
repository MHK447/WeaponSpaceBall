﻿using System;
using System.Numerics;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using BanpoFri;
using System.Linq;
using BanpoFri.Data;

public interface IReadOnlyData : ICloneable
{
	void Create();
}
public interface IClientData { }


public class FacilityData
{
	public bool IsOpen = false;

	public int FacilityIdx = 0;

	public System.Numerics.BigInteger MoneyCount = 0;

	public IReactiveProperty<int> CapacityCountProperty = new ReactiveProperty<int>(0);

	public FacilityData(int facilityidx, System.Numerics.BigInteger moneycount, bool isopen, int capacitycount)
	{
		IsOpen = isopen;
		FacilityIdx = facilityidx;
		MoneyCount = moneycount;
		CapacityCountProperty.Value = capacitycount;

	}

}

public class SelectFoodUpgradeData
{
	public int FoodIdx = 0;

	public int FoodCount = 0;

	public void ClearFoodData()
	{
		FoodIdx = 0;
		FoodCount = 0;
	}


	public void SelectFoodUpgrade(int foodidx)
	{
		if (FoodIdx == foodidx)
		{
			FoodCount += 1;
		}
		else
		{
			FoodIdx = foodidx;
			FoodCount = 0;
		}
	}
}


public class StageFishUpgradeData
{
	public int FishIdx = 0;
	public int Level = 0;

	public StageFishUpgradeData(int fishidx, int level)
	{
		FishIdx = fishidx;
		Level = level;
	}
}

public class NoticeData
{
	public int NotiIdx = 0;
	public Transform Target;

	public NoticeData(int notiidx, Transform target)
	{
		NotiIdx = notiidx;
		Target = target;
	}

}

public class PlayerData
{
	public IReactiveProperty<int> VehiclePropertyIdx = new ReactiveProperty<int>();
}



public class RaceData
{
	public IReactiveProperty<float> RaceProductCount = new ReactiveProperty<float>();
	public IReactiveProperty<float> RaceDistanceProperty = new ReactiveProperty<float>();

	public IReactiveProperty<float> BalanceValueProperty = new ReactiveProperty<float>();



	public void DataClear()
	{
		RaceProductCount.Value = 0;
		BalanceValueProperty.Value = 0;
	}


}






