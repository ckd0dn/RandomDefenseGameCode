using System;
using CWLib;
using UnityEngine;
using static Define;

public class EnhanceController : Singleton<EnhanceController>
{
    public int HeroEnhanceLevel;
    public int GoblinEnhanceLevel;
    public int UndeadEnhanceLevel;
    public int UnitSpawnProbabilityLevel;

    public int HeroEnhanceCost;
    public int GoblinEnhanceCost;
    public int UndeadEnhanceCost;
    public int UnitSpawnProbabilityCost;
    public int UnitSpawnProbabilityCostIncrease = 3;
    
    public event Action UpgradeUnitCallBack;

    public void UpgradeEnhanceUnit(UnitSpecies species)
    {
        switch (species)
        {
            case UnitSpecies.Hero:
                if (GameScene.Instance.Gem >= HeroEnhanceCost)
                {
                    HeroEnhanceLevel++;
                    GameScene.Instance.Gem -= HeroEnhanceCost;
                    HeroEnhanceCost++;
                }
                break;
            case UnitSpecies.Goblin:
                if (GameScene.Instance.Gem >= GoblinEnhanceCost)
                {
                    GoblinEnhanceLevel++;
                    GameScene.Instance.Gem -= GoblinEnhanceCost;
                    GoblinEnhanceCost++;
                }
                break;
            case UnitSpecies.Undead:
                if (GameScene.Instance.Gem >= UndeadEnhanceCost)
                {
                    UndeadEnhanceLevel++;
                    GameScene.Instance.Gem -= UndeadEnhanceCost;
                    UndeadEnhanceCost++;
                }
                break;
        }
        
        UpgradeUnitCallBack?.Invoke();
    }

    public void UpgradeEnhanceUnitSpawnProbability()
    {
        if (GameScene.Instance.Money >= UnitSpawnProbabilityCost)
        {
            UnitSpawnProbabilityLevel++;
            GameScene.Instance.Money -= UnitSpawnProbabilityCost;
            GameScene.Instance.SpawnUnitEnhanceLevel++;
            UnitSpawnProbabilityCost *= UnitSpawnProbabilityCostIncrease;
        }
    }
}
