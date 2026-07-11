using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAudioSettings", menuName = "Scriptable Objects/WeaponAudioSettings")]
public class WeaponAudioSettings : ScriptableObject
{
    [Header("Weapon Sound")]
    public AudioClip SlashClip;
    public AudioClip AxeClip;
    public AudioClip ArrowClip;
    public AudioClip BombClip;
    public AudioClip BoomerangClip;
    public AudioClip KinifeClip;
    public AudioClip PinwheelClip;
    public AudioClip ShurikenClip;
    public AudioClip ShieldClip;

    [Header("Sound Effect")]
    [Tooltip("宝箱所得時の獲得効果音")]
    public AudioClip PickupSoundClip;
    [Tooltip("クリック音の効果音")]
    public AudioClip ClickClip;
}
