using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataManager
{
    public Dictionary<string, Datas.MonsterData> MonsterDic { get; private set; } = new();
    public Dictionary<string, Datas.RewardMonsterData> RewardMonsterDic { get; private set; } = new();
    public Dictionary<string, Datas.UnitData> UnitDic { get; private set; } = new();
    public List<Datas.GradeProbabilityData> GradeProbList { get; private set; } = new();

    public void Init()
    {
        MonsterDic = LoadData<Datas.MonsterData>("MonsterData", "PrefabName");
        RewardMonsterDic = LoadData<Datas.RewardMonsterData>("RewardMonsterData", "PrefabName");
        UnitDic = LoadData<Datas.UnitData>("UnitData", "PrefabName");
        GradeProbList = new List<Datas.GradeProbabilityData>(LoadDataList<Datas.GradeProbabilityData>("GradeProbabilityData"));
    }

    private Dictionary<string, T> LoadData<T>(string fileName, string keyField = "Name") where T : new()
    {
        var table = new Dictionary<string, T>();

        var textAsset = Managers.Resource.Load<TextAsset>($"{fileName}.csv");
        if (textAsset == null)
        {
            Debug.LogError($"[DataManager] Failed to load {fileName}.csv");
            return table;
        }

        // 구분자 자동 감지 (탭, 쉼표 둘 다 지원)
        char delimiter = DetectDelimiter(textAsset.text);

        var lines = textAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            Debug.LogWarning($"[DataManager] Not enough lines in {fileName}.csv");
            return table;
        }

        var headers = lines[0].Split(delimiter);

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split(delimiter);
            var data = new T();
            var type = typeof(T);

            for (int j = 0; j < headers.Length && j < fields.Length; j++)
            {
                var header = headers[j].Trim();
                var value = fields[j].Trim();

                var fieldInfo = type.GetField(header, BindingFlags.Public | BindingFlags.Instance);
                var propInfo = type.GetProperty(header, BindingFlags.Public | BindingFlags.Instance);

                if (fieldInfo != null)
                {
                    var parsedValue = ConvertToType(value, fieldInfo.FieldType);
                    fieldInfo.SetValue(data, parsedValue);
                }
                else if (propInfo != null && propInfo.CanWrite)
                {
                    var parsedValue = ConvertToType(value, propInfo.PropertyType);
                    propInfo.SetValue(data, parsedValue);
                }
                else
                {
                    //Debug.LogWarning($"[DataManager] Field or Property '{header}' not found in {type.Name}");
                }
            }

            // Key 추출
            object keyObj = null;
            var keyFieldInfo = type.GetField(keyField, BindingFlags.Public | BindingFlags.Instance);
            var keyPropInfo = type.GetProperty(keyField, BindingFlags.Public | BindingFlags.Instance);

            if (keyFieldInfo != null)
                keyObj = keyFieldInfo.GetValue(data);
            else if (keyPropInfo != null)
                keyObj = keyPropInfo.GetValue(data);

            if (keyObj != null)
                table[keyObj.ToString()] = data;
            else
                Debug.LogWarning($"[DataManager] Key field {keyField} not found or null in {type.Name}");
        }

        return table;
    }

    
    // 리스트형 데이터용 LoadDataList 추가
    private List<T> LoadDataList<T>(string fileName) where T : new()
    {
        var list = new List<T>();

        var textAsset = Managers.Resource.Load<TextAsset>($"{fileName}.csv");
        if (textAsset == null)
        {
            Debug.LogError($"[DataManager] Failed to load {fileName}.csv");
            return list;
        }

        char delimiter = DetectDelimiter(textAsset.text);
        var lines = textAsset.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2) return list;

        var headers = lines[0].Split(delimiter);

        for (int i = 1; i < lines.Length; i++)
        {
            var fields = lines[i].Split(delimiter);
            var data = new T();
            var type = typeof(T);

            for (int j = 0; j < headers.Length && j < fields.Length; j++)
            {
                var header = headers[j].Trim();
                var value = fields[j].Trim();

                var fieldInfo = type.GetField(header, BindingFlags.Public | BindingFlags.Instance);
                var propInfo = type.GetProperty(header, BindingFlags.Public | BindingFlags.Instance);

                if (fieldInfo != null)
                {
                    var parsedValue = ConvertToType(value, fieldInfo.FieldType);
                    fieldInfo.SetValue(data, parsedValue);
                }
                else if (propInfo != null && propInfo.CanWrite)
                {
                    var parsedValue = ConvertToType(value, propInfo.PropertyType);
                    propInfo.SetValue(data, parsedValue);
                }
            }

            list.Add(data);
        }

        return list;
    }
    private object ConvertToType(string value, Type type)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(float)) return float.Parse(value);
            if (type == typeof(bool)) return bool.Parse(value);
            if (type == typeof(string)) return value;
            if (type.IsEnum) return Enum.Parse(type, value, true);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[DataManager] Failed to parse '{value}' to {type.Name}: {ex.Message}");
        }

        return null;
    }

    private char DetectDelimiter(string text)
    {
        int commaCount = text.Split(',').Length;
        int tabCount = text.Split('\t').Length;

        return tabCount > commaCount ? '\t' : ','; // 탭이 더 많으면 탭, 아니면 쉼표
    }
}
