using System;
using UnityEngine;

namespace KazegamesKit
{
    public class SoundManager : SingletonMono
    {
        public int maxSoundCapacity = 6;

        private float _soundEffectsVolume = 1f;
        public float SoundEffectsVolume
        {
            get { return _soundEffectsVolume; }

            set
            {
                _soundEffectsVolume = value;

                if (backgroundSound != null)
                    backgroundSound.SetVolume(backgroundSound.volume);

                if (_oneShotSound != null)
                    _oneShotSound.SetVolume(_oneShotSound.volume);

                for (int i = 0; i < _playingSounds.Length; i++)
                    _playingSounds[i].SetVolume(_playingSounds[i].volume);
            }
        }

        public USound backgroundSound;
        private USound _oneShotSound;
        private Array<USound> _playingSounds;
        private Array<USound> _availableSounds;

        private float _dt;
        
        public void PlayBackgroundMusic(AudioClip clip, float volume, bool loop = true)
        {
            if (backgroundSound == null)
                backgroundSound = new USound(this);

            backgroundSound.Play(clip, volume, 1, 0);
            backgroundSound.SetLoop(loop);
        }

        public void PlayOneShotSound(AudioClip clip, float volume)
        {
            if (_oneShotSound == null)
                _oneShotSound = new USound(this);

            _oneShotSound.audioSource.PlayOneShot(clip, volume * _soundEffectsVolume);
        }

        public USound PlaySound(AudioClip clip, float volume, float pitch, float pan, bool loop = false)
        {
            USound sound = GetAvaliableSound();
            
            sound.Play(clip, volume, pitch, pan);
            sound.SetLoop(loop);

            return sound;
        }

        public USound PlaySound(AudioClip clip, float volume, bool loop = false)
        {
            return PlaySound(clip, volume, 1, 0, loop);
        }

        public USound PlaySound(AudioClip clip, bool loop = false)
        {
            return PlaySound(clip, 1, 1, 0, loop);
        }

        public void ClearAllAudioClips()
        {
            while(_playingSounds.Length > 0)
            {
                var s = _playingSounds.Pop();
                
                s.audioSource.clip = null;
                _availableSounds.Push(s);
            }
        }

        public void RecycleSound(USound sound)
        {
            if (sound == backgroundSound)
                return;

            int index = 0;
            while(index < _playingSounds.Length)
            {
                if (_playingSounds[index] == sound)
                    break;

                index++;
            }

            _playingSounds.Erase(index);

            if(_availableSounds.Length + _playingSounds.Length > maxSoundCapacity)
            {
                Destroy(sound.audioSource);
            }
            else
            {
                _availableSounds.Push(sound);
            }
        }

        private USound GetAvaliableSound()
        {
            USound sound = null;

            if(!_availableSounds.IsEmpty())
            {
                sound = _availableSounds.Pop();
            }

            if (sound == null)
                sound = new USound(this);

            _playingSounds.Push(sound);

            return sound;
        }

        public override void Init()
        {
            _availableSounds = new Array<USound>(6);
            _playingSounds = new Array<USound>(6);

            for (int i = 0; i < _availableSounds.Length; i++)
                _availableSounds.Push(new USound(this));
        }

        public override void Dispose()
        {
            backgroundSound = null;
            _oneShotSound = null;
            _availableSounds = null;
            _playingSounds = null;
        }

        private void Update()
        {
            _dt = Time.deltaTime;

            for(int i=0; i<_playingSounds.Length; i++)
            {
                if (_playingSounds[i].loop)
                    continue;

                _playingSounds[i].elpasedTime += _dt;

                if (_playingSounds[i].elpasedTime >= _playingSounds[i].audioSource.clip.length)
                    _playingSounds[i].Stop();
            }
        }
    }
}
