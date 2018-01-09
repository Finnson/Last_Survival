using System.Collections;
using UnityEngine;  //第一人称控制需要rigidbody和碰撞器         

[RequireComponent(typeof(Rigidbody))]   
[RequireComponent(typeof(CapsuleCollider))]   
public class PlayerController : MonoBehaviour 
{          
	//把move相关的参数，独立出来       
	[System.Serializable]       
	public class MoveSetting       
	{           
		public float ForwarSpeed = 7/5f;           
		public float BackSpeed = 5/5f;          
		public float HorizonSpeed = 6/5f;              
		public float RunValue = 1.5f;           
		public float JumpForce = 25/5f;       
	}       

	//把rotation相关的独立出来       
	[System.Serializable]       
	public class MouseLook       
	{           
		public float XSensitive = 2f;           
		public float YSensitive = 2f;       
	}              

	public MoveSetting moveSet;       
	public MouseLook CameraSet;          

	//当前速度       
	public float currentSpeed;       

	//一段跳       
	private bool m_jump;       
	//二段跳       
	private bool m_jump2;  
	private int jumpTime = 0;

	//第一人称，胶囊碰撞       
	private CapsuleCollider m_capsule;  

	//第一人称，rigidbody     
	private Rigidbody m_rigidbody;         

	private Camera m_camera;       
	//相机的Transform（减少Update中transform的reference）       
	private Transform m_camTrans;       
	//主角的Transform      
	private Transform m_chaTrans;              
	//camera四元数       
	private Quaternion m_camQutation;       
	//character四元数       
	private Quaternion m_chaQutation;          

	//爬坡的speed curve 
	//public AnimationCurve SlopCurve;          
	//是否在地面上       
	private bool m_isOnGround;      
	//地面normal   
	private Vector3 curGroundNormal;  

	private bool lockCursor = true;

	// Use this for initialization       
	void Start () {           
		m_capsule = GetComponent<CapsuleCollider>();           
		m_rigidbody = GetComponent<Rigidbody>();                     
		m_camera = Camera.main;      
		m_camTrans = m_camera.transform;     
		m_chaTrans = transform;          
		//初始化参数        
		m_camQutation = m_camTrans.rotation;     
		m_chaQutation = m_chaTrans.rotation;

		Cursor.lockState = CursorLockMode.Locked;
	}           

	// Update is called once per frame     
	void Update () {       
		//update rotation angle        
		RotateView();        
		if (Input.GetKeyDown(KeyCode.Space))      
		{           
			m_jump = true;      
		}

		cursorLock ();//lock cursor
	}        

	//物理的moving，需要放到FixedUpdate中，固定zhen率0.02秒，可在Edit.time中修改       
	void FixedUpdate()   
	{        
		DoMove();    
	}      

	//update rotation angle   
	void RotateView()      
	{       
		float xRot = Input.GetAxis("Mouse Y") * CameraSet.YSensitive;    
		float yRot = Input.GetAxis("Mouse X") * CameraSet.XSensitive;           

		//四元数使用      
		m_camQutation *= Quaternion.Euler(-xRot, 0f, 0f);       
		//限制camera rotation angle在【-90，90】内           
		m_camQutation = ClampRotation(m_camQutation);     
		m_camTrans.localRotation = m_camQutation; 

		m_chaQutation *= Quaternion.Euler(0f, yRot, 0f);          
		m_chaTrans.localRotation = m_chaQutation;          
	}         

	void DoMove()  
	{    
		//check地面    
		CheckGround();       

		//get input for moving   
		Vector2 input = GetInput();       
		//更新当前力的大小       
		CaculateSpeed(input);             
		//判断是否有moving的速度，没有就不施加力    
		if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_isOnGround)    
		{          
			//calculate方向力    
			Vector3 desireMove = m_camTrans.forward * input.y + m_camTrans.right * input.x;           
			//力在地面投影的向量       
			desireMove = Vector3.ProjectOnPlane(desireMove, curGroundNormal).normalized;     
			desireMove.x = desireMove.x * currentSpeed;         
			desireMove.y = 0;          
			desireMove.z = desireMove.z * currentSpeed;   

			//当前速度不能大于max速度（Magnitude方法，需要开平方根，使用sqr减少运算）   
			//currentSpeed是max速度      //rigidbody施加（calculate坡度后）的力
			//if (m_rigidbody.velocity.sqrMagnitude < currentSpeed * currentSpeed) {         
				//m_rigidbody.AddForce (desireMove * SlopeValue (), ForceMode.Impulse);    
			//	m_rigidbody.AddForce (desireMove, ForceMode.Impulse);//don't use SlopeValue
			//}
			transform.position += desireMove*Time.deltaTime;
		}           

