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
    static Vector3 begin = new Vector3(0f, 0f, 0f);        //�������
    static Vector3 end = new Vector3(5f, 5f, 5f);          //�����յ�
    float time = 2f;           //���ô���㵽�յ��ʱ��Ϊ5s
    Vector3 targetDirection;        //�ƶ�����
    float moveSpeed;            //�ƶ��ٶ�
    bool pingpong = true;       //�����Ƿ�ѭ���ƶ�,���Ҫѭ���ƶ����޸�Ϊtrue
    static bool moveAgain = true;
    static bool curseMoveAgain = true;

    [SerializeField]
    bool isStartCurve = false;      //�Ƿ�������Ч��
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
        transform.position = begin;         //��gameObjectλ�ó�ʼ�������

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

    //ʹgameObject��time���ڣ���begin�ƶ���end
    //��pingpongΪtrue�����ڽ���ʱ��ʹgameObject��time���ڴ�end�ƶ���begin���������
    public void Move(GameObject gameObject, ref Vector3 begin, ref Vector3 end, float time, bool pingpong)
    {

        if (Vector3.Distance(gameObject.transform.position, end) > 0.1f)      //pingpongΪfalseֻ�ƶ�һ��
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

    //������㣬�յ��Լ�ʱ������������ƶ��ٶ�
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
