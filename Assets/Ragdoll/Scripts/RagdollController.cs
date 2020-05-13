using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
	public Animator animator = null;

	new Rigidbody rigidbody;
	CapsuleCollider capsuleCollider;

	private Transform mainCameraTransform = null;
    [SerializeField]
    Transform spine, aim, player;

	Vector3 targetRotation;

    [SerializeField]
	private float rotationSpeed = 10f;
    [SerializeField]
	private float speed = 3f;
    [SerializeField]
	private float jumpForce = 3;
    [SerializeField]
	private float outValue = 10;
	private float vertical;
	private float horizontal;

    float mouseY;

	bool isGround = true;
	public bool alive = true;



    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		mainCameraTransform = Camera.main.transform;
	}

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	void Update()
    {
        Aim();
        if (Input.GetButtonDown("Jump") && isGround)
        {
            rigidbody.AddForce(new Vector3(0, jumpForce * 100, 0), ForceMode.Impulse);
            isGround = false;
        }
    }

    private void Aim()
    {
        mouseY += Input.GetAxisRaw("Mouse Y") * 2f;
        mouseY = Mathf.Clamp(mouseY, -50, 30);
        aim.eulerAngles = new Vector3(/*transform.eulerAngles.x*/-mouseY * 2f, player.eulerAngles.y, player.eulerAngles.z);
    }

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
    void FixedUpdate()
    {
        if (alive)
        {
            Move();
            Bend();
        }
        else
        {
            animator.enabled = false;
        }


    }

    /// <summary>
    /// Böjer radollens överkropp till siktets rotation i y-led.
    /// /Jonathan & Johan
    /// </summary>
    private void Bend()
    {
        spine.eulerAngles = new Vector3(spine.eulerAngles.x, spine.eulerAngles.y, -aim.eulerAngles.x - 90);
    }

    /// <summary>
    /// 
    /// /Johan (& Jonathan)
    /// </summary>
    private void Move()
    {
        Vector3 forward = mainCameraTransform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
      
        Vector3 moveDirection = (horizontal * right + vertical * forward);
        moveDirection.Normalize();
        if(moveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);
        }


        Vector3 input = new Vector3(horizontal, 0, vertical);

        if (input != Vector3.zero)
        {
            animator.enabled = true;
        }
        else if (input.sqrMagnitude == 0)
        {
            animator.enabled = false;
        }


        Vector3 velocity = moveDirection * speed;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
    void OnCollisionEnter(Collision other)
	{
		if (other.relativeVelocity.magnitude > outValue)
		{
			StartCoroutine(Out());
		}
		if (other.gameObject.CompareTag("Terrain"))
		{
			isGround = true;
		}
	}

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	IEnumerator Out()
	{
		rigidbody.constraints = RigidbodyConstraints.None;
		capsuleCollider.enabled = false;
		alive = false;
		yield return new WaitForSeconds(4);
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		capsuleCollider.enabled = true;
		alive = true;
	}
}
