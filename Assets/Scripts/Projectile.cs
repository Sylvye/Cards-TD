using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public float lifetime = 1;
    public int pierce = 0;
    public bool randomFX = true;
    public GameObject[] deathFX;
    public GameObject[] despawnFX;
    private Rigidbody2D rb;
 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(lifetime));
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
            int spawnCount = deathFX.Length;
            if (randomFX)
                spawnCount = 1;
            for (int i = 0; i < spawnCount; i++)
            {
                int objIndex = i;
                if (randomFX)
                    objIndex = Random.Range(0, deathFX.Length);
                GameObject fx = Instantiate(deathFX[objIndex], transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < despawnFX.Length; i++)
        {
            Instantiate(despawnFX[i], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
