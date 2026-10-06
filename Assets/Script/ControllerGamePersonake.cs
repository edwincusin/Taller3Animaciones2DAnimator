using UnityEngine;

public class ControllerGamePersonake : MonoBehaviour
{
    // ---------- VARIABLES (fuera de las funciones) ----------
    private Rigidbody2D cuerpoRigido;
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;
    private bool estaEnSuelo;

    public Animator animador;
    public bool mirandoDerecha = true;

    // Start se ejecuta una sola vez, al iniciar
    void Start()
    {
        cuerpoRigido = GetComponent<Rigidbody2D>();
        animador = GetComponent<Animator>();
    }

    // Update se ejecuta en cada frame
    void Update()
    {
        //MOVIMIENTO HORIZONTAL
        float movimiento = Input.GetAxis("Horizontal");
        cuerpoRigido.linearVelocity = new Vector2(movimiento * velocidad, cuerpoRigido.linearVelocityY);

        //SALTO
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            cuerpoRigido.linearVelocityY = fuerzaSalto;
            estaEnSuelo = false;
            animador.SetBool("estaSaltando", true);
        }

        //CALCULA VELOCIDAD PARA TRANSICION 
        float velocidadAnimacion = Mathf.Abs(movimiento);
        animador.SetFloat("Velocidad", velocidadAnimacion);

        //VOLTEAR SPRITE O PERSONAJE --> se voltea si se mueve el lado contrario para que se ejecute movimiento debe ser mayor o menor a 0.
        if(movimiento>0 && !mirandoDerecha)
        {
            Voltear();
        } else if (movimiento < 0 && mirandoDerecha)
        {
            Voltear();
        }

        //PARA ATACAR
        if (Input.GetKeyDown(KeyCode.C))
        {
            animador.SetTrigger("Atacar");
        }


    }
    //CUANDO EL PERSONAJE TOCAR OTRO OBJETO
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = true;
            animador.SetBool("estaSaltando", false);
        }
    }

    //CUANDO EL PERSONAJE DEJA DE TOCAR OTRO OBJETO
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            estaEnSuelo = false;
            animador.SetBool("estaSaltando", true);
        }
    }


    // ---------- VOLTEAR ----------
    // Invierte hacia dónde mira el personaje
    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}