using LHEPackage.Helper;
using UnityEngine;
using UnityEngine.Serialization;

namespace CommonQuizFramework.CommonClass
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] private AudioSource SFXAudioSource;
        [SerializeField] private AudioSource BGMAudioSource;

        [SerializeField] private AudioClip correctAnswerSFX;
        [SerializeField] private AudioClip wrongAnswerSFX;
        [SerializeField] private AudioClip stateChangeSFX;
        [SerializeField] private AudioClip[] attackSoundsSFX;

        public float BGMVolume
        {
            set => BGMAudioSource.volume = value;
        }

        public float SFXVolume
        {
            set => SFXAudioSource.volume = value;
        }

        public AudioClip BGMClip
        {
            get => BGMAudioSource.clip;
            set
            {
                value.LoadAudioData();
                BGMAudioSource.clip = value;
            }
        }

        public int CurrentBGMID { set; get; } = -1;

        public void Initialization()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayBGM()
        {
            BGMAudioSource.Play();
        }

        public void StopBGM()
        {
            BGMAudioSource.Stop();
        }

        public void PlaySFX(SFXType type, int? id = null)
        {
            switch (type)
            {
                case SFXType.CorrectAnswer:
                    SFXAudioSource.PlayOneShot(correctAnswerSFX);
                    break;

                case SFXType.WrongAnswer:
                    SFXAudioSource.PlayOneShot(wrongAnswerSFX);
                    break;

                case SFXType.StateChange:
                    SFXAudioSource.PlayOneShot(stateChangeSFX);
                    break;
                case SFXType.AttackSound:
                    if (id != null) SFXAudioSource.PlayOneShot(attackSoundsSFX[(int)id]);
                    break;
                default:
                    LHELogger.LogWarning("SFX Type is wrong {type}");
                    break;
            }
        }
    }

    public enum SFXType
    {
        CorrectAnswer,
        WrongAnswer,
        StateChange,
        AttackSound
    }
}