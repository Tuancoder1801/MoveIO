using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;
using Debug = UnityEngine.Debug;

public enum BehaviourState
{
    Idle,
    Run,
    Attack,
    Dead,
    Dance
}

public class Character : MonoBehaviour
{
    public int score = 0;

    public float moveSpeed;
    public float rotationSpeed;
    public float attackRange = 5f;
    public BehaviourState state = BehaviourState.Idle;

    public Character target;

    public Transform weaponPoint;
    public Transform hatPoint;
    public Transform pantPoint;
    public Transform shieldPoint;
    public Transform clothesPoint;
    public Transform firePoint;

    public Material defaultPant;

    public BaseWeapon[] weaponPrefabs;
    public BaseHat[] hatPrefabs;
    public BasePant[] pantPrefabs;
    public BaseShield[] shieldPrefabs;

    public BaseClothes clothesPrefabs;

    public List<Material> characterMaterial;
    public Renderer characterRenderer;

    public string killerName;

    public bool isDead => hp <= 0f;

    public float hp;
    protected BaseWeapon weapon;
    protected BaseHat hat;
    protected BasePant pant;
    protected BaseShield shield;

    protected Rigidbody playerRigidbody;
    protected Animator animator;

    protected const string ANIM_TRIGGER_IDLE = "isIdle";
    protected const string ANIM_TRIGGER_RUN = "isRun";
    protected const string ANIM_TRIGGER_ATTACK = "isAttack";
    protected const string ANIM_TRIGGER_DEAD = "isDead";
    protected const string ANIM_TRIGGER_DANCE = "isDance";

