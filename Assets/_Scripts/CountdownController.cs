using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.Signals;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public TextMeshProUGUI _timerTMP;
    public Animator _timerAnim;
    public string _timerStreamCategory = "Game";
    public string _timerShowStreamName = "ShowTimer";
    public string _timerHideStreamName = "HideTimer";
    public int _secondsToSet = 30;

    private bool _isCountingDown = false;
    private bool _timerIsHidden = true;
    private float _secondsLeft;
    private float _timeCounter = 0.0f;
    private int _finalSecounds = 10;

    public void SetAndStartCountdown(float seconds)
    {
        _secondsLeft = seconds;
        _finalSecounds = 10;
        _timeCounter = 0.0f;

        Signal.Send(_timerStreamCategory, _timerShowStreamName);

        UpdateText(_secondsLeft);

        _isCountingDown = true;
        _timerIsHidden = false;
    }

    void Update()
    {
        if (_isCountingDown)
        {
            _secondsLeft -= Time.deltaTime;
            UpdateText(_secondsLeft);
            if (_secondsLeft <= 0.0f)
            {
                _isCountingDown = false;
                Signal.Send(_timerStreamCategory, _timerHideStreamName);
                _timerIsHidden = true;
            } else if (_secondsLeft <= _finalSecounds)
            {
                _finalSecounds--;
                if (_secondsLeft < 5.0f)
                    _timerAnim.SetTrigger("Pulse2");
                else
                    _timerAnim.SetTrigger("Pulse");
            }
        }
    }

    private void UpdateText(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _timerTMP.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));

    }
}
