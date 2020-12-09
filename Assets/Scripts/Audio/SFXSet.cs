using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SFX Set", menuName = "Config/SFX Set")]
    public class SFXSet : ScriptableObject
    {
        public delegate void SoundEffectDelegate();
        public event SoundEffectDelegate OnPlay;

        public List<AudioClip> clips;

        public void Play()
        {
            OnPlay?.Invoke();
        }
    }
}
