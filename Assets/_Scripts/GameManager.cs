using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Doozy;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Animators;
using Databox;


public class GameManager : MonoBehaviour
{

    //Card list for different game phases
    public List<Card> _cards1, _cards2, _cards3, _cards4;
    public List<Player> _players, _currentPlayers;
    public DataboxObject _cardDatabase;
    public string _tableName = "English";
    public TextMeshProUGUI _roundTitleTMP, _roundCardBodyTMP;

    //Signal stuff
    public string _streamCategory = "Game";
    public string _streamNextCard = "NextCard";
    private SignalReceiver _signalReceiver;
    private SignalStream _signalStream;

    //there are 4 phases of the game. 
    //phase 1 = draw from phase 1/2 cards
    //phase 2 = draw from phase 1/2/3 cards
    //phase 3 = draw from phase 2/3/4 cards
    //phase 4 = draw from phase 3/4 cards
    public int _gamePhase = 0;
    public int _numberOfPlayers;
    public int _lengthOfGame = 80;
    public int _numberOfCardsPlayed = 0;

    public CardController _single1CC, _single2CC, _singleNoResultsCC, _double1CC, _double2CC, 
        _triple1CC, _triple2CC, _timerCC;
    public UIContainer _singleCardNoResultsContainer, _singleCard1Container, _singleCard2Container, 
        _doubleCardContainer1, _doubleCardContainer2, _tripleCardContainer1, _tripleCardContainer2, 
        _timerCardContainer;
    private Card _currentCard;
    private UIContainer _currentContainer;


    private string _cardChannel = " ";
    private string _lastPlayerNamed = " ";
    float lowscore = 0;
    bool _singleCardToggle = false;
    bool _doubleCardToggle = false;
    bool _tripleCardToggle = false;

    #region SetupAndUtility
    private void Start()
    {
        _players = new List<Player>();

        //permanent
        _cards1 = new List<Card>();
        _cards2 = new List<Card>();
        _cards3 = new List<Card>();
        _cards4 = new List<Card>();
        _currentPlayers = new List<Player>();

    }

    private void Awake()
    {
        _signalStream = SignalStream.Get(_streamCategory, _streamNextCard);
        _signalReceiver = new SignalReceiver().SetOnSignalCallback(OnSignal);
        
    }

    public void StartGame()
    {
        LoadDatabase();
    }

    public void SendLastSignal()
    {
        Signal.Send("Game", _cardChannel);
    }

    public void LoadDatabase()
    {
        Debug.Log("LoadDatabase called");
        _cardDatabase.LoadDatabase();
    }

    void DatabaseLoaded()
    {
        Debug.Log("Database loaded");
        GameSetup();
    }

    void GameSetup()
    {
        
        var _table = _cardDatabase.GetEntriesFromTable(_tableName);

        foreach (var item in _table.Keys)
        {
            int phase = _cardDatabase.GetData<IntType>(_tableName, item, "Phase").Value;
            int numberOfPlayers = _cardDatabase.GetData<IntType>(_tableName, item, "NumberOfPlayers").Value;
            //int pointsPrime = _cardDatabase.GetData<IntType>(_tableName, item, "PointsPrime").Value;
            //int pointsSecondary = _cardDatabase.GetData<IntType>(_tableName, item, "PointsSecondary").Value;
            int timerAmount = _cardDatabase.GetData<IntType>(_tableName, item, "TimerAmount").Value;

            string cardType = _cardDatabase.GetData<StringType>(_tableName, item, "CardType").Value;
            string card1Text = _cardDatabase.GetData<StringType>(_tableName, item, "Card1Text").Value;
            string card2Text = _cardDatabase.GetData<StringType>(_tableName, item, "Card2Text").Value;
            string card3Text = _cardDatabase.GetData<StringType>(_tableName, item, "Card3Text").Value;
            string succeedText = _cardDatabase.GetData<StringType>(_tableName, item, "SucceedText").Value;
            //string succeedButtonText = _cardDatabase.GetData<StringType>(_tableName, item, "SucceedButtonText").Value;
            //string failButtonText = _cardDatabase.GetData<StringType>(_tableName, item, "FailButtonText").Value;
            //string giveUpButtonText = _cardDatabase.GetData<StringType>(_tableName, item, "GiveUpButtonText").Value;
            
            Card card = new Card(phase, numberOfPlayers, cardType, card1Text, card2Text, card3Text, succeedText,
                timerAmount);

            if (card._phase == 1)
                _cards1.Add(card);
            else if (card._phase == 2)
                _cards2.Add(card);
            else if (card._phase == 3)
                _cards3.Add(card);
            else if (card._phase == 4)
                _cards4.Add(card);
        }

        Debug.Log("GameSetup run");
        GameLoop();
    }

