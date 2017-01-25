using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadioBehaviour : MonoBehaviour {

    [SerializeField] List<AudioClip> popSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> rockSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> classicalSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> countrySongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> jazzSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> ambientalSongs = new List<AudioClip>();[SerializeField] AudioClip fmSound;
    AudioSource[] audioSources;
    AudioSource radioSource;
    int currentFM;
    bool off = false;

    [Range(0,1)] public float maxVolume;

	void Start () {
        audioSources = GetComponents<AudioSource>();
        radioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        radioSource.playOnAwake = false;
        radioSource.volume = 0.25f;
        radioSource.Stop();
        radioSource.clip = fmSound;

        currentFM = audioSources.Length-1;

        PlayNextSong(0, classicalSongs);
        PlayNextSong(1, rockSongs);
        PlayNextSong(2, popSongs);
        PlayNextSong(3, countrySongs);
        PlayNextSong(4, jazzSongs);
        PlayNextSong(5, ambientalSongs);

        TurnOffRadio();
    }

    void Update () {
        if (Hacks.isOver(this.gameObject))
        {
            if (Input.GetMouseButtonDown(0)) {
                if (off)
                    TurnOnRadio();
                else
                    TurnOffRadio();
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                NextFM();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                PreviousFM();
            }
        }
	}

    void TurnOffRadio() {
        off = true;

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].volume = 0;
        }

        radioSource.Stop();
    }

    void TurnOnRadio()
    {
        off = false;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i == currentFM)
                audioSources[i].volume = maxVolume;
            else
                audioSources[i].volume = 0;
        }
    }

    void NextFM() {
        radioSource.time = 0f;
        radioSource.Play();

        currentFM = (currentFM >= audioSources.Length - 1) ? 0 : currentFM + 1;    

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i == currentFM)
                audioSources[i].volume = maxVolume;
            else
                audioSources[i].volume = 0;
        }
    }

    void PreviousFM() {
        radioSource.time = 0f;
        radioSource.Play();

        currentFM = (currentFM <= 0) ? audioSources.Length - 1 : currentFM - 1;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i == currentFM)
                audioSources[i].volume = maxVolume;
            else
                audioSources[i].volume = 0;
        }
    }

    void PlayNextSong(int audioSource, List<AudioClip> songList) {
        audioSources[audioSource].Stop();
        AudioClip nextSong = songList[0];
        audioSources[audioSource].clip = nextSong;
        songList.RemoveAt(0);
        songList.Add(nextSong);
        audioSources[audioSource].Play();
        StartCoroutine(WaitEndSong(nextSong.length, audioSource, songList));
    }

    IEnumerator WaitEndSong(float songLength, int audioSource, List<AudioClip> songList) {
        yield return new WaitForSeconds(songLength);
        PlayNextSong(audioSource, songList);
    }
}
