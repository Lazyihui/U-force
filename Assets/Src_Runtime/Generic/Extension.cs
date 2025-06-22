using System;
using UnityEngine;

namespace UF {

    public static class Extension {
        // 用于判断一个浮点数 (float) 是否接近于零
        public static bool IsAlmostZero(this float value, float epsilon) {
            return Mathf.Abs(value) <= epsilon;
        }

        // 得到两个正方形环的随机位置
        public static Vector2 GetSquareAnnulusRandomPos(Vector2 center, float outerSquareSize, float innerSquareSize) {
            if (innerSquareSize >= outerSquareSize) {
                Debug.LogError("小正方形的边长必须小于大正方形！");
                return Vector2.zero;
            }

            float halfOuter = outerSquareSize * 0.5f;
            float halfInner = innerSquareSize * 0.5f;

            // 随机选择一个区域（0~3 对应左、右、上、下）
            int zone = UnityEngine.Random.Range(0, 4);
            float x = 0f, y = 0f;

            // TODO:这里可以优化一下，因为四个角出现比较多
            // 左侧条带
            if (zone == 0) {
                x = UnityEngine.Random.Range(-halfOuter, -halfInner);
                y = UnityEngine.Random.Range(-halfOuter, halfOuter);
            }
            // 右侧条带
            else if (zone == 1) {
                x = UnityEngine.Random.Range(halfInner, halfOuter);
                y = UnityEngine.Random.Range(-halfOuter, halfOuter);
            }
            // 上部条带
            else if (zone == 2) {
                x = UnityEngine.Random.Range(-halfOuter, halfOuter);
                y = UnityEngine.Random.Range(halfInner, halfOuter);
            }
            // 下部条带
            else if (zone == 3) {
                x = UnityEngine.Random.Range(-halfOuter, halfOuter);
                y = UnityEngine.Random.Range(-halfOuter, -halfInner);
            }

            return new Vector2(x, y);

        }

        public static Vector2 GetRandomPositionInRingOptimized(Vector2 outerMin, Vector2 outerMax, Vector2 innerMin, Vector2 innerMax) {
            // 计算环形区域的四个矩形部分
            float leftWidth = innerMin.x - outerMin.x;
            float rightWidth = outerMax.x - innerMax.x;
            float middleWidth = innerMax.x - innerMin.x;

            float bottomHeight = innerMin.y - outerMin.y;
            float topHeight = outerMax.y - innerMax.y;
            float middleHeight = innerMax.y - innerMin.y;

            // 计算各区域面积
            float leftArea = leftWidth * (outerMax.y - outerMin.y);
            float rightArea = rightWidth * (outerMax.y - outerMin.y);
            float bottomArea = middleWidth * bottomHeight;
            float topArea = middleWidth * topHeight;

            float totalArea = leftArea + rightArea + bottomArea + topArea;

            // 按面积比例随机选择区域
            float randomValue = UnityEngine.Random.Range(0f, totalArea);

            Vector2 randomPoint;

            if (randomValue < leftArea) {
                // 左侧区域
                randomPoint = new Vector2(
                   UnityEngine.Random.Range(outerMin.x, innerMin.x),
                   UnityEngine.Random.Range(outerMin.y, outerMax.y)
                );
            } else if (randomValue < leftArea + rightArea) {
                // 右侧区域
                randomPoint = new Vector2(
                   UnityEngine.Random.Range(innerMax.x, outerMax.x),
                   UnityEngine.Random.Range(outerMin.y, outerMax.y)
                );
            } else if (randomValue < leftArea + rightArea + bottomArea) {
                // 底部区域
                randomPoint = new Vector2(
                   UnityEngine.Random.Range(innerMin.x, innerMax.x),
                   UnityEngine.Random.Range(outerMin.y, innerMin.y)
                );
            } else {
                // 顶部区域
                randomPoint = new Vector2(
                   UnityEngine.Random.Range(innerMin.x, innerMax.x),
                   UnityEngine.Random.Range(innerMax.y, outerMax.y)
                );
            }

            return randomPoint;
        }

    }
}