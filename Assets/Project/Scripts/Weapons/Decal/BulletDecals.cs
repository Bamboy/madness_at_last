﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Edelweiss.DecalSystem;
using Excelsion.WeaponSystem;

//Created by: Nick Evans

public class BulletDecals : MonoBehaviour 
{
	//public static GunShooting gs;
	
	// The prefab which contains the DS_Decals script with already set material and
	// uv rectangles.
	public GameObject decalsPrefab;
	public Transform shootPos;
	
	// The reference to the instantiated prefab's DS_Decals instance.
	private DS_Decals m_Decals;
	private Matrix4x4 m_WorldToDecalsMatrix;
	
	// All the projectors that were created at runtime.
	private List <DecalProjector> m_DecalProjectors = new List <DecalProjector> ();
	
	// Intermediate mesh data. Mesh data is added to that one for a specific projector
	// in order to perform the cutting.
	private DecalsMesh m_DecalsMesh;
	private DecalsMeshCutter m_DecalsMeshCutter;
	
	// The raycast hits a collider at a certain position. This value indicated how far we need to
	// go back from that hit point along the ray of the raycast to place the new decal projector. Set
	// this value to 0.0f to see why this is needed.
	public float decalProjectorOffset = 0.5f;
	
	// The size of new decal projectors.
	public Vector3 decalProjectorScale = new Vector3 (0.2f, 2.0f, 0.2f);
	public float cullingAngle = 90.0f;
	public float meshOffset = 0.001f;
	
	// We iterate through all the defined uv rectangles. This one indices which index we are using at
	// the moment.
	private int m_UVRectangleIndex = 0;

	// Move on to the next uv rectangle index.
	private void NextUVRectangleIndex () {
		m_UVRectangleIndex = m_UVRectangleIndex + 1;
		if (m_UVRectangleIndex >= m_Decals.uvRectangles.Length) {
			m_UVRectangleIndex = 0;
		}
	}
	
	private void Start () {
		if( shootPos == null )
		{
			Debug.LogError("No shoot pos transform was given!", this);
			return;
		}
		// Instantiate the prefab and get its decals instance.
		GameObject l_Instance = Instantiate (decalsPrefab) as GameObject;
		m_Decals = l_Instance.GetComponentInChildren <DS_Decals> ();
		//gs = GameObject.Find("Inventory").GetComponent<GunShooting>();

		if (m_Decals == null) {
			Debug.LogError ("The 'decalsPrefab' does not contain a 'DS_Decals' instance!");
		} else {
			
			// Create the decals mesh (intermediate mesh data) for our decals instance.
			// Further we need a decals mesh cutter instance and the world to decals matrix.
			m_DecalsMesh = new DecalsMesh (m_Decals);
			m_DecalsMeshCutter = new DecalsMeshCutter ();
			m_WorldToDecalsMatrix = m_Decals.CachedTransform.worldToLocalMatrix;
		}
	}

	public void CalculateVectors( out Vector3 upDir, out Vector3 forwardDir, RaycastHit rayData, Ray ray )
	{
		upDir = - rayData.normal;

		Vector3 linePoint;
		Vector3 lineVec; //Directional vector
		//Plane1 is the plane from which the shot is fired.
		//Plane2 is the plane which the decal gets applied to.
		//					         								   //Plane1 Normal   //Plane1 Position	//Plane2 Normal //Plane2 Position
		VectorExtras.PlanePlaneIntersection( out linePoint, out lineVec, shootPos.right, shootPos.position, rayData.normal, rayData.point );

		forwardDir = lineVec;
	}

