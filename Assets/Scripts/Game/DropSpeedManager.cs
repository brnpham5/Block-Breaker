using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game
{
    public class DropSpeedManager : MonoBehaviour
    {
        public delegate void LvlDelegate();
        public static event LvlDelegate OnLvl;

        [Header("Scriptable Reference")]
        public LevelData data;
        public FloatVariable dropDelay;
        public IntVariable breakCount;
        public IntVariable level;
        public SFXSet sfx;


        public void Setup()
        {
            level.SetValue(1);
            dropDelay.SetValue(data.levels[0].dropSpeed);
        }

        public void Check()
        {
            int tempLvl = level.GetValue();
            int tempBrk = breakCount.GetValue();

            if(tempLvl < data.levels.Count)
            {
                int lvlUp = data.levels[tempLvl].breakCount;

                if (tempBrk >= lvlUp)
                {
                    IncreaseLevel();
                    sfx.Play();
                }
            }
        }

        public void IncreaseLevel()
        {
            float dropSpeed = data.levels[level.GetValue() - 1].dropSpeed;
            level.ApplyChange(1);
            dropDelay.SetValue(dropSpeed);
            OnLvl?.Invoke();
        }


        private void OnEnable()
        {
            breakCount.OnChange += Check;
            GameStartManager.OnGameSetup += Setup;
        }

        private void OnDisable()
        {
            breakCount.OnChange -= Check;
            GameStartManager.OnGameSetup -= Setup;
        }
    }

}
