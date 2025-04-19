using System;
using UnityEngine;

namespace UF {

    public class ModuleCamera {

        public Camera cam;

        public Vector2 mapMinBounds; // 地图的最小边界（左下角）
        public Vector2 mapMaxBounds; // 地图的最大边界（右上角）

        // public float smoothSpeed = 0.125f; // 相机跟随的平滑速度
        public Vector3 offset; // 相机与目标的偏移量

        float halfCameraWidth;//相机视口的一半宽度 
        float halfCameraHeight;//相机视口的一半高度 

        public ModuleCamera() {
            offset = new Vector3(0, 0, -10);

            mapMinBounds = new Vector2(-30, -16.875f);
            mapMaxBounds = new Vector2(30, 16.875f);
        }

        public void ResetCamera() {
            cam.transform.position = new Vector3(0, 0, -10);
        }

        public void Inject(Camera camera) {
            this.cam = camera;

            // 计算相机视口的一半宽度和高度
            halfCameraHeight = cam.orthographicSize;
            halfCameraWidth = halfCameraHeight * cam.aspect;
        }

        public void Follow(Transform target) {
            if (target == null)
                return;

            // 计算目标位置
            Vector3 desiredPosition = target.position + offset;

            // 场景大小(相机范围) = (屏幕宽*1.5) * (屏幕高*1.5)
            // 限制相机位置在地图边界内
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, mapMinBounds.x + halfCameraWidth, mapMaxBounds.x - halfCameraWidth);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, mapMinBounds.y + halfCameraHeight, mapMaxBounds.y - halfCameraHeight);

            // 平滑移动相机
            // Vector3 smoothedPosition = Vector3.Lerp(cam.transform.position, desiredPosition, smoothSpeed);
            cam.transform.position = desiredPosition;
        }

    }
}
