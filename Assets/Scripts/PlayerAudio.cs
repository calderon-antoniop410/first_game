using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sword SFX Clips")]
    [SerializeField] private AudioClip swordSwingSound;

    private void Awake()
    {
        // Auto-assign AudioSource on the same GameObject if not set in Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlaySwordSwing()
    {
        if (audioSource != null && swordSwingSound != null)
        {
            audioSource.PlayOneShot(swordSwingSound);
        }
    }
}