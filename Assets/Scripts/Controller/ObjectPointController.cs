using System.Collections.Generic;
using System.Linq;
using SerializableHelper;
using UnityEngine;

public class ObjectPointController : MonoBehaviour {
    public FrameController frame;

    public ObjectPointData opoint;

    public bool opointOneTimePerFrame;

    public int currentFrameId;

    public Map<string, Queue<ObjectPointCache>> opoints = new Map<string, Queue<ObjectPointCache>>();
    public GameObject gameObjectOpoint;

    public int normalHitId;
    public GameObject normalHit;

    public int swordHitId;
    public GameObject swordHit;

    void Start() {
        this.opointOneTimePerFrame = true;
        this.currentFrameId = -1;

        var framesWithOpoints = this.frame.data.frames
        .Where(frame => frame.Value.opoint != null && frame.Value.opoint.HasValue())
        .ToList();

        foreach (KeyValuePair<int, FrameData> frame in framesWithOpoints) {
            var prefab = Resources.Load<GameObject>(frame.Value.opoint.object_id);
            CacheOpoints(prefab, frame.Value.opoint);
        }

        CacheOpoints(normalHit);
        CacheOpoints(swordHit);
    }

    void CacheOpoints(GameObject prefab, ObjectPointData opoint = null) {
        if (prefab == null) {
            return;
        }

        if (opoint != null) {
            opoint.quantity = opoint.quantity == null || opoint.quantity <= 0 ? 0 : opoint.quantity;
            opoint.quantity = opoint.quantity > 5 ? 5 : opoint.quantity;

            float x = transform.position.x + PositionUtil.FacingByX(opoint.x, frame.facingRight);
            float y = transform.position.y + opoint.y;
            float z = transform.position.z + opoint.z;

            switch (opoint.quantity) {
                case 1:
                    InstantiateCache(prefab, new Vector3(x, y, z));
                    break;
                case 2:
                    InstantiateCache(prefab, new Vector3(x, y, z + opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z - opoint.z_division_per_quantity));
                    break;
                case 3:
                    InstantiateCache(prefab, new Vector3(x, y, z));
                    InstantiateCache(prefab, new Vector3(x, y, z + opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z - opoint.z_division_per_quantity));
                    break;
                case 4:
                    InstantiateCache(prefab, new Vector3(x, y, z + opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z - opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z + (opoint.z_division_per_quantity * 2)));
                    InstantiateCache(prefab, new Vector3(x, y, z - (opoint.z_division_per_quantity * 2)));
                    break;
                case 5:
                    InstantiateCache(prefab, new Vector3(x, y, z));
                    InstantiateCache(prefab, new Vector3(x, y, z + opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z - opoint.z_division_per_quantity));
                    InstantiateCache(prefab, new Vector3(x, y, z + (opoint.z_division_per_quantity * 2)));
                    InstantiateCache(prefab, new Vector3(x, y, z - (opoint.z_division_per_quantity * 2)));
                    break;
            }
        } else {
            InstantiateCache(prefab, transform.position);
        }
    }

    private void InstantiateCache(GameObject prefab, Vector3 position) {
        var opointInstantiate = Instantiate<GameObject>(prefab, position, Quaternion.identity);
        var opointData = opointInstantiate.GetComponent<AbstractDataController>();
        var opointPhysics = opointInstantiate.GetComponent<PhysicController>();
        var opointFrame = opointPhysics.frame;

        opointInstantiate.transform.parent = gameObjectOpoint.transform;
        opointInstantiate.SetActive(false);
        var prefabCacheName = opointData.assetPath;

        var cache = new ObjectPointCache();
        cache.gameObject = opointInstantiate;
        cache.frameController = opointFrame;
        cache.frameController.ownerOpointController = this;
        cache.physicController = opointPhysics;
        cache.key = prefabCacheName;
        cache.originalPosition = opointInstantiate.transform.localPosition;

        var queueObjectPointCache = new Queue<ObjectPointCache>();
        queueObjectPointCache.Enqueue(cache);

        opoints.Add(prefabCacheName, queueObjectPointCache);

        var invoke_limit = opointInstantiate.GetComponent<AbstractDataController>().header.invoke_limit;

        for (int i = 1; invoke_limit > i; i++) {
            var opointInstantiateExtend = Instantiate<GameObject>(prefab, position, Quaternion.identity);
            var opointPhysicsExtend = opointInstantiateExtend.GetComponent<PhysicController>();
            var opointFrameExtend = opointPhysicsExtend.frame;

            opointInstantiateExtend.transform.parent = gameObjectOpoint.transform;
            opointInstantiateExtend.SetActive(false);

            var cacheExtend = new ObjectPointCache();
            cacheExtend.gameObject = opointInstantiateExtend;
            cacheExtend.frameController = opointFrameExtend;
            cacheExtend.frameController.ownerOpointController = this;
            cacheExtend.physicController = opointPhysicsExtend;
            cacheExtend.key = prefabCacheName;
            cacheExtend.originalPosition = opointInstantiate.transform.localPosition;

            queueObjectPointCache.Enqueue(cacheExtend);
        }
    }

