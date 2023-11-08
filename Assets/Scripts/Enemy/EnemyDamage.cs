using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _attackDelay;

    float _attackCount;

    void Update()
    {
        _attackCount -= Time.deltaTime / _attackDelay;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Damage(c);
    }

    void OnTriggerStay2D(Collider2D c)
    {
        Damage(c);
    }

    void Damage(Collider2D c)
    {

        if(c.gameObject.layer != 3)
            return;

        if(c.gameObject.GetComponent<PlayerHealth>() == null)
            return;

        if(c.gameObject.GetComponent<PlayerMovement>() == null)
            return;
            

        if(_attackCount > 0)
            return;
        
        c.gameObject.GetComponent<PlayerHealth>().health -= _damage;
        c.gameObject.GetComponent<PlayerMovement>().KnockBack(transform.position.x);

        _attackCount = 1f;


    }
}