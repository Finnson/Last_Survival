using System.Collections;
using UnityEngine;  //第一人称控制需要rigidbody和碰撞器         

[RequireComponent(typeof(Rigidbody))]   
[RequireComponent(typeof(CapsuleCollider))]   
public class VRPlayerController : MonoBehaviour 
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

	public MoveSetting moveSet;

	public VRMovieController movieController;

	//当前速度       
	public float currentSpeed;       

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

	public VRInput vrinput;
	public Vector3 desireMove;

	// Use this for initialization       
	void Start () {           
		m_capsule = GetComponent<CapsuleCollider>();           
		m_rigidbody = GetComponent<Rigidbody>();                     
		m_camera = Camera.main;      
		m_camTrans = m_camera.transform;     
		m_chaTrans = transform;          
		//初始化参数           
		m_chaQutation = m_chaTrans.rotation;
	}           

	// Update is called once per frame     
	void Update () {          
		m_chaQutation *= Quaternion.Euler(0f, 0f, 0f);          
		m_chaTrans.localRotation = m_chaQutation;

		m_chaTrans.localPosition = new Vector3 (m_chaTrans.localPosition.x, 0, m_chaTrans.localPosition.z);
		m_rigidbody.velocity = Vector3.zero;
	}        

	//物理的moving，需要放到FixedUpdate中，固定zhen率0.02秒，可在Edit.time中修改       
	void FixedUpdate()   
	{ 
		if (movieController.IsPlayEnd()) {
			DoMove ();    
		}
	}             

	void DoMove()  
	{    
		//check地面    
		//CheckGround();       

		//get input for moving   
		Vector2 input = vrinput.leftTouchpadAxis;
		//更新当前力的大小       
		CaculateSpeed(input);             
		//判断是否有moving的速度，没有就不施加力    
		if ((Mathf.Abs (input.x) > float.Epsilon || Mathf.Abs (input.y) > float.Epsilon)) {          
			//calculate方向力    
			desireMove = m_camTrans.forward * input.y + m_camTrans.right * input.x;           
			//力在地面投影的向量       
			desireMove = desireMove.normalized;     
			desireMove.x = desireMove.x * currentSpeed;         
			desireMove.y = 0;          
			desireMove.z = desireMove.z * currentSpeed;   

			//当前速度不能大于max速度（Magnitude方法，需要开平方根，使用sqr减少运算）   
			//currentSpeed是max速度      //rigidbody施加（calculate坡度后）的力
			//if (m_rigidbody.velocity.sqrMagnitude < currentSpeed * currentSpeed) {         
			//m_rigidbody.AddForce (desireMove * SlopeValue (), ForceMode.Impulse);    
			//	m_rigidbody.AddForce (desireMove, ForceMode.Impulse);//don't use SlopeValue
			//}

			//change position
			transform.position += desireMove * Time.deltaTime;
		} else {
			desireMove = Vector3.zero;
		}               
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
}