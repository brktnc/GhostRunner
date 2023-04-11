using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement = movement.normalized * speed * Time.deltaTime;

        // Nesne konumunu transform.position ile güncelle
        transform.position = new Vector2(transform.position.x + movement.x, transform.position.y + movement.y);

        // Nesnenin RigidBody bileşeninin konumunu da güncelle
        rb2d.MovePosition(transform.position);
    }
}
