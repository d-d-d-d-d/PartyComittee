using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameButton : MonoBehaviour
{
    public string _name;
    public RosterController _rosterController;
    public GameManager _gameManager;
    public TextMeshProUGUI _label;


    void Start()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        }
        if (_rosterController == null)
        {
            _rosterController = FindObjectOfType<RosterController>().GetComponent<RosterController>();
        }
    }

    public void OnButtonPush()
    {
        RemoveButtonAndName();

    }

    public void UpdateButton(string name)
    {
        _name = name.ToUpper();
        _label.SetText(name);

    }

    public void RemoveButtonAndName()
    {
        _gameManager.RemovePlayerFromGame(_name);
        Destroy(gameObject);
    }
}
