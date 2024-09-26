using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 施設単体のデータ
/// </summary>
[System.Serializable]
public class FacilityData
{
    public int facilityId;
    public int level;
}

/// <summary>
/// 施設配列のデータ
/// </summary>
[System.Serializable]
public class FacilityListData
{
    public FacilityData[] facilities;
}

/// <summary>
/// 餌単体情報
/// </summary>
[System.Serializable]
public class FoodData
{
    public int foodId;
    public int count;
}

/// <summary>
/// 所持餌データ
/// </summary>
[System.Serializable]
public class HavingFoodData
{
    public SerializableDictionary<int , FoodData> havingFoodDictionary;
}

[System.Serializable]
public class HamsterData
{
    public int hamsterId;
    public DateTime bugFixTime;
}

[System.Serializable]
public class HamsterExistData
{
    public SerializableDictionary<int, HamsterData> hamsterExistDictionary;
}

/// <summary>
/// ユーザー共通データ
/// </summary>
[System.Serializable]
public class UserCommonData
{
    public int coinCount;
}



[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializableKeyValuePair<TKey, TValue>> data = new();
        
    public void OnBeforeSerialize()
    {
        data.Clear();
        using var e = GetEnumerator();
        while (e.MoveNext())
        {
            data.Add(new SerializableKeyValuePair<TKey, TValue>(e.Current.Key, e.Current.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        foreach (var pair in data)
        {
            this[pair.Key] = pair.Value;
        }
    }
}

[System.Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    [SerializeField] public TKey Key;
    [SerializeField] public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}