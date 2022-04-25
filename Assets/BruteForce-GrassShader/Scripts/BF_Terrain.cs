using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BF_Terrain : MonoBehaviour
{
    public Terrain terrainToCopy;

    private Terrain terrainAsset;
    private TerrainData terrainData;
    private TerrainData terrainDataOld = null;
    private Vector3 sizeOld = Vector3.one * 100f;
    private float[,] terrainHeightOld = null;
    private int heightRezOld = 0;
    private Vector3 posOld = Vector3.zero;
    private Material grassTerrainMaterial;
    private GameObject selectGO;
    private bool isSynced = false;

    void Start()
    {
        UpdateTerrainData();
    }
    void OnEnable()
    {
        UpdateTerrainData();
    }

    private void StoreTerrainData()
    {
        if (terrainDataOld == null)
        {
            terrainDataOld = terrainAsset.terrainData;
            terrainHeightOld = terrainAsset.terrainData.GetHeights(0, 0, terrainAsset.terrainData.heightmapResolution, terrainAsset.terrainData.heightmapResolution);
            sizeOld = terrainAsset.terrainData.size;
            heightRezOld = terrainAsset.terrainData.heightmapResolution;
            posOld = terrainAsset.transform.position;
        }
    }
    private void ClearTerrainData()
    {
        terrainDataOld = null;
        terrainHeightOld = null;
        isSynced = false;
        sizeOld = Vector3.one*100f;
    }

    private void UpdateTerrainData()
    {
        terrainAsset = this.GetComponent<Terrain>();
        terrainData = terrainAsset.terrainData;

        grassTerrainMaterial = terrainAsset.materialTemplate;
        grassTerrainMaterial.SetTexture("_Control0", terrainData.GetAlphamapTexture(0));
        grassTerrainMaterial.SetTexture("_Control1", terrainData.GetAlphamapTexture(1));

        if (terrainData.terrainLayers.Length > 4)
        {
            grassTerrainMaterial.SetTexture("_Normal4", terrainData.terrainLayers[4].normalMapTexture);
            grassTerrainMaterial.SetTexture("_Splat4", terrainData.terrainLayers[4].diffuseTexture);
            grassTerrainMaterial.SetVector("_Splat4_STn", new Vector4(terrainData.terrainLayers[4].tileOffset.x, terrainData.terrainLayers[4].tileOffset.y, terrainData.terrainLayers[4].tileSize.x, terrainData.terrainLayers[4].tileSize.y));
            grassTerrainMaterial.SetColor("_Specular4", terrainData.terrainLayers[4].specular);
            grassTerrainMaterial.SetFloat("_Metallic4", terrainData.terrainLayers[4].metallic);
        }
        if (terrainData.terrainLayers.Length > 5)
        {
            grassTerrainMaterial.SetTexture("_Normal5", terrainData.terrainLayers[5].normalMapTexture);
            grassTerrainMaterial.SetTexture("_Splat5", terrainData.terrainLayers[5].diffuseTexture);
            grassTerrainMaterial.SetVector("_Splat5_STn", new Vector4(terrainData.terrainLayers[5].tileOffset.x, terrainData.terrainLayers[5].tileOffset.y, terrainData.terrainLayers[5].tileSize.x, terrainData.terrainLayers[5].tileSize.y));
            grassTerrainMaterial.SetColor("_Specular5", terrainData.terrainLayers[5].specular);
            grassTerrainMaterial.SetFloat("_Metallic5", terrainData.terrainLayers[5].metallic);
        }
        if (terrainData.terrainLayers.Length > 6)
        {
            grassTerrainMaterial.SetTexture("_Normal6", terrainData.terrainLayers[6].normalMapTexture);
            grassTerrainMaterial.SetTexture("_Splat6", terrainData.terrainLayers[6].diffuseTexture);
            grassTerrainMaterial.SetVector("_Splat6_STn", new Vector4(terrainData.terrainLayers[6].tileOffset.x, terrainData.terrainLayers[6].tileOffset.y, terrainData.terrainLayers[6].tileSize.x, terrainData.terrainLayers[6].tileSize.y));
            grassTerrainMaterial.SetColor("_Specular6", terrainData.terrainLayers[6].specular);
            grassTerrainMaterial.SetFloat("_Metallic6", terrainData.terrainLayers[6].metallic);
        }
        if (terrainData.terrainLayers.Length > 7)
        {
            grassTerrainMaterial.SetTexture("_Normal7", terrainData.terrainLayers[7].normalMapTexture);
            grassTerrainMaterial.SetTexture("_Splat7", terrainData.terrainLayers[7].diffuseTexture);
            grassTerrainMaterial.SetVector("_Splat7_STn", new Vector4(terrainData.terrainLayers[7].tileOffset.x, terrainData.terrainLayers[7].tileOffset.y, terrainData.terrainLayers[7].tileSize.x, terrainData.terrainLayers[7].tileSize.y));
            grassTerrainMaterial.SetColor("_Specular7", terrainData.terrainLayers[7].specular);
            grassTerrainMaterial.SetFloat("_Metallic7", terrainData.terrainLayers[7].metallic);
        }
    }

    public void CopyTerrainData()
    {
        if(terrainToCopy != null && terrainToCopy != terrainAsset)
        {
            StoreTerrainData();

            terrainAsset.terrainData.heightmapResolution = terrainToCopy.terrainData.heightmapResolution;
            terrainAsset.terrainData.SetHeights(0, 0, terrainToCopy.terrainData.GetHeights(0, 0, terrainToCopy.terrainData.heightmapResolution, terrainToCopy.terrainData.heightmapResolution));
            terrainAsset.terrainData.size = terrainToCopy.terrainData.size;
        }
    }

    public void MoveTerrainSync()
    {
        if (terrainToCopy != null && terrainToCopy != terrainAsset)
        {
            isSynced = true;
            terrainAsset.transform.position = terrainToCopy.transform.position + Vector3.up * 0.01f;
            grassTerrainMaterial.SetFloat("_GrassCut", 1);

            //terrainAsset.terrainData.heightmapResolution = terrainToCopy.terrainData.heightmapResolution;
            //terrainAsset.terrainData.baseMapResolution = terrainToCopy.terrainData.baseMapResolution;
            //terrainAsset.terrainData.alphamapResolution = terrainToCopy.terrainData.alphamapResolution;
        }
        else
        {
            grassTerrainMaterial.SetFloat("_GrassCut", 0);
        }
    }
    public void RevertTerrainData()
    {
        if (terrainDataOld != null)
        {
            terrainToCopy = null;
            terrainAsset.terrainData = terrainDataOld;
            terrainAsset.terrainData.heightmapResolution = heightRezOld;
            terrainAsset.terrainData.size = sizeOld;
            terrainAsset.terrainData.SetHeights(0, 0, terrainHeightOld);
            grassTerrainMaterial.SetFloat("_GrassCut", 0);
            terrainAsset.transform.position = posOld;
            ClearTerrainData();
        }
    }

    void OnRenderObject()
    {
#if UNITY_EDITOR
        if ((Application.isEditor && !Application.isPlaying)&& terrainToCopy!=null && isSynced)
        {
            if (Selection.activeGameObject == this.gameObject && selectGO != this.gameObject)
            {
                selectGO = Selection.activeGameObject;
                terrainAsset.transform.position = terrainToCopy.transform.position + Vector3.up * 0.01f;
            }
            else if (Selection.activeGameObject == terrainToCopy.gameObject && selectGO != terrainToCopy.gameObject)
            {
                selectGO = Selection.activeGameObject;
                terrainAsset.transform.position = terrainToCopy.transform.position + Vector3.down * 0.01f;
            }
        }
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {
            UpdateTerrainData();
            if (isSynced && terrainToCopy != null && terrainToCopy.terrainData.GetHeights(0, 0, terrainToCopy.terrainData.heightmapResolution, terrainToCopy.terrainData.heightmapResolution) != terrainAsset.terrainData.GetHeights(0, 0, terrainAsset.terrainData.heightmapResolution, terrainAsset.terrainData.heightmapResolution))
            {
                CopyTerrainData();
            }
        }
#endif
    }

}