		if(m_isOnGround)    
		{             
			//m_rigidbody.drag = 5f;         
			jumpTime = 0;     
			//一段跳            
			if (m_jump)          
			{             
				JumpUp();       
			}       
		}           
		else          
		{              
			if(m_jump)               
			{                   
				jumpTime++;                   
				//二段跳                   
				if (jumpTime < 2)                   
				{                       
					JumpUp();                   
				}               
			}           
		}              
		m_jump = false;       
	}                 

	//jump方法       
	void JumpUp()       
	{           
		m_rigidbody.drag = 0f;           
		//把object的上下方向的速度先set 0           
		m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0f, m_rigidbody.velocity.z);    
		//Since I reset force here, so moving force is invalid, so we can't move while we are jumping
		m_rigidbody.AddForce(new Vector3(0, moveSet.JumpForce, 0), ForceMode.Impulse);       
	}          

	//get input for moving       
	Vector2 GetInput()       
	{           
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));                        
		return input;       
	}              

	//udpate速度       
	void CaculateSpeed(Vector2 _input)       
	{           
		currentSpeed = moveSet.ForwarSpeed;              
		//横向move           
		if(Mathf.Abs(_input.x)>float.Epsilon)           
		{               
			currentSpeed = moveSet.HorizonSpeed;          
		}           
		//前后move           
		else if (_input.y < 0)           
		{               
			currentSpeed = moveSet.BackSpeed;           
		}           
		//Shift跑加速           
		if (Input.GetKey(KeyCode.LeftShift))           
		{               
			currentSpeed *= moveSet.RunValue;           
		}           
	}          

	//爬坡参数       
	//float SlopeValue()       
	//{           
	//	float angle = Vector3.Angle(curGroundNormal,Vector3.up);              
	//	float value = SlopCurve.Evaluate(angle);           
	//	return value;       
	//}          

	//check地面      
	void CheckGround()       
	{           
		RaycastHit hit;              
		//球形碰撞check ground          
		if (Physics.SphereCast(m_capsule.transform.position,m_capsule.radius,Vector3.down,
			out hit,((m_capsule.height / 2 - m_capsule.radius)+0.01f)))           
		{              
			//use hit normal as ground normal              
			curGroundNormal = hit.normal;               
			m_isOnGround = true;           
		}           
		else           
		{               
			curGroundNormal = Vector3.up;               
			m_isOnGround = false;           
		}       
	}          

	void CheckBuffer()      
	{           
		RaycastHit hit;           
		float speed = m_rigidbody.velocity.y;           
		if (speed < 0)           
		{               
			if (Physics.SphereCast(m_capsule.transform.position, m_capsule.radius, Vector3.down, 
				out hit, ((m_capsule.height / 2 - m_capsule.radius) + 1f)))               
			{                   
				speed *= 0.5f;                   
				m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, speed, m_rigidbody.velocity.z);               
			}           
		}       
	}          

	//四元数俯角，仰角限制       
	Quaternion ClampRotation(Quaternion q)       
	{          
		//四元数的xyzw，除以同一个数
		q.x /= q.w;           
		q.y /= q.w;           
		q.z /= q.w;           
		q.w = 1;              
		/*给定一个欧拉旋转(X, Y, Z)（即分别绕x轴、y轴和z轴旋转X、Y、Z度），则对应的四元数为  
		 * x = sin(Y/2)sin(Z/2)cos(X/2)+cos(Y/2)cos(Z/2)sin(X/2)  
		 * y = sin(Y/2)cos(Z/2)cos(X/2)+cos(Y/2)sin(Z/2)sin(X/2)  z = cos(Y/2)sin(Z/2)cos(X/2)-sin(Y/2)cos(Z/2)sin(X/2)  
		 * w = cos(Y/2)cos(Z/2)cos(X/2)-sin(Y/2)sin(Z/2)sin(X/2)           
		*/                       

		//得到公式[欧拉角x=2*Aan(q.x)]           
		float angle = 2 * Mathf.Rad2Deg * Mathf.Atan(q.x);//Euler angle of x Axis           
		//限制速度           
		angle = Mathf.Clamp(angle,-90f,90f);           
		//反推出q的新四元数的x           
		q.x = Mathf.Tan(Mathf.Deg2Rad * (angle / 2));              
		return q;       
	}

	void cursorLock()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			lockCursor = false;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			lockCursor = true;
		}

		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		else if (!lockCursor)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}
}