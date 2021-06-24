using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;

public class enemyGFX : MonoBehaviour
{
    public AIPath aIPath;

    GameObject player;
    AIDestinationSetter setter;
    private void Start()
    {
      
        setter = this.GetComponent<AIDestinationSetter>();
    }
    void Awake()
    {
        
      setter = this.GetComponent<AIDestinationSetter>();
      player = GameObject.Find("Cat");
      setter.target = player.transform;
      StartCoroutine(placeTag());
      
    }

    // Update is called once per frame
    void Update()
    {
        if (aIPath.desiredVelocity.x >= 0.01f) {
            this.GetComponent<SpriteRenderer>().flipX = false;
        } else if (aIPath.desiredVelocity.x <= -0.01f) {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void FixedUpdate()
    {
        
      
    }

   
    IEnumerator placeTag() {
        this.gameObject.layer = 9;
        
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Light2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        this.gameObject.layer = 7;
        if (this.gameObject.tag == "Untagged")
        {

            this.gameObject.tag = "addedEnemy";
        }

    }
}
