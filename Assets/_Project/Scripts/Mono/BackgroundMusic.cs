using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public AudioClip[] bgClips;

    private void Start()
    {
        //select a clip randomly and play the looping background music
        var source = GetComponent<AudioSource>();
        source.clip = bgClips[Random.Range(0, bgClips.Length)];
        source.Play();

    }
}
