using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class NerfGunScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 startTouchPosition, endTouchPosition;
    private Animation anim;
    public GameObject SpawnPoint;
    public GameObject BallsReference;
    public Material[] AllMaterialsColor;
    public ParticleSystem PartDest;
    public ParticleSystem PartDestSplash;
    public Button ColorButton;
    private bool AnimEnd = true;
    private bool notEmpty = false;
    private int ObjectId = 0;
    private int ColorChoice = 0;
    private bool ButtonNotPressed = true;
    private Color RunningColorNerf;
    public GameObject TextDisplayPoints;
    Color TextFadeColor;
    List<GameObject> Balls= new List<GameObject>();
    Color RunningColor;
    private void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        TextFadeColor.a = 1.0f;
        TextFadeColor = Color.white;
        startTouchPosition.x = 0.4f;
        endTouchPosition.x = 0.2f;
        startTouchPosition.y = 0.4f;
        endTouchPosition.y = 0.2f;
    }
    // Update is called once per frame
    void Update()
    {

        




        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            if (ButtonNotPressed == false)
            {
                ButtonNotPressed = true;
            }
           



        }
        if (endTouchPosition.y == startTouchPosition.y && endTouchPosition.y == startTouchPosition.y && AnimEnd && startTouchPosition.y > 40 && startTouchPosition.x > 30 )
        {
            anim.Play();
            AnimEnd = false;
            GameObject ga;

            ga = Instantiate(BallsReference, SpawnPoint.transform.position, Quaternion.identity);
            ga.tag = "Projectile";
            ga.name = "Ball" + ObjectId.ToString();
            ga.GetComponent<MeshRenderer>().material = AllMaterialsColor[ColorChoice];
            Balls.Add(ga);
            ObjectId++;
            notEmpty = true;
            startTouchPosition.x = 0.4f;
            endTouchPosition.x = 0.2f;
            startTouchPosition.y = 0.4f;
            endTouchPosition.y = 0.2f;
        }
        if (!anim.isPlaying)
        {
            AnimEnd = true;
        }
        MoveBalls();
        if (notEmpty)
        {
            CheckHit();
        }
        if (Time.frameCount % 40 == 0)
        {
            System.GC.Collect();
        }
        if (TextDisplayPoints.activeSelf)
        {
            TextFadeColor.a -= 0.002f;
            if(TextDisplayPoints.gameObject.transform.position.y > 5.56f)
            {
                TextDisplayPoints.gameObject.transform.position = new Vector3(TextDisplayPoints.gameObject.transform.position.x, TextDisplayPoints.gameObject.transform.position.y - 0.01f, TextDisplayPoints.gameObject.transform.position.z);

            }
            TextDisplayPoints.gameObject.GetComponent<TextMesh>().color = TextFadeColor;
            if (TextFadeColor.a <= 0.0f)
            {

                TextFadeColor.a = 1.0f;
                TextFadeColor = Color.white;
               TextDisplayPoints.SetActive(false);
            }
        }



    }

    public void ChangeColor()
    {
        ColorChoice++;
        if (ColorChoice == 3)
        {
            ColorChoice = 0;
        }
        RunningColorNerf = AllMaterialsColor[ColorChoice].GetColor("_Color1");
        ColorButton.gameObject.GetComponent<Image>().color = RunningColorNerf;
        ButtonNotPressed = false;
    }

    void MoveBalls()
    {
        for (int i=0;i<Balls.Count;i++)
        {
           
            Balls[i].GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 30f);

        }
    }
    void CheckHit()
    {
        for (int i = 0; i < Balls.Count; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(Balls[i].transform.position, Balls[i].transform.forward);

            if (Physics.Raycast(ray, out hit, 1f))
            {
                if (hit.collider.tag == "CubeEnemies")
                {
                   

                    if (Balls[i].GetComponent<MeshRenderer>().material.name == hit.collider.gameObject.GetComponent<MeshRenderer>().material.name)
                    {

                        TextDisplayPoints.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x,7.0f, hit.collider.gameObject.transform.position.z);
                        TextDisplayPoints.gameObject.SetActive(true);
                       

                        PartDest.transform.position = hit.collider.gameObject.transform.position;
                        Destroy(Balls[i]);
                        
                        Balls.RemoveAt(i);
                    
                        hit.collider.gameObject.SetActive(false);
                        Destroy(hit.collider);

                        RunningColor = hit.collider.gameObject.GetComponent<MeshRenderer>().material.GetColor("_Color1");
                        PartDest.startColor = RunningColor;
                        
                        PartDestSplash.startColor = RunningColor;

                        PartDest.gameObject.SetActive(true);
                        PartDest.Play();
                        PartDestSplash.gameObject.SetActive(true);
                        PartDestSplash.transform.position = new Vector3(11.05f, 0.8000002f, PartDest.transform.position.z);
                        PartDestSplash.Play();
                       
                    }
                    else
                    {

                        Destroy(Balls[i]);
                        Balls.RemoveAt(i);
                    }
                    
                }
                if (hit.collider.tag == "Obstacles")
                {
                    Destroy(Balls[i]);
                    Balls.RemoveAt(i);
                }
            }
        }
        

    }
    IEnumerator TextHandling()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);



        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        
        
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CubeEnemies")
        {
            //Lose Condition
            Destroy(gameObject);
        }
    }
}
