using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip walking;
	public AudioClip goalReached;

	private AudioSource[] sounds;
    private IEnumerator coroutine;
	
	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void playFootsteps() {
        sounds[1].Play();
	}
	
	public void stopFootsteps() {
        sounds[1].Stop();
	}
	
	public void stopAllSound() {
		StopAllCoroutines();
	}

    public void playGoalReached()
    {
        sounds[0].PlayOneShot(goalReached);
    }

}
