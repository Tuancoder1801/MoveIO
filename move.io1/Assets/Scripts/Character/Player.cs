using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public bool isLobby;
    public GameObject nameTag;

    public Joystick joystick;
    public GameObject circle;

    public int kill;

    private bool isMoving;
    private Vector3 moveVector;
    private float timerIdle;
    private float danceTime = 5f;

    private float valueVertical;
    private float valueHorizontal;

    public override void Start()
    {
        base.Start();
        joystick = FindObjectOfType<Joystick>();

        EquipWeapon((WeaponId)UserData.weapon.equippedId);


        EquipHat((HatId)UserData.outfit.GetEquippedSkin(SkinTabType.Hat));

        EquipPant((PantId)UserData.outfit.GetEquippedSkin(SkinTabType.Pant));

        EquipShield((ShieldId)UserData.outfit.GetEquippedSkin(SkinTabType.Shield));

        EquipClothes((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
    }

    public override void Update()
    {
        if(isLobby)
        {   
            circle.SetActive(false);
            nameTag.SetActive(false);
            return;
        }

        DetectCharacter();
        CheckInput();
        base.Update();
        UpdateDance();

    }

    private void CheckInput()
    {
        valueVertical = joystick.Vertical;
        valueHorizontal = joystick.Horizontal;
        isMoving = valueVertical != 0 || valueHorizontal != 0;

        /*if (Input.GetKeyUp(KeyCode.X))
        {
            ChangeState(BehaviourState.Dead);
        }*/
    }

    public override void DetectCharacter()
    {
        Character neareastEnemy = null;
        float nearestDistance = 10000f;

        for (int i = 0; i < GameController.Instance.characters.Count; i++)
        {
            var enemy = GameController.Instance.characters[i];
            if (enemy != null && enemy.CompareTag(tag) == false && enemy.isDead == false)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= attackRange && distance < nearestDistance)
                {
                    neareastEnemy = enemy;
                    nearestDistance = distance;
                    Debug.DrawLine(transform.position, enemy.transform.position, Color.green);
                }
            }
        }

        if (neareastEnemy != null)
        {
            target = neareastEnemy;
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }
        else
        {
            target = null;
        }
    }

    #region Idle
    public override void BeginIdle()
    {
        //Debug.LogFormat("BeginIdle | valueVertical={0}, valueHorizontal={1}", valueVertical, valueHorizontal);
        timerIdle = 0f;
        base.BeginIdle();
    }

    public override void UpdateIdle()
    {
        if (state == BehaviourState.Idle)
        {
            timerIdle += Time.deltaTime;

            if (isMoving)
            {
                ChangeState(BehaviourState.Run);
                return;
            }

            if (target != null)
            {
                ChangeState(BehaviourState.Attack);
                return;
            }

            if (timerIdle >= danceTime)
            {
                ChangeState(BehaviourState.Dance);
            }
        }
    }
    #endregion

    #region Run
    public override void BeginRun()
    {
        //Debug.LogFormat("BeginRun | valueVertical={0}, valueHorizontal={1}", valueVertical, valueHorizontal);
        base.BeginRun();
    }

    public override void UpdateRun()
    {
        if (state == BehaviourState.Run)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ChangeState(BehaviourState.Attack);
                return;
            }

            if (!isMoving)
            {
                ChangeState(BehaviourState.Idle);
                return;
            }

            moveVector = Vector3.zero;
            moveVector.x = valueHorizontal * moveSpeed * Time.deltaTime;
            moveVector.z = valueVertical * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveVector);

            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    #endregion

    #region Attack
    public override void BeginAttack()
    {
        //DetectCharacter();
        base.BeginAttack();
    }

    public override void UpdateAttack()
    {
        if (state == BehaviourState.Attack)
        {
            if (target != null)
            {
                RotationCharacter();
            }
        }
    }


    public override void EndAttack()
    {
        if (isMoving)
        {
            ChangeState(BehaviourState.Run);
        }
        else
        {
            ChangeState(BehaviourState.Idle);
        }
    }
    #endregion

    #region Dance
    public override void UpdateDance()
    {
        if (state == BehaviourState.Dance)
        {
            if (isMoving)
            {
                ChangeState(BehaviourState.Run);
                return;
            }
        }
    }

    #endregion

    public override void AddScore()
    {
        base.AddScore();

        if (score % 2 == 0)
        {
            circle.transform.localScale /= 1.1f;
        }
    }

    public void AddKill()
    {
        kill++;
    }

}

