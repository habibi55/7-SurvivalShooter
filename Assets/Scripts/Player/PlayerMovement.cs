using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigidbody;
    private int floorMask;
    private float camRayLength = 100f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 6f * 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 6f;
        }
    }
    private void Awake()
    {
        // mendapatkan nilai mask dari layer yang bernama Floor
        floorMask = LayerMask.GetMask("Floor");
        
        // mendapatkan komponen animator
        anim = GetComponent<Animator>();
        
        // mendapatkan komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Mendapatkan nilai input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        //Mendapatkan nilai input vertical (-1,0,1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    // method yang membuat karakter melakukan animasi
    public void Animating(float horizontalAxis, float verticalAxis)
    {
        bool walking = horizontalAxis != 0f || verticalAxis != 0f;
        anim.SetBool("IsWalking", walking);
    }

    // method yang membuat karakter bisa melihat ke arah cursor mouse berada saat main
    private void Turning()
    {
        // buat ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // buat raycast untuk floorHit
        RaycastHit floorHit;
        
        // lakukan raycast
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // mendapatkan vector dari posisi player ke posisi floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            
            // mendapatkan look rotation baru ke posisi hit berada
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            
            // Rotasi karakter
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    // method yang membuat karakter bergerak
    public void Move(float horizontalAxis, float verticalAxis)
    {
        // set nilai x dan y
        movement.Set(horizontalAxis, 0f, verticalAxis);
        
        // menormalisasi nilai vector agar total panjang dari vector adalah 1
        movement = movement.normalized * speed * Time.deltaTime;
        
        // bergerak ke posisi tujuan
        playerRigidbody.MovePosition(transform.position + movement);
    }
}