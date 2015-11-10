using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public AudioClip footstep1;
	public AudioClip footstep2;
	public AudioClip footstep3;
	public AudioClip walking;
	public AudioClip goalReached;
	public AudioClip backgroundSound;

	private AudioSource sound;
	
	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void playFootsteps() {
		//StartCoroutine("firstFoot");
		StartCoroutine("walkingSound");
	}
	
	public void stopFootsteps() {
		
	}
	
	private IEnumerator walkingSound() {
		sound.PlayOneShot(walking);
		yield return new WaitForSeconds(walking.length);
	}
	
	private IEnumerator firstFoot() {
		sound.PlayOneShot(footstep1);
		yield return new WaitForSeconds(footstep1.length);
		StartCoroutine("secondFoot");
	}
	
	private IEnumerator secondFoot() {
		sound.PlayOneShot(footstep2);
		yield return new WaitForSeconds(footstep2.length);
		StartCoroutine("thirdFoot");
	}
	
	private IEnumerator thirdFoot() {
		sound.PlayOneShot(footstep3);
		yield return new WaitForSeconds(footstep3.length);
	}
	
	public void playBackgroundMusic() {
		StartCoroutine("background");
	}
	
	public void stopAllSound() {
		StopAllCoroutines();
	}
	
	private IEnumerator background() {
		sound.PlayOneShot(backgroundSound);
		yield return new WaitForSeconds(backgroundSound.length);
		StartCoroutine("background");
	}
}
