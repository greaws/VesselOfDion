using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextReveal : MonoBehaviour
{
    private TMP_Text textMeshPro;
    public float revealTime, shakeIntensity, shakeInterval;
    public string[] gameOverMessage;
    private int currentText = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.enabled = false;
    }

    private void OnEnable()
    {
        textMeshPro = GetComponent<TMP_Text>();
        textMeshPro.enabled = true;
        textMeshPro.text = gameOverMessage[currentText];
        currentText = (currentText + 1) % gameOverMessage.Length;
        StartCoroutine(Reveal());        
    }

    public void Hide()
    {
        textMeshPro.enabled = false;
    }

    public IEnumerator Reveal()
    {
        float t = 0;
        while (t < 1)
        {
            textMeshPro.maxVisibleCharacters = (int)Mathf.Lerp(0, textMeshPro.text.Length, t);
            t += Time.deltaTime / revealTime;
            yield return null;
        }
        textMeshPro.maxVisibleCharacters = textMeshPro.text.Length;
        //StartCoroutine(AnimateShake(textMeshPro));
    }

    private IEnumerator AnimateShake(TMP_Text text)
    {
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

                Vector3 shakeOffset = UnityEngine.Random.insideUnitCircle * shakeIntensity / 16;
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