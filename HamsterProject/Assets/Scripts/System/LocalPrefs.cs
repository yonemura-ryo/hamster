using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Í���PlayerPrefs
/// �Q�l�Fhttps://blog.naichilab.com/entry/encrypted-playerprefs
/// </summary>
public static class LocalPrefs
{
    /// <summary>
    /// int�^�̃Z�[�u
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveInt(string key, int value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// float�^�̃Z�[�u
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveFloat(string key, float value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// bool�^�̃Z�[�u
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SaveBool(string key, bool value)
    {
        string valueString = value.ToString();
        SaveString(key, valueString);
    }

    /// <summary>
    /// �N���X�f�[�^�̃Z�[�u
    /// �N���X��[System.Serializable]�K�{
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public static void Save<T>(string key, T data)
    {
        string dataJson = JsonUtility.ToJson(data);
        SaveString(key, dataJson);
    }

    /// <summary>
    /// string�^�̃Z�[�u
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
    /// int�^�̃��[�h
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
    /// float�^�̃��[�h
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
    /// bool�^�̃��[�h
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
    /// �N���X�f�[�^�̃��[�h
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
    /// string�^�̃��[�h
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
    /// ������̈Í����E������
    /// �Q�l�Fhttp://dobon.net/vb/dotnet/string/encryptstring.html
    /// </summary>
    private static class EncryptString
    {
        const string Pass = "CpVuJRzLgQa6qs8rjgay";
        const string Salt = "YmmfUUGcYuhtg3P55xTc";

        static System.Security.Cryptography.RijndaelManaged rijndael;

        static EncryptString()
        {
            // RijndaelManaged�I�u�W�F�N�g�̍쐬
            rijndael = new System.Security.Cryptography.RijndaelManaged();
            byte[] key, iv;
            GenerateKeyFromPassword(rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;
        }

        /// <summary>
        /// ������̈Í���
        /// </summary>
        /// <param name="sourceString">�Í������镶����</param>
        /// <returns>�Í������ꂽ������</returns>
        public static string Encrypt(string sourceString)
        {
            // ��������o�C�g�^�z��ɕϊ�����
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);
            //�Ώ̈Í����I�u�W�F�N�g�̍쐬
            System.Security.Cryptography.ICryptoTransform encryptor = rijndael.CreateEncryptor();
            //�o�C�g�^�z����Í�������
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //����
            encryptor.Dispose();
            //�o�C�g�^�z��𕶎���ɕϊ����ĕԂ�
            return System.Convert.ToBase64String(encBytes);
        }

        /// <summary>
        /// �Í������ꂽ������𕜍���
        /// </summary>
        /// <param name="sourceString">�Í������ꂽ������</param>
        /// <returns>���������ꂽ������</returns>
        public static string Decrypt(string sourceString)
        {
            //��������o�C�g�^�z��ɖ߂�
            byte[] strBytes = System.Convert.FromBase64String(sourceString);
            //�Ώ̈Í����I�u�W�F�N�g�̍쐬
            System.Security.Cryptography.ICryptoTransform decryptor = rijndael.CreateDecryptor();
            //�o�C�g�^�z��𕜍�������
            //�������Ɏ��s����Ɨ�OCryptographicException������
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //����
            decryptor.Dispose();
            //�o�C�g�^�z��𕶎���ɖ߂��ĕԂ�
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// �p�X���[�h���狤�L�L�[�Ə����x�N�^�𐶐�
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="key"></param>
        /// <param name="blockSize"></param>
        /// <param name="iv"></param>
        private static void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(Salt);
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(Pass, salt);
            //.NET Framework 1.1�ȉ��̎��́APasswordDeriveBytes���g�p����
            //System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
            //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
            // ���������񐔂��w�肷�� �f�t�H���g��1000��
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}
