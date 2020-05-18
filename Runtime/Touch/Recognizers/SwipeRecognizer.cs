using System;
using UnityEngine;
using KazegamesKit.Collections;

namespace KazegamesKit.Touch
{
    [System.Flags]
    public enum ESwipeDirection
    {
        Left = (1 << 0),
        Right = (1 << 1),
        Up = (1 << 2),
        Down = (1 << 3),

        All = (Left | Right | Up | Down)
    }

    public class SwipeRecognizer : GestureRecognizer
    {
        public event Action<SwipeRecognizer> onRecognized;

        private float _minDistanceCm;
        private float _screenPixelPerCm;

        private float _startTime;

        private Array<Vector2> _points;

        public float SwipeVelocity { get; private set; }
        public ESwipeDirection Direction { get; private set; }
        public float ElpasedTime { get; private set; }

        public Vector2 startPoint { get { return _points.First; } }
        public Vector2 endPoint { get { return _points.Last; } }

        public SwipeRecognizer() : this(2f)
        {

        }

        public SwipeRecognizer(float minDistance)
        {
            _points = new Array<Vector2>(128);
            _points.growSize = 128;

            _minDistanceCm = minDistance;
            _screenPixelPerCm = Screen.dpi / 2.54f; // inches to Cm = 2.54f
        }

        private bool CheckSwipe(UTouch touch)
        {
            if (_points.Length < 2)
                return false;

            float idealDistance = Vector2.Distance(startPoint, endPoint);
            float idealDistanceCm = idealDistance / _screenPixelPerCm;

            if (idealDistanceCm < _minDistanceCm)
                return false;

            ElpasedTime = Time.time - _startTime;
            SwipeVelocity = idealDistance / ElpasedTime;


            Vector2 dir = (endPoint - startPoint).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle = 360 + angle;
            angle = 360 - angle;

            if (angle >= 225f && angle <= 315f)
                Direction = ESwipeDirection.Up;
            else if (angle >= 135f && angle <= 225f)
                Direction = ESwipeDirection.Left;
            else if (angle >= 45f && angle <= 135f)
                Direction = ESwipeDirection.Down;
            else // angle >= 315f && angle <= 45f
                Direction = ESwipeDirection.Right;

            return true;
        }

        public override void OnTriggerEvents()
        {
            onRecognized?.Invoke(this);
        }

        public override bool OnTouchBegan(Array<UTouch> touches)
        {
            if (state == EState.Ready)
            {
                for (int i = 0; i < touches.Length; i++)
                    _tracking.Push(touches[i]);

                if (_tracking.Length > 0)
                {
                    if (!_points.Empty)
                        _points.Erase(0, _points.Length - 1);

                    _points.Push(touches[0].position);

                    _startTime = Time.realtimeSinceStartup;
                    state = EState.Began;
                }
            }

            return false;
        }

        public override void OnTouchMoved(Array<UTouch> touches)
        {
            if (state == EState.Began)
            {
                _points.Push(touches[0].position);
            }
        }

        public override void OnTouchEnded(Array<UTouch> touches)
        {
            if (state == EState.Began)
            {
                _points.Push(touches[0].position);

                if (CheckSwipe(touches[0]))
                    state = EState.Recognized;
                else
                    state = EState.FailedOrEnded;
            }
        }
    }
}