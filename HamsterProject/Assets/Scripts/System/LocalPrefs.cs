using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暗号化PlayerPrefs
/// 参考：https://blog.naichilab.com/entry/encrypted-playerprefs
/// </summary>
public static class LocalPrefs
{
    /// <summary>
    /// int型のセーブ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveInt(string key, int value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// float型のセーブ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveFloat(string key, float value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// bool型のセーブ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveBool(string key, bool value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// クラスデータのセーブ
    /// クラスは[System.Serializable]必須
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void Save<T>(string key, T data)
    {
        string dataJson = JsonUtility.ToJson(data);
        SaveString(key, dataJson);
    }

    /// <summary>
    /// string型のセーブ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveString(string key, string value)
    {
        string encKey = EncryptString.Encrypt(key);
        string encValue = EncryptString.Encrypt(value.ToString());
        PlayerPrefs.SetString(encKey, encValue);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// int型のロード
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defult"></param>
    /// <returns></returns>
    public static int LoadInt(string key, int defult)
    {
        string defaultValueString = defult.ToString();
        string valueString = LoadString(key, defaultValueString);

        int res;
        if (int.TryParse(valueString, out res))
        {
            return res;
        }
        return defult;
    }

    /// <summary>
    /// float型のロード
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defult"></param>
    /// <returns></returns>
    public static float LoadFloat(string key, float defult)
    {
        string defaultValueString = defult.ToString();
        string valueString = LoadString(key, defaultValueString);

        float res;
        if (float.TryParse(valueString, out res))
        {
            return res;
        }
        return defult;
    }

    /// <summary>
    /// bool型のロード
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defult"></param>
    /// <returns></returns>
    public static bool LoadBool(string key, bool defult)
    {
        string defaultValueString = defult.ToString();
        string valueString = LoadString(key, defaultValueString);

        bool res;
        if (bool.TryParse(valueString, out res))
        {
            return res;
        }
        return defult;
    }

    /// <summary>
    /// クラスデータのロード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Load<T>(string key)
    {
        string defaultValueString = string.Empty;
        string valueString = LoadString(key, defaultValueString);

        if(valueString == defaultValueString)
        {
            return default;
        }

        return JsonUtility.FromJson<T>(valueString);
    }

    /// <summary>
    /// string型のロード
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defult"></param>
    /// <returns></returns>
    public static string LoadString(string key, string defult)
    {
        string encKey = EncryptString.Encrypt(key);
        string encString = PlayerPrefs.GetString(encKey, string.Empty);
        if (string.IsNullOrEmpty(encString))
        {
            return defult;
        }
        string decryptedValueString = EncryptString.Decrypt(encString);
        return decryptedValueString;
    }

    /// <summary>
    /// 文字列の暗号化・複合化
    /// 参考：http://dobon.net/vb/dotnet/string/encryptstring.html
    /// </summary>
    private static class EncryptString
    {
        const string Pass = "CpVuJRzLgQa6qs8rjgay";
        const string Salt = "YmmfUUGcYuhtg3P55xTc";

        static System.Security.Cryptography.RijndaelManaged rijndael;

        static EncryptString()
        {
            // RijndaelManagedオブジェクトの作成
            rijndael = new System.Security.Cryptography.RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
        }

        /// <summary>
        /// 文字列の暗号化
        /// </summary>
        /// <param name="sourceString">暗号化する文字列</param>
        /// <returns>暗号化された文字列</returns>
        public static string Encrypt(string sourceString)
        {
            // 文字列をバイト型配列に変換する
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);
            //対称暗号化オブジェクトの作成
            System.Security.Cryptography.ICryptoTransform encryptor = rijndael.CreateEncryptor();
            //バイト型配列を暗号化する
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //閉じる
            encryptor.Dispose();
            //バイト型配列を文字列に変換して返す
            return System.Convert.ToBase64String(encBytes);
        }

        /// <summary>
        /// 暗号化された文字列を復号化
        /// </summary>
        /// <param name="sourceString">暗号化された文字列</param>
        /// <returns>復号化された文字列</returns>
        public static string Decrypt(string sourceString)
        {
            //文字列をバイト型配列に戻す
            byte[] strBytes = System.Convert.FromBase64String(sourceString);
            //対称暗号化オブジェクトの作成
            System.Security.Cryptography.ICryptoTransform decryptor = rijndael.CreateDecryptor();
            //バイト型配列を復号化する
            //復号化に失敗すると例外CryptographicExceptionが発生
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //閉じる
            decryptor.Dispose();
            //バイト型配列を文字列に戻して返す
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// パスワードから共有キーと初期ベクタを生成
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="key"></param>
        /// <param name="blockSize"></param>
        /// <param name="iv"></param>
        private static void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(Salt);
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(Pass, salt);
            //.NET Framework 1.1以下の時は、PasswordDeriveBytesを使用する
            //System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
            //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
            // 反復処理回数を指定する デフォルトで1000回
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}