	public void OnGunRaycastFired( BulletInfo[] bullets )
	{
		for(int i = 0; i < bullets.Length; i++){
				Ray l_Ray = bullets[i].ray;
				RaycastHit l_RaycastHit = bullets[i].data;
			    //Vector3 l_ProjectorPosition = l_RaycastHit.point - (decalProjectorOffset * l_Ray.direction.normalized);
				Vector3 l_ProjectorPosition = l_RaycastHit.point - (decalProjectorOffset * l_RaycastHit.normal);
				//Vector3 l_ForwardDirection = Camera.main.transform.up;
				//Vector3 l_UpDirection = - Camera.main.transform.forward;


				Vector3 l_UpDirection;
				Vector3 l_ForwardDirection;
				CalculateVectors( out l_UpDirection, out l_ForwardDirection, l_RaycastHit, l_Ray );
				
//				Debug.Log ("Normal: "+ l_UpDirection +" Forward: "+ l_ForwardDirection);
				Quaternion l_ProjectorRotation = Quaternion.LookRotation(l_ForwardDirection, l_UpDirection);
				
				// Randomize the rotation.
				Quaternion l_RandomRotation = Quaternion.Euler (0.0f, Random.Range(0.0f, 360.0f), 0.0f);
				l_ProjectorRotation = l_ProjectorRotation * l_RandomRotation;
				
				TerrainCollider l_TerrainCollider = l_RaycastHit.collider as TerrainCollider;
				if (l_TerrainCollider != null) {
					
					// Terrain collider hit.
					
					Terrain l_Terrain = l_TerrainCollider.GetComponent <Terrain> ();
					if (l_Terrain != null) {
						
						// Create the decal projector with all the required information.
						DecalProjector l_DecalProjector = new DecalProjector (l_ProjectorPosition, l_ProjectorRotation, decalProjectorScale, cullingAngle, meshOffset, m_UVRectangleIndex, m_UVRectangleIndex);
						
						// Add the projector to our list and the decals mesh, such that both are
						// synchronized. All the mesh data that is now added to the decals mesh
						// will belong to this projector.
						m_DecalProjectors.Add (l_DecalProjector);
						m_DecalsMesh.AddProjector (l_DecalProjector);
						
						// The terrain data has to be converted to the decals instance's space.
						Matrix4x4 l_TerrainToDecalsMatrix = Matrix4x4.TRS (l_Terrain.transform.position, Quaternion.identity, Vector3.one) * m_WorldToDecalsMatrix;
						
						// Pass the terrain data with the corresponding conversion to the decals mesh.
						m_DecalsMesh.Add (l_Terrain, l_TerrainToDecalsMatrix);
						
						// Cut the data in the decals mesh accoring to the size and position of the decal projector. Offset the
						// vertices afterwards and pass the newly computed mesh to the decals instance, such that it becomes
						// visible.
						m_DecalsMeshCutter.CutDecalsPlanes (m_DecalsMesh);
						m_DecalsMesh.OffsetActiveProjectorVertices ();
						m_Decals.UpdateDecalsMeshes (m_DecalsMesh);
						
						// For the next hit, use a new uv rectangle. Usually, you would select the uv rectangle
						// based on the surface you have hit.
						NextUVRectangleIndex ();
					} else {
						Debug.Log ("Terrain is null!");
					}
					
				} else {
					
					// We hit a collider. Next we have to find the mesh that belongs to the collider.
					// That step depends on how you set up your mesh filters and collider relative to
					// each other in the game objects. It is important to have a consistent way in order
					// to have a simpler implementation.
					
					MeshCollider l_MeshCollider = l_RaycastHit.collider.GetComponent <MeshCollider> ();
					MeshFilter l_MeshFilter = l_RaycastHit.collider.GetComponent <MeshFilter> ();
					
					if (l_MeshCollider != null || l_MeshFilter != null) {
						Mesh l_Mesh = null;
						if (l_MeshCollider != null) {
							
							// Mesh collider was hit. Just use the mesh data from that one.
							l_Mesh = l_MeshCollider.sharedMesh;
						} else if (l_MeshFilter != null) {
							
							// Otherwise take the data from the shared mesh.
							l_Mesh = l_MeshFilter.sharedMesh;
						}
						
						if (l_Mesh != null) {
							
							// Create the decal projector.
							DecalProjector l_DecalProjector = new DecalProjector (l_ProjectorPosition, l_ProjectorRotation, decalProjectorScale, cullingAngle, meshOffset, m_UVRectangleIndex, m_UVRectangleIndex);
							
							// Add the projector to our list and the decals mesh, such that both are
							// synchronized. All the mesh data that is now added to the decals mesh
							// will belong to this projector.
							m_DecalProjectors.Add (l_DecalProjector);
							m_DecalsMesh.AddProjector (l_DecalProjector);
							
							// Get the required matrices.
							Matrix4x4 l_WorldToMeshMatrix = l_RaycastHit.collider.renderer.transform.worldToLocalMatrix;
							Matrix4x4 l_MeshToWorldMatrix = l_RaycastHit.collider.renderer.transform.localToWorldMatrix;
							 
							// Add the mesh data to the decals mesh, cut and offset it before we pass it
							// to the decals instance to be displayed.
							m_DecalsMesh.Add (l_Mesh, l_WorldToMeshMatrix, l_MeshToWorldMatrix);						
							m_DecalsMeshCutter.CutDecalsPlanes (m_DecalsMesh);
							m_DecalsMesh.OffsetActiveProjectorVertices ();
							m_Decals.UpdateDecalsMeshes (m_DecalsMesh);
							
							// For the next hit, use a new uv rectangle. Usually, you would select the uv rectangle
							// based on the surface you have hit.
							NextUVRectangleIndex ();
					}
				}
			}
		}
	}
}
