using System;
using System.Collections;
using UnityEngine;

namespace KazegamesKit.Sound
{
    public class USound
    {
        private SoundManager _manager;

        public AudioSource source;

        public event Action onFinished;


        public float volume { get; private set; }

        public bool loop { get; internal set; }

        public float elpasedTime { get; internal set; }


        public USound(SoundManager manager)
        {
            _manager = manager;

            source = manager.gameObject.AddComponent<AudioSource>();
            source.priority = 128;
            source.pitch = 1;
            source.panStereo = 0;
            source.playOnAwake = false;
        }

        public void SetLoop(bool loop)
        {
            this.loop = loop;
            source.loop = loop;
        }

        public void Stop()
        {
            source.Stop();

            onFinished?.Invoke();

            _manager.RecycleSound(this);
        }

        public void FadeOutAndStop(float duration, Action onCompleted = null)
        {
            CoroutineTask.CreateTask(FadeOut(duration, onCompleted));
        }

        IEnumerator FadeOut(float duration, Action onCompleted)
        {
            float from = source.volume;

            while (source.volume > 0.0f && elpasedTime < source.clip.length)
            {
                source.volume -= Time.deltaTime * from / duration;
                yield return Yielders.EndOfFrame;
            }

            Stop();

            onCompleted?.Invoke();
        }

        public void Play(AudioClip clip, float volume, float pitch, float pan)
        {
            this.elpasedTime = 0;
            this.volume = volume;

            SetLoop(false);
            SetAudioSourceVolume(this.volume * _manager.volumeScale);

            source.clip = clip;
            source.pitch = pitch;
            source.panStereo = pan;
            source.mute = false;

            source.Play();
        }

        public void SetAudioSourceVolume(float volume)
        {
            source.volume = volume;
        }
    }
}