    void Update() {
        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.opointOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;
        }

        if (this.opointOneTimePerFrame) {
            if (frame.currentFrame.opoint != null) {
                opoint = frame.currentFrame.opoint;
                return;
            }

            if (opoint != null && opoint.HasValue()) {
                Instantiate(opoint);
                this.opointOneTimePerFrame = false;
                this.opoint = null;
            }
        }
    }

    void Instantiate(ObjectPointData opoint) {
        opoint.quantity = opoint.quantity == null || opoint.quantity <= 0 ? 0 : opoint.quantity;
        opoint.quantity = opoint.quantity > 5 ? 5 : opoint.quantity;

        switch (opoint.quantity) {
            case 1:
                Instantiate(opoint, 0f);
                break;
            case 2:
                Instantiate(opoint, +opoint.z_division_per_quantity);
                Instantiate(opoint, -opoint.z_division_per_quantity);
                break;
            case 3:
                Instantiate(opoint, 0f);
                Instantiate(opoint, +opoint.z_division_per_quantity);
                Instantiate(opoint, -opoint.z_division_per_quantity);
                break;
            case 4:
                Instantiate(opoint, +opoint.z_division_per_quantity);
                Instantiate(opoint, -opoint.z_division_per_quantity);
                Instantiate(opoint, +(opoint.z_division_per_quantity * 2));
                Instantiate(opoint, -(opoint.z_division_per_quantity * 2));
                break;
            case 5:
                Instantiate(opoint, 0f);
                Instantiate(opoint, +opoint.z_division_per_quantity);
                Instantiate(opoint, -opoint.z_division_per_quantity);
                Instantiate(opoint, +(opoint.z_division_per_quantity * 2));
                Instantiate(opoint, -(opoint.z_division_per_quantity * 2));
                break;
        }
    }

    void Instantiate(ObjectPointData opoint, float z_division_per_quantity) {
        var cacheOpoint = GetCacheOpoint(opoint.object_id);

        cacheOpoint.frameController.ResetValues(frame.facingRight);
        cacheOpoint.frameController.ownerId = this.frame.selfId;
        cacheOpoint.frameController.team = this.frame.team;
        cacheOpoint.frameController.objectPointCache = cacheOpoint;
        cacheOpoint.frameController.summonAction = opoint.action;

        cacheOpoint.physicController.ResetValues();
        cacheOpoint.physicController.externForce = new Vector3(opoint.dvx, opoint.dvy, opoint.dvz);
        cacheOpoint.physicController.isExternForce = true;
        cacheOpoint.physicController.enable_dvz_invocation = opoint.enable_dvz_invocation;
        if (cacheOpoint.physicController.enable_dvz_invocation) {
            cacheOpoint.physicController.lockInputDirection = this.frame.inputDirection;
        }

        cacheOpoint.gameObject.SetActive(true);
        cacheOpoint.gameObject.transform.parent = null;

        cacheOpoint.frameController.facingRight = frame.facingRight ? opoint.facing : !opoint.facing;
    }

    private ObjectPointCache GetCacheOpoint(string opointObjectId) {
        return this.opoints[opointObjectId].Dequeue();
    }

    public void InvokeNormalHit(Vector3 position) {
        InvokeHit(position, normalHit, normalHitId);
    }

    public void InvokeSwordHit(Vector3 position) {
        InvokeHit(position, swordHit, swordHitId);
    }

    void InvokeHit(Vector3 position, GameObject hit, int hitId) {
        var opointSpawn = Instantiate(hit, new Vector3(position.x, position.y, position.z), Quaternion.identity);
        var spawnFrame = opointSpawn.GetComponent<FrameController>();
        spawnFrame.summonAction = hitId;
    }
}
