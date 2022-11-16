using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RosterController : MonoBehaviour
{
    public GameObject _nameItemPrefab;
    public TMP_InputField _inputField;
    public GameManager _gameManager;
    public RectTransform _nameItemHolder;
    
    public void AddPlayerButton()
    {
        AddNewName(_inputField.text);
        
    }

    public void AddNewName(string name)
    {
        if (name != null && name != " ")
        {
            var newNameButton = GameObject.Instantiate(_nameItemPrefab);

            NameButton nb = newNameButton.GetComponent<NameButton>();
            nb.transform.SetParent(_nameItemHolder);
            newNameButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            
            nb.UpdateButton(name);
            _gameManager.AddPlayerToGame(name.ToUpper());
        }
    }
    
    void Start()
    {

        if (_gameManager == null)
            _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
    
}
