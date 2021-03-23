using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private Vector3 destPos = Vector3.zero;
    private float lastAttackTime;
    private bool stopAttack;
    private bool keepAttack;

    private Transform myTransform;
    private Enemy targetEntity;
    private Transform targetTransform;
    private PlayerInput playerInput;
    private PlayerStat playerStat;
    private PlayerSkill playerSkill;
    private Animator playerAnimator;

    [SerializeField] private GameObject destPosImage;
    [SerializeField] private LayerMask checkLayer;

    private Define.PlayerState _playerState;
    public Define.PlayerState playerState
    {
        get { return _playerState; }
        set
        {
            _playerState = value;
            switch (_playerState)
            {
                case Define.PlayerState.Die:
                    playerAnimator.SetBool("Die", true);
                    break;
                case Define.PlayerState.Idle:
                    playerAnimator.SetInteger("Move", 0);
                    playerAnimator.SetInteger("Attack", 0);
                    break;
                case Define.PlayerState.Moving:
                    playerAnimator.SetInteger("Move", 1);
                    playerAnimator.SetInteger("Attack", 0);
                    break;
                case Define.PlayerState.Attack:
                    playerAnimator.SetInteger("Move", 0);
                    playerAnimator.SetInteger("Attack", 1);
                    break;
                case Define.PlayerState.AttackIdle:
                    playerAnimator.SetInteger("Move", 0);
                    playerAnimator.SetInteger("Attack", 2);
                    break;
                case Define.PlayerState.Skill1:
                    playerAnimator.SetInteger("Move", 0);
                    playerAnimator.SetInteger("Attack", 3);
                    break;
                case Define.PlayerState.Skill2:
                    playerAnimator.SetInteger("Move", 0);
                    playerAnimator.SetInteger("Attack", 4);
                    break;
            }
        }
    }

    private bool isAttackAble
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
                if (Vector3.Distance(myTransform.position, targetTransform.position) <= playerStat.finalAttackRange)
                    return true;

            return false;
        }
    }

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();
        playerStat = GetComponent<PlayerStat>();
        playerSkill = GetComponent<PlayerSkill>();
        playerAnimator = GetComponent<Animator>();
        playerState = Define.PlayerState.Idle; 
        lastAttackTime = 0;
        stopAttack = true;
        keepAttack = false;

        Managers.Input.keyAction -= OnSkillKeyPress;
        Managers.Input.keyAction += OnSkillKeyPress;
        Managers.Input.mouseAction -= OnMouse0Clicked;
        Managers.Input.mouseAction += OnMouse0Clicked;
    }

    private void OnDisable()
    {
        if (Managers.instance != null)
        {
            Managers.Input.keyAction -= OnSkillKeyPress;
            Managers.Input.mouseAction -= OnMouse0Clicked;
        }
    }

    private void Update()
    {
        if (!playerStat.dead)
        {
            switch (playerState)
            {
                case Define.PlayerState.Idle:
                    UpdateIdle();
                    break;
                case Define.PlayerState.Moving:
                    UpdateMoving();
                    break;
                case Define.PlayerState.AttackIdle:
                    UpdateAttack();
                    break;
                case Define.PlayerState.Skill1:
                case Define.PlayerState.Skill2:
                    UpdateSkill();
                    break;
            }
        }
    }

    private void UpdateIdle()
    {
        if (targetEntity != null)
        {
            Vector3 dir = targetTransform.position - myTransform.position;
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.LookRotation(dir), playerStat.rotateSpeed * Time.deltaTime);

            destPosImage.SetActive(true);
            destPosImage.transform.position = targetTransform.position + Vector3.up * 0.1f;
        }
        else
            destPosImage.SetActive(false);
    }

    private void UpdateMoving()
    {
        destPosImage.SetActive(true);

        if (isAttackAble)
        {
            destPos = transform.position;
            playerState = Define.PlayerState.AttackIdle;
            return;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Attack"))
            return;

        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.0001f)
            playerState = Define.PlayerState.Idle;
        else
        {
            if (Physics.Raycast(myTransform.position, dir, 1.5f, (int)Define.Layer.Terrain))
            {
                playerState = Define.PlayerState.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(playerStat.moveSpeed * Time.deltaTime, 0, dir.magnitude);
            myTransform.position += dir.normalized * moveDist;
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(dir), playerStat.rotateSpeed * Time.deltaTime);
            myTransform.LookAt(destPos);
        }
    }

    private void UpdateAttack()
    {
        if (targetEntity != null)
        {
            destPosImage.SetActive(true);
            destPosImage.transform.position = targetTransform.position + Vector3.up * 0.1f;
            myTransform.LookAt(targetTransform);
        }
        else
            keepAttack = false;


        if (Time.time >= lastAttackTime + playerStat.finalIntvlAttackTime)
        {
            stopAttack = false;
            playerState = Define.PlayerState.Attack;
            lastAttackTime = Time.time;
        }
    }

    private void UpdateSkill()
    {
        switch (playerState)
        {
            case Define.PlayerState.Skill1:
                {
                    if (targetEntity != null)
                    {
                        if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Skill_1))
                        {
                            playerStat.currentMP -= playerSkill.skillList[0].mpCost;
                            playerSkill.UseSkill1(playerStat.attackDamage, targetEntity, targetTransform);
                        }
                    }
                }
                break;
            case Define.PlayerState.Skill2:
                {
                    if (UIManager.instance.UseQuickSlot(Define.QuckSlot.Skill_2))
                    {
                        playerStat.currentMP -= playerSkill.skillList[0].mpCost;
                        playerSkill.UseSkill2(playerStat.attackDamage, myTransform);
                    }
                }
                break;
        }

        if (keepAttack)
            playerState = Define.PlayerState.AttackIdle;
        else
            playerState = Define.PlayerState.Idle;
    }

    public void OnAttackEvent()
    {
        if (targetEntity != null)
        {
            targetEntity.OnDamage(playerStat.finalAttackDamage);

            if (targetEntity.dead)
            {
                stopAttack = true;
                keepAttack = false;

                targetEntity = null;
            }
        }

        if (stopAttack)
        {
            stopAttack = false;
            playerState = Define.PlayerState.Idle;
        }
        else
        {
            if (isAttackAble)
                playerState = Define.PlayerState.AttackIdle;
            else
                playerState = Define.PlayerState.Moving;
        }
    }

    private void OnSkillKeyPress()
    {
        if (!playerStat.dead)
        {
            if (playerInput.skill1)
            {
                if (playerState == Define.PlayerState.AttackIdle)
                    keepAttack = true;

                playerState = Define.PlayerState.Skill1;
            }
            if (playerInput.skill2)
            {
                if (playerState == Define.PlayerState.AttackIdle)
                    keepAttack = true;

                playerState = Define.PlayerState.Skill2;
            }
        }
    }

    private void OnMouse0Clicked(Define.Mouse mouse, Define.MouseEvent evt)
    {
        if (mouse != Define.Mouse.Mouse_0)
            return;

        if (!Managers.Game.isGameOver)
        {
            if (UIManager.instance.isWindowOpen)
                return;

            Ray  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 100.0f, checkLayer);

            switch (evt)
            {
                case Define.MouseEvent.Down:
                    {
                        if (raycastHit)
                        {
                            if (hit.collider.gameObject.layer == (int)Define.Layer.Shop)
                            {
                                hit.collider.gameObject.GetComponent<Shop>().OpenShop();
                                playerState = Define.PlayerState.Idle;
                            }
                            else
                            {
                                destPos = hit.point;
                                stopAttack = false;
                                destPosImage.transform.position = destPos + Vector3.up * 0.1f;

                                playerState = Define.PlayerState.Moving;

                                if (hit.collider.gameObject.layer == (int)Define.Layer.Enemy)
                                {
                                    if (targetEntity != null)
                                        targetEntity.UnTargetting();

                                    targetEntity = hit.collider.gameObject.GetComponent<Enemy>();
                                    targetTransform = targetEntity.GetComponent<Transform>();
                                    targetEntity.Targetting();
                                }
                                else
                                {
                                    if (targetEntity != null)
                                        targetEntity.UnTargetting();

                                    targetEntity = null;
                                    targetTransform = null;
                                }
                            }
                        }
                    }
                    break;
                case Define.MouseEvent.Press:
                    {
                        if (targetEntity != null)
                        {
                            destPos = targetTransform.position;
                            destPosImage.transform.position = destPos + Vector3.up * 0.1f;
                        }
                        else if (raycastHit)
                        {
                            if (hit.collider.gameObject.layer == (int)Define.Layer.Shop)
                            {
                                destPos = Vector3.zero;
                                hit.collider.gameObject.GetComponent<Shop>().OpenShop();
                                playerState = Define.PlayerState.Idle;
                            }
                            else
                            {
                                if (hit.collider.gameObject.layer == (int)Define.Layer.Enemy)
                                {
                                    if (targetEntity != null)
                                        targetEntity.UnTargetting();

                                    targetEntity = hit.collider.gameObject.GetComponent<Enemy>();
                                    targetTransform = targetEntity.GetComponent<Transform>();
                                    targetEntity.Targetting();
                                }
                                else
                                {
                                    destPos = hit.point;
                                    destPosImage.transform.position = destPos + Vector3.up * 0.1f;

                                    playerState = Define.PlayerState.Moving;
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}