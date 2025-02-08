using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Projectile : MonoBehaviour
{
    public GameObject parentTower;
    [NonSerialized]
    public Stats stats;
    public bool combo = false;
    public bool curse = false;
    public bool randomFX = true;
    public GameObject[] deathFX;
    public GameObject[] despawnFX;
    public float angle;
    private Rigidbody2D rb;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        stats.AddStat("damage", 0);
        stats.AddStat("speed", 1);
        stats.AddStat("lifetime", 1);
        stats.AddStat("pierce", 0);
        stats.AddStat("explosion_radius", 0);
        stats.AddStat("homing", 0);
    }

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Despawn(stats.GetStat("lifetime")));
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stats.GetStat("homing") > 0)
        {
            GameObject target = parentTower.GetComponent<Turret>().targetEnemy;
            if (target != null)
            {
                float angleToTarget = AngleHelper.VectorToDegrees(target.transform.position - transform.position);
                angle = Mathf.LerpAngle(angle, angleToTarget, Time.deltaTime * stats.GetStat("homing"));
            }
        }

        rb.velocity = 5 * stats.GetStat("speed") * AngleHelper.DegreesToVector(angle);

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
        stats.AddToStat("pierce", -1);
        if (stats.GetStat("pierce") <= -1)
            Destroy(gameObject);

        if (stats.GetStat("explosion_radius") == 0) // contact damage
        {
            if (target.GetComponent<Enemy>().Damage(Mathf.RoundToInt(stats.GetStat("damage")), this)) // deals damage & checks for combo
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
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, stats.GetStat("explosion_radius"), Vector2.zero, 0, Main.enemyLayerMask_);
            foreach (RaycastHit2D rcH2d in hit)
            {
                GameObject obj = rcH2d.collider.gameObject;
                if (obj.GetComponent<Enemy>().Damage(Mathf.RoundToInt(stats.GetStat("damage")), this)) // deals damage & checks for combo
                {
                    if (combo)
                    {
                        parentTower.GetComponent<Turret>().lastShot = -999;
                        combo = false;
                    }
                }
            }
        }


        stats.SetStat("homing", 0);
        if (deathFX != null) // spawns FX
        {
            int spawnCount = deathFX.Length;
            if (randomFX)
                spawnCount = 1;
            for (int i = 0; i < spawnCount; i++)
            {
                int objIndex = i;
                if (randomFX)
                    objIndex = UnityEngine.Random.Range(0, deathFX.Length);
                GameObject fx = Instantiate(deathFX[objIndex], transform.position + Vector3.back, Quaternion.identity);
                if (stats.GetStat("explosion_radius") > 0)
                {
                    fx.transform.localScale = stats.GetStat("explosion_radius") * 2 * Vector2.one;
                }
            }
        }
    }

    private IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < despawnFX.Length; i++)
        {
            Instantiate(despawnFX[i], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
