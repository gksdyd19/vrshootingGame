using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    //������ ���� 
    private enum ZombieState
    {
        Walking,
        Ragdoll,
        Attack
    }

    //���� �ٶ� ī�޶�
    [SerializeField]
    private Camera _camera;

    //������ �� �ִ� �Ÿ�
    [SerializeField]
    private float _attackArea;


    private Rigidbody[]  _ragdollrigidbodies; // ������ ������ �����ִ� ������ٵ� ��� �迭
    private ZombieState  _currentState = ZombieState.Walking; //��������ʵ�
    private Animator _animator;
    private CharacterController _characterController;

   void Awake()
    {
        _ragdollrigidbodies     = GetComponentsInChildren<Rigidbody>();
        _animator               = GetComponent<Animator>();
        _characterController    = GetComponent<CharacterController>();
        DisableRagdoll();


        _attackArea = 1.8f; 
    }

    private void Update()
    {
        // ������ ���¿����� �ż���ȣ��
        switch (_currentState)
        {
            case ZombieState.Walking:
                WalkingBehavior();
                break;
            case ZombieState.Ragdoll:
                RagdollBehavior();
                break;
            case ZombieState.Attack:
                AttackBehavior();
                break;
        }
           
    }

    //�����ϴ� ������ �ż���
    private void AttackBehavior()
    {
        
        if (AttackAreaCheck() == false)
        {
            _currentState = ZombieState.Walking;
            return;
        }
        FindPlayerDirection();
        _animator.Play("Anim_Zombie_Attack");
    }

    public void BeAttacked()
    {

        EnableRagdoll();
        _currentState = ZombieState.Ragdoll;

    }

    //������ ������ ���� ��� ������ �����ִ� �� ������ٵ��� iskinematic Ȱ��ȭ
    //iskinematic�� Ȱ��ȭ �Ǿ������� ��������X
    //�ִϸ�����, ĳ���� ��Ʈ�ѷ� Ȱ��ȭ
    private void DisableRagdoll()
    {
        foreach(var rigidbody in _ragdollrigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _characterController.enabled = true;
    }

    //������ ���ɶ� ������ �����ִ� �� ������ �ٵ��� iskinematic ��Ȱ��ȭ
    //�ִϸ�����, ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
    private void EnableRagdoll()
    {
        foreach(var rigidbody in _ragdollrigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        _animator.enabled = false;
        _characterController.enabled = false;
    }


    //�ȴ� �����϶��� �ż���
    private void WalkingBehavior()
    { 
        if(AttackAreaCheck() == true)
        {
            _currentState = ZombieState.Attack;
            return;
        }
        FindPlayerDirection();
        _animator.SetTrigger("tWalking");           
    }

    //���ݹ����ȿ� �������� Ȯ���ϴ� �ż���
    private bool AttackAreaCheck()
    {
        bool attack = false;
        Vector3 direction = _camera.transform.position - transform.position;
        if (direction.magnitude > _attackArea)
        {
            return attack;
        }
        else
        {
            attack = true;
            
        }
        return attack;
    }


    //�÷��̾� ������ Ȯ���ϰ� ȸ���ϴ� �ż���
    private void FindPlayerDirection()
    {
        Vector3 direction = _camera.transform.position - transform.position;
        direction.y = 0; // ���⿡ ������� ���� �پ��־�� �ϹǷ� y = 0
        direction.Normalize(); //������ ���� ũ��� 1�� vector�� ��ȯ 
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);  //Vector3.up �Ӹ��� vector3.up������ ���ϰ��ϰ� direction�������� ȸ���ϴ� ���� quarternion������ ��ȯ
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 60f * Time.deltaTime);//�� ��ġ���� toRotation �������� 3��° �Ķ���� �ӵ��� ȸ��

    }
    private void RagdollBehavior()
    {
        
    }

    

}
