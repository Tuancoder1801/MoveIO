using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : Character
{ 
    public string enemyName;
    public float attackCooldown;

    public NameTagEnemy nameTagEnemy;

    private float idleStartTime;
    private NavMeshAgent agent;

    private static List<string> namePool = new List<string> {
        "Goblin", "Orc", "Troll", "Vampire", "Zombie", "Skeleton", "Demon", "Wraith", "Banshee", "Minotaur"
    };

    public override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Start();

        enemyName = GetRandomName();

        characterRenderer.material = characterMaterial[UnityEngine.Random.Range(0, characterMaterial.Count)];

        WeaponId randomWeapon = weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Length)].id;
        EquipWeapon(randomWeapon);

        HatId randomHat = hatPrefabs[UnityEngine.Random.Range(0, hatPrefabs.Length)].hatId;
        EquipHat(randomHat);

        PantId randomPant = pantPrefabs[UnityEngine.Random.Range(0, pantPrefabs.Length)].pantId;
        EquipPant(randomPant);

        /*ShieldId randomShield = shieldPrefabs[UnityEngine.Random.Range(0, shieldPrefabs.Length)].shieldId;
        EquipShield(randomShield);*/
    }

    private string GetRandomName()
    {
        int randomIndex = UnityEngine.Random.Range(0, namePool.Count);
        string randomName = namePool[randomIndex];
        //namePool.RemoveAt(randomIndex);
        return randomName;
    }


    public override void Update()
    {
        base.Update();
        DetectCharacter();
    }

    #region Idle

    public override void BeginIdle()
    {
        idleStartTime = Time.time;
        base.BeginIdle();
    }

    public override void UpdateIdle()
    {
        if (state == BehaviourState.Idle)
        {
            if (target != null)
            {
                ChangeState(BehaviourState.Attack);
                return;
            }
            else if (Time.time - idleStartTime >= 3f && target == null)
            {
                agent.isStopped = false;
                Vector3 destination = RandomDestinationMove();
                if (destination != Vector3.zero)
                {
                    agent.SetDestination(destination);
                    ChangeState(BehaviourState.Run);
                }
                return;
            }
        }
    }

    #endregion

    #region Run

    public override void UpdateRun()
    {
        if (state == BehaviourState.Run)
        {
            if (target != null)
            {
                agent.isStopped = true;
                ChangeState(BehaviourState.Attack);
                return;
            }

            if(isDead)
            {
                agent.isStopped = true;
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                ChangeState(BehaviourState.Idle);
                return;
            }
        }
    }

    /* private Vector3 RandomDestinationMove()
     {
         float minDistance = 5f;
         float maxDistance = 15f;

         Vector3 destination = Vector3.zero;
         while (destination == Vector3.zero)
         {
             float distance = UnityEngine.Random.Range(minDistance, maxDistance);
             Vector2 randomDir = UnityEngine.Random.insideUnitCircle * distance;
             Vector3 destinationMove = new Vector3(transform.position.x + randomDir.x, transform.position.y, transform.position.z + randomDir.y);

             NavMeshHit hit;
             if (NavMesh.SamplePosition(destinationMove, out hit, maxDistance, NavMesh.AllAreas))
             {
                 destination = hit.position;
                 return destination;
             }
         }

         return Vector3.zero;
     }*/

    private Vector3 RandomDestinationMove()
    {
        float minDistance = 5f;
        float maxDistance = 15f;
        float safeDistanceFromWall = 5f;  // Khoảng cách an toàn từ tường
        Vector3 destination = Vector3.zero;

        while (destination == Vector3.zero)
        {
            float distance = UnityEngine.Random.Range(minDistance, maxDistance);
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle * distance;
            Vector3 destinationMove = new Vector3(transform.position.x + randomDir.x, transform.position.y, transform.position.z + randomDir.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(destinationMove, out hit, maxDistance, NavMesh.AllAreas))
            {
                Vector3 potentialDestination = hit.position;

                // Sử dụng Raycast để kiểm tra nếu có tường ở gần điểm này
                if (!IsNearWall(potentialDestination, safeDistanceFromWall))
                {
                    destination = potentialDestination;
                    return destination;
                }
            }
        }

        return Vector3.zero;
    }

    // Hàm kiểm tra xem điểm có gần tường hay không
    private bool IsNearWall(Vector3 point, float safeDistance)
    {
        // Tạo raycast từ điểm kiểm tra theo mọi hướng xung quanh
        RaycastHit hitInfo;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(point, dir, out hitInfo, safeDistance))
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Wall")) // Kiểm tra layer
                {
                    return true;
                }
            }
        }

        return false; // Không có tường gần
    }
    #endregion

    #region attack

    public override void UpdateAttack()
    {
        if (state == BehaviourState.Attack)
        {
            if (target != null)
            {
                RotationCharacter();
            }
            else
            {
                ChangeState(BehaviourState.Idle);
                return;
            }
        }
    }

    #endregion

    #region Dead

    /*public override void TakeAttack(float damage)
    {
        base.TakeAttack(damage);

        if (hp <= 0)
        {
            
        }
    }*/
    #endregion


}
