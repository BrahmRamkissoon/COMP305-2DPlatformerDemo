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

    public VelocityRange ( float vMin, float vMax )
    {
        this.vMin = vMin;
        this.vMax = vMax;
    }
}

// PLAYERCONTROLLER CLASS
public class PlayerController : MonoBehaviour
{

    // PUBLIC INSTANCE VARIABLES
    public float speed = 50f;
    public float jump = 500f;

    public VelocityRange velocityRange = new VelocityRange(300f, 1000f);


    // PRIVATE INSTANCE VARIABLES
    private AudioSource [] _audioSources;
    private AudioSource _coinSound;
    private AudioSource _jumpSound;
    private Rigidbody2D _rigidBody2D;
    private Transform _transform;
    private Animator _animator;
    

    private float _movingValue = 0f;
    private bool _isFacingRight = true;
    private bool _isGrounded = true;

    // Use this for initialization
    void Start ()
    {
        this._rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        this._transform = gameObject.GetComponent<Transform>();
        this._animator = gameObject.GetComponent<Animator>();

        // Reference to audio source
        this._audioSources = gameObject.GetComponents<AudioSource>();
        this._coinSound = this._audioSources [0];
        this._jumpSound = this._audioSources [1];
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float forceX = 0f;
        float forceY = 0f;

        float absVelX = Mathf.Abs(this._rigidBody2D.velocity.x);
        float absVelY = Mathf.Abs(this._rigidBody2D.velocity.y);

        this._movingValue = Input.GetAxis("Horizontal");    // value is between -1 and 1

        // Check if player is moving
        if ( this._movingValue != 0 )
        {
            // we're moving
            // set anim state
            this._animator.SetInteger("AnimState", 1);    // play walk clip
            if ( this._movingValue > 0 )     // moving right
            {
                this._isFacingRight = true;
                this._flip();
                if ( absVelX < this.velocityRange.vMax )
                {
                    forceX = this.speed;
                }

            }   else if ( this._movingValue < 0 )        //moving left
            {
                this._isFacingRight = false;
                this._flip();
                if ( absVelX < this.velocityRange.vMax )
                {
                    forceX = -this.speed;
                }
            }
        } else if ( this._movingValue == 0 ) // we're idle
        {
            this._animator.SetInteger("AnimState", 0);  // play idle clip
        }

        // Check if player is jumping
        if (Input.GetKey( "up" ) || Input.GetKey( KeyCode.W ))
        {
            // Check if the player is grounded
            if (this._isGrounded)
            {
                // player is jumping
                this._animator.SetInteger( "AnimState", 2 );       // play jump anim
                if ( absVelY < this.velocityRange.vMax )
                {
                    forceY = this.jump;
                    this._jumpSound.Play();
                    this._isGrounded = false;
                }
            }
            
        }

        // add force along X and Y axis depending on player input
        this._rigidBody2D.AddForce(new Vector2(forceX, forceY));
    }

    // Play sound on collision with coin
    void OnCollisionEnter2D ( Collision2D otherCollider )
    {
        if ( otherCollider.gameObject.CompareTag("Coin") )
        {
            this._coinSound.Play();
        }
    }

    // Set isGrounded true while touching ground
    void OnCollisionStay2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.CompareTag( "Platform" ))
        {
            this._isGrounded = true;
        }
    }

    // PRIVATE METHODS
    private void _flip()
    {
        if (this._isFacingRight )
        {
            this._transform.localScale = new Vector3( 1f, 1f, 1f );     // flip to face right ( normal )
        }
        else
        {
            this._transform.localScale = new Vector3( -1f, 1f, 1f );    // flip to face left
        }
    }
}
