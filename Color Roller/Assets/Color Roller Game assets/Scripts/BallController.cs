using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorRoller { 
    public class BallController : MonoBehaviour
    {
        public Rigidbody rb;
        public float speed = 15;

        public int minSwipeRecognition;

        private bool isTraveling;
        private Vector3 travelDirection;

        private Vector2 swipePosLastFrame;
        private Vector2 swipePosCurrentFrame;
        private Vector2 currentSwipe;

        private Vector3 nextCollisionPosition;
        private Vector3 PreDirection, PreDirection2;
        private ParticleSystem splash;
        private AudioSource MyAudio;
        private TrailColorPicker myTrailerColor;

        private Color solveColor;

        private void Start()
        {
            myTrailerColor = GetComponent<TrailColorPicker>();
            solveColor = myTrailerColor.BallNTrailColor[Random.Range(0, myTrailerColor.BallNTrailColor.Length)];

            GetComponent<MeshRenderer>().material.color = solveColor;

            minSwipeRecognition = 1000;

            PreDirection = Vector3.zero;

            splash = GetComponent<ParticleSystem>();
            MyAudio = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (!GameManager.singleton.endReached)
            {
                // Set the balls speed when it should travel
                if (isTraveling)
                {
                    rb.velocity = travelDirection * speed;
                }

                // Paint the ground
                Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
                int i = 0;
                GroundPiece prevground = hitColliders[0].transform.GetComponent<GroundPiece>();
                while (i < hitColliders.Length)
                {
                    GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();

                    if (ground && !ground.isColored)
                    {
                        ground.Colored(solveColor);
                        if(!GameManager.singleton.devMode)
                            Session.Instance.UpdateScore(GameManager.points,GameManager.points+1);
                        GameManager.points += 1;
                        GameManager.levelScore += 1;
                    }

                    i++;
                }

                // Check if we have reached our destination
                if (nextCollisionPosition != Vector3.zero)
                {
                    if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
                    {
                        if (PreDirection2 != travelDirection)
                        {
                            splash.Play();
                            MyAudio.Play();
                        }
                        isTraveling = false;
                        PreDirection2 = travelDirection;
                        travelDirection = Vector3.zero;
                        nextCollisionPosition = Vector3.zero;
                        transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
                    }
                }

                if (isTraveling)
                    return;

                // Swipe mechanism
                if (Input.GetMouseButton(0))
                {
                    // Where is the mouse now?
                    swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                    if (swipePosLastFrame != Vector2.zero)
                    {

                        // Calculate the swipe direction
                        currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                        if (currentSwipe.sqrMagnitude < minSwipeRecognition) // Minium amount of swipe recognition
                            return;

                        currentSwipe.Normalize(); // Normalize it to only get the direction not the distance (would fake the balls speed)

                        // Up/Down swipe
                        if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                        }

                        // Left/Right swipe
                        if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                        }
                    }


                    swipePosLastFrame = swipePosCurrentFrame;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    swipePosLastFrame = Vector2.zero;
                    currentSwipe = Vector2.zero;
                }
            }
        }

        private void SetDestination(Vector3 direction)
        {
            travelDirection = direction;

            // Check with which object we will collide
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 100f))
            {
                nextCollisionPosition = hit.point;
            }

            isTraveling = true;

            if (direction != PreDirection)
            {
                if (direction == Vector3.right || direction == Vector3.left)
                {
                    transform.localScale = new Vector3(0.95f, 0.95f, 0.6f);
                    PreDirection = direction;
                }
                else
                {
                    transform.localScale = new Vector3(0.6f, 0.95f, 0.95f);
                    PreDirection = direction;
                }
            } 
        }
    }
}