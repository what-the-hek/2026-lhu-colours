using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip magicObjectSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = magicObjectSound;
    }

    public void PlayMagicObject()
    {
        audioSource.Play();
    }
}
