using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.Signals;
using Doozy;
using TMPro;
using System.Text;
using UnityEngine.UI;



public class CardController : MonoBehaviour
{
    public GameManager _gameManager;

    public TextMeshProUGUI _firstCardTMP;
    public TextMeshProUGUI _secondCardTMP;
    public TextMeshProUGUI _thirdCardTMP;
    public TextMeshProUGUI _resultsTMP;

    public int _pointsPrime;
    public int _pointsSecondary;
    public int _timerAmount;

    public TextMeshProUGUI _succeedButtonTMP;
    public TextMeshProUGUI _failButtonTMP;
    public TextMeshProUGUI _giveUpButtonTMP;

    public string[] _succeedMessageList;
    public string[] _failMessageList;

    private List<Player> _players;
    

    void Start()
    {
        _players = new List<Player>();

        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    #region ButtonFunctions

    public void SendWinSignal()
    {
        Signal.Send("Game", "CardWin");
    }

    public void SendFailSignal()
    {
        Signal.Send("Game", "CardFail");
    }

    public void SendGiveUpSignal()
    {
        Signal.Send("Game", "CardGiveUp");
    }

    public void SuccessButtonFunctionality()
    {
        Signal.Send("Game", "NextCard");
        
    }

    #endregion

    #region CardTexts
    public void ParseAndUpdateCardText(string _cardText1)
    {
        ResetCard();

        _firstCardTMP.SetText(_cardText1);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2)
    {
        ResetCard();
        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, string _cardText3)
    {
        ResetCard();
        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
        _thirdCardTMP.SetText(_cardText3);
    }



    public void ParseAndUpdateCardText(string _cardText1, Player player1)
    {
        ResetCard();

        _cardText1 = _cardText1.Replace("Player1", player1._name);

        _firstCardTMP.SetText(_cardText1);
        _players.Add(player1);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, Player player1)
    {
        ResetCard();

        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
        _players.Add(player1);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, string _cardText3, Player player1)
    {
        ResetCard();

        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);
        _cardText3 = _cardText3.Replace("Player1", player1._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
        _thirdCardTMP.SetText(_cardText3);

        _players.Add(player1);
    }

    public void ParseAndUpdateCardText(string _cardText1, Player player1, Player player2)
    {
        ResetCard();

        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _firstCardTMP.SetText(_cardText1);

        _players.Add(player1);
        _players.Add(player2);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, Player player1, Player player2)
    {
        ResetCard();
        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _cardText2 = _cardText2.Replace("Player2", player2._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);

        _players.Add(player1);
        _players.Add(player2);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, string _cardText3, Player player1, Player player2)
    {
        ResetCard();
        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);
        _cardText3 = _cardText3.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _cardText2 = _cardText2.Replace("Player2", player2._name);
        _cardText3 = _cardText3.Replace("Player2", player2._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
        _thirdCardTMP.SetText(_cardText3);

        _players.Add(player1);
        _players.Add(player2);
    }

    public void ParseAndUpdateCardText(string _cardText1, Player player1, Player player2, Player player3)
    {
        ResetCard();

        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _cardText1 = _cardText1.Replace("Player3", player3._name);
        _firstCardTMP.SetText(_cardText1);

        _players.Add(player1);
        _players.Add(player2);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, Player player1, Player player2, Player player3)
    {
        ResetCard();
        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _cardText2 = _cardText2.Replace("Player2", player2._name);
        _cardText1 = _cardText1.Replace("Player3", player3._name);
        _cardText2 = _cardText2.Replace("Player3", player3._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);

        _players.Add(player1);
        _players.Add(player2);
        _players.Add(player3);
    }

    public void ParseAndUpdateCardText(string _cardText1, string _cardText2, string _cardText3, Player player1, Player player2, Player player3)
    {
        ResetCard();
        _cardText1 = _cardText1.Replace("Player1", player1._name);
        _cardText2 = _cardText2.Replace("Player1", player1._name);
        _cardText3 = _cardText3.Replace("Player1", player1._name);
        _cardText1 = _cardText1.Replace("Player2", player2._name);
        _cardText2 = _cardText2.Replace("Player2", player2._name);
        _cardText3 = _cardText3.Replace("Player2", player2._name);
        _cardText1 = _cardText1.Replace("Player3", player3._name);
        _cardText2 = _cardText2.Replace("Player3", player3._name);
        _cardText3 = _cardText3.Replace("Player3", player3._name);

        _firstCardTMP.SetText(_cardText1);
        _secondCardTMP.SetText(_cardText2);
        _thirdCardTMP.SetText(_cardText3);

        _players.Add(player1);
        _players.Add(player2);
        _players.Add(player3);
    }

    private void ResetCard()
    {
        _players.Clear();

        _firstCardTMP.SetText(" ");

        if (_secondCardTMP)
            _secondCardTMP.SetText(" ");

        if (_thirdCardTMP)
            _thirdCardTMP.SetText(" ");
    }

    #endregion

    public void SetUpAndStartTimer()
    {
        CountdownController cDC = GameObject.FindObjectOfType<CountdownController>().GetComponent<CountdownController>();
        cDC.SetAndStartCountdown(_timerAmount);
    }

    #region ButtonTexts
    public void UpdateButtonTMPs()
    {
        _succeedButtonTMP.SetText(_succeedMessageList[Random.Range(0, _succeedMessageList.Length)]);
        _failButtonTMP.SetText(_failMessageList[Random.Range(0, _failMessageList.Length)]);
    }

    public void UpdateButtonTMPs(string successButtonText, string failButtonText)
    {
        _succeedButtonTMP.SetText(successButtonText);
        _failButtonTMP.SetText(failButtonText);
    }

    #endregion
}
