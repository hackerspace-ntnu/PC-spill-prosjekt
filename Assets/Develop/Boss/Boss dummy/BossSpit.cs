﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    public GameObject spitPref;
    public Transform spawnPos;

    public float smallSpitSpeed = 1000.0f;
    public float bigSpitSpeed = 1500.0f;

    public float angleVariationDeg = 90;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(EasySpit());
        }
    }

    IEnumerator EasySpit()
    {
        /* This is a timed event where the boss first shoots a large spit, then three smaller spits in slightly different angles. 
         * 
         * 
         * 
        */

        // This should be player later
        Vector2 target = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized * 20;

        // Calculations for new target to hit, via angle variation and random angle difference
        float targetAngle = Mathf.Atan(target.y / target.x);
        float angleVariationRad = Mathf.Deg2Rad * angleVariationDeg;
        float randomAngle = Random.Range(-angleVariationRad, angleVariationRad);
        Vector2 newTarget = new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20, Mathf.Sin(targetAngle + randomAngle) * 20);

        SpawnSpitBullet(
            2.0f, 
            bigSpitSpeed,
            // This should be the player
            target);
        yield return new WaitForSeconds(0.5f);

        SpawnSpitBullet(
            1.0f,
            smallSpitSpeed,
            // This should be player + angle
            newTarget);
        yield return new WaitForSeconds(0.15f);

        randomAngle = Random.Range(-angleVariationRad, angleVariationRad);
        newTarget = new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20, Mathf.Sin(targetAngle + randomAngle) * 20);

        SpawnSpitBullet(
            1.0f, 
            smallSpitSpeed,
            // This should be player + angle
            new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20, Mathf.Sin(targetAngle + randomAngle) * 20));
        yield return new WaitForSeconds(0.15f);

        randomAngle = Random.Range(-angleVariationRad, angleVariationRad);
        newTarget = new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20, Mathf.Sin(targetAngle + randomAngle) * 20);

        SpawnSpitBullet(
            1.0f, 
            smallSpitSpeed,
            // This should be player + angle
            new Vector2(Mathf.Cos(targetAngle + randomAngle) * 20, Mathf.Sin(targetAngle + randomAngle) * 20));
        yield return null;
    }

    void SpawnSpitBullet(float scale, float speed, Vector2 target)
    {
        GameObject spit = Instantiate(spitPref, new Vector2(spawnPos.position.x, spawnPos.position.y), Quaternion.identity);
        SpitController spitController = spit.GetComponent<SpitController>();
        spitController.scale = scale;
        spitController.spitSpeed = speed;
        spitController.target = target;
    }
}
