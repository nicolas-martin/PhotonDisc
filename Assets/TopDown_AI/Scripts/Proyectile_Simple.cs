using UnityEngine;
using System.Collections;

public class Proyectile_Simple : MonoBehaviour
{
    public enum CollisionTarget
    {
        PLAYER,
        ENEMIES
    }

    public PlayerBehavior sourcePlayer;

    public CollisionTarget collisionTarget;
    public float lifeTime = 999999.0f;
    public float speed = 0.005f;
    public bool ToBeDeleted { get; private set; }


    bool hitTest = true;
    bool moving;



    void Start()
    {
        moving = true;
        //Destroy(gameObject, lifeTime);
    }


    void Update()
    {
        if (moving)
            transform.Translate(transform.forward * speed, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collisionTarget == CollisionTarget.PLAYER && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>().DamagePlayer();
        }
        else if (collisionTarget == CollisionTarget.ENEMIES && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<NPC_Enemy>().Damage();
        }
        else if (collisionTarget == CollisionTarget.ENEMIES && collision.gameObject.tag == "Player")
        {
            if (sourcePlayer == collision.gameObject.GetComponent<PlayerBehavior>())
            {
                this.ToBeDeleted = true;
                sourcePlayer.Collect(this);
            }
            else
            {
                //collision.gameObject.GetComponent<PlayerBehavior>().DamagePlayer();
            }
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //This is to detect if the proyectile collides with the world, i used this tag because it is standard in Unity (To prevent asset importing issues)
            var test = Vector3.Reflect(this.transform.forward, collision.contacts[0].normal);

            // rotate the object by the same ammount we changed its velocity
            Quaternion rotation = Quaternion.FromToRotation(this.transform.forward, test);
            transform.rotation = rotation * transform.rotation;
            //DestroyProyectile();
        }
    }

    void DestroyProyectile()
    {
        /*hitTest=false;
                gameObject.GetComponent<Rigidbody> ().isKinematic = true;
                gameObject.GetComponent<Collider> ().enabled = false;
                moving = false;*/
        Destroy(gameObject);
    }
}