    private void OnEnable()
    {
        _signalStream.ConnectReceiver(_signalReceiver);
        _cardDatabase.OnDatabaseLoaded += DatabaseLoaded;
    }
    private void OnDisable()
    {
        _signalStream.DisconnectReceiver(_signalReceiver);
        _cardDatabase.OnDatabaseLoaded -= DatabaseLoaded;
    }

    private void OnSignal(Signal signal)
    {
        Debug.Log("GameManger received signal");
        GameLoop();
    }

    #endregion

    #region Gameplay
    public void GameLoop()
    {
        _currentPlayers.Clear();
        int ranInt = 0;
        bool skipQ = false;

        //See what phase it is, choose what phase the cards will be chosen from
        if (_gamePhase <= 1 && _numberOfCardsPlayed < (0.25f * _lengthOfGame))
        {
            if (_gamePhase < 1)
                skipQ = true;

            _gamePhase = 1;
            ranInt = Random.Range(1, 3);
        }
        else if (_gamePhase <= 2 && _numberOfCardsPlayed < (0.5f * _lengthOfGame))
        {
            if (_gamePhase < 2)
                skipQ = true;

            _gamePhase = 2;
            ranInt = Random.Range(1, 4);
        }
        else if (_gamePhase <= 3 && _numberOfCardsPlayed < (0.75f * _lengthOfGame))
        {
            if (_gamePhase < 3)
                skipQ = true;

            _gamePhase = 3;
            ranInt = Random.Range(1, 5);
        }
        else if (_gamePhase <= 4 && _numberOfCardsPlayed < _lengthOfGame)
        {
            if (_gamePhase < 4)
                skipQ = true;

            _gamePhase = 4;
            ranInt = Random.Range(2, 5);
        }
        else if (_numberOfCardsPlayed >= _lengthOfGame)
        {
            EndSequence();
        }



        //Pick a card
        if (skipQ == false)
        {
            if (ranInt == 1)
            {
                int ind = Random.Range(0, _cards1.Count);
                _currentCard = _cards1[ind];
                _cards1.RemoveAt(ind);
            }
            else if (ranInt == 2)
            {
                int ind = Random.Range(0, _cards2.Count);
                _currentCard = _cards2[ind];
                _cards2.RemoveAt(ind);
            }
            else if (ranInt == 3)
            {
                int ind = Random.Range(0, _cards3.Count);
                _currentCard = _cards3[ind];
                _cards3.RemoveAt(ind);
            }
            else if (ranInt == 4)
            {
                int ind = Random.Range(0, _cards4.Count);
                _currentCard = _cards4[ind];
                _cards4.RemoveAt(ind);
            }

            NextCard(_currentCard);
        } else if (skipQ == true)
        {
            _roundTitleTMP.SetText("Round " + _gamePhase);
            if (_gamePhase == 1)
            {
                _roundCardBodyTMP.SetText("EASY - MODERATE");
            } else if (_gamePhase == 2)
            {
                _roundCardBodyTMP.SetText("EASY - MODERATE - HARD");
            } else if (_gamePhase == 3)
            {
                _roundCardBodyTMP.SetText("EASY - MODERATE - HARD - BRUTAL");
            } else if (_gamePhase == 4)
            {
                _roundCardBodyTMP.SetText("MODERATE - HARD - BRUTAL");
            }

            _cardChannel = "RoundCard";
            Signal.Send("Game", _cardChannel);
            //summon the RoundCard
        }

    }

