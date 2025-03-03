using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollisionController : MonoBehaviour
{   
    public bool changeColor;
    public Material myMaterial;
    public bool destroyEnemy;
    public bool destroyCollectibles;
    public Text scoreUI;
	public int score;
	protected static int potions;
	public static int sickChickens = 15;
	protected static int sickTrees = 12;

    public float pushPower = 2.0f;

    public AudioClip collisionAudio;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
		
        if(scoreUI != null){
            scoreUI.text = "Sick Chickens: " + sickChickens+ "\nSick Trees: " + sickTrees + "\nPotions: " + potions.ToString() ;
        }
    }
    // only for GameObjects with a mesh, box, or other collider except for character controller and wheel colliders
    void OnCollisionEnter(Collision other)
    {
        if(changeColor == true && other.gameObject.tag == "Tree"){
            Renderer renderer = other.gameObject.GetComponent<Renderer>();
			renderer.material = myMaterial;
			Destroy(other.transform.GetChild(0).gameObject);
			sickTrees--;				
			other.gameObject.transform.tag = "healed";
        }

		 if(changeColor == true && other.gameObject.tag == "SickChicken"){
            Renderer renderer = other.transform.GetChild(4).gameObject.GetComponent<Renderer>();
			renderer.material = myMaterial;
			Destroy(other.transform.GetChild(5).gameObject);
			sickChickens--;				
			other.gameObject.transform.tag = "healed";
        }
		
        if(audioSource != null && !audioSource.isPlaying){
            audioSource.PlayOneShot(collisionAudio, 0.5F);
        }
        if(destroyEnemy == true && other.gameObject.tag == "Enemy" || destroyCollectibles == true && other.gameObject.tag == "Collectible"){
            Destroy(other.gameObject);
        }
    }
    // only for GameObjects with a character controller applied
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // if no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }
        // don't push ground or platform GameObjects below player
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }
        // calculate push direction from move direction, only along x and z axes - no vertical pushing
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        // apply the push and character's velocity to the pushed object
        if(hit.gameObject.tag == "Object"){
            body.velocity = pushDir * pushPower;
        }
		
		/**
        if(changeColor == true && hit.gameObject.tag == "Collectible"){
            Renderer renderer = hit.transform.GetChild(4).gameObject.GetComponent<Renderer>();
			renderer.material = myMaterial;
        }
		*/
 
        if(audioSource != null && !audioSource.isPlaying){
            audioSource.PlayOneShot(collisionAudio, 0.5F);
        }
        if(hit.gameObject.tag == "Pollution"){
            Destroy(hit.gameObject);
			potions += 1;
        }
    }
}
 
