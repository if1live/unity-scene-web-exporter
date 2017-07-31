using UnityEngine;

namespace Assets.Kanau.Utils
{
    public class ComponentHelper
    {
        public static Mesh GetMesh(GameObject go) {
            Mesh mesh = null;
            if (go.GetComponent<Renderer>() is MeshRenderer) {
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                mesh = meshFilter.sharedMesh;
			} else if (go.GetComponent<SkinnedMeshRenderer>() != null) {
                mesh = go.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
			else
			{
				Debug.LogWarning("Unknown Mesh on object", go);
			}
            return mesh;
        }
    }
}