    private void NextCard(Card currentCard) {
        Debug.Log("NextCard called. currentCard is " + currentCard._card1Text + " ; it has " + currentCard._numberOfPlayers + " players. And _cardType of " + currentCard._cardType);
        if (currentCard._cardType == "Single")
        {
            UIContainer currentSingleContainer;
            CardController currentCC;
            if (_singleCardToggle == false)
            {
                currentSingleContainer = _singleCard1Container;
                currentCC = _single1CC;
                _singleCardToggle = true;
                _cardChannel = "CardSingle1";
            } else
            {
                currentSingleContainer = _singleCard2Container;
                currentCC = _single2CC;
                _singleCardToggle = false;
                _cardChannel = "CardSingle2";
            }

            if (currentCard._numberOfPlayers == 0)
            {
                currentCC.ParseAndUpdateCardText(_currentCard._card1Text);
                _currentContainer = currentSingleContainer;
            }
            else if (currentCard._numberOfPlayers == 1)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne);
                _currentContainer = currentSingleContainer;
                AddPlayersForCard(playerOne);

            }
            else if (currentCard._numberOfPlayers == 2)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne, playerTwo);
                _currentContainer = currentSingleContainer;
                AddPlayersForCard(playerOne, playerTwo);
            }
            else if (currentCard._numberOfPlayers == 3)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;
                Player playerThree = GetPlayer();
                playerThree._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne, playerTwo, playerThree);
                _currentContainer = currentSingleContainer;
                AddPlayersForCard(playerOne, playerTwo, playerThree);
            }
            
        }
        else if (currentCard._cardType == "SingleNoResults")
        {
            _cardChannel = "CardSingleNoResults";
            if (currentCard._numberOfPlayers == 0)
            {
                _singleNoResultsCC.ParseAndUpdateCardText(_currentCard._card1Text);
                _currentContainer = _singleCardNoResultsContainer;
            }
            else if (currentCard._numberOfPlayers == 1)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;

                _singleNoResultsCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne);
                _currentContainer = _singleCardNoResultsContainer;
                AddPlayersForCard(playerOne);

            }
            else if (currentCard._numberOfPlayers == 2)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;

                _singleNoResultsCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne, playerTwo);
                _currentContainer = _singleCardNoResultsContainer;
                AddPlayersForCard(playerOne, playerTwo);
            }
            else if (currentCard._numberOfPlayers == 3)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;
                Player playerThree = GetPlayer();
                playerThree._juice += currentCard._phase;

                _singleNoResultsCC.ParseAndUpdateCardText(_currentCard._card1Text, playerOne, playerTwo, playerThree);
                _currentContainer = _singleCardNoResultsContainer;
                AddPlayersForCard(playerOne, playerTwo, playerThree);
            }
        }
        else if (currentCard._cardType == "Double")
        {
            UIContainer currentDoubleContainer;
            CardController currentCC;
            if (_doubleCardToggle == false)
            {
                _cardChannel = "CardDouble1";
                _doubleCardToggle = true;
                currentCC = _double1CC;
                currentDoubleContainer = _doubleCardContainer1;
            } else
            {
                _cardChannel = "CardDouble2";
                _doubleCardToggle = false;
                currentCC = _double2CC;
                currentDoubleContainer = _doubleCardContainer2;
            }

            if (currentCard._numberOfPlayers == 0)
            {
                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text);
                _currentContainer = currentDoubleContainer;
            }
            else if (currentCard._numberOfPlayers == 1)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne);
                _currentContainer = currentDoubleContainer;
                AddPlayersForCard(playerOne);
            }
            else if (currentCard._numberOfPlayers == 2)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne, playerTwo);
                _currentContainer = currentDoubleContainer;
                AddPlayersForCard(playerOne, playerTwo);
            }
            else if (currentCard._numberOfPlayers == 3)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;
                Player playerThree = GetPlayer();
                playerThree._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne, playerTwo, playerThree);
                _currentContainer = currentDoubleContainer;
                AddPlayersForCard(playerOne, playerTwo, playerThree);
            }
        }
        else if (currentCard._cardType == "Triple")
        {
            UIContainer currentTripleContainer;
            CardController currentCC;

            if (_tripleCardToggle == false)
            {
                _cardChannel = "CardTriple1";
                _tripleCardToggle = true;
                currentTripleContainer = _tripleCardContainer1;
                currentCC = _triple1CC;
            } else
            {
                _cardChannel = "CardTriple2";
                _tripleCardToggle = false;
                currentTripleContainer = _tripleCardContainer2;
                currentCC = _triple2CC;
            }

            if (currentCard._numberOfPlayers == 0)
            {
                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, _currentCard._card3Text);
                _currentContainer = currentTripleContainer;
            }
            else if (currentCard._numberOfPlayers == 1)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, _currentCard._card3Text, playerOne);
                _currentContainer = currentTripleContainer;
                AddPlayersForCard(playerOne);
            }
            else if (currentCard._numberOfPlayers == 2)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, _currentCard._card3Text, playerOne, playerTwo);
                _currentContainer = currentTripleContainer;
                AddPlayersForCard(playerOne, playerTwo);
            }
            else if (currentCard._numberOfPlayers == 3)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;
                Player playerThree = GetPlayer();
                playerThree._juice += currentCard._phase;

                currentCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, _currentCard._card3Text, playerOne, playerTwo, playerThree);
                _currentContainer = currentTripleContainer;
                AddPlayersForCard(playerOne, playerTwo, playerThree);
            }

        }
        else if (currentCard._cardType == "Timer")
        {
            _cardChannel = "CardTimer";
            _currentContainer = _timerCardContainer;
            if (currentCard._numberOfPlayers == 0)
            {
                _timerCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text);
                _timerCC._timerAmount = currentCard._timerAmount;

            }
            else if (currentCard._numberOfPlayers == 1)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;

                _timerCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne);
                _timerCC._timerAmount = currentCard._timerAmount;

                AddPlayersForCard(playerOne);
            }
            else if (currentCard._numberOfPlayers == 2)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;

                _timerCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne, playerTwo);
                _timerCC._timerAmount = currentCard._timerAmount;

                AddPlayersForCard(playerOne, playerTwo);
            }
            else if (currentCard._numberOfPlayers == 3)
            {
                Player playerOne = GetPlayer();
                playerOne._juice += currentCard._phase;
                Player playerTwo = GetPlayer();
                playerTwo._juice += currentCard._phase;
                Player playerThree = GetPlayer();
                playerThree._juice += currentCard._phase;

                _timerCC.ParseAndUpdateCardText(_currentCard._card1Text, _currentCard._card2Text, playerOne, playerTwo, playerThree);
                _timerCC._timerAmount = currentCard._timerAmount;

                AddPlayersForCard(playerOne, playerTwo, playerThree);
            }
        }
        else
            Debug.Log("NextCard had difficulty choosing.");

        //_currentContainer.Show();
        _numberOfCardsPlayed++;
        Debug.Log("NextCard sending " + _cardChannel + " on Game");
        Signal.Send("Game", _cardChannel);
    }
    #endregion

    #region PlayerManagement
    private Player GetPlayer()
    {

        //_players.Sort(SortByJuice);
        //int ind = 0;
        Player playerToPlay = _players[0];

        foreach (var item in _players)
        {
            if (item._juice < playerToPlay._juice)
                playerToPlay = item;
        }
        Debug.Log("GetPlayer is gonna return " + playerToPlay._name + " & juice is " + playerToPlay._juice);
        //_lastPlayerNamed = playerToPlay._name;
        return playerToPlay;
    }
    

    static int SortByJuice(Player p1, Player p2)
    {
        return p1._juice.CompareTo(p2._juice);
    }



    public void AddPlayerToGame(string playerName)
    {
        Player newPlayer = new Player(playerName, 0);
        _players.Add(newPlayer);
        _numberOfPlayers++;
    }

    public void RemovePlayerFromGame(Player playerToRemove)
    {
        _players.Remove(playerToRemove);

    }

    public void RemovePlayerFromGame(string playerName)
    {
        bool alreadyRemoved = false;
        foreach (var item in _players)
        {
            if (item._name == playerName && alreadyRemoved == false)
            {
                _players.Remove(item);
                alreadyRemoved = true;
            }
        }
    }

    private void AddPlayersForCard(Player player1)
    {
        _currentPlayers.Add(player1);
    }

    private void AddPlayersForCard(Player player1, Player player2)
    {
        _currentPlayers.Add(player1);
        _currentPlayers.Add(player2);
    }

    private void AddPlayersForCard(Player player1, Player player2, Player player3)
    {
        _currentPlayers.Add(player1);
        _currentPlayers.Add(player2);
        _currentPlayers.Add(player3);
    }

    private void AddPlayersForCard(bool allPlayers)
    {
        _currentPlayers.AddRange(_players);
    }


    private string InsertPlayerName(string text, int playerNumber)
    {
        string playerText = ("#Player" + (playerNumber + 1));
        text.Replace(playerText, _currentPlayers[playerNumber]._name);
        return text;
    }

    #endregion
    
    private void EndSequence() { }
}
