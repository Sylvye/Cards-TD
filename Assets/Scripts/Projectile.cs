using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.U2D.IK;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public GameObject parentTower;
    [NonSerialized]
    public Tower.Type type;
    [SerializedDictionary("Name", "Stat")]
    public Stats stats;
    public bool randomFX = true;
    public GameObject[] FX;
    public GameObject[] despawnFX;
    public float angle;
    private Rigidbody2D rb;
    private float spawnTime;
    private readonly static float AOE_RADIUS_CONSTANT = 1f;

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
        if (stats.GetStat("pierce") < -1) // Prevents piercing through >1 enemy if a projectile hits multiple in one frame
            return;

        Enemy e = target.GetComponent<Enemy>();
        float leftover = 0;

        if (stats.GetStat("explosion_radius") <= 0) // use contact damage if tower has no aoe
        {
            leftover = e.Damage(Mathf.RoundToInt(stats.GetStat("damage")), this);
        }
        else // use explosion damage
        {
            Explode();
        }

        if (!e.IsAlive())
        {
            OnKill(transform.position, leftover, e);
        }
        else
        {
            float stun = stats.GetStat("stun");
            if (stun > 0)
            {
                e.Stun(stun);
            }

            if (stats.GetStat("stun") > 0)
            {
                Stun(stats.GetStat("stun"), stats.GetStat("stun") / 2); // TEMP - work on this formula
            }
        }

        stats.SetStat("homing", 0); // once a projectile hits its target, it stops homing
        
        if (FX != null)
        {
            SpawnHitFX(true);
        }
    }

    public void SpawnHitFX(bool scaleWithExplosionRadius)
    {
        GameObject fx = null;
        int FXCount = FX.Length;
        if (randomFX)
            FXCount = 1;
        for (int i = 0; i < FXCount; i++)
        {
            int objIndex = i;
            if (randomFX)
                objIndex = UnityEngine.Random.Range(0, FX.Length);
            fx = Instantiate(FX[objIndex], transform.position + Vector3.back, Quaternion.identity);
            if (scaleWithExplosionRadius && stats.GetStat("explosion_radius") > 0) // scales fx to match aoe size, if applicable
            {
                fx.transform.localScale = stats.GetStat("explosion_radius") * AOE_RADIUS_CONSTANT * 2 * Vector2.one;
            }
        }
    }

    public virtual void OnKill(Vector2 pos, float leftover, Enemy e)
    {
        if (stats.GetStat("raze") > 0)
        {
            AOE(pos, stats.GetStat("raze"), stats.GetStat("damage") * 2, false);
        }

        if (stats.GetStat("cleave") > 0)
        {
            Chain(stats.GetStat("cleave"), leftover, e, true);
        }
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
        AOE(transform.position, stats.GetStat("explosion_radius"), stats.GetStat("damage"), false);
    }

    public void AOE(Vector2 pos, float r, float dmg, bool useOnKill)
    {
        float radius = r * AOE_RADIUS_CONSTANT;
        if (radius > 0)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(pos, radius, Vector2.zero, 0, Main.main.enemyLayerMask);
            //float segments = 36;
            //Vector2 pos = transform.position + Vector3.right * radius;
            //for (int i=1; i<segments+1; i++) // DEBUG ONLY
            //{
            //    Vector2 newPos = transform.position + (Vector3)AngleHelper.DegreesToVector(i * 360 / segments) * radius;
            //    Debug.DrawLine(pos, newPos, Color.red, 1);
            //    pos = newPos;
            //}
            foreach (RaycastHit2D rayC in hit)
            {
                Enemy e = rayC.collider.gameObject.GetComponent<Enemy>();
                float leftover = e.Damage(Mathf.RoundToInt(dmg), this);
                if (!e.IsAlive() && useOnKill) // deals damage, then, if killed...
                {
                    OnKill(e.transform.position, leftover, e);
                }
            }
        }
    }

    public void Chain(float r, float dmg, Enemy ignore, bool useOnKill)
    {
        float radius = r * AOE_RADIUS_CONSTANT;
        if (radius > 0)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, Main.main.enemyLayerMask);
            float segments = 36;
            Vector2 pos = transform.position + Vector3.right * radius;
            for (int i = 1; i < segments + 1; i++) // DEBUG ONLY
            {
                Vector2 newPos = transform.position + (Vector3)AngleHelper.DegreesToVector(i * 360 / segments) * radius;
                Debug.DrawLine(pos, newPos, Color.blue, 1);
                pos = newPos;
            }
            GameObject closest = null;
            float closestDist = float.MaxValue;
            foreach (RaycastHit2D rayC in hit)
            {
                GameObject obj = rayC.collider.gameObject;
                if (obj != ignore.gameObject && Vector2.Distance(obj.transform.position, transform.position) < closestDist)
                {
                    closest = obj;
                }
            }

            if (closest != null)
            {
                Enemy e = closest.GetComponent<Enemy>();
                float leftover = e.Damage(Mathf.RoundToInt(dmg), this);
                SpawnHitFX(false);
                // instantiate chain effect here
                float distMod = closestDist / r / 255;
                Debug.DrawLine(ignore.transform.position, e.transform.position, new Color(distMod, 255 - distMod, 0), 0.5f);
                if (!e.IsAlive() && useOnKill) // deals damage, then, if killed...
                {
                    OnKill(e.transform.position, leftover, e);
                }
            }
        }
    }

    public void Stun(float radius, float time)
    {
        float r = radius * AOE_RADIUS_CONSTANT;
        if (r > 0)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, r, Vector2.zero, 0, Main.main.enemyLayerMask);
            //float segments = 36;
            //Vector2 pos = transform.position + Vector3.right * radius;
            //for (int i = 1; i < segments + 1; i++) // DEBUG ONLY
            //{
            //    Vector2 newPos = transform.position + (Vector3)AngleHelper.DegreesToVector(i * 360 / segments) * radius;
            //    Debug.DrawLine(pos, newPos, Color.yellow, 1);
            //    pos = newPos;
            //}
            foreach (RaycastHit2D rayC in hit)
            {
                GameObject obj = rayC.collider.gameObject;
                obj.GetComponent<Enemy>().Stun(time);
            }
        }
    }
}
