using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyBox;

[ExecuteInEditMode]
public class UseLineRenderer : MonoBehaviour {

	public static bool brokenChainsHolderCreated = false;	//used to save chains which aren't connected to another chain

	//used to set linerenderer's material and width
	private LineRenderer lineRend;
	public Material ropeMaterial;
	public float width = 0.0f;
	
	private List<Transform> chains;			//used to save chains
	private GameObject brokenChainsHolder;	//used to save "Broken Chains Holder" object
	private Transform tr;					//used to save rope's transform for future use to iterate through its children

	//used to initialize rope
	private bool started = false;
	[ReadOnly] [SerializeField] private float startChainCount;


	// Use this for initialization
	void Start ()
	{
		tr = transform; //get transform

		//if chains list is empty fill it
		if(chains == null)
		{
			chains = new List<Transform>();

			foreach(Transform child in tr)
				chains.Add (child);
		}

		//if LineRenderer component isn't added, add it
		if(chains.Count > 0 && !GetComponent<LineRenderer>())
		{
			lineRend = gameObject.AddComponent<LineRenderer>();
			lineRend.positionCount = chains.Count;
			lineRend.material = ropeMaterial;
		}
		else lineRend = GetComponent<LineRenderer>();

		//set linerenderer's width
		if(lineRend)
		{
			if(width <= 0.0f)
			{
				width = chains[0].GetComponent<Renderer>().bounds.size.x;
			}

			lineRend.startWidth = width;
			lineRend.endWidth = width;
		}

		startChainCount = chains.Count;
		
		started = true;
	}


	//add chain into chains list
	public void AddChain(Transform chain)
	{
		//if chains list isn't created, create it
		if(chains == null)
			chains = new List<Transform>();

		//if linerenderer isn't added, add it
		if(lineRend == null)
		{
			lineRend = GetComponent<LineRenderer>();

			if(!lineRend)
			{
				lineRend = gameObject.AddComponent<LineRenderer>();
				lineRend.material = ropeMaterial;
			}
		}

		chains.Add (chain);	//add chain into chains list

		//fill LineRenderer component's positions
		lineRend.positionCount = chains.Count;
		lineRend.SetPosition (chains.Count - 1, chains[chains.Count - 1].position);

		startChainCount = chains.Count;
	}


	// Update is called once per frame
	void Update ()
	{
		//if rope hasn't any child object, destroy it
		if(tr.childCount < 1)
			Destroy (gameObject);
		else if(chains != null && chains.Count > 0)
		{
			//update linerenderer component's positions
			for(int i = 0; i < chains.Count; i++)
				lineRend.SetPosition (i, chains[i].position);
		}
	}
}
