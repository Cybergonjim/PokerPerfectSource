//using UnityEditor;
//using UnityEditor.SearchService;
using UnityEngine;
//using static Unity.Burst.Intrinsics.Arm;
//using static UnityEngine.Rendering.GPUSort;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CreateMaterials : MonoBehaviour
{
  public Texture[] textures; // Array to hold your textures

  void Start()
  {

    // Loop through each texture and create a material for it
//    foreach (Texture tex in textures)
//    {
//      Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

//      newMaterial.SetTexture("_BaseMap", tex);

//      // Assign the texture to the material's main texture slot
////      newMaterial.mainTexture = tex;

//      newMaterial.SetFloat("_Mode", 2);

//      string path = "Assets/Cards/Materials/Faces/Materials/" + tex.name + ".mat";
//      AssetDatabase.CreateAsset(newMaterial, path);
//    }
  }
}
