using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private Collider2D pl;
    private enum Estado{idle,running,Jumping,falling,hurt}
    private Estado estado = Estado.idle;
    public CheckMaster cm;
    private bool aumentado = false;

    [SerializeField] private LayerMask suelo;
    public float poderSalto;
    public float velocidad;
    public float Daño;
    [SerializeField] private AudioSource punto;
    [SerializeField] private AudioSource pasos;


    [SerializeField] GameObject menuPause;
    public GameObject  MenuPause {get {return menuPause;}}
    [SerializeField] GameObject menuSkills;
    public GameObject  MenuSkills {get {return menuSkills;}}
    [SerializeField] Text textoInfo;
    public Text TextoInfo {get {return textoInfo;}}
    public static bool GameIsPaused = false;


    public void Pause()
    {
        MenuPause.gameObject.SetActive(true);
        GameIsPaused = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void SkillMenu()
    {
        MenuPause.gameObject.SetActive(false);
        MenuSkills.gameObject.SetActive(true);
        TextoInfo.gameObject.SetActive(false);
    }
    public void skillCheckSalto()
    {
        if(PermanentUI.perm.puntos >= 4 && aumentado == false) {poderSalto += 5;aumentado=true;}
        else TextoInfo.gameObject.SetActive(true);
    }
    public void skillCheckVel()
    {
        if(PermanentUI.perm.puntos >= 4 && aumentado == false) {velocidad += 5;aumentado=true;}
        else TextoInfo.gameObject.SetActive(true);
    }
    public void skillCheckDaño()
    {
        if(PermanentUI.perm.puntos >= 4 && aumentado == false) {Daño += 1;aumentado=true;PermanentUI.perm.daño=true;}
        else TextoInfo.gameObject.SetActive(true);
    }
    public void NormalMenu()
    {
        MenuPause.gameObject.SetActive(true);
        MenuSkills.gameObject.SetActive(false);
    }
    public void Resume()
    {
        MenuPause.gameObject.SetActive(false);
        GameIsPaused = false;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        pl = GetComponent<Collider2D>();
        PermanentUI.perm.cantidadVida.text = PermanentUI.perm.vida.ToString();
        cm = GameObject.FindGameObjectWithTag("CheckMaster").GetComponent<CheckMaster>(); 
        transform.position = cm.posicionUltimoPuntoControl;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused){Resume();}
            else {Pause();}
        }
        if(estado != Estado.hurt)
        {
            Movimiento();
        }
        EstadoAnimacion();
        anim.SetInteger("state", (int)estado);
        //if(!PermanentUI.perm) PermanentUI.perm.puntos = pun;
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        string s = "QuizPoint" + PermanentUI.perm.puntos.ToString();
        if(colision.tag == s)
        {
            punto.Play();
            SceneManager.LoadScene("Puzzle1");
            PermanentUI.perm.puntos += 1;
            PermanentUI.perm.puntosTexto.text = PermanentUI.perm.puntos.ToString();
            PermanentUI.perm.cont++;
        }
    }

    private void OnCollisionEnter2D(Collision2D otro)
    {
        if(otro.gameObject.tag == "Enemy")
        {
            Enemy enemigo = otro.gameObject.GetComponent<Enemy>();
            if(estado == Estado.falling)
            {
                enemigo.Saltado();
                Salto();
            }
            else
            {
                estado = Estado.hurt;
                ControlarVida();
                if(otro.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-Daño,rb.velocity.y);
                } 
                else
                {
                    rb.velocity = new Vector2(Daño,rb.velocity.y);
                }
            }
        }
        if(otro.gameObject.tag == "Boss")
        {
            Enemy enemigo = otro.gameObject.GetComponent<Enemy>();
            if(estado == Estado.falling)
            {
                enemigo.Saltado();
                Salto();
            }
            else
            {
                estado = Estado.hurt;
                ControlarVida();
                if(otro.gameObject.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-Daño,rb.velocity.y);
                } 
                else
                {
                    rb.velocity = new Vector2(Daño,rb.velocity.y);
                }
            }
            if(enemigo.death == true){PermanentUI.perm.puntos += 4;}
        }
    }




    private void ControlarVida()
    {
        PermanentUI.perm.vida -= 1;
        PermanentUI.perm.cantidadVida.text = PermanentUI.perm.vida.ToString();
        if(PermanentUI.perm.vida <= 0)
        {
            PermanentUI.perm.ResetearVida();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Movimiento()
    {
        float mueve = Input.GetAxis("Horizontal");
        if (mueve < 0)
        {
            rb.velocity = new Vector2(-velocidad, rb.velocity.y);
            sprite.flipX = true;

        }
        else if (mueve > 0)
        {
            rb.velocity = new Vector2(velocidad, rb.velocity.y);
            sprite.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && pl.IsTouchingLayers(suelo))
        {
            Salto();
        }
        
    }
    private void Salto()
    {
        rb.velocity = new Vector2(rb.velocity.x, poderSalto);
        estado = Estado.Jumping;
    }

    private void EstadoAnimacion()
    {
        if(estado == Estado.Jumping)
        {
            if(rb.velocity.y < 0)
            {
                estado = Estado.falling;
            }
        }
        else if(estado == Estado.falling)
        {
            if(pl.IsTouchingLayers(suelo))
            {
                estado = Estado.idle;
            }
        }
        else if(estado == Estado.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                estado = Estado.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            estado = Estado.running;
        }
        else
        {
            estado = Estado.idle;
        }
        
        
    }

    private void Pasos()
    {
        pasos.Play();
    }
}
