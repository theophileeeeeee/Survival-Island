using UnityEngine;

public class FootSound : MonoBehaviour
{
    [Header("Footstep sounds")]
    [SerializeField] private AudioClip[] footstepClips;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float baseVolume = 0.25f; // volume moyen (plus faible)
    [Range(0f, 1f)]
    [SerializeField] private float volumeVariation = 0.08f; // variation aléatoire

    // Appelée par un Animation Event
    public void PlayFootstep()
    {
        if (footstepClips.Length == 0 || audioSource == null)
            return;

        int randomIndex = Random.Range(0, footstepClips.Length);

        float volume = baseVolume + Random.Range(-volumeVariation, volumeVariation);
        volume = Mathf.Clamp01(volume);

        audioSource.PlayOneShot(footstepClips[randomIndex], volume);
    }
}
