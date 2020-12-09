using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EZCameraShake;

using Core;

namespace Game {
    public class BlockBreakable : MonoBehaviour, IBreakable {
        public delegate void BreakDelegate();
        public event BreakDelegate OnBreak;
        public event BreakDelegate BeforeBreak;

        public IntVariable breakCount;

        private ParticleSystem ps;
        private float Magnitude = 1f;
        private float Roughness = 1f;
        private float FadeOutTime = 0.5f;

        public SFXSet sfxSet;
        public void Setup(ParticleSystem ps) {
            this.ps = ps;
        }

        public void Break() {
            BeforeBreak?.Invoke();

            ParticleSystem tempPs = Instantiate(ps);
            tempPs.transform.position = this.transform.position;
            tempPs.Play();
            sfxSet.Play();

            breakCount.ApplyChange(1);

            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);

            OnBreak?.Invoke();
        }

        public void CleanBreak()
        {
            ParticleSystem tempPs = Instantiate(ps);
            tempPs.transform.position = this.transform.position;
            tempPs.Play();
            sfxSet.Play();

            OnBreak?.Invoke();
        }
    }

}
