using System;
using System.Collections.Generic;
using UnityEngine;

namespace KazegamesKit.Touch
{
    public class TouchManager : SingletonMono
    {
        private Array<UTouch> _cacheTouches;
        private Array<UTouch> _liveTouches;
        private List<GestureRecognizer> _gestureRecognizers;

        public const int MAX_TOUCHES_PROCESS = 5;

        public void AddRecognizer(GestureRecognizer recognizer)
        {
            _gestureRecognizers.Add(recognizer);

            if (_gestureRecognizers.Count > 1)
                _gestureRecognizers.Sort();
        }

        public void RemoveRecognizer(GestureRecognizer recognizer)
        {
            _gestureRecognizers.Remove(recognizer);
        }

        public void RemoveAllRecognizers()
        {
            _gestureRecognizers.Clear();
        }

        void UpdateTouches()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if(Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
            {
                _liveTouches.Push(_cacheTouches[0].UpdateFromMouse());
            }
#else
            if(Input.touchCount > 0)
            {
                int c = Mathf.Min(Input.touchCount, MAX_TOUCHES_PROCESS);
                for(int i=0; i<c; i++)
                {
                    UnityEngine.Touch touch = Input.GetTouch(i);
                    
                    if (touch.fingerId < MAX_TOUCHES_PROCESS)
                        _liveTouches.Push(_cacheTouches[touch.fingerId].UpdateFromTouch(touch));
                }
            }
#endif

            if (!_liveTouches.IsEmpty())
            {
                for (int i = 0; i < _gestureRecognizers.Count; i++)
                    _gestureRecognizers[i].Recognize(_liveTouches);

                _liveTouches.Erase(0, _liveTouches.Length);
            }
        }

        public override void Init()
        {
            _cacheTouches = new Array<UTouch>(MAX_TOUCHES_PROCESS);
            _liveTouches = new Array<UTouch>(MAX_TOUCHES_PROCESS);
            _gestureRecognizers = new List<GestureRecognizer>();

            for (int i = 0; i < MAX_TOUCHES_PROCESS; i++)
                _cacheTouches.Push(new UTouch(i));
        }

        public override void Dispose()
        {
            _cacheTouches = null;
            _liveTouches = null;
            _gestureRecognizers = null;
        }

        private void Update()
        {
            UpdateTouches();
        }
    }
}