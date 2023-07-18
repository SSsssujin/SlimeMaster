using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
	// JSON
    // [Serializable]
    // public class PlayerData
    // {
    // 	public int level;
    // 	public int maxHp;
    // 	public int attack;
    // 	public int totalExp;
    // }
    //
    // [Serializable]
    // public class PlayerDataLoader : ILoader<int, PlayerData>
    // {
    // 	public List<PlayerData> stats = new List<PlayerData>();
    //
    // 	public Dictionary<int, PlayerData> MakeDict()
    // 	{
    // 		Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
    // 		foreach (PlayerData stat in stats)
    // 			dict.Add(stat.level, stat);
    // 		return dict;
    // 	}
    // }
    
    public class PlayerData
    {
	    [XmlAttribute]
	    public int level;
	    [XmlAttribute]
	    public int maxHp;
	    [XmlAttribute]
	    public int attack;
	    [XmlAttribute]
	    public int totalExp;
    }

    [Serializable, XmlRoot("PlayerDatas")]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
	    [XmlElement("PlayerData")]
	    public List<PlayerData> stats = new List<PlayerData>();

	    public Dictionary<int, PlayerData> MakeDict()
	    {
		    Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
		    foreach (PlayerData stat in stats)
			    dict.Add(stat.level, stat);
		    return dict;
	    }
    }
    
    public class MonsterData
    {
	    [XmlAttribute]
	    public string name;
	    [XmlAttribute]
	    public string prefab;
	    [XmlAttribute]
	    public int level;
	    [XmlAttribute]
	    public int maxHp;
	    [XmlAttribute]
	    public int attack;
	    [XmlAttribute]
	    public float speed;
	    // DropData
	    // - 일정 확률로
	    // - 어떤 아이템을 (보석, 스킬 가차, 골드, 고기)
	    // - 몇 개 드랍할지?
    }
}