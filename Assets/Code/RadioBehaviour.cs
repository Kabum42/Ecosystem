using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadioBehaviour : MonoBehaviour {

    [SerializeField] List<AudioClip> popSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> rockSongs = new List<AudioClip>();
    [SerializeField] List<AudioClip> classicalSongs = new List<AudioClip>();
    AudioSource[] audioSources;
    int currentFM;

    [Range(0,1)] public float maxVolume;

	void Start () {
        audioSources = GetComponents<AudioSource>();
        currentFM = 3;

        PlayNextSong(0, classicalSongs);
        PlayNextSong(1, rockSongs);
        PlayNextSong(2, popSongs);

        NextFM();
    }

    void Update () {
        if (Hacks.isOver(this.gameObject))
            if (Input.GetMouseButtonDown(0))
                NextFM();
	}

    void NextFM() {
        currentFM = (currentFM >= audioSources.Length - 1) ? 0 : currentFM + 1;    

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
        Invoke("PlayNextSong", nextSong.length);
    }
}
