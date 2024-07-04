using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifetime = 1;
    public int pierce = 0;
    public GameObject deathFX;
    private Rigidbody2D rb;
 
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, AngleHelper.VectorToDegrees(rb.velocity.normalized)-90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag.Equals("Enemy"))
        {
            Hit(collision.gameObject);
        }
    }

    public void Hit(GameObject target)
    {
        Destroy(gameObject);
        target.GetComponent<Enemy>().Damage(damage);
        if (deathFX != null)
        {
            GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
            int rand = Random.Range(0, 4);
            fx.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rand * 90));
        }
    }
}
