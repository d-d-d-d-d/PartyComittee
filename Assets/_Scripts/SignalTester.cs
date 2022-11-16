using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.Signals;

public class SignalTester : MonoBehaviour
{
    public string _streamCategory = "Game";
    public string _hideTimerStream = "HideTimer";
    public string _showTimerStream = "ShowTimer";
    public string _showResultsStream = "ShowResults";

    public void SignalTimerShow()
    {
        Signal.Send(_streamCategory, _showTimerStream);
    }

    public void SignalTimerHide()
    {
        Signal.Send(_streamCategory, _hideTimerStream);
    }

    public void SignalShowResults()
    {
        Signal.Send(_streamCategory, _showResultsStream);
    }
}
