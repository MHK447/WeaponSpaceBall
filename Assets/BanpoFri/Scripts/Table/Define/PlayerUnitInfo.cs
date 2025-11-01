using UnityEngine;
using System.Collections.Generic;

namespace BanpoFri
{
    [System.Serializable]
    public class PlayerUnitInfoData
    {
        [SerializeField]
		private int _idx;
		public int idx
		{
			get { return _idx;}
			set { _idx = value;}
		}
		[SerializeField]
		private List<int> _weapon_idx;
		public List<int> weapon_idx
		{
			get { return _weapon_idx;}
			set { _weapon_idx = value;}
		}
		[SerializeField]
		private int _base_dmg;
		public int base_dmg
		{
			get { return _base_dmg;}
			set { _base_dmg = value;}
		}
		[SerializeField]
		private int _base_hp;
		public int base_hp
		{
			get { return _base_hp;}
			set { _base_hp = value;}
		}
		[SerializeField]
		private float _atk_speed;
		public float atk_speed
		{
			get { return _atk_speed;}
			set { _atk_speed = value;}
		}
		[SerializeField]
		private string _name;
		public string name
		{
			get { return _name;}
			set { _name = value;}
		}
		[SerializeField]
		private string _image;
		public string image
		{
			get { return _image;}
			set { _image = value;}
		}

    }

    [System.Serializable]
    public class PlayerUnitInfo : Table<PlayerUnitInfoData, int>
    {
    }
}

