using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    public int _phase;
    public int _numberOfPlayers;
    public string _cardType;
    public string _card1Text, _card2Text, _card3Text, _succeedText, _succeedButtonText, _failButtonText, _giveUpButtonText;
    public int _timerAmount;
    
    public Card(int phase, int numberOfPlayers, string cardType, string card1Text, string card2Text, string card3Text,
        string succeedText, string succeedButtonText, string failButtonText, string giveUpButtonText,
        int timerAmount)
    {
        _phase = phase;
        _numberOfPlayers = numberOfPlayers;
        _cardType = cardType;
        _card1Text = card1Text;
        _card2Text = card2Text;
        _card3Text = card3Text;
        _succeedText = succeedText;
        //_succeedButtonText = succeedButtonText;
        //_failButtonText = failButtonText;
        //_giveUpButtonText = giveUpButtonText;

        _timerAmount = timerAmount;
    }
    public Card(int phase, int numberOfPlayers, string cardType, string card1Text, string card2Text, string card3Text,
       string succeedText, 
       int timerAmount)
    {
        _phase = phase;
        _numberOfPlayers = numberOfPlayers;
        _cardType = cardType;
        _card1Text = card1Text;
        _card2Text = card2Text;
        _card3Text = card3Text;
        _succeedText = succeedText;
        //_succeedButtonText = succeedButtonText;
        //_failButtonText = failButtonText;
        //_giveUpButtonText = giveUpButtonText;

        _timerAmount = timerAmount;
    }
}
