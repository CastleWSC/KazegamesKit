using System;
using UnityEngine;

namespace KazegamesKit.Touch
{
    public class TapRecognizer : GestureRecognizer
    {
        public event Action<TapRecognizer> onRecognized;

        public int numOfTapsRequired = 1;
        public int numOfTouchesRequired = 1;

        private float _touchBeganTime = 0;
        private int _prformedTapsCount = 0;

        private float _maxDurationForTaps = 0.5f;


        public Vector2 startPoint { get; private set; }
        public Vector2 endPoint { get; private set; }

        public TapRecognizer() : this(1, 1, 0.5f)
        {

        }

        public TapRecognizer(int touchesRequired, int tapsRequired, float maxDurationForTaps)
        {
            numOfTouchesRequired = touchesRequired;
            numOfTapsRequired = tapsRequired;
            _maxDurationForTaps = maxDurationForTaps;
        }

        public override void OnTriggerEvents()
        {
            onRecognized?.Invoke(this);
        }

        public override bool OnTouchBegan(Array<UTouch> touches)
        {
            if(state == EState.Ready)
            {
                for(int i=0; i<touches.Length; i++)
                {
                    if(touches[i].phase == TouchPhase.Began)
                    {
                        _tracking.Push(touches[i]);

                        if (_tracking.Length == numOfTouchesRequired)
                            break;
                    }
                }

                if(_tracking.Length == numOfTouchesRequired)
                {
                    _touchBeganTime = Time.realtimeSinceStartup;
                    _prformedTapsCount = 0;
                    state = EState.Began;

                    return true;
                }
            }

            return false;
        }

        public override void OnTouchEnded(Array<UTouch> touches)
        {
            if(state == EState.Began && Time.realtimeSinceStartup <= _touchBeganTime + _maxDurationForTaps)
            {
                ++_prformedTapsCount;

                if (_prformedTapsCount == numOfTapsRequired)
                {
                    UTouch primary = PrimaryTouch;
                    if(primary != null)
                    {
                        startPoint = primary.startPosition;
                        endPoint = primary.position;
                    }

                    state = EState.Recognized;
                }
            }
            else
            {
                state = EState.FailedOrEnded;
            }
        }
    }
}