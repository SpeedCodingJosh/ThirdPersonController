using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour 
{
	private Animator anim;	
	
	private void Start () 
	{
		anim = GetComponent<Animator>();
	}

	public void SetValue (string key, object value)
	{
		if(value == null) // TRIGGER
		{
			anim.SetTrigger(key);
		}
		else if(value.GetType() == typeof(float)) // FLOAT
		{
			anim.SetFloat(key, (float)value);
		}
		else if(value.GetType() == typeof(int)) // INT
		{
			anim.SetInteger(key, (int)value);
		}
		else if(value.GetType() == typeof(bool)) // BOOL
		{
			anim.SetBool(key, (bool)value);
		}
	}

	public void ActivateRootMotion (bool isActive)
	{
		anim.applyRootMotion = isActive;
	}
}
