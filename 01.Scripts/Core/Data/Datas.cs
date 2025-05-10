using System;
using UnityEngine;
using static Define;

public class Datas : MonoBehaviour
{
    [Serializable]
    public class MonsterData
    {
        public string PrefabName;
        public int Hp;
    }
    
    [Serializable]
    public class RewardMonsterData
    {
        public string PrefabName;
        public int Hp;
        public int Gem;
        public int Money;
    }
    
    [Serializable]
    public class UnitData
    {
        public string PrefabName;
        public string Name;
        public float Damage;
        public float AttackDelay;
        public float AttackRange;
        public float Scale;
        public UnitGrade Grade;
        public string Description;
        public UnitSpecies Species;

        public string GetSpeciesName()
        {
            switch (Species)
            {
                 case UnitSpecies.Goblin:
                     return "고블린";
                 case UnitSpecies.Undead:
                     return "언데드";
                 case UnitSpecies.Hero:
                     return "영웅";
                 default:
                     return "";
            }
        }
    }
    
    [Serializable]
    public class GradeProbabilityData
    {
        public UnitGrade Grade;
        public int EnhanceLevel;
        public float Probability;
    }
}
