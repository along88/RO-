using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource uiAudioSource;

    [SerializeField]
    private AudioClip navigationChime;

    [SerializeField]
    private AudioClip confirmChime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (uiAudioSource == null)
        {
            Debug.LogError(
                "UIAudioManager requires an AudioSource.",
                this
            );

            enabled = false;
            return;
        }

        uiAudioSource.playOnAwake = false;
        uiAudioSource.loop = false;
    }

    public void PlayNavigationChime()
    {
        if (navigationChime != null)
            uiAudioSource.PlayOneShot(navigationChime);
    }

    public void PlayConfirmChime()
    {
        if (confirmChime != null)
            uiAudioSource.PlayOneShot(confirmChime);
    }
}