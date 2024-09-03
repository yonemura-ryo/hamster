using System;

public class HamsterModel
{
    private HamsterMaster hamsterMaster;
    private HamsterMaster fixedHamsterMaster = null;
    
    private float requireBbugfixTime = 0f;

    private DateTime finishBugFixDatetime = DateTime.MinValue;
    public HamsterModel(HamsterMaster hamsterMaster, HamsterMaster fixedHamsterMaster=null)
    {
        // hamsterIDから対象のデータを持って設定する
        this.hamsterMaster = hamsterMaster;
        this.fixedHamsterMaster = fixedHamsterMaster;
        requireBbugfixTime = hamsterMaster.BugFixTime;
    }

    /// <summary>
    /// ハム画像パス
    /// </summary>
    /// <returns></returns>
    public string GetHamsterImagePath()
    {
        return "Hamsters/" + hamsterMaster.ImagePath;
    }

    public string GetFixedHamsterImagePath()
    {
        if (fixedHamsterMaster == null)
        {
            return "";
        }
        return "Hamsters/" + fixedHamsterMaster.ImagePath;
    }

    /// <summary>
    /// バグID取得
    /// </summary>
    /// <returns></returns>
    public int GetBugID()
    {
        return hamsterMaster.BugID;
    }

    public float GetBugFixTime()
    {
        return requireBbugfixTime;
    }

    public Enum OnClickFixBug()
    {
        if (GetBugID() <= 0)
        {
            // バグじゃなければ一旦何もしない
            return HamsterDefine.ClickFunctionType.None;
        }

        if (finishBugFixDatetime == DateTime.MinValue)
        {
            // 終了日時を設定
            TimeSpan timeSpan = TimeSpan.FromSeconds(requireBbugfixTime);
            finishBugFixDatetime = DateTime.Now.Add(timeSpan);
            // バグ修正開始する
            return HamsterDefine.ClickFunctionType.StartFix;
        }

        if (finishBugFixDatetime < DateTime.Now)
        {
            finishFixBug();
            // 修正が完了しているので通常ハム獲得のち初期化
            return HamsterDefine.ClickFunctionType.FinishFix;
        }
        
        // ここにくるのは修正中のため、短縮するかダイアログ出す
        return HamsterDefine.ClickFunctionType.ProgressFix;
    }

    public void finishFixBug()
    {
        // 通常ハムのデータにする
        finishBugFixDatetime = DateTime.MinValue;
        hamsterMaster = fixedHamsterMaster;
    }
}