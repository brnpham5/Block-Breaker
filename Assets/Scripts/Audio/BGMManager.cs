using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BGMManager : MonoBehaviour
    {
        public AudioSource audioSource;

        public SFXSet set;

        public List<WaitForSecondsRealtime> bgmLengths = new List<WaitForSecondsRealtime>();

        public bool shuffling = false;
        public int playCount = 2;
        public int playsRemaining;

        public bool waitingForNextSong = false;

        private void Awake()
        {
            playsRemaining = playCount;

            for (int j = 0; j < set.clips.Count; j++)
            {
                bgmLengths.Add(new WaitForSecondsRealtime(set.clips[j].length - 0.5f));
            }
        }

        public void PlaySet()
        {
            shuffling = true;
            waitingForNextSong = false;
            StopAllCoroutines();
            StartCoroutine(ShuffleCoroutine(set));
        }

        public int GetRandomIndex()
        {
            return Random.Range(0, set.clips.Count);
        }

        public void Play(int index)
        {
            audioSource.clip = set.clips[index];
            audioSource.Play();
        }

        public IEnumerator ShuffleCoroutine(SFXSet set)
        {
            int index = GetRandomIndex();

            //Make up one repeat count, otherwise first song repeats one less than subsequent songs.
            waitingForNextSong = true;

            while (shuffling)
            {
                if (playsRemaining <= 0)
                {
                    index = GetRandomIndex();
                    playsRemaining = playCount;
                }

                while (waitingForNextSong == true) {
                    if (audioSource.isPlaying == false)
                    {
                        Play(index);
                        waitingForNextSong = false;
                    }
                    yield return null;
                }

                waitingForNextSong = true;
                yield return bgmLengths[index];

                if (playsRemaining > 0)
                {
                    playsRemaining--;
                }

            }
        }

        private void OnEnable()
        {
            set.OnPlay += PlaySet;
        }

        private void OnDisable()
        {

            set.OnPlay -= PlaySet;
        }
    }

}
