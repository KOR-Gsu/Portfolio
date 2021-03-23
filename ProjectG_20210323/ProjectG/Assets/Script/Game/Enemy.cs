using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string hpGaugePrefabName;

    private Transform myTransform;
    private LivingEntity targetEntity;
    private Transform targetTransform;
    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;
    private GameObject hpGauge;
    private MonsterHPGauge hpGaugeScript;
    private DropItem dropItem;
    private float attackDamage;
    private float defense;
    private float scanRange;
    private float attackRange;
    private float intvlAttackTime;
    private float lastAttackTime;
    private float waitTime;
    private float moveTime;
    private Vector3 randRotate;
    private Vector3 originPosition;
    private bool isOutOfRange;

    public float exp { get; private set; }
    public float gold { get; private set; }
    private bool stopAttack;

    private Define.EnemyState _enemyState;
    public Define.EnemyState enemyState
    {
        get { return _enemyState; }
        set
        {
            _enemyState = value;
            switch (_enemyState)
            {
                case Define.EnemyState.Die:
                    enemyAnimator.SetBool("Die", true);
                    break;
                case Define.EnemyState.Idle:
                    enemyAnimator.SetInteger("Move", 0);
                    enemyAnimator.SetInteger("Attack", 0);
                    break;
                case Define.EnemyState.Moving:
                    enemyAnimator.SetInteger("Attack", 0);
                    enemyAnimator.SetInteger("Move", 1);
                    break;
                case Define.EnemyState.Attack:
                    enemyAnimator.SetInteger("Move", 0);
                    enemyAnimator.SetInteger("Attack", 1);
                    break;
                case Define.EnemyState.AttackIdle:
                    enemyAnimator.SetInteger("Move", 0);
                    enemyAnimator.SetInteger("Attack", 2);
                    break;
            }
        }
    }

    private bool isAttackAble
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
                if (Vector3.Distance(myTransform.position, targetTransform.position) <= attackRange)
                    return true;

            return false;
        }
    }

    public void Initializing(MonsterData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        attackDamage = data.attackDamage;
        defense = data.defense;
        scanRange = data.scanRange;
        attackRange = data.attackRange;
        pathFinder.speed = data.moveSpeed;
        pathFinder.angularSpeed = data.rotateSpeed;
        intvlAttackTime = data.intvlAttackTime;
        exp = data.exp;
        gold = data.gold;
    }

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        dropItem = GetComponent<DropItem>();
    }
    
    void Start()
    {
        damagedTextColor = Color.white;
        enemyState = Define.EnemyState.Idle;
        stopAttack = true;

        hpGauge = Managers.Resource.Instantiate(string.Format("{0}/{1}", nameof(Define.ResourcePath.UI), hpGaugePrefabName), UIManager.instance.myCanvas.transform);
        hpGaugeScript = hpGauge.GetComponentInChildren<MonsterHPGauge>();
        hpGaugeScript.targetTransform = hudPos;
        hpGaugeScript.Initialize(currentHP / maxHP);
        HideHPBar();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        enemyState = Define.EnemyState.Idle;
        pathFinder.enabled = true;
        lastAttackTime = 0;
        waitTime = 0;
        moveTime = 0;
        randRotate = Vector3.zero;
        originPosition = myTransform.position;
        isOutOfRange = false;

        targetTransform = null;

        StartCoroutine(UpdateScanPath());

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = true;
    }

    void Update()
    {
        if (!dead)
        {
            if (isTarget || currentHP < maxHP)
                ShowHPBar();
            else
                HideHPBar();

            switch (enemyState)
            {
                case Define.EnemyState.Idle:
                    UpdateIdle();
                    break;
                case Define.EnemyState.Moving:
                    UpdateMoving();
                    break;
                case Define.EnemyState.AttackIdle:
                    UpdateAttack();
                    break;
            }
        }
    }

    private void UpdateIdle()
    {
        pathFinder.isStopped = true;

        if (targetEntity == null)
        {
            if (Vector3.Distance(myTransform.position, originPosition) >= 10f)
                isOutOfRange = true;

            waitTime -= Time.deltaTime;
            if(waitTime <= 0)
            {
                moveTime = Random.Range(2f, 2.5f);
                randRotate.Set(0f, Random.Range(0f, 360f), 0f);
                enemyState = Define.EnemyState.Moving;
            }
        }
    }

    private void UpdateMoving()
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Attack"))
            return;

        if (targetEntity == null)
            MoveAround();
        else
        {
            if (isAttackAble)
            {
                enemyState = Define.EnemyState.AttackIdle;
                pathFinder.SetDestination(myTransform.position);
                return;
            }

            Vector3 dir = targetTransform.position - myTransform.position;
            float moveDist = Mathf.Clamp(pathFinder.speed * Time.deltaTime, 0, dir.magnitude);

            pathFinder.Move(moveDist * dir.normalized);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(dir), pathFinder.angularSpeed * Time.deltaTime);
        }
    }

    private void UpdateAttack()
    {
        pathFinder.isStopped = true;
        myTransform.LookAt(targetTransform);

        if (Time.time >= lastAttackTime + intvlAttackTime)
        {
            stopAttack = false;
            enemyState = Define.EnemyState.Attack;
            lastAttackTime = Time.time;
        }
    }

    private void MoveAround()
    {
        if (Physics.Raycast(myTransform.position, myTransform.forward, 1f, (int)Define.Layer.Terrain))
        {
            moveTime = 0;
            pathFinder.ResetPath();
            waitTime = Random.Range(1f, 1.5f);
            enemyState = Define.EnemyState.Idle;

            return;
        }

        if (isOutOfRange)
        {
            Vector3 dir = originPosition - myTransform.position;
            if (dir.magnitude < 0.2f)
            {
                isOutOfRange = false;
                enemyState = Define.EnemyState.Idle;
            }

            float moveDist = Mathf.Clamp(pathFinder.speed * Time.deltaTime, 0, dir.magnitude);

            pathFinder.Move(moveDist * dir.normalized);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(dir), pathFinder.angularSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 rotate = Vector3.Lerp(myTransform.eulerAngles, randRotate, 0.01f);
            myTransform.rotation = Quaternion.Euler(rotate);

            Vector3 pos = myTransform.forward * pathFinder.speed * Time.deltaTime;
            pathFinder.Move(pos);

            moveTime -= Time.deltaTime;
            if (moveTime <= 0)
            {
                waitTime = Random.Range(1f, 1.5f);
                enemyState = Define.EnemyState.Idle;
            }
        }
    }

    private IEnumerator UpdateScanPath()
    {
        while(!dead)
        {
            if (!isAttackAble)
            {
                Collider[] colliders = Physics.OverlapSphere(myTransform.position, scanRange, targetLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        targetTransform = targetEntity.transform;
                        enemyState = Define.EnemyState.Moving;
                        break;
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnAttackEvent()
    {
        if (targetEntity != null)
        {
            targetEntity.OnDamage(attackDamage);

            if (targetEntity.dead)
            {
                stopAttack = true;
                targetEntity = null;
                targetTransform = null;
            }
        }

        if (stopAttack)
            enemyState = Define.EnemyState.Idle;
        else
        {
            if (isAttackAble)
                enemyState = Define.EnemyState.AttackIdle;
            else
                enemyState = Define.EnemyState.Moving;
        }
    }

    public override void OnDamage(float damage)
    {
        float finalDamage = Mathf.Max(0, damage - defense);

        base.OnDamage(finalDamage);

        hpGaugeScript.Initialize(currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();

        GameObject.Find("Player").GetComponent<PlayerStat>().GetExp((int)exp, (int)gold);

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = false;

        targetEntity = null;
        enemyState = Define.EnemyState.Die;
        pathFinder.enabled = false;
        stopAttack = true;
        currentHP = maxHP;

        StopCoroutine(UpdateScanPath());
        hpGaugeScript.ResetGauge();
        HideHPBar();

        dropItem.DropItems();

        Invoke("SetActiveFalse", 2.5f);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);

        Managers.Game.OnSpawnEvent.Invoke();
    }

    private void ShowHPBar()
    {
        if (hpGauge != null)
            hpGauge.gameObject.SetActive(true);
    }

    private void HideHPBar()
    {
        if(hpGauge != null)
            hpGauge.gameObject.SetActive(false);
    }
}