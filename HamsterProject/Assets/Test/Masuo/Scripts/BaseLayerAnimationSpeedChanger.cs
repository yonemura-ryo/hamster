using UnityEngine;

public class BaseLayerAnimationSpeedChanger : MonoBehaviour
{
    // UnityエディタからAnimatorを設定できるようにする
    [SerializeField] private Animator animator;

    // Animatorの初期速度を保持するフィールド
    private float originalSpeed;

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animatorが設定されていません！");
            return;
        }

        // 初期状態の速度を保持
        originalSpeed = animator.speed;
    }

    /// <summary>
    /// Base Layerで再生中のアニメーション速度をイベントの引数で変更するメソッド
    /// </summary>
    /// <param name="speedMultiplier">変更したい速度の倍率（例: 2なら2倍速）</param>
    public void ChangeBaseLayerSpeed(float speedMultiplier)
    {
        if(animator == null)
        {
            Debug.LogWarning("Animatorが設定されていません！");
            return;
        }

        // Base Layerにて再生しているアニメーションの速度に倍率を適用
        // ※ Animator.speedは全レイヤーに影響するため、Base Layerのみで運用している場合にご利用ください
        animator.speed = originalSpeed * speedMultiplier;
    }

    /// <summary>
    /// アニメーション速度を初期状態に戻すメソッド
    /// </summary>
    public void ResetBaseLayerSpeed()
    {
        if(animator == null)
        {
            Debug.LogWarning("Animatorが設定されていません！");
            return;
        }

        animator.speed = originalSpeed;
    }
}