using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct playerInputData : INetworkInput
{
    //public Vector3 movementInput;
    //public playerInputData playerInformationInput;
    public int moodSelectionSort;
    public int characterSelectionSort;

}

public struct PlayerInformation : INetworkInput
{
    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;

    public float MaxmentEnergy;
    public float Energy;
}

public struct AtkInputData : INetworkInput
{
    public bool AtkFinished;//�����I�s

    public float AtkStr;//�����O

    public float AtkDir;//��������
}

public struct DefInputData : INetworkInput
{
    public bool DefFinished;//�����I�s

    //public float FrameChanged;//�[�����ܼƾ�

    public float DefDir;//���u���ר��� �Ω�P�B���u�ʵe
    //public float DefCur;//���u���ɨ���
    //public float ABloack;//��Ǯ��ɨ��� Accurate Block

    //public int injuredType; //���˺��� �Ω�P�B����ʵe�������� 0 = ������; 1 = ���ˤ@�b; 2 = �������;
}

public struct SpecialData : INetworkInput
{
    public float silent;//�I�q�ӽ�
    public float moreRoundRequest;//��h�^�X�� 
}

public struct TestStruck : INetworkInput
{
    public int uploadInt;
}

public struct DebugSpace : INetworkInput
{
    public float debugAtkDir;
    public float debugDefDir;
}