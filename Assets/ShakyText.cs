using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShakyText : MonoBehaviour
{
    private TextMeshPro textMeshPro;

    public Vector2 shakeIntensity;
    public float shakeInterval;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();        
    }


    private void OnEnable()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        StartCoroutine(AnimateShake(textMeshPro));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator AnimateShake(TMP_Text text)
    {
        Debug.Log("rf0");
        text.ForceMeshUpdate(); // Ensure the text mesh is updated
        TMP_TextInfo textInfo = text.textInfo;
        int characters = textInfo.meshInfo.Length;

        Vector3[][] originalVertices = new Vector3[characters][];
        for (int i = 0; i < characters; i++)
        {
            originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
            Array.Copy(textInfo.meshInfo[i].vertices, originalVertices[i], textInfo.meshInfo[i].vertices.Length);
        }


        while (true)
        {
            text.ForceMeshUpdate();
            textInfo = text.textInfo;
            int maxVisible = text.maxVisibleCharacters;


            for (int i = 0; i < textInfo.characterInfo.Length; i++)
            {
                // Skip characters beyond maxVisibleCharacters
                if (i >= maxVisible || !textInfo.characterInfo[i].isVisible)
                    continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                Vector3 shakeOffset = UnityEngine.Random.insideUnitCircle * shakeIntensity/16;
                vertices[vertexIndex + 0] = originalVertices[materialIndex][vertexIndex + 0] + shakeOffset;
                vertices[vertexIndex + 1] = originalVertices[materialIndex][vertexIndex + 1] + shakeOffset;
                vertices[vertexIndex + 2] = originalVertices[materialIndex][vertexIndex + 2] + shakeOffset;
                vertices[vertexIndex + 3] = originalVertices[materialIndex][vertexIndex + 3] + shakeOffset;
            }

            // Update the mesh with modified vertices
            for (int i = 0; i < characters; i++)
            {
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                text.UpdateGeometry(meshInfo.mesh, i);
            }
            yield return new WaitForSeconds(shakeInterval);
        }
    }
}
