using UnityEngine;
using System.Collections.Generic;
using System;
public class UnitBase : MonoBehaviour
{
    public class UnitStatus
    {
        public double Damage = 0;
        public double Hp = 0;
        public double AtkSpeed = 0;

        public void SetStatus(double damage, double hp, double atkSpeed)
        {
            Damage = damage;
            Hp = hp;
            AtkSpeed = atkSpeed;
        }
    }


    [SerializeField]
    protected SpriteRenderer UnitImg;

    public Transform GetBodyTr { get { return UnitImg.transform; } }

    [SerializeField]
    protected bool HasHP = true;


    [HideInInspector]
    public bool IsDead = false;


    [HideInInspector]
    public WeaponController WeaponController = null;

    protected UnitStatus Status = new UnitStatus();


    protected InGameHpProgress HpProgress;

    [HideInInspector]
    public double StartHp;
    private double _Hp;
    public double Hp
    {
        get
        {
            return _Hp;
        }
        set
        {
            value = Math.Max(0, value);
            if (value == _Hp) return;
            _Hp = value;
            if (HpProgress != null) HpProgress.SetHpText(_Hp, StartHp);
            if (_Hp <= 0 && HasHP && !IsDead)
            {
                Dead();
            }
        }
    }

    public virtual void Awake()
    {
        WeaponController = GetComponent<WeaponController>();
    }


    public virtual void Set(int idx)
    {
        WeaponController.Set(this);

        IsDead = false;
        ShowHP();
    }


    public virtual void Clear()
    {

    }

    public virtual void Dead()
    {

    }


    protected void ShowHP()
    {
        if (HpProgress != null)
        {
            ProjectUtility.SetActiveCheck(HpProgress.gameObject, true);
            return;
        }

        GameRoot.Instance.UISystem.LoadFloatingUI<InGameHpProgress>(ui =>
        {
            HpProgress = ui;
            HpProgress.Init(this.transform);
            ui.SetHpText(Hp, StartHp);
        });
    }





}
