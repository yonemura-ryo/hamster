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
    public SerializableDictionary<int,FacilityData> facilityDictionary;
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

/// <summary>
/// 出現中ハムスター1体ごとのデータ
///
/// 初期出現時：[bugAppearTime,bugFixTime] = [出現予定時刻, DateTime.MaxValue]
/// バグ修正開始後：[bugAppearTime,bugFixTime] = [出現予定時刻, 修正完了予定時刻]
/// </summary>
[System.Serializable]
public class HamsterData
{
    public int hamsterId;
    public int colorId;
    public string bugAppearTime;
    public string bugFixTime;
}

/// <summary>
/// 出現中ハムスターデータ
/// </summary>
[System.Serializable]
public class HamsterExistData
{
    public SerializableDictionary<int, HamsterData> hamsterExistDictionary;
}

/// <summary>
/// ハムスターの捕獲データ配列
///
/// key = {hamsterId}:{colorId}
/// </summary>
[System.Serializable]
public class HamsterCapturedListData
{
    public SerializableDictionary<string, HamsterCapturedData> capturedDataDictionary;
}

/// <summary>
/// ハムスターの捕獲データ
/// </summary>
[System.Serializable]
public class HamsterCapturedData
{
    public int hamsterId;
    public int colorId;
    public int capturedCount;
}


/// <summary>
/// ユーザー共通データ
/// </summary>
[System.Serializable]
public class UserCommonData
{
    public int coinCount;
    public int exp;
    public int userRank;
}

/// <summary>
/// ミッション進捗状態データ配列
/// </summary>
[System.Serializable]
public class MissionProgressListData
{
    public SerializableDictionary<int, MissionProgressData> missionProgressDictionary;
}

/// <summary>
/// ミッション進捗状態データ
/// </summary>
[System.Serializable]
public class MissionProgressData
{
    public int missionId;
    public int progressValue;
    public bool isCompleted;
    public bool isReceived;
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