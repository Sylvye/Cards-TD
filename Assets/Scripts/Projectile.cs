using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Projectile : MonoBehaviour
{
    public GameObject parentTower;
    public int damage;
    public float speed;
    public float lifetime = 1;
    public int pierce = 0;
    public float explosionRadius = 0;
    public bool combo = false;
    public bool curse = false;
    public bool randomFX = true;
    public float homingSpeed = 0;
    public GameObject[] deathFX;
    public GameObject[] despawnFX;
    public float angle;
    private Rigidbody2D rb;
 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn(lifetime));
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (homingSpeed > 0)
        {
            GameObject target = parentTower.GetComponent<Turret>().targetEnemy;
            if (target != null)
            {
                float angleToTarget = AngleHelper.VectorToDegrees(target.transform.position - transform.position);
                angle = Mathf.LerpAngle(angle, angleToTarget, Time.deltaTime * homingSpeed);
            }
        }

        rb.velocity = speed * AngleHelper.DegreesToVector(angle);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, AngleHelper.VectorToDegrees(rb.velocity.normalized)-90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            Hit(collision.gameObject);
        }
    }

    public void Hit(GameObject target)
    {
        if (--pierce <= -1)
            Destroy(gameObject);
        if (pierce < -1)
            return;

        bool hitSuccessfully = false;

        if (explosionRadius == 0) // contact damage
        {
            if (target.GetComponent<Enemy>().parentKiller == null)
                hitSuccessfully = true;
            else if (!target.GetComponent<Enemy>().parentKiller.Equals(this))
                hitSuccessfully = true;
            if (target.GetComponent<Enemy>().Damage(damage, this)) // deals damage & checks for combo
            {
                if (combo)
                {
                    parentTower.GetComponent<Turret>().lastShot = -999;
                    combo = false;
                }
            }
        }
        else // explosion damage
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero, 0, Main.enemyLayerMask_);
            foreach (RaycastHit2D rcH2d in hit)
            {
                GameObject obj = rcH2d.collider.gameObject;
                if (obj.GetComponent<Enemy>().parentKiller == null)
                    hitSuccessfully = true;
                else if (!obj.GetComponent<Enemy>().parentKiller.Equals(this))
                    hitSuccessfully = true;
                if (obj.GetComponent<Enemy>().Damage(damage, this)) // deals damage & checks for combo
                {
                    if (combo)
                    {
                        parentTower.GetComponent<Turret>().lastShot = -999;
                        combo = false;
                    }
                }
            }
        }

        
        if (hitSuccessfully)
        {
            homingSpeed = 0;
            if (deathFX != null) // spawns FX
            {
                int spawnCount = deathFX.Length;
                if (randomFX)
                    spawnCount = 1;
                for (int i = 0; i < spawnCount; i++)
                {
                    int objIndex = i;
                    if (randomFX)
                        objIndex = Random.Range(0, deathFX.Length);
                    GameObject fx = Instantiate(deathFX[objIndex], transform.position + Vector3.back, Quaternion.identity);
                    if (explosionRadius > 0)
                    {
                        fx.transform.localScale = explosionRadius * 2 * Vector2.one;
                    }
                }
            }
        }
        else
        {
            pierce++;
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
