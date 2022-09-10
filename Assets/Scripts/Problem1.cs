using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurveType
{
    easeIn,
    easeOut,
    easeInOut
}

public class Problem1 : MonoBehaviour
{
    static Vector3 begin = new Vector3(0f, 0f, 0f);        //设置起点
    static Vector3 end = new Vector3(5f, 5f, 5f);          //设置终点
    float time = 2f;           //设置从起点到终点的时间为5s
    Vector3 targetDirection;        //移动方向
    float moveSpeed;            //移动速度
    bool pingpong = true;       //设置是否循环移动,如果要循环移动，修改为true
    static bool moveAgain = true;
    static bool curseMoveAgain = true;

    [SerializeField]
    bool isStartCurve = false;      //是否开启动画效果
    [SerializeField]
    private AnimationCurve _easeInCurve;
    [SerializeField]
    private AnimationCurve _easeOutCurve;
    [SerializeField]
    private AnimationCurve _easeInOutCurve;
    [SerializeField]
    private CurveType curveType;
    float curveValue;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = begin;         //将gameObject位置初始化到起点

        if (isStartCurve) StartCoroutine(StartCurve());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStartCurve)
        {
            Move(this.gameObject, ref begin, ref end, time, pingpong);
        }
    }

    //使gameObject在time秒内，从begin移动到end
    //若pingpong为true，则在结束时，使gameObject在time秒内从end移动到begin，如此往复
    public void Move(GameObject gameObject, ref Vector3 begin, ref Vector3 end, float time, bool pingpong)
    {

        if (Vector3.Distance(gameObject.transform.position, end) > 0.1f)      //pingpong为false只移动一次
        {
            if (moveAgain)
            {
                CountParam(begin, end, time, targetDirection, moveSpeed);
                moveAgain = false;

            }
            gameObject.transform.Translate(Vector3.Normalize(targetDirection) * moveSpeed * Time.deltaTime);

        }
        else
        {
            if (pingpong && moveAgain == false)
            {
                moveAgain = true;
                Vector3 temp = begin;
                begin = end;
                end = temp;
            }
        }


    }

    //根据起点，终点以及时间计算出方向和移动速度
    public void CountParam(Vector3 begin,Vector3 end,float time, Vector3 targetDerection,float movespeed)
    {       
        targetDirection = end - begin;
        moveSpeed = Vector3.Distance(begin, end) / time;
    }

    IEnumerator StartCurve()
    {
        float tempTime = 0f;
        Vector3 direction = end - begin;

        while (tempTime <= time)
        {
            float normalizedTime = tempTime / time;
            tempTime += Time.deltaTime;
            switch (curveType)
            {
                case CurveType.easeIn:
                    curveValue = _easeInCurve.Evaluate(normalizedTime);
                    break;
                case CurveType.easeOut:
                    curveValue = _easeInOutCurve.Evaluate(normalizedTime);
                    break;
                case CurveType.easeInOut:
                    curveValue = _easeInOutCurve.Evaluate(normalizedTime);
                    break;
                default:
                    break;
            }
            transform.position = begin + new Vector3(direction.x * curveValue, direction.y * curveValue, direction.z * curveValue);

            yield return null;
        }
        Vector3 temp = begin;
        begin = end;
        end = temp;
        StartCoroutine(StartCurve());
    }
}
