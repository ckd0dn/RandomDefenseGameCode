using System;
using System.Collections.Generic;
using System.Linq;
using CWLib;
using UnityEngine;
using static Define;

public class ObjectManager : BaseObjectManager
{
    public HashSet<Tile> Tiles { get; private set; } = new HashSet<Tile>();
    public HashSet<Monster> Monsters { get; private set; } = new HashSet<Monster>();
    public HashSet<RewardMonster> RewardMonsters { get; private set; } = new HashSet<RewardMonster>();
    public HashSet<Unit> Units { get; private set; } = new HashSet<Unit>();
    public HashSet<HpBar> HpBars { get; private set; } = new HashSet<HpBar>();
    public HashSet<DirectionIndicator> DirectionIndicators { get; private set; } = new HashSet<DirectionIndicator>();
    public HashSet<Projectile> Projectiles { get; private set; } = new HashSet<Projectile>();
    public HashSet<SkillObject> SkillObjects { get; private set; } = new HashSet<SkillObject>();
    
    // Event
    
    public event Action MonsterCountChangedCallBack;
    
    public override T Spawn<T>(string key)
    {
        System.Type type = typeof(T);
        
        if (type == typeof(Tile))
        {
            GameObject go = Managers.Resource.Instantiate(key);
            Tile tile = go.GetComponent<Tile>();
            
            Tiles.Add(tile);
            return tile as T;
        }
        else if (type == typeof(Monster))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
            Monster monster = go.GetComponent<Monster>();
            monster.InitData(key);
            monster.Init();

            Monsters.Add(monster);
            MonsterCountChangedCallBack?.Invoke();
            return monster as T;
        }
        else if (type == typeof(RewardMonster))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
            RewardMonster monster = go.GetComponent<RewardMonster>();
            monster.InitData(key);
            monster.Init();
            
            RewardMonsters.Add(monster);
            return monster as T;
        }
        else if (typeof(Unit).IsAssignableFrom(type))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
        
            T unit = go.GetComponent<T>();

            (unit as Unit)?.InitData(key); 
            (unit as Unit)?.Init(); 
        
            Units.Add(unit as Unit); 

            return unit;
        }
        else if (type == typeof(HpBar))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
            HpBar h = go.GetComponent<HpBar>();
            h.transform.position = Vector3.zero;
            
            HpBars.Add(h);
            return h as T;
        }
        else if (type == typeof(DirectionIndicator))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
            DirectionIndicator directionIndicator = go.GetComponent<DirectionIndicator>();

            DirectionIndicators.Add(directionIndicator);
            return directionIndicator as T;
        }
        else if (typeof(Projectile).IsAssignableFrom(type))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
        
            T projectile = go.GetComponent<T>();
            
            Projectiles.Add(projectile as Projectile); 

            return projectile;
        }
        else if (typeof(SkillObject).IsAssignableFrom(type))
        {
            GameObject go = Managers.Resource.Instantiate(key, pooling: true);
        
            T skill = go.GetComponent<T>();
            
            SkillObjects.Add(skill as SkillObject); 

            return skill;
        }
        return null;
    }

    public override void Despawn<T>(T obj)
    {
        System.Type type = typeof(T);

        if (type == typeof(Monster))
        {
            Monsters.Remove(obj as Monster);
            Managers.Resource.Destroy(obj.gameObject);
            MonsterCountChangedCallBack?.Invoke();
        }
        else if (type == typeof(RewardMonster))
        {
            RewardMonsters.Remove(obj as RewardMonster);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Unit))
        {
            Units.Remove(obj as Unit);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(HpBar))
        {
            (obj as HpBar)?.Destroy();
            HpBars.Remove(obj as HpBar);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(DirectionIndicator))
        {
            DirectionIndicators.Remove(obj as DirectionIndicator);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Projectile))
        {
            Projectiles.Remove(obj as Projectile);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(SkillObject))
        {
            SkillObjects.Remove(obj as SkillObject);
            Managers.Resource.Destroy(obj.gameObject);
        }
   
    }
    
    public void ClearObjects()
    {
        // var tilesToDespawn = Cards.ToList();
        // foreach (var tile in tilesToDespawn)
        // {
        //     Despawn(tile); 
        // }
        // Cards.Clear();
    }
    
    public void SelectTile(Tile selectedTile)
    {
        foreach (Tile tile in Tiles)
        {
            bool isSelected = tile == selectedTile;
            tile.SetSelected(isSelected);
        }
    }
    
    public bool HasThreeOrMoreUnitsWithSameName(Unit targetUnit)
    {
        string targetName = targetUnit.Name;
        int count = Units.Count(unit => unit.Name == targetName);
        return count >= 3;
    }
    
}