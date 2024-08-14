using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    [SerializeField]
    private bool _endGame = false;
    public void setEndGame(bool endGame) => _endGame = endGame;
    public bool getEndGame() => _endGame;

    [SerializeField]
    private float _distance = 0;

    [SerializeField]
    private Vector3 startPosition; 

    private GameObject _player;

    private UIManager _UIManager;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

         if (startPosition == Vector3.zero)
        {
            startPosition = _player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_player.transform.position, startPosition);
        _UIManager.setDistanceText(_distance);
    }

    public void endGame(){
        _endGame = true;
        //do something 
    }
}
