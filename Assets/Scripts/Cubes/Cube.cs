using System;
using System.Collections;
using UnityEngine;

public class Cube : Colorable
{
    [Header("Yellow")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveTime;
    [SerializeField] private float moveHeight;
    [SerializeField] private float latency;
    [Header("Dark")]
    [SerializeField] private Material darkMaterial;
    [SerializeField] private float darkTime;


    [SerializeField] private bool isDark;
    private bool isMoving = false;
    private Coroutine coroutine;
      

    private Vector3 startPosition;
    private void Start()
    {
        defaultColor = Color;
        defaultTag = tag;
        startPosition = transform.position;
        if (isDark)
            StartCoroutine(DarkCube());
    }

    private void Update()
    {
        if (tag == "Yellow" && Transparency == 1)
        {
            if(!isMoving)
                coroutine = StartCoroutine(MoveObject());
        }
        else
        {
            if(coroutine != null)
            {
                isMoving = false;
                StopAllCoroutines();
            }
        }
    }

    #region YELLOW
    private IEnumerator MoveObject()
    {
        isMoving = true;
        if (transform.position != startPosition)
        {
            yield return StartCoroutine(Move(transform.position, startPosition));
        }

        // ��������� ������ �� ��������� ������
        Vector3 endPosition = startPosition + Vector3.up * moveHeight;
        yield return StartCoroutine(Move(startPosition, endPosition));

        // ���������� ������ � ��������� �������
        yield return StartCoroutine(Move(endPosition, startPosition));

        isMoving = false;
    }
    IEnumerator Move(Vector3 from, Vector3 where)
    {
        float t = 0;
        while (t < 1)
        {
            t += moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(from, where, t);
            yield return null;
        }

        // ������������� ������ � ������  �������
        transform.position = where;

        // ���� ��������� ����� � �����
        yield return new WaitForSeconds(latency);
    }
    #endregion

    public void SetTransparent(float transparent)
    {
        Material materia = GetComponent<MeshRenderer>().sharedMaterial;
        materia.SetFloat("_Transparent", transparent);
        if (transparent == 1)
        {
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 4;
        }
    }
    private IEnumerator DarkCube()
    {
        while(true)
        {
            yield return new WaitForSeconds(darkTime);
            Color = darkMaterial;
            tag = "Dark";
            SetTransparent(0.5f);

            yield return new WaitForSeconds(darkTime);
            SetColor(defaultColor, defaultTag);
            SetTransparent(1f);
        }
    }
}
