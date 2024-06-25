using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        if (!GameManager.Instance.islive)
            return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;


            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                    break;

        }

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10,1);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0)
        {
            Batch();
        }

        //player.BroadcastMessage("ApplyGear");
    }

    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamge;
        if(GameManager.Instance.PlayerID == 0)
        { 
            count = data.baseCount + Character.Count;
        }
        else if(GameManager.Instance.PlayerID == 2)
        {
            count = data.baseCount - Character.Hard;
        }

        for (int index = 0; index < GameManager.Instance.pool.prefabs.Length; index++)
        {
            if(data.projectiles == GameManager.Instance.pool.prefabs[index])
            {
                prefabID = index;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = 220 * Character.WeaoponSpeed;
                Batch();
                break;

            default:
                speed = 0.3f * Character.WeaoponRate;
                break;
        }

        //player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index = 0; index < count; index++)
        {
            Transform bullet;
            if(index < transform.childCount)
            {
                bullet=transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabID).transform;
                bullet.parent = transform;
            }
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.7f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.pool.Get(prefabID).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
