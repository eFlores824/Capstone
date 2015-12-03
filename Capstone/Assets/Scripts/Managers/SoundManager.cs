using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

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
        sounds[0].Stop();
        sounds[1].Stop();
	}

    public void playGoalReached()
    {
        sounds[2].Play();
    }

}
