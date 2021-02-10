using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float limiteIzq;
    [SerializeField] private float limiteDer;
    [SerializeField] private float longitudSalto;
    [SerializeField] private float alturaSalto;
    [SerializeField] private LayerMask Suelo;

    private Collider2D coll;
    private bool miraIzq = true;
    private Transform target;
    private float speed = 1f;

    protected void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        float distancia = transform.position.x - target.position.x;
        if(Mathf.Abs(distancia) < 20)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        if(anim.GetBool("Jumping"))
        {
            if(rbody.velocity.y < .1)
            {
                anim.SetBool("Falling",true);
                anim.SetBool("Jumping",false);
            }
        }
        if(coll.IsTouchingLayers(Suelo)&&anim.GetBool("Falling"))
        {
            anim.SetBool("Falling",false);
            anim.SetBool("Jumping",false);
        }
        
    }
    private void move()
    {
        if(miraIzq)
        {
            if(transform.position.x > limiteIzq)
            {
                if(transform.position.x != 1)
                {
                    transform.localScale = new Vector3(1,1);
                }
                if(coll.IsTouchingLayers(Suelo))
                {
                    rbody.velocity = new Vector2(-longitudSalto,alturaSalto);
                    anim.SetBool("Jumping",true);
                }
            }
            else
            {
                miraIzq = false;

            }
        }
        else{
            if(transform.position.x < limiteDer)
            {
                if(transform.position.x != -1)
                {
                    transform.localScale = new Vector3(-1,1);
                }
                if(coll.IsTouchingLayers(Suelo))
                {
                    rbody.velocity = new Vector2(longitudSalto,alturaSalto);
                    anim.SetBool("Jumping",true);
                }
            }
            else
            {
                miraIzq = true;

            }
        }
    }
    

}
