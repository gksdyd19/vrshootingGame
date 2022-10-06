using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    //������ ���� 
    private enum ZombieState
    {
        Walking,
        Ragdoll
    }

    //���� �ٶ� ī�޶�
    [SerializeField]
    private Camera _camera;


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

        }
           
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
        Vector3 direction = _camera.transform.position - transform.position;
        direction.y = 0; // ���⿡ ������� ���� �پ��־�� �ϹǷ� y = 0
        direction.Normalize(); //������ ���� ũ��� 1�� vector�� ��ȯ 

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);  //Vector3.up �Ӹ��� vector3.up������ ���ϰ��ϰ� direction�������� ȸ���ϴ� ���� quarternion������ ��ȯ
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 20f * Time.deltaTime);//�� ��ġ���� toRotation �������� 3��° �Ķ���� �ӵ��� ȸ��
                    
    }

    private void RagdollBehavior()
    {
        
    }

    

}
