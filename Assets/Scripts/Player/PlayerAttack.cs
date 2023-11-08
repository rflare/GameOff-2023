using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //Dependencies
    [SerializeField] GameObject _attackObject;
    //Params
    [SerializeField] float _damage;
    [SerializeField] float _attackDuration;
    [SerializeField] float _attackDelay;

    //Buffers
    float _attackCount;

    void Update()
    {

        if(_attackCount > 0)
        {
            _attackCount = Mathf.Max(0, _attackCount - Time.deltaTime / _attackDelay);
            return;
        }
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        if(!Input.GetMouseButtonDown(0))
            yield break;

        _attackObject.GetComponent<SpriteRenderer>().enabled = true;

        _attackCount = 1;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        ContactFilter2D cf = new ContactFilter2D();
        cf.SetLayerMask(64);
        cf.useTriggers = true;

        _attackObject.GetComponent<Collider2D>().Cast(Vector2.down, cf, hits);


        foreach(RaycastHit2D hit in hits)
        {
            hit.transform.GetComponent<EnemyHealth>().health -= _damage;
        }

        yield return new WaitForSeconds(_attackDuration);
        _attackObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}