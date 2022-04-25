using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_SetInteractiveShaderEffectsTriplanar : MonoBehaviour
{
    public Transform transformToFollow;
    public RenderTexture rt;
    public string GlobalTexName = "_GlobalEffectRT";
    public string GlobalOrthoName = "_OrthographicCamSize";
    public Transform player;
    public ParticleSystem pSPlayer;
    public bool mirrorX = false;
    public bool mirrorY = false;

    private float orthoMem = 0;
    private BF_BallPlayerTriplanar ballPlayer;
    private ParticleSystem psX;
    private ParticleSystem psY;
    private Camera cam;

    private void Awake()
    {
        orthoMem = GetComponent<Camera>().orthographicSize;
        Shader.SetGlobalFloat(GlobalOrthoName, orthoMem);
        Shader.SetGlobalTexture(GlobalTexName, rt);
        Shader.SetGlobalFloat("_HasRT", 1);
        ballPlayer = player.GetComponent<BF_BallPlayerTriplanar>();
        cam = this.GetComponent<Camera>();
    }
    private void Update()
    {
        if (transformToFollow != null)
        {
            transform.position = new Vector3(transformToFollow.position.x, transformToFollow.position.y + 20, transformToFollow.position.z);
        }
        Shader.SetGlobalVector("_Position", transform.position);
        // transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        RenderTriplanar();
        MirrorPS();
    }

    private void RenderTriplanar()
    {
        int layerMask = 1 << 0;
        RaycastHit hit;

        if(Physics.Raycast(player.position, ballPlayer.gravityCustom, out hit,5, layerMask))
        {
            Debug.DrawRay(player.position, ballPlayer.gravityCustom);

            Vector2 pixelUV = hit.textureCoord;
            Vector3 ParticleWorldPosition = cam.ViewportToWorldPoint(new Vector3(pixelUV.x, pixelUV.y, 1));
            pSPlayer.transform.position = ParticleWorldPosition;
        }
    }

    private void MirrorPS()
    {
        if(psX == null && mirrorX)
        {
            psX = Instantiate(pSPlayer);
        }
        else if(psX != null && !mirrorX)
        {
            Destroy(psX);
        }
        if(psY == null && mirrorY)
        {
            psY = Instantiate(pSPlayer);
        }
        else if (psY != null && !mirrorY)
        {
            Destroy(psY);
        }

        if(psX != null)
        {
            if (pSPlayer.transform.position.x < cam.transform.position.x)
            {
                psX.transform.position = pSPlayer.transform.position + (Vector3.right)* cam.orthographicSize*2;
            }
            else
            {
                psX.transform.position = pSPlayer.transform.position - (Vector3.right) * cam.orthographicSize * 2;
            }
        }
        if(psY != null)
        {
            if (pSPlayer.transform.position.y < cam.transform.position.y)
            {
                psY.transform.position = pSPlayer.transform.position + (Vector3.up)* cam.orthographicSize * 2;
            }
            else
            {
                psY.transform.position = pSPlayer.transform.position - (Vector3.up) * cam.orthographicSize * 2;
            }
        }
    }
}