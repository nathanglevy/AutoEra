using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
	[CreateAssetMenu]
	[CustomGridBrush(false, true, false, "Item Brush")]
	public class ItemBrush : GridBrushBase
	{
		private const float k_PerlinOffset = 100000f;
		public GameObject[] m_Prefabs;
		public float m_PerlinScale = 0.5f;
	    public int m_amount = 50;
		public int m_Z;

		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

		    Erase(grid, brushTarget, position);

            int index = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, m_PerlinScale, k_PerlinOffset)*m_Prefabs.Length), 0, m_Prefabs.Length - 1);
			GameObject prefab = m_Prefabs[index];
			GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            TextMesh textMesh = instance.GetComponentInChildren<TextMesh>();
		    textMesh.text = m_amount.ToString();
			Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
			if (instance != null)
			{
				instance.transform.SetParent(brushTarget.transform);
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, m_Z) + new Vector3(.5f, .5f, .5f)));
			}
		}

		public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, m_Z));
			if (erased != null)
				Undo.DestroyObjectImmediate(erased.gameObject);
		}

		private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
		{
			int childCount = parent.childCount;
			Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
			Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
			Bounds bounds = new Bounds((max + min)*.5f, max - min);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (bounds.Contains(child.position))
					return child;
			}
			return null;
		}

		private static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise((position.x + offset)*scale, (position.y + offset)*scale);
		}
	}

	[CustomEditor(typeof(ItemBrush))]
	public class ItemBrushEditor : GridBrushEditorBase
	{
		private ItemBrush prefabBrush { get { return target as ItemBrush; } }

		private SerializedProperty m_Prefabs;
		private SerializedObject m_SerializedObject;

		protected void OnEnable()
		{
			m_SerializedObject = new SerializedObject(target);
			m_Prefabs = m_SerializedObject.FindProperty("m_Prefabs");
		}

		public override void OnPaintInspectorGUI()
		{
			m_SerializedObject.UpdateIfRequiredOrScript();
			prefabBrush.m_PerlinScale = EditorGUILayout.Slider("Perlin Scale", prefabBrush.m_PerlinScale, 0.001f, 0.999f);
			prefabBrush.m_Z = EditorGUILayout.IntField("Position Z", prefabBrush.m_Z);
			prefabBrush.m_amount = (EditorGUILayout.IntSlider("Amount", prefabBrush.m_amount, 5,500)/5)*5;
				
			EditorGUILayout.PropertyField(m_Prefabs, true);
			m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}
