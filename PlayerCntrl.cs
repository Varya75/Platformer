using UnityEngine;

public class PlayerCntrl : MonoBehaviour
{
    public float speed;
	private Rigidbody2D rb;
	
	private bool faceRight = true;
	
	//ссылка на компонент Transform объекта
	//для определения соприкосновения с землей
	public Transform LayerCheck;
	//радиус определения соприкосновения с землей
	private float Radius = 0.2f;
	//ссылка на слой, представляющий землю
	public LayerMask whatIsGround;
	public LayerMask whatIsLader;
	public AudioSource jumpSound;
	public GameObject Chest; 
	public AudioSource moveSound;
	public AudioSource laderSound;
	public Sprite openChest;

	public AudioClip[] moveSoundArray;
	int i;
	public AudioClip[] laderSoundArray;
	int l;

	private bool OnGround;
	private bool OnLader;


	void Start()
    {
        rb = GetComponent <Rigidbody2D> ();
		if(PlayerPrefs.HasKey("x") && PlayerPrefs.HasKey("y") && PlayerPrefs.HasKey("z"))
        {
			transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));
        }
	}

    
    void Update()
    {
		float moveX = Input.GetAxis ("Horizontal");
		//rb.MovePosition (rb.position + Vector2.right * moveX * speed * 6 * Time.deltaTime);
		rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
		OnGround = Physics2D.OverlapCircle(LayerCheck.position, Radius, whatIsGround);
		OnLader = Physics2D.OverlapCircle(LayerCheck.position, Radius, whatIsLader);

		if (OnGround && Input.GetKeyDown(KeyCode.Space))
		{
			rb.AddForce(Vector2.up * 2000);
			jumpSound.Play();
		}

		if (rb.velocity.x != 0 && OnGround)
		{
			if (!moveSound.isPlaying)
				MoveSound();
		}
		else
			moveSound.Stop();



		if (moveX > 0 && !faceRight)
			Flip();
		else if (moveX < 0 && faceRight)
			Flip();
		
		if (rb.velocity.y != 0 && OnLader)
		{
			if (!laderSound.isPlaying)
				LaderSound();
	    }
	  else
			laderSound.Stop();
	   
	 
	}
	void Flip()
	{
		faceRight = !faceRight;
		transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
	}

	void MoveSound()
    {
		moveSound.PlayOneShot(moveSoundArray[i]);
		i++;
		if (i == moveSoundArray.Length)
			i = 0;
    }
	void LaderSound()
    {
		laderSound.PlayOneShot(laderSoundArray[l]);
		l++;
		if (l == laderSoundArray.Length)
			l = 0;
	}
	
		 private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Coin"))
        {
          CoinText.coinCount += 1;
		  Destroy(coll.gameObject);
		  
        }
		if (coll.tag.Equals("Chest"))
        {
          CoinText.coinCount += 20;
		  Chest.GetComponent<SpriteRenderer>().sprite = openChest;
        }
        
    }
	
  
}