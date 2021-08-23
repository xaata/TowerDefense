using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySFX("click");
        }
    }

    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value / 2;

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }
    public void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.2f);
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume/2;
    }
}
