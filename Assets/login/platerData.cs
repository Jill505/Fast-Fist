using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerData
{
    //��¦�b���ƾ�
    public float gold;
    public float diamond;
    public string Name;//���a�W�r

    //��¦���a�ƾ�
    public int selectionMoodsort;//��ܤ��߱�
    public int selectionCharacter;//��ܤ�����

    //���U�g�������S��item�P�ƶq
    public int[] item;
    //���U�g�����}��
    public bool[] haveCharacter;
    //���U�g�����߱�
    public bool[] haveMood;

    //���U�g�S���� �H��ηs���ۭq���O
    public bool[] loginTimes;//�n������
}
