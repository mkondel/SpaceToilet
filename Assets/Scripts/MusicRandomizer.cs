using UnityEngine;
using System.Collections;

public class MusicRandomizer : MonoBehaviour {

	public AudioClip[] bgMusicClip;
	public int music_index_override=-1;

	private AudioSource bgMusic;

	// Use this for initialization
	void Start () {
		if (bgMusicClip.Length>0) {
			bgMusic = GetComponent<AudioSource> ();
			if (music_index_override > -1 && music_index_override < bgMusicClip.Length) {
				bgMusic.clip = bgMusicClip [music_index_override];
			} else {
				bgMusic.clip = bgMusicClip [Random.Range (0, bgMusicClip.Length)];
			}
			bgMusic.Play ();
		}
	}
}
