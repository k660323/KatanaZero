using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Creature GetCreature;

    protected Vector2 _lookDir;
    public Vector2 LookDir 
    { 
        get 
        { 
            return _lookDir; 
        }
        set
        {
            _lookDir = value;

            Vector3 localScale = GetCreature.transform.localScale;
            // ¿À¸¥ÂÊ
            if (value == Vector2.right)
            {
                GetCreature.transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
            // ¿ÞÂÊ
            else
            {
                GetCreature.transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
        }
    }

    [SerializeField]
    protected GameObject target;
    public virtual GameObject Target
    {
        get => target;
        set
        {
            target = value;

            if ((GetCreature is KissfaceEnemy) == false)
            {
                if (target)
                {
                    GetCreature.FollowMark.SetActive(true);
                }
                else
                {
                    GetCreature.FollowMark.SetActive(false);
                }
            }
        }
    }

    public Vector3 SpawnPos;
    public Vector3 ChaseStartPos;

    private void Awake()
    {
        TryGetComponent(out GetCreature);
    }

    private void Start()
    {
        Init();   
    }

    protected virtual void Init()
    {
        LookDir = Vector2.right;
        Vector3 pos = Managers.Map.CurrentGrid.WorldToCell(GetCreature.transform.position) + new Vector3(0.5f, 0.0f);
        transform.position = pos;

        if (GetCreature is KissfaceEnemy)
        {
            Target = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
