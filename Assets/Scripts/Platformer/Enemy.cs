using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rbody;
    protected AudioSource muerte;
    public int vida = 3;
    public bool death = false;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        muerte = GetComponent<AudioSource>();
    }
    public void Saltado()
    {
        vida--;
        if(PermanentUI.perm.daño == true)vida--;
        if(vida == 0)
        {
            anim.SetTrigger("Death");
            muerte.Play();
            rbody.velocity = Vector2.zero;
            rbody.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Collider2D>().enabled = false;
            death = true;
        }
        
    }
    private void Muerte()
    {
        Destroy(this.gameObject);
    }
}
