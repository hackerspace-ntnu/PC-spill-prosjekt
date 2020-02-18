using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    public GameObject spitPref; 
    public Transform spawnPos;
    public Transform player;
    [Space(10)]
    public float chargeUp = 1f;
    public float bigSpitAmmount = 3f;
    public float bigSpitSize = 2f;
    public float bigSpitSpeed = 1500.0f;
    public float waitAfterBig = 0.5f;
    [Space(10)]
    public float smallSpitAmmount = 3f;
    public float smallSpitSize = 1f;
    public float smallSpitSpeed = 1000.0f;
    public float angleVariationDeg = 20f;
    public float waitAfterSmall = 0.15f;

    private BossController bc;

    private void OnEnable()
    {
        StartCoroutine(EasySpit());
    }

    /// <summary>
    /// Must be separate from OnDisable, as this script might be disabled from override state.
    /// </summary>
    public void Disable()
    {
        bc.updateState(this);
        enabled = false;
    }

    /// <summary>
    /// Must stop all Coroutines
    /// </summary>
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void Start()
    {
        bc = GetComponent<BossController>();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(EasySpit());
        }
    }
    */

    // This is an IEnumerator that spawns spits glitch acid with intervals
    IEnumerator EasySpit()
    {
        // Variables for local calculations
        float smallSpits = smallSpitAmmount;
        float bigSpits = bigSpitAmmount;
        float angleVariationRad = Mathf.Deg2Rad * angleVariationDeg;
        Vector2 target;
        float side;
        float targetAngle;
        float randomAngle;
        Vector2 newTarget;

        // Charging up
        yield return new WaitForSeconds(chargeUp);

        // Big spits are shot towards the player. Shoots a set ammount of projectiles towards player with a set size and speed, and on a set interval.
        while (bigSpits > 0)
        {
            // Calculates the target
            target = ((Vector2)player.position - (Vector2)transform.position).normalized * 20;

            // Spawns the projectile
            SpawnSpitBullet(
                bigSpitSize,
                bigSpitSpeed,
                target);
            // Wait
            yield return new WaitForSeconds(waitAfterBig);

            bigSpits -= 1;
        }

        // Small spits have a new target that is the player target with a random new angle variation, otherwise works the same as the big projetile, 
        // but with its own variables.
        while (smallSpits > 0)
        {
            // Calculates the new target
            target = ((Vector2)player.position - (Vector2)transform.position).normalized * 20;
            if (target.x <= 0) side = -1;
            else side = 1;
            targetAngle = Mathf.Atan(target.y / target.x);
            randomAngle = Random.Range(-angleVariationRad, angleVariationRad);
            newTarget = new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20 * side, Mathf.Sin(targetAngle + randomAngle) * 20 * side);

            // Spawns the projectile
            SpawnSpitBullet(
                smallSpitSize,
                smallSpitSpeed,
                newTarget);
            // Wait
            yield return new WaitForSeconds(waitAfterSmall);

            smallSpits -= 1;
        }

        // If we end up here we find a new state for the bossController
        Disable();
    }

    void SpawnSpitBullet(float scale, float speed, Vector2 target)
    {

        float angle = Mathf.Atan2(target.y - spawnPos.position.y, target.x - spawnPos.position.x) * Mathf.Rad2Deg;

        GameObject spit = Instantiate(spitPref, new Vector2(spawnPos.position.x, spawnPos.position.y), Quaternion.AngleAxis(angle, Vector3.forward));
        SpitController spitController = spit.GetComponent<SpitController>();
        spitController.scale = scale;
        spitController.spitSpeed = speed;
        spitController.target = target;
    }
}
