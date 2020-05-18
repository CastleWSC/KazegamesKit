using System;
using UnityEngine;
using KazegamesKit.Collections;

namespace KazegamesKit.Sound
{
    public class SoundManager : SingletonMono
    {
        public int maxSoundCapacity = 6;
        
        [Range(0f, 1f)]public float volumeScale = 1;

        public USound BGM;
        private USound _oneShotSound;
        private Array<USound> _playingSounds;
        private Array<USound> _availableSounds;


        public void PlayBackgroundMusic(AudioClip clip, float volume, bool loop = true)
        {
            if (BGM == null)
                BGM = new USound(this);

            BGM.Play(clip, volume, 1, 0);
            BGM.SetLoop(loop);
        }

        public void PlayOneShotSound(AudioClip clip, float volume)
        {
            if (_oneShotSound == null)
                _oneShotSound = new USound(this);

            _oneShotSound.source.PlayOneShot(clip, volume);
        }

        public USound PlaySound(AudioClip clip, float volume, float pitch, float pan, bool loop)
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
            while(!_playingSounds.Empty)
            {
                var s = _playingSounds.Pop();

                s.source.clip = null;
                _availableSounds.Push(s);
            }
        }

        public void RecycleSound(USound sound)
        {
            if (sound == BGM)
                return;

            int indx = 0;
            while(indx <_playingSounds.Length)
            {
                if (_playingSounds[indx] == sound)
                    break;

                indx++;
            }

            _playingSounds.Erase(indx);

            if(_availableSounds.Length + _playingSounds.Length > maxSoundCapacity)
            {
                Destroy(sound.source);
            }
            else
            {
                _availableSounds.Push(sound);
            }
        }

        private USound GetAvaliableSound()
        {
            USound sound = null;

            if(!_availableSounds.Empty)
            {
                sound = _availableSounds.Pop();
            }

            if(sound == null)
            {
                sound = new USound(this);
            }

            _playingSounds.Push(sound);

            return sound;
        }

        public override void Init()
        {
            _availableSounds = new Array<USound>(maxSoundCapacity);
            _playingSounds = new Array<USound>(maxSoundCapacity);

            for (int i = 0; i < _availableSounds.Length; i++)
                _availableSounds.Push(new USound(this));
            
        }

        public override void Dispose()
        {
            BGM = null;
            _oneShotSound = null;
            _playingSounds = null;
            _availableSounds = null;
        }

        float _dt;
        private void Update()
        {
            _dt = Time.deltaTime;
            for(int i=0; i<_playingSounds.Length; i++)
            {
                if (_playingSounds[i].loop)
                    continue;

                _playingSounds[i].elpasedTime += _dt;

                if (_playingSounds[i].elpasedTime >= _playingSounds[i].source.clip.length)
                    _playingSounds[i].Stop();
            }
        }
    }
}
