using UnityEngine;

public class characterController : MonoBehaviour
{
     public bool move = false;
    public GameObject sprites;
    public Rigidbody2D rb;
    public float speed = 20f;
    public bool isgrounded = true;
    public float rotationspeed = 1.5f;
    public float backwardrotspeed = -1f;
    public bool didihitground = false;
    public float jumpforce = 10f;
    public float boostspeed;
    public GameObject cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
        {
            move = true;
        }
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Jump"))
        {
            move = false;
        }
        

    }
    private void FixedUpdate()
    {
        
        if (didihitground == false)
        {
            rb.AddForce(transform.right * speed * Time.deltaTime * 100f, ForceMode2D.Force);
        }


        if (move == true)
        {
            if (isgrounded==false)
            {
                rb.AddTorque(rotationspeed * rotationspeed, ForceMode2D.Force);
       
            }
            else
            {
                rb.AddForce(transform.up * jumpforce * Time.fixedDeltaTime * 100f, ForceMode2D.Force);
            }

        }
        
        
        if(move == false )
        {
            if(isgrounded == false)
            {
                rb.AddTorque(backwardrotspeed * 1 * Time.fixedDeltaTime * 100f, ForceMode2D.Force);
            }
        }

    }
    public void OnCollisionEnter2D()
    {
        isgrounded = true;

    }
    public void OnCollisionExit2D()
    {
        isgrounded = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boost")
        {
            rb.AddForce(transform.right * boostspeed, ForceMode2D.Impulse);
        }
        if(collision.tag == "Ground")
        {
            didihitground = true;
            Destroy(gameObject);

        }
        if(collision.tag == "Rock")
        {
            didihitground = true;
            Destroy(gameObject);
        }
    }
}
