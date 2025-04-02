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
    public Tower.Type type;
    public Stats stats;
    public bool randomFX = true;
    public GameObject[] FX;
    public GameObject[] despawnFX;
    public float angle;
    private Rigidbody2D rb;
    private float spawnTime;
    private readonly static float EXPLOSION_RADIUS_CONSTANT = 1f;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        spawnTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.time > spawnTime + stats.GetStat("lifetime"))
        {
            OnDespawn();
            return;
        }
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
        stats.ModifyStat("pierce", -1);
        if (stats.GetStat("pierce") <= -1)
            Destroy(gameObject);
        if (stats.GetStat("pierce") < -1) // Prevents piercing through too many enemies if a projectile hits multiple in one frame
            return;

        Enemy e = target.GetComponent<Enemy>();

        float stun = stats.GetStat("stun");
        if (stun > 0)
        {
            e.Stun(stun);
        }

        if (stats.GetStat("explosion_radius") <= 0) // contact damage
        {
            if (e.Damage(Mathf.RoundToInt(stats.GetStat("damage")), this)) // deals damage, then, if killed...
            {
                OnKill();
            }
        }
        else // explosion damage
        {
            Explode();
        }


        stats.SetStat("homing", 0);
        if (FX != null) // spawns FX
        {
            int FXCount = FX.Length;
            if (randomFX)
                FXCount = 1;
            for (int i = 0; i < FXCount; i++)
            {
                int objIndex = i;
                if (randomFX)
                    objIndex = UnityEngine.Random.Range(0, FX.Length);
                Vector3 spawnPos = Vector3.Lerp(transform.position, target.transform.position, 0.5f);
                GameObject fx = Instantiate(FX[objIndex], spawnPos + Vector3.back, Quaternion.identity);
                if (stats.GetStat("explosion_radius") > 0)
                {
                    fx.transform.localScale = stats.GetStat("explosion_radius") * EXPLOSION_RADIUS_CONSTANT * 2 * Vector2.one;
                }
            }
        }
    }

    public virtual void OnKill()
    {
        
    }

    public virtual void OnDespawn()
    {
        for (int i = 0; i < despawnFX.Length; i++)
        {
            Instantiate(despawnFX[i], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public void Explode()
    {
        float radius = stats.GetStat("explosion_radius") * EXPLOSION_RADIUS_CONSTANT;
        if (radius > 0)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, Main.enemyLayerMask_);
            float segments = 36;
            Vector2 pos = transform.position + Vector3.right * radius;
            for (int i=1; i<segments+1; i++)
            {
                Vector2 newPos = transform.position + (Vector3)AngleHelper.DegreesToVector(i * 360 / segments) * radius;
                Debug.DrawLine(pos, newPos, Color.red, 1);
                pos = newPos;
            }
            foreach (RaycastHit2D rayC in hit)
            {
                GameObject obj = rayC.collider.gameObject;
                if (obj.GetComponent<Enemy>().Damage(Mathf.RoundToInt(stats.GetStat("damage")), this)) // deals damage, then, if killed...
                {
                    OnKill();
                }
            }
        }
    }
}
