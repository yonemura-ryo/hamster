using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 拡張用ボタンクラス
/// </summary>
public class MyButton : Button
{
    [SerializeField] public TextMeshProUGUI TMPText;
    [SerializeField] public ButtonSoundType SoundType = ButtonSoundType.Push;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;
        base.OnPointerClick(eventData);

        if (SoundType == ButtonSoundType.None) return;
        SystemScene.Instance.SoundPlayer.PlaySe(ButtonDefine.SoundTypeNames[(int)SoundType]);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MyButton))]
    public class CustomButtonEditor : UnityEditor.UI.ButtonEditor
    {
        /// <summary>
        /// インスペクターにアタッチする用.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var component = (MyButton)target;

            component.SoundType = (ButtonSoundType)EditorGUILayout.EnumPopup("Sound Type", component.SoundType);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TMPText"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
