using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPC), true)]
public class NPCEditor : Editor
{
	NPC npc;

	private void OnEnable()
	{
		npc = target as NPC;
	}

	private void OnSceneGUI()
	{
		DrawNPCGUI();
	}

	private void DrawNPCGUI()
	{
		if(npc.reference == null)
		{
			return;
		}

		Vector3 dirAngleA = npc.DirFromAngle(-npc.normalVisionAngle / 2, false);
		Vector3 dirAngleB = npc.DirFromAngle(npc.normalVisionAngle / 2, false);
		Handles.color = Color.white;
		Handles.DrawWireArc(npc.transform.position, Vector3.up, Vector3.forward, 360, npc.wideDistanceVisionRadius);
		Handles.color = Color.red - new Color(0f, 0f, 0f, 0.6f);
		Handles.DrawSolidArc(npc.transform.position, Vector3.up, dirAngleB, 360 - npc.normalVisionAngle, npc.perifericVisionRadius);
		Handles.DrawSolidArc(npc.transform.position, Vector3.up, dirAngleA, npc.normalVisionAngle, npc.normalVisionRadius);
	}

}
