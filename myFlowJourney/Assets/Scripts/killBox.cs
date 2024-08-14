using UnityEngine;

public class killBox : MonoBehaviour
{

    private gameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            //game end. 
            _gameManager.endGame();
        }
    }
}