    public virtual void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hp = 1f;
        
    }

    public virtual void Update()
    {
        if (GameController.Instance.isCountDownActive) return;
        if (isDead) return;

        UpdateIdle();
        UpdateRun();
        UpdateAttack();
    }

    public virtual void ChangeState(BehaviourState newState)
    {
        if (state != newState)
        {
            //Debug.LogFormat("{0} to {1}", state, newState);
            //state = newState;

            switch (newState)
            {
                case BehaviourState.Idle:
                    BeginIdle();
                    break;
                case BehaviourState.Run:
                    BeginRun();
                    break;
                case BehaviourState.Attack:
                    BeginAttack();
                    break;
                case BehaviourState.Dead:
                    BeginDead();
                    break;
                case BehaviourState.Dance:
                    BeginDance();
                    break;
            }
        }
    }

    public void ResetAllTriggers()
    {
        animator.ResetTrigger(ANIM_TRIGGER_IDLE);
        animator.ResetTrigger(ANIM_TRIGGER_RUN);
        animator.ResetTrigger(ANIM_TRIGGER_ATTACK);
        animator.ResetTrigger(ANIM_TRIGGER_DEAD);
        animator.ResetTrigger(ANIM_TRIGGER_DANCE);
    }

    #region Idle
    public virtual void BeginIdle()
    {
        ResetAllTriggers();
        state = BehaviourState.Idle;
        animator.SetTrigger(ANIM_TRIGGER_IDLE);
    }

    public virtual void UpdateIdle()
    {
    }
    #endregion

    #region Run
    public virtual void BeginRun()
    {
        ResetAllTriggers();
        state = BehaviourState.Run;
        animator.SetTrigger(ANIM_TRIGGER_RUN);
    }

    public virtual void UpdateRun()
    {
    }
    #endregion

    #region Attack

    public virtual void BeginAttack()
    {
        ResetAllTriggers();
        state = BehaviourState.Attack;
        animator.SetTrigger(ANIM_TRIGGER_ATTACK);
    }

    public virtual void UpdateAttack()
    {
    }

    public void Shoot()
    {
        if (weapon != null)
        {
            weapon.CreateBullet(this);
        }
    }

    public virtual void EndAttack()
    {
    }
    #endregion

    #region Dead

    public virtual void BeginDead()
    {
        ResetAllTriggers();
        hp = 0f;
        state = BehaviourState.Dead;
        animator.SetTrigger(ANIM_TRIGGER_DEAD);

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
    #endregion

    #region Dance

    public virtual void BeginDance()
    {
        ResetAllTriggers();
        state = BehaviourState.Dance;
        animator.SetTrigger(ANIM_TRIGGER_DANCE);
    }

    public virtual void UpdateDance()
    {
    }

    #endregion

    #region Equip
    public virtual void EquipWeapon(WeaponId weaponId)
    {
        if (weaponPrefabs.Length == 0)
        {
            //Debug.LogWarning("Weapon Prefabs array is empty!");
            return;
        }

        if (weaponId != WeaponId.NONE)
        {
            for (int i = 0; i < weaponPrefabs.Length; i++)
            {
                BaseWeapon prefab = weaponPrefabs[i];
                if (prefab != null && prefab.id == weaponId)
                {
                    if (weapon != null)
                    {
                        Destroy(weapon.gameObject);
                    }

                    weapon = Instantiate(prefab, weaponPoint.position, weaponPoint.rotation);
                    weapon.transform.SetParent(weaponPoint);
                    return;
                }
            }
        }

    }

    public virtual void EquipHat(HatId hatId)
    {
        if (clothesPrefabs != null && clothesPrefabs.clothesId != ClothesId.NONE)
        {

            return;
        }

        if (hatPrefabs.Length == 0)
        {
            return;
        }

        if (hatId != HatId.NONE)
        {
            for (int i = 0; i < hatPrefabs.Length; i++)
            {
                BaseHat prefab = hatPrefabs[i];
                if (prefab != null && prefab.hatId == hatId)
                {
                    if (hat != null)
                    {
                        Destroy(hat.gameObject);
                    }

                    hat = Instantiate(prefab, hatPoint.position, hatPoint.rotation);
                    hat.transform.SetParent(hatPoint);
                    return;
                }
            }
        }
        else
        {
            if (hat != null)
            {
                Destroy(hat.gameObject);
            }
        }
    }

    public virtual void EquipPant(PantId pantId)
    {
        if (clothesPrefabs != null && clothesPrefabs.clothesId != ClothesId.NONE)
        {

            return;
        }

        if (hatPrefabs.Length == 0)
            return;

        if (pantId != PantId.NONE)
        {
            for (int i = 0; i < pantPrefabs.Length; i++)
            {
                BasePant prefab = pantPrefabs[i];
                if (prefab != null && prefab.pantId == pantId)
                {
                    Renderer pantPointRenderer = pantPoint.GetComponent<Renderer>();
                    if (pantPointRenderer != null)
                    {
                        pantPointRenderer.material = prefab.pantMaterial;
                    }
                    return;
                }
            }
        }
        else
        {
            if (pantPoint != null)
            {
                Renderer pantRenderer = pantPoint.GetComponent<Renderer>();
                if (pantRenderer != null)
                {
                    pantRenderer.material = defaultPant;
                }
            }
        }
    }

    public virtual void EquipShield(ShieldId shieldId)
    {
        if (clothesPrefabs != null && clothesPrefabs.clothesId != ClothesId.NONE)
        {
            return;
        }

        if (shieldPrefabs.Length == 0)
            return;

        if (shieldId != ShieldId.NONE)
        {
            for (int i = 0; i < shieldPrefabs.Length; i++)
            {
                BaseShield prefab = shieldPrefabs[i];
                if (prefab != null && prefab.shieldId == shieldId)
                {
                    if (shield != null)
                    {
                        Destroy(shield.gameObject);
                    }

                    shield = Instantiate(prefab, shieldPoint.position, shieldPoint.rotation);
                    shield.transform.SetParent(shieldPoint);
                    return;
                }
            }
        }
        else
        {
            if (shield != null)
            {
                Destroy(shield.gameObject);
            }
        }
    }

    public virtual void EquipClothes(ClothesId clothesId)
    {
        if (clothesPrefabs == null)
        {
            return;
        }

        BaseClothes prefab = clothesPrefabs;

        if (prefab != null)
        {
            if (prefab.clothesId == clothesId)
            {
                prefab.gameObject.SetActive(true);
            }
        }
    }

    #endregion

    public virtual void TakeAttack(float damage, Enemy killer)
    {
        hp -= damage;

        if (hp <= 0)
        {
            ChangeState(BehaviourState.Dead);

            if (killer != null)
            {
                killerName = killer.enemyName;
                //characterRenderer.material = killer.characterRenderer.material;
                GameController.Instance.killerName = killerName;
                GameController.Instance.killerColor = killer.characterRenderer.material.color;
            }
        }
    }

    public virtual void DetectCharacter()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, GameController.Instance.layerMask);

        if (hitColliders.Length > 0)
        {
            List<Character> nearByEnemies = new List<Character>();

            for (int i = 0; i < hitColliders.Length; i++)
            {
                Character enemy = hitColliders[i].transform.root.GetComponent<Character>();

                if (enemy != null && enemy.CompareTag(tag) == false && enemy.isDead == false)
                {
                    nearByEnemies.Add(enemy);

                    Debug.DrawLine(transform.position, enemy.transform.position, Color.green);
                }
            }

            if (nearByEnemies.Count > 0)
            {
                target = nearByEnemies[0];
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
    }

    public virtual void RotationCharacter()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    
    public virtual void AddScore()
    {
        score += 1;

        if(score % 2 ==0)
        {
            transform.localScale *= 1.1f;
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw the OverlapSphere radius as a wireframe sphere
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}