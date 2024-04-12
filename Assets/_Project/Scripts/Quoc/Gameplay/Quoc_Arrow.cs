using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Quoc_Arrow : MonoBehaviour
{
    [SerializeField] private Sprite spriteArrowMustHit;

    [SerializeField] private Sprite spriteArrow;

    [SerializeField] private Sprite spriteTailMustHit;
    
    [SerializeField] private Sprite spriteTail;

    [SerializeField] private SpriteRenderer spriteRendererArrow;
    
    [SerializeField] private SpriteRenderer spriteRendererTail;

    [SerializeField] private BoxCollider2D collider2D;

    private bool isMustHit;

    private bool isPress;

    public bool isCollider;

    public bool isCorrectArrow;

    public float timerAnim;

    private int indexArrow;

    private const float DeltaMoveMustHit = 4;


    public bool IsPress
    {
        get => isPress;
        set => isPress = value;
    }

    private void Awake()
    {
        spriteRendererArrow = GetComponent<SpriteRenderer>();
        spriteRendererTail = transform.GetChild(0).GetComponent<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
    }

   public void SetupArrow(float timeMove, float timeTail, int indexArrow ,bool isMustHit, float deltaMove, int serial, int sumArrow)
    {
        this.isMustHit = isMustHit;
        transform.name = serial.ToString();
        isCollider = false;
        isCorrectArrow = false;


        if(sumArrow > 0)
        {
            Vector3 posCur = transform.position;
            posCur.z = (float)serial / (float)sumArrow;
            transform.position = posCur;
        }

        this.indexArrow = indexArrow;
        timeTail -= 0.22f;
        if (timeTail <= 0.22f)
        {
            spriteRendererTail.enabled = false;
            collider2D.size = new Vector2(1.25f, 1.25f);
            collider2D.offset = Vector2.zero;
            timeTail = 0;
        }
        else
        {
            spriteRendererTail.enabled = true;
            float sizeY = timeTail * 10;
            collider2D.size = new Vector2(1.2f, 1.25f+ sizeY);
            spriteRendererTail.size = new Vector2(0.5f, sizeY);

        }

        timerAnim = timeTail;
        float newTimemove = deltaMove / timeMove;
        float posDesY = transform.position.y + timeTail * 10 + deltaMove;

       
        if (isMustHit)
        {
            spriteRendererArrow.sortingOrder = 20;
            spriteRendererTail.sortingOrder = 19;

            newTimemove = (timeTail * 10 + deltaMove + DeltaMoveMustHit) / newTimemove;
            spriteRendererArrow.sprite = spriteArrowMustHit;
            spriteRendererTail.sprite = spriteTailMustHit;
            posDesY += DeltaMoveMustHit;

        }
        else
        {
            //spriteRendererArrow.sprite = spriteArrow;
            spriteRendererArrow.sortingOrder = 15;
            spriteRendererTail.sortingOrder = 13;

            newTimemove = (timeTail * 10 + deltaMove) / newTimemove;
            spriteRendererArrow.sprite = spriteArrow;
            spriteRendererTail.sprite = spriteTail;
        }

        transform.DOMoveY(posDesY, newTimemove).SetEase(Ease.Linear).OnComplete(() =>
        {
            //Destroy self
            DestroySelf();
        });
    }

    public void DestroySelf()
    {
        DOTween.Kill(transform);
        Destroy(gameObject);
    }

    public void SetCorrect()
    {
        Debug.Log("setcorrect");

        if (!isMustHit) return;

        if (isCorrectArrow) return;

        Debug.Log("setcorrect 1");

        spriteRendererTail.sortingOrder = 3;
        isCorrectArrow = true;

        spriteRendererArrow.enabled = false;
        if (timerAnim == 0)
        {
            collider2D.enabled = false;
        }

        Quoc_GameManager.Instance.uiGameplay.SetCorrectArrow(indexArrow);
        Quoc_GameManager.Instance.lsTargetArrows[indexArrow].SetCorrectCollider(indexArrow + 1, timerAnim);
    }

    public void SetInvisibleTail()
    {
        spriteRendererTail.enabled = false;
        collider2D.enabled = false;

        //set animation idle for main
        Quoc_GameManager.Instance.SetAnimationBoy(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter: " + other.gameObject.name);
        if (other.gameObject.layer == 8)
        {
            isCollider = true;
            if (isMustHit)
            {
                //det colloder for target
                Quoc_GameManager.Instance.lsTargetArrows[indexArrow].SetCollider(this);
            }
            else
            {
                //ser animation  Enemy
                Quoc_GameManager.Instance.SetAnimationEnemy(indexArrow+1);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            isCollider = false;
            if (isMustHit)
            {
                if (isCorrectArrow)
                {
                    //add score
                    Quoc_GameManager.Instance.AddScore();
                }
                else
                {
                    //sub score
                    Quoc_GameManager.Instance.SubScore();
                    //set anim fail 4 main
                    Quoc_GameManager.Instance.SetAnimationBoy(indexArrow + 5);
                }
                //exit collider target arrow
                Quoc_GameManager.Instance.lsTargetArrows[indexArrow].ExitCollider(this);

            }
        }
    }
}
