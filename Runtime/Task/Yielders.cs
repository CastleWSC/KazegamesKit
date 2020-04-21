using UnityEngine;
using UnityEngine.Assertions.Comparers;
using System.Collections.Generic;

namespace KazegamesKit
{
    public static class Yielders
    {

        public static bool Enabled = true;

        private static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();

        public static WaitForEndOfFrame EndOfFrame
        {
            get { return Enabled ? _endOfFrame : new WaitForEndOfFrame(); }
        }


        const int WAIT_TIMES_LEN = 64;

        private static Dictionary<float, WaitForSeconds> _waitTimes = new Dictionary<float, WaitForSeconds>(WAIT_TIMES_LEN, new FloatComparer());

        public static WaitForSeconds GetWaitTimes(float seconds)
        {
            if (!Enabled) return new WaitForSeconds(seconds);

            WaitForSeconds wfs = null;
            if(!_waitTimes.TryGetValue(seconds, out wfs))
            {
                if (_waitTimes.Count >= WAIT_TIMES_LEN)
                    _waitTimes.Clear();

                _waitTimes.Add(seconds, wfs = new WaitForSeconds(seconds));
            }

            return wfs;
        }
    }
}
