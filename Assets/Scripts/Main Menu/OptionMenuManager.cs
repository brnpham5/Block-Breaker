using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using Core;

namespace Game
{
    public class OptionMenuManager : PanelController
    {
        public delegate void MenuDelegate();
        public static event MenuDelegate OnClose;
        public static event MenuDelegate OnForceClose;

        [Header("Asset Reference")]
        public AudioMixer mixer;
        public Slider sfxSlider;
        public Slider bgmSlider;

        public Button buttonAccept;
        public Button buttonDecline;

        public float tempBGMVol;
        public float tempSFXVol;
        

        private void Start()
        {
            Hide();

            sfxSlider.onValueChanged.AddListener(AdjustSFX);
            bgmSlider.onValueChanged.AddListener(AdjustBGM);

            buttonAccept.onClick.AddListener(OnAccept);
            buttonDecline.onClick.AddListener(OnDecline);

            GetVolumePrefs();
        }

        public void GetVolumePrefs()
        {
            float bgm_vol = PlayerPrefs.GetFloat("pref_bgm_vol", 0);
            float sfx_vol = PlayerPrefs.GetFloat("pref_sfx_vol", 0);
            AdjustBGM(bgm_vol);
            AdjustSFX(sfx_vol);

            tempBGMVol = bgm_vol;
            tempSFXVol = sfx_vol;
            bgmSlider.value = bgm_vol;
            sfxSlider.value = sfx_vol;
        }


        public void AdjustBGM(float value)
        {
            mixer.SetFloat("BGM Vol", Mathf.Log10(value) * 20);
        }

        public void AdjustSFX(float value)
        {
            mixer.SetFloat("SFX Vol", Mathf.Log10(value) * 20);
        }

        public void OnAccept()
        {
            AdjustBGM(bgmSlider.value);
            AdjustSFX(sfxSlider.value);

            PlayerPrefs.SetFloat("pref_bgm_vol", bgmSlider.value);
            PlayerPrefs.SetFloat("pref_sfx_vol", sfxSlider.value);

            tempBGMVol = bgmSlider.value;
            tempSFXVol = sfxSlider.value;

            Hide();

            OnClose?.Invoke();
        }

        public void OnDecline()
        {

            AdjustBGM(tempBGMVol);
            AdjustSFX(tempSFXVol);

            bgmSlider.value = tempBGMVol;
            sfxSlider.value = tempSFXVol;

            Hide();

            OnClose?.Invoke();
        }

        public void Unpause()
        {
            OnDecline();
            Hide();
            OnForceClose?.Invoke();
        }

        public override void Show()
        {
            base.Show();
            tempBGMVol = bgmSlider.value;
            tempSFXVol = sfxSlider.value;
        }


        private void OnEnable()
        {
            MainMenuManager.OnOption += Show;
            PauseUIManager.OnOptions += Show;
            PauseManager.OnUnpause += Unpause;
        }

        private void OnDisable()
        {
            MainMenuManager.OnOption -= Show;
            PauseUIManager.OnOptions -= Show;
            PauseManager.OnUnpause -= Unpause;
        }
    }

}
