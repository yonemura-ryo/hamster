using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugDialog : DialogBase
{
    private Action<int> addCoin = null;
    private Action clearData = null;
    private Action<int,int> acquireFood = null;
    private Action<int> addExp = null;
    private MissionManager missionManager = null;

    private List<int> missionOptions = new List<int>();
    
    [SerializeField] private Button addCoinButton;
    [SerializeField] private Button removeCoinButton;
    [SerializeField] private Button clearDataButton;
    [SerializeField] private Button acquireFoodButton;
    [SerializeField] private Button notificationButton;
    [SerializeField] private Button addExpButton;
    [SerializeField] private Button captureHamButton;
    [SerializeField] private Button releaseHamButton;
    [SerializeField] private TMP_Dropdown missionClearDropDown;
    [SerializeField] private Button missionClearButton;
    [SerializeField] private TMP_Dropdown missionResetDropDown;
    [SerializeField] private Button missionResetButton;



    public void Initialize(
        Action<int> addCoin,
        Action clearData,
        Action<int,int> acquireFood,
        Action<int> addExp,
        Action captureAllHamster,
        Action releaseAllHamster,
        MissionManager missionManager
        )
    {
        this.addCoin = addCoin;
        this.clearData = clearData;
        this.acquireFood = acquireFood;
        this.addExp = addExp;
        this.missionManager = missionManager;

        // ボタンイベント
        addCoinButton.onClick.AddListener(OnClickAddCoin);
        removeCoinButton.onClick.AddListener(OnClickRemoveCoin);
        clearDataButton.onClick.AddListener(OnClickClearData);
        acquireFoodButton.onClick.AddListener(OnClickAcquireFood);
        notificationButton.onClick.AddListener(OnClickNotification);
        addExpButton.onClick.AddListener(OnClickAddExp);
        captureHamButton.onClick.AddListener(()=>captureAllHamster());
        releaseHamButton.onClick.AddListener(() => releaseAllHamster());
        missionClearButton.onClick.AddListener(OnClickMissionClear);
        missionResetButton.onClick.AddListener(OnClickMissionReset);
        missionOptions = missionManager.getAllMissionIds();
        // ミッションIDドロップダウン
        missionClearDropDown.ClearOptions();
        missionResetDropDown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (int value in missionOptions)
        {
            options.Add(new TMP_Dropdown.OptionData(value.ToString()));
        }

        missionClearDropDown.AddOptions(options);
        missionResetDropDown.AddOptions(options);
    }

    public void OnClickAddCoin()
    {
        // コイン付与処理
        addCoin(1000);
    }

    public void OnClickRemoveCoin()
    {
        // コイン没収処理
        addCoin(-1000);
    }

    public void OnClickClearData()
    {
        // データ削除(PlayerPrefs削除)
        clearData();
    }

    public void OnClickAcquireFood()
    {
        acquireFood(1,5);
        acquireFood(2,5);
        acquireFood(3,5);
        acquireFood(4,5);
        acquireFood(5,5);
    }

    public void OnClickNotification()
    {
        // 10秒後に通知
        LocalPushNotification.RegisterChannel("debugChannel", "PushTest", "デバッグ用の通知");
        LocalPushNotification.AllClear();
        LocalPushNotification.AddSchedule(" テストプッシュ通知", "[デバッグ用]通知時間になりました！", 1, 10, "debugChannel");
    }

    public void OnClickAddExp()
    {
        // 経験値10追加する
        addExp(10);
    }

    /// <summary>
    /// ミッションクリアデバッグ
    /// </summary>
    public void OnClickMissionClear()
    {
        int index = missionClearDropDown.value;
        int missionId = missionOptions[index];
        missionManager.completeMission(missionId);
    }

    /// <summary>
    /// ミッションクリアデバッグ
    /// </summary>
    public void OnClickMissionReset()
    {
        int index = missionResetDropDown.value;
        int missionId = missionOptions[index];
        missionManager.resetMission(missionId);
    }
}