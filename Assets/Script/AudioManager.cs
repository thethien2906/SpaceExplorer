using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private float pausedVolume = 0.7f;
    [SerializeField] private float lowPassFrequency = 1000f;

    // Chua thuoc tinh ban dau
    private AudioLowPassFilter[] bgmLowPassFilters;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
            // Them LowPassFilter component vao cac AudioSource
            bgmLowPassFilters = new AudioLowPassFilter[bgm.Length];
            for (int i = 0; i < bgm.Length; i++)
            {
                bgmLowPassFilters[i] = bgm[i].gameObject.GetComponent<AudioLowPassFilter>();
                if (bgmLowPassFilters[i] == null)
                {
                    bgmLowPassFilters[i] = bgm[i].gameObject.AddComponent<AudioLowPassFilter>();
                }
                // Initially disable the filter
                bgmLowPassFilters[i].enabled = false;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void PlaySFX(int sfxToPlay, bool randomPitch = true)
    {
        if (sfxToPlay >= sfx.Length)
            return;
        if (randomPitch) {
            sfx[sfxToPlay].pitch = Random.Range(.9f, 1.1f);
        }
        sfx[sfxToPlay].Play();
    }

    public void PlayBGM(int bgmToPlay)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
        if (bgmToPlay >= bgm.Length)
            return;
        bgm[bgmToPlay].Play();
    }
    public void AdjustBGMForPause(bool isPaused)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (isPaused)
            {
                bgm[i].volume *= pausedVolume;
                // Thay doi am thanh
                bgmLowPassFilters[i].enabled = true;
                bgmLowPassFilters[i].cutoffFrequency = lowPassFrequency;
            }
            else
            {
                bgm[i].volume = 1f; // Reset trang thai cu
                // Huy thay doi am thanh
                bgmLowPassFilters[i].enabled = false;
            }
        }
    }

    public void StopSFX(int sfxToStop) => sfx[sfxToStop].Stop();
}
