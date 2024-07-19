using System;

/// <summary>
/// ダイアログ管理インターフェース.
/// </summary>
public interface IDialogContainer
{
    /// <summary>
    /// ダイアログ表示.
    /// </summary>
    /// <typeparam name="T">Dialog Base</typeparam>
    /// <returns></returns>
    T Show<T>(Action closeAction) where T : DialogBase;
}
