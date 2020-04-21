using System;
using UnityEngine;
using System.Collections;

namespace KazegamesKit
{
    public class USound
    {
        private SoundManager _manager;

        public AudioSource audioSource;

        public event Action onFinished;


        public float volume { get; private set; }

        public bool loop { get; internal set; }

        public float elpasedTime { get; internal set; }


        public USound(SoundManager manager)
        {
            _manager = manager;

            audioSource = manager.gameObject.AddComponent<AudioSource>();
            audioSource.priority = 128;
            audioSource.pitch = 1;
            audioSource.panStereo = 0;
            audioSource.playOnAwake = false;
        }

        public void SetLoop(bool loop)
        {
            this.loop = loop;
            audioSource.loop = loop;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
            audioSource.volume = this.volume * _manager.SoundEffectsVolume;
        }

        public void Stop()
        {
            audioSource.Stop();

            onFinished?.Invoke();

            _manager.RecycleSound(this);
        }

        public void FadeOutAndStop(float duration, Action onCompleted = null)
        {
            CoroutineTask.CreateTask(FadeOut(duration, onCompleted));
        }

        IEnumerator FadeOut(float duration, Action onCompleted)
        {
            float from = audioSource.volume;

            while(audioSource.volume > 0.0f && elpasedTime < audioSource.clip.length)
            {
                audioSource.volume -= Time.deltaTime * from / duration;
                yield return Yielders.EndOfFrame;
            }

            Stop();

            onCompleted?.Invoke();
        }

        public void Play(AudioClip clip, float volume, float pitch, float pan)
        {
            this.elpasedTime = 0;

            SetLoop(false);
            SetVolume(volume);

            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.panStereo = pan;
            audioSource.mute = false;

            audioSource.Play();
        }
    }
}
