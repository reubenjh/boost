using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float loadTime = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip winSound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // somewhere stop sound on death
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                BeatLevel();
                break;
            default:
                Die();               
                break;
        }
    }

    private void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1); // TODO allow for loading on final level
    }

    private void LoadCurrentScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }



    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust(float currentThrust)
    {
        rigidBody.AddRelativeForce(Vector3.up * currentThrust);
        if (!audioSource.isPlaying) // starts the thruster clip once
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics  control of rotation
    }

    private void BeatLevel()
    {
        state = State.Transcending;
        Invoke("LoadNextScene", loadTime);

        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
    }

    private void Die()
    {
        state = State.Dying;
        Invoke("LoadCurrentScene", loadTime);

        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
    }
}
