using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource _aSEffects;
    public AudioSource _aSAmbient;
    [Header("UI Sounds")]
    public AudioClip _select, _menuSound, _appOpen, _gameEnd, _addPlayer, _error, _timerReg, _timerUrgent;
    public AudioClip[] _swipes;
    [Header("Ambient sounds")]
    public AudioClip _titleScreen, _menuScreen, _addPlayerScreen;
    public AudioClip[] _ambients;

    public enum _soundLibrary
    {
        UISelect, UIError, UIAppOpen, UIMenuButtonSelect, UITimerRegular, UITimerUrgent, UISwipe, UIGameEnd, 
        AmbientTitleScreen, AmbientMenuScreen, AmbientAddPlayerScreen, AmbientGeneral
    };

    private 

    // Start is called before the first frame update
    void Start()
    {
        if (_aSEffects == null)
        {
            _aSEffects = gameObject.GetComponent<AudioSource>();
        }
    }

    public void PlayFromLibrary(_soundLibrary sound)
    {
        if (sound == _soundLibrary.AmbientGeneral)
        {

        } else if (sound == _soundLibrary.UISwipe)
        {

        }
    }

    public void FadeOutAmbient()
    {

    }

    
}
