using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float gravity = 5f;
    public float speed = 5f;

    private float moveX;
    private float moveY;

    private int numberOfWay = 1;

    internal float startTime; // время анимации перед стартом
    private float changeWayTime; // время смены дорожки
    private float runningSlideTime; // время анимации подката
    private float flipTime; // время анимации сальто

    private float worldSpeed;
    private float timer;

    private Vector3 boxColCenterStandart = new Vector3(-0.008206278f, 1.24f, 0.1676967f);
    private Vector3 boxColSizeStandart = new Vector3(0.5121227f, 0.51f, 0.2942567f);

    internal Vector3 boxColCenterSlide = new Vector3(-0.005006045f, 0.3195103f, 0.1513492f);
    internal Vector3 boxColSizeSlide = new Vector3(0.7233371f, 0.8105202f, 0.2615619f);

    private Vector3 boxColCenterJump = new Vector3(-0.005006045f, 1.24222f, 0.4360184f);
    private Vector3 boxColSizeJump = new Vector3(0.7233371f, 0.7805223f, 0.8309003f);

    public Effects effects;
    public BoxCollider colliderInDeath;
    public Transform[] pos;
    public GameObject cloak;
    public GameObject flyTarget;
    internal Animator playerAnimator;
    internal BoxCollider boxCol;
    internal Rigidbody rb;
    internal CharacterController characterController;
    internal Vector3 direction = Vector3.zero;
    private Vector3[] positions;

    public WorldBuilder worldBuilder;

    internal bool isRun = false;
    internal bool isFly = false;
    internal bool isGravity = true;
    private bool isInMovement = false;
    private bool isSlide = false;
    private bool isJump = false;
    private bool isFlip = true;

    public delegate void Death();
    public event Death IfPlayerDeath;

    private void Awake()
    {
        if(PlayerController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerController.instance = this;
    }

    void Start()
    {
        positions = new Vector3[3];

        for (int i = 0; i < positions.Length; i++)
        {
            for (int k = 0; k < pos.Length; k++)
            {
                positions[i] = pos[k].position;
            }
        }

        boxCol = GetComponent<BoxCollider>();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        GetAnimationTime();
        StartCoroutine(StartRun());
    }

    void Update()
    {
        Gravity();
        if (isRun)
        {
            //moveX = Input.GetAxisRaw("Horizontal");
            //moveY = Input.GetAxisRaw("Vertical");

            moveX = SwipeManager.instance.inputFloatX;
            moveY = SwipeManager.instance.inputFloatY;

            Move();
            MoveToTheSide();

            DoJump();
            DoSlide();

            StartFly();
        }
    }


    #region Character movements and animation
    private void Move()
    {
        characterController.Move(direction);
    }

    /// <summary>
    /// Гравитация персонажа
    /// </summary>
    private void Gravity()
    {
        if(isGravity)
        {
            direction.y -= (gravity * 0.1f) * Time.deltaTime;
            if (characterController.isGrounded)
            {
                direction.y = -0.01f;
                if (moveY > 0 && !isSlide)
                    direction.y = jumpSpeed * 0.01f;
            }
        }
    }

    /// <summary>
    /// Прыжок персонажа в сторону
    /// </summary>
    private void MoveToTheSide()
    {
        positions[numberOfWay] = pos[numberOfWay].position;

        if (!isInMovement && moveX != 0)
        {
            if (moveX > 0)
            {
                StartCoroutine(ChangeWay(1));
                if (numberOfWay > 2)
                {
                    numberOfWay = 2;
                    return;
                }

                GoRight();
            }
            if (moveX < 0)
            {
                StartCoroutine(ChangeWay(-1));
                if (numberOfWay < 0)
                {
                    numberOfWay = 0;
                    return;
                }

                GoLeft();
            }
            SwipeManager.instance.inputFloatX = 0;
        }

        float step = speed * Time.deltaTime;
        float getXPosition = Mathf.MoveTowards(transform.position.x, positions[numberOfWay].x, step);
        direction.x = getXPosition - transform.position.x;
    }

    #region Анимация перемещения по дорожкам
    /// <summary>
    /// Анимация перемещения на лево
    /// </summary>
    private void GoLeft()
    {
        if (!isJump && !isSlide)
        {
            effects.sounds[6].Play();
            playerAnimator.SetBool("GoLeft", true);
            StartCoroutine(GoLeftCoroutine());
        }
    }

    /// <summary>
    /// Проигрывание анимации перемещения на лево
    /// </summary>
    IEnumerator GoLeftCoroutine()
    {
        yield return new WaitForSeconds(changeWayTime - 0.2f);
        playerAnimator.SetBool("GoLeft", false);
    }

    /// <summary>
    /// Анимация перемещения на право
    /// </summary>
    private void GoRight()
    {
        if (!isJump && !isSlide)
        {
            effects.sounds[6].Play();
            playerAnimator.SetBool("GoRight", true);
            StartCoroutine(GoRightCoroutine());
        }
    }

    /// <summary>
    /// Проигрывание анимации перемещения на право
    /// </summary>
    IEnumerator GoRightCoroutine()
    {
        yield return new WaitForSeconds(changeWayTime - 0.2f);
        playerAnimator.SetBool("GoRight", false);
    }
    #endregion

    /// <summary>
    /// Смена дорожки
    /// </summary>
    /// <param name="way">параметр перемещения по дорожкам</param>
    /// <returns></returns>
    IEnumerator ChangeWay(int way)
    {
        isInMovement = true;
        numberOfWay += way;
        yield return new WaitForSeconds(changeWayTime - 0.2f);
        isInMovement = false;
    }

    /// <summary>
    /// Подкат
    /// </summary>
    private void DoSlide()
    {
        if (moveY < 0)
        {
            if (isJump) return;
            StartCoroutine(StopSlideAfterTime());
        }
    }

    /// <summary>
    /// Анимация подката
    /// </summary>
    /// <returns></returns>
    IEnumerator StopSlideAfterTime()
    {
        isSlide = true;
        boxCol.center = boxColCenterSlide;
        boxCol.size = boxColSizeSlide;
        playerAnimator.SetBool("Sliding", true);
        effects.sounds[10].Play();
        SwipeManager.instance.inputFloatY = 0;
        yield return new WaitForSeconds(runningSlideTime);
        playerAnimator.SetBool("Sliding", false);
        yield return new WaitForSeconds(0.5f);
        boxCol.center = boxColCenterStandart;
        boxCol.size = boxColSizeStandart;
        isSlide = false;
    }

    /// <summary>
    /// Прыжок вверх
    /// </summary>
    private void DoJump()
    {
        if (characterController.isGrounded)
        {
            if (moveY > 0)
            {
                if (isSlide) return;

                playerAnimator.SetTrigger("JumpUpStart");
                effects.sounds[4].Play();
                StartCoroutine(ChangeColliderSize());
                SwipeManager.instance.inputFloatY = 0;
            }
        }

        if (!characterController.isGrounded)
        {
            playerAnimator.SetBool("JumpUpFly", true);
            isJump = true;
        }
        else
        {
            playerAnimator.SetBool("JumpUpFly", false);
            playerAnimator.SetTrigger("JumpUpEnd");
            if (isJump)
                effects.sounds[5].Play();
            isJump = false;
        }
    }

    /// <summary>
    /// Меняем размер коллайдера при прыжке
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColliderSize()
    {
        boxCol.center = boxColCenterJump;
        boxCol.size = boxColSizeJump;
        yield return new WaitForSeconds(1f);
        boxCol.center = boxColCenterStandart;
        boxCol.size = boxColSizeStandart;
    }

    /// <summary>
    /// Задержка перед началом бега
    /// </summary>
    /// <returns> бег возможен </returns>
    IEnumerator StartRun()
    {
        yield return new WaitForSeconds(startTime);
        isRun = true;
    }
    #endregion

    #region Fly
    /// <summary>
    /// Начало полета
    /// </summary>
    private void StartFly()
    {
        if (isFly)
        {
            timer += Time.deltaTime;
            boxCol.enabled = false;
            WorldController.instance.speed = 25f;

            if (isFlip)
            {
                StartCoroutine(CloakSetActive());
                playerAnimator.SetTrigger("Flip");
                isFlip = false;
            }

            playerAnimator.SetBool("IsFly", true);
            isGravity = false;
            float step = speed * Time.deltaTime;
            float getYPosition = Mathf.MoveTowards(transform.position.y, flyTarget.transform.position.y, step);
            direction.y = getYPosition - transform.position.y;

            if (timer > 4.2f)
                WorldController.instance.speed = 10f;
            if (timer > 11.8f)
                StopFly();
            //Debug.Log(timer);
        }
    }

    /// <summary>
    /// Задержка при активации плаща
    /// </summary>
    /// <returns></returns>
    IEnumerator CloakSetActive()
    {
        yield return new WaitForSeconds(flipTime);
        cloak.SetActive(true);
        effects.FlySmoke();
    }

    /// <summary>
    /// Окончание полета
    /// </summary>
    private void StopFly()
    {
        boxCol.enabled = true;
        cloak.SetActive(false);
        playerAnimator.SetBool("IsFly", false);
        playerAnimator.SetTrigger("FlipDown");
        isGravity = true;
        WorldController.instance.speed = worldSpeed;
        worldSpeed = 0f;
        timer = 0;
        effects.StopFlySmoke();
        effects.StopFlySound();
        isFly = false;
    }
    #endregion

    /// <summary>
    /// Обработчик столкновений
    /// </summary>
    /// <param name="col"> коллайдер  с которым столкнулись </param>
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Obstruction"))
        {
            //StartCoroutine(ActionsAfterDeath());
            effects.sounds[8].Play();
            IfPlayerDeath?.Invoke();
        }

        if (col.CompareTag("Fly"))
        {
            isFly = true;
            isFlip = true;
            effects.sounds[7].Play();
            effects.sounds[9].Play();
            Destroy(col.gameObject);
            worldBuilder.isUpperPlatform = true;
            worldBuilder.isObstacle = false;
            worldSpeed = WorldController.instance.speed;
        }
    }

    /// <summary>
    /// Действия после смерти персонажа
    /// </summary>
    /// <returns></returns>
    //IEnumerator ActionsAfterDeath()
    //{
    //    effects.sounds[8].Play();
    //    //isRun = false;
    //    //playerAnimator.SetTrigger("Death");
    //    //WorldController.instance.speed = 0f;
    //    //rb.useGravity = true;
    //    //collider.isTrigger = false;
    //    //boxCol.enabled = false;
    //    //Debug.Log($"boxCol: {boxCol}");
    //    //Debug.Log($"Colider: {collider}");
    //    //yield return new WaitForSeconds(5f);
    //    IfPlayerDeath?.Invoke();
    //}

    private void OnDestroy()
    {
        PlayerController.instance = null;
    }

    /// <summary>
    /// Получаем время проигрывания анимационного клипа
    /// </summary>
    public void GetAnimationTime()
    {
        AnimationClip[] clips = playerAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "start":
                    startTime = clip.length;
                    break;
                case "turnLeft":
                    changeWayTime = clip.length;
                    break;
                case "runingSlide":
                    runningSlideTime = clip.length;
                    break;
                case "flip":
                    flipTime = clip.length;
                    break;
            }
        }
    }
}
