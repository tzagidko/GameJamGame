using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class SingleShotGun : Gun
{
    [SerializeField] private Camera cam;
    private PhotonView PV;
    [SerializeField] private GameObject bubblesPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void Use()
   {
     Shoot();
   }

   void Shoot()
   {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
       ray.origin = cam.transform.position;
       if (Physics.Raycast(ray, out RaycastHit hit))
       {
          hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
           PV.RPC("RPC_Shoot",RpcTarget.All,hit.point, hit.normal);
           
       }
   }

   [PunRPC] void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
   {
       Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
       if (colliders.Length != 0)
       {
           GameObject bulletImpactObj=Instantiate(bulletImpactPrefab, hitPosition, Quaternion.LookRotation(hitNormal,Vector3.up)*bulletImpactPrefab.transform.rotation);
           GameObject bubblesObj = Instantiate(bubblesPrefab, ItemGameObject.transform.position, ItemGameObject.transform.rotation);
           bubblesObj.transform.SetParent(ItemGameObject.transform);
           Destroy(bulletImpactObj,10f);
           Destroy(bubblesObj,1.2f);
           bulletImpactObj.transform.SetParent(colliders[0].transform);
       }
       
   }
}
