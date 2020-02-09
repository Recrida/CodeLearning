using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;
     
    private Animator animator;
    private AudioSource audioPlayer;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = transform.position.y == startY;
        bool userAction = Input.GetKeyDown("up") || Input.GetKeyDown("space") || Input.GetMouseButtonDown(0);
        bool gamePlaying = game.GetComponent<CreateControler>().gameState == GameState.Playing;
        if (gamePlaying && userAction && isGrounded)
        {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip; 
            audioPlayer.Play();

        }
    }
    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerDie");
            DustStop(); 
            game.GetComponent<CreateControler>().gameState = GameState.Fin;
            enemyGenerator.SendMessage("StopGenerator", true);
            game.SendMessage("ResetTimeScale", 0.5f); 
           
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        }
        if (other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();

        }

    }
    void GameReady()
    {
        game.GetComponent<CreateControler>().gameState = GameState.Ready;
    }
    void DustPlay()
    {
        dust.Play();
    }
    void DustStop()
    {
        dust.Stop();
    }

}
