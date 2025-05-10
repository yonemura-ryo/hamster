using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MissionDialog : DialogBase
{
    [SerializeField] GameObject missionContentPrefab = null;
    [SerializeField] ScrollRect scrollRect = null;
    [SerializeField] RectTransform easyContent = null;
    [SerializeField] RectTransform longContent = null;
    [SerializeField] CustomButton easyTabButton = null;
    [SerializeField] CustomButton longTabButton = null;
    [SerializeField] CustomButton allReceiveButton = null;

    private IDialogContainer dialogContainer;
    private IReadOnlyDictionary<int, MissionMaster> missionMasters = null;
    private Action<int, int, int> receiveMission = null;
    private MissionProgressListData missionProgressListData;

    private class MissionObjectInstantiateInfo
    {
        GameObject contentObject;
        RectTransform content;
        public MissionObjectInstantiateInfo(GameObject contentObject, RectTransform content)
        {
            this.contentObject = contentObject;
            this.content = content;
        }

        public void setParentContent()
        {
            contentObject.transform.SetParent(content);
            contentObject.transform.localScale = Vector3.one;
        }
    }

    private void Start()
    {
        // 初期化
        easyContent.gameObject.SetActive(true);
        longContent.gameObject.SetActive(false);
        scrollRect.content = easyContent;
        // ボタンのリスナー登録
        easyTabButton.OnClickAsObservable().Subscribe(_ =>
        {
            easyContent.gameObject.SetActive(true);
            longContent.gameObject.SetActive(false);
            scrollRect.content = easyContent;
        });
        longTabButton.OnClickAsObservable().Subscribe(_ =>
        {
            easyContent.gameObject.SetActive(false);
            longContent.gameObject.SetActive(true);
            scrollRect.content = longContent;
        });

        // まとめて受け取り
        allReceiveButton.OnClickAsObservable().Subscribe(_ =>
        {
            ReceiveAllMission();
        });
    }

    public void Initialize(
         IDialogContainer dialogContainer,
         MissionProgressListData missionProgressListData,
         Action<int, int, int> receiveMission
        )
    {
        this.dialogContainer = dialogContainer;
        this.receiveMission = receiveMission;
        missionMasters = MasterData.DB.MissionMaster;
        this.missionProgressListData = missionProgressListData;
        renderMission();
    }

    /// <summary>
    /// ミッションを表示する
    /// </summary>
    private void renderMission()
    {
        // contentの子オブジェクトを削除する
        for (int i = easyContent.childCount - 1; i >= 0; i--)
        {
            Transform child = easyContent.GetChild(i);
            Destroy(child.gameObject);
        }
        for (int i = longContent.childCount - 1; i >= 0; i--)
        {
            Transform child = longContent.GetChild(i);
            Destroy(child.gameObject);
        }
        List<MissionObjectInstantiateInfo> completedMissionList = new List<MissionObjectInstantiateInfo>();
        List<MissionObjectInstantiateInfo> progressMissionList = new List<MissionObjectInstantiateInfo>();
        List<MissionObjectInstantiateInfo> receivedMissionList = new List<MissionObjectInstantiateInfo>();
        // TODO クリア状況によって並び替え
        foreach ((int missionId, MissionMaster missionMaster) in missionMasters)
        {
            RectTransform targetContent = easyContent;
            // TODO 定数化
            if (missionMaster.MissionTab == 2)
            {
                targetContent = longContent;
            }
            int progressValue = 0;
            bool isCompleted = false;
            bool isReceived = false;
            // ユーザーデータで上書き
            if (missionProgressListData.missionProgressDictionary.ContainsKey(missionId))
            {
                MissionProgressData missionProgressData = missionProgressListData.missionProgressDictionary[missionId];
                progressValue = missionProgressData.progressValue;
                isCompleted = missionProgressData.isCompleted;
                isReceived = missionProgressData.isReceived;
                //Debug.Log("missionData:" + "missionID[" + missionId.ToString() + "], isComp["+ isCompleted + "], isRec[" + isReceived + "]");
            }
            GameObject missionContentObject = Instantiate(missionContentPrefab, Vector3.zero, Quaternion.identity);
            missionContentObject.GetComponent<MissionContent>().Initialize(
                missionId,
                // TODO リソース仮（報酬次第で引っ張り方検討）
                Resources.Load<Sprite>($"2DAssets/Images/SpriteAtlasImages/item/item_{missionMaster.RewardItemId}"),
                missionMaster.RewardItemNum,
                missionMaster.MissionText,
                missionMaster.CompleteValue,
                // 進行度
                progressValue,
                // 達成状態
                isCompleted,
                // 受取状態
                isReceived,
                (missionId) => { ReceiveMission(missionId, missionMaster.RewardItemId, missionMaster.RewardItemNum); }
                );
            // ステータスごとにListに格納する
            if (isReceived)
            {
                receivedMissionList.Add(new MissionObjectInstantiateInfo(missionContentObject, targetContent));
            }
            else if (isCompleted)
            {
                completedMissionList.Add(new MissionObjectInstantiateInfo(missionContentObject, targetContent));
            }
            else
            {
                progressMissionList.Add(new MissionObjectInstantiateInfo(missionContentObject, targetContent));
            }
        }

        // ステータス順でスクロールに追加する(未受取＞進行中＞受取済み)
        foreach (MissionObjectInstantiateInfo missionObjectInstantiateInfo in completedMissionList)
        {
            missionObjectInstantiateInfo.setParentContent();
        }
        foreach (MissionObjectInstantiateInfo missionObjectInstantiateInfo in progressMissionList)
        {
            missionObjectInstantiateInfo.setParentContent();
        }
        foreach (MissionObjectInstantiateInfo missionObjectInstantiateInfo in receivedMissionList)
        {
            missionObjectInstantiateInfo.setParentContent();
        }
    }

    /// <summary>
    /// ミッション受取
    /// </summary>
    /// <param name="missionId"></param>
    /// <param name="rewardItemId"></param>
    /// <param name="rewardItemNum"></param>
    private void ReceiveMission(int missionId, int rewardItemId, int rewardItemNum)
    {
        receiveMission?.Invoke(missionId, rewardItemId, rewardItemNum);
        gameObject.SetActive(false);
        Dictionary<int, int> receiveItemList = new Dictionary<int, int>();
        receiveItemList.Add(rewardItemId, rewardItemNum);
        MissionReceiveDialog missionReceiveDialog = dialogContainer.Show<MissionReceiveDialog>(OnCloseNextDialog);
        missionReceiveDialog.Initialize(receiveItemList);
    }

    /// <summary>
    /// ミッションまとめて受取
    /// </summary>
    private void ReceiveAllMission()
    {
        // TODO アクティブなタブだけにする？
        List<int> missionIdList = new List<int>();
        Dictionary<int, int> receiveItemList = new Dictionary<int, int>();
        // 受取可能なミッションを探索(Dictionaryの変更を考慮して別Listに一度格納する)
        foreach (KeyValuePair<int, MissionProgressData>kvp in missionProgressListData.missionProgressDictionary)
        {
            MissionProgressData missionProgressData = kvp.Value;
            // 受取可能
            if (missionProgressData.isCompleted && !missionProgressData.isReceived)
            {
                missionIdList.Add(kvp.Key);
            }
        }
        foreach(int missionId in missionIdList)
        {
            MissionMaster missionMaster = missionMasters[missionId];
            receiveMission?.Invoke(
                    missionId,
                    missionMaster.RewardItemId,
                    missionMaster.RewardItemNum
                    );
            if (receiveItemList.ContainsKey(missionMaster.RewardItemId))
            {
                // 付与アイテム個数をまとめる
                receiveItemList[missionMaster.RewardItemId] = receiveItemList[missionMaster.RewardItemId] + missionMaster.RewardItemNum;
            }
            else
            {
                receiveItemList.Add(missionMaster.RewardItemId, missionMaster.RewardItemNum);
            }
        }
        gameObject.SetActive(false);
        MissionReceiveDialog missionReceiveDialog = dialogContainer.Show<MissionReceiveDialog>(OnCloseNextDialog);
        missionReceiveDialog.Initialize(receiveItemList);
    }

    public override void Show(Action closeAction = null)
    {
        base.Show(closeAction);
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnCloseNextDialog()
    {
        // 再読み込み
        renderMission();
        gameObject.SetActive(true);
    }
}
