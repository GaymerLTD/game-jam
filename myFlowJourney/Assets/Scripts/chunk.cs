using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunk : MonoBehaviour
{

    private GameObject _player;
    private float _distance;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _distance = GameObject.FindGameObjectWithTag("levelGenerator").GetComponent<levelGenerator>().getDespawnDistance();
    }

    private void FixedUpdate()
    {
        checkDistanceFromPlayer();
    }

    private void checkDistanceFromPlayer(){
        if(Vector3.Distance(_player.transform.position, this.gameObject.transform.position) > _distance){
            Destroy(this.gameObject);
        }
    }


}
