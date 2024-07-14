using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D colli;
    void Awake()
    {
        colli = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;

        //Vector3 playerDir = GameManager.Instance.player.inputVec;


        switch(transform.tag)
        {
            case "Ground":
                float difx =playerPos.x - myPos.x;
                float dify =playerPos.y - myPos.y;
                float dirx = difx < 0 ? -1 : 1;
                float diry = dify < 0 ? -1 : 1;
                difx = Mathf.Abs(difx);
                dify = Mathf.Abs(dify);

                if (difx>dify)
                {
                    transform.Translate(Vector3.right * dirx * 40);
                }
                else if (difx < dify)
                {
                    transform.Translate(Vector3.up * diry * 40);
                }
                break;
            case "Enemy":
                if(colli.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
