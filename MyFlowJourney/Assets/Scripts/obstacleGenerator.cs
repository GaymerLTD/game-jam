using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

public class obstacleGenerator : MonoBehaviour
{
    private RaycastHit2D hit;
    private new Rigidbody2D rigidbody2D;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private int _waitTime = 10;
    private float length = 1f;


    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        hit = Physics2D.Raycast(transform.position, -Vector2.up, length, groundMask);

        if (hit.collider != null)
        {   
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(_waitTime * Time.deltaTime);
        Destroy(rigidbody2D);
    }
}