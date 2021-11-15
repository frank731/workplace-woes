using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public GameObject paper;
    public Rigidbody rb;
    public AudioSource audioSource;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.lostEvent.AddListener(Lost);
        transform.DOMove(gameManager.bossDesk.position, 6).OnComplete(Rotate);
        //StartCoroutine(KinematicFunctions.MoveObject(transform, transform.position, GameManager.Instance.bossDesk.position, 3f));
    }
    void Lost()
    {
        rb.isKinematic = false;
        audioSource.enabled = false;
        DOTween.KillAll(true);
    }

    void Rotate()
    {
        audioSource.Pause();
        animator.SetTrigger("Stand");
        transform.DORotate(new Vector3(0, 270, 0), 1).OnComplete(DropPaper);
    }

    void DropPaper()
    {
        for(int i = 0; i < Random.Range(6, gameManager.maxPaper); i++)
        {
            Instantiate(paper, gameManager.paperSpot.position + new Vector3(0, i * 0.1f, 0), gameManager.paperSpot.rotation);
        }
        gameManager.maxPaper = Mathf.Min(gameManager.maxPaper + 2, 15);
        transform.DORotate(new Vector3(0, 180, 0), 1).OnComplete(Leave);
    }
    void Leave()
    {
        audioSource.Play();
        animator.SetTrigger("Walk");
        transform.DOMove(gameManager.bossEnd.position, 6).OnComplete(Del);
    }
    void Del()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("BossCheck"))
        {
            gameManager.watched = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("BossCheck")) gameManager.watched = false;
    }
    
}
