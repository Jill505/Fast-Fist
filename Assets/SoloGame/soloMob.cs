using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soloMob : MonoBehaviour
{
    soloCenter SoloCenter;
    dirInputSolo dirInput;

    public int mobSort;

    public string mobName;

    public float Hps;
    public float Str;
    public float Rac;
    public float Cur;
    public float Ablock;

    public float ultComsume;

    public float wantToStab = 0f;
    public float addtionalStab = 1f;

    public float wantToUlt = 0f;
    public float addtionalUlt = 1f;

    public float wantToNAttack = 4f;
    public float addtionalNA = 4f;

    public float ABrate = 0.1f;
    public float CBrate = 0.2f;
    

    private void Awake()
    {
        SoloCenter = GameObject.Find("SoloCenter").GetComponent<soloCenter>();
        GameObject.Find("SoloCenter").GetComponent<soloCenter>().mob = this;
        dirInput = GameObject.Find("DirInput").GetComponent<dirInputSolo>();
        dirInput.theMob = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mobAttack()
    {
        if (wantToUlt > ultComsume)
        {
            mobUlt();
            wantToUlt = 0f;
        }
        else if (wantToStab > wantToNAttack)
        {
            mobStab();
            wantToStab = 0f;
        }
        else
        {
            mobNormalAttack();
        }

        wantToUlt += addtionalUlt + Random.Range(0f, 50f);
        wantToStab += addtionalStab;
        wantToNAttack += addtionalNA;
    }

    public virtual float mobDefence(float str)
    {
        float blockRate = Random.Range(0f, 100f);
        blockRate = blockRate / 1000f;
        if (blockRate >= (1f - ABrate))
        {
            //��������
            blockRate = 1f;
        }
        else if (blockRate <= CBrate)
        {
            //������
        }
        else
        {
            blockRate = 0f;
        }

        return blockRate;
    }
    public virtual void mobDefStab()
    {
        //mob���˩Ϊ�^Stab
    }

    public virtual void mobNormalAttack()
    {
        //�Ұʪ��a���q���m
        float dir = Random.Range(0f,360f);
        SoloCenter.mobNormalAttack(dir);
        
    }
    public virtual void mobSpecialAttack()
    {
        //�Ұʪ��a�S���m
        mobNormalAttack();
    }
    public virtual void mobUlt()
    {
        //Ĳ�omob���������Ҧ�
        mobNormalAttack();
    }
    public virtual void mobStab()
    {
        //�Ұʪ��a��먾�m
        mobNormalAttack();
    }



    //���� �o�q�i�H�o�F �i�H������prefab��ƭȴN���� ��H�Ҧ����}�o�u���ӥi�R�F ���}�h
    void sortingMob()
    {
        if (mobSort == 0)
        {
            //���թ�
            mobName = "���թǤH";

            ABrate = 0.1f;
            CBrate = 0.2f;
            
            Hps = 60f;
            Str = 30f;
            
            ultComsume = 300f;

            //���������ɦV
            wantToStab = 0f;
            addtionalStab = 1f;

            wantToUlt = 0f;
            addtionalUlt = 1f;

            wantToNAttack = 4f;
            addtionalNA = 4f;
        }
    }
}
