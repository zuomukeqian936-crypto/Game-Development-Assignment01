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
    public AudioClip PickupSoundClip;
}
