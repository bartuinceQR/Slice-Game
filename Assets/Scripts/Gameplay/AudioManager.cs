using UnityEngine;

namespace Gameplay
{
    public class AudioManager : MonoBehaviour
    {
    
        private AudioSource _audioSource;
    
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.pitch = 1;
            _audioSource.PlayOneShot(clip);
        }
        
        //could be handled more generally, but is it REALLY needed in this case?
        public void PlaySoundWithPitch(AudioClip clip, float gain)
        {
            Debug.Log(gain);
            _audioSource.pitch = 1 + gain / 15f;
            _audioSource.PlayOneShot(clip);
        }
    }
}
