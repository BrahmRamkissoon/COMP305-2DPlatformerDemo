// Filename: PlayerController.cs
// Author: Brahm Ramkissoon
// Created Date  (dd/mm/yyyy): 22/10/2015
// Description: manage player movement
// recall we are using a gravity scale of 200 and reduced player mass of 0.03

using UnityEngine;
using System.Collections;

/*          --Range of force on an object---
*       
*   Online tutorial uses Vector2 but here a custom class
*   VelocityRange with constructor is used 
*   public Vector2 maxVelocity = new Vector2( 300f, 1000f );
*/
// VELOCITY RANGE UTILITY CLASS
[System.Serializable]
public class VelocityRange
{
    // PUBLIC INSTANCE VARIABLES
    public float vMin, vMax;

    public VelocityRange(float vMin, float vMax)
    {
        this.vMin = vMin;
        this.vMax = vMax;
    }
}

// PLAYERCONTROLLER CLASS
public class PlayerController : MonoBehaviour {

    // PUBLIC INSTANCE VARIABLES
    public float speed = 50f;
    public float jump = 500f;

    public VelocityRange velocityRange = new VelocityRange( 300f, 1000f );


    // PRIVATE INSTANCE VARIABLES
    private AudioSource [] _audioSources;
    private AudioSource _coinSound;
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    private Animator _animator;     // use this later

    private float _moving = 0f;

	// Use this for initialization
	void Start ()
	{
	    this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
	    this._transform = gameObject.GetComponent<Transform>();
	    //this._animator = gameObject.GetComponent<Animator>();

        // Reference to audio source
        this._audioSources = gameObject.GetComponents<AudioSource>();
	    this._coinSound = this._audioSources[0];
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    float forceX = 0f;
	    float forceY = 0f;

	    float absVelX = Mathf.Abs( this._rigidBody2D.velocity.x );
        float absVelY = Mathf.Abs( this._rigidBody2D.velocity.y );

	    this._moving = Input.GetAxis( "Horizontal" );    // value is between -1 and 1

	    if (this._moving != 0)
	    {
            // we're moving
	        if (this._moving > 0)
	        {
	            // moving right
	            if (absVelX < this.velocityRange.vMax)
	            {
                    forceX = this.speed;
	            }
	        } else if (this._moving < 0)
	        {
                //moving left
                if ( absVelX < this.velocityRange.vMax )
                {
                    forceX = -this.speed;
                }
            }
	    } else if (this._moving == 0)
	    {
            // we're idle
	    }

        this._rigidBody2D.AddForce(new Vector2 (forceX, forceY));
	    

	}

    void OnCollisionEnter2D ( Collision2D otherCollider )
    {
        if ( otherCollider.gameObject.CompareTag("Coin") )
        {
            this._coinSound.Play();
        }
    }
}
