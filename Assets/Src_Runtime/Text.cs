using UnityEngine;

public class RandomPositionInRings : MonoBehaviour
{
    /// <summary>
    /// 在矩形环内生成随机点
    /// </summary>
    /// <param name="outerMin">外矩形左下角坐标</param>
    /// <param name="outerMax">外矩形右上角坐标</param>
    /// <param name="innerMin">内矩形左下角坐标</param>
    /// <param name="innerMax">内矩形右上角坐标</param>
    /// <returns>环形区域内的随机点</returns>
    public static Vector2 GetRandomPositionInRing(Vector2 outerMin, Vector2 outerMax, Vector2 innerMin, Vector2 innerMax)
    {
        Vector2 randomPoint;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            // 在外矩形范围内生成随机点
            randomPoint = new Vector2(
                Random.Range(outerMin.x, outerMax.x),
                Random.Range(outerMin.y, outerMax.y)
            );
            
            attempts++;
            
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("达到最大尝试次数，可能内矩形太大或环形区域太小");
                break;
            }
            
        // 确保点不在内矩形内
        } while (IsPointInsideRectangle(randomPoint, innerMin, innerMax));

        return randomPoint;
    }

    /// <summary>
    /// 检查点是否在矩形内
    /// </summary>
    private static bool IsPointInsideRectangle(Vector2 point, Vector2 rectMin, Vector2 rectMax)
    {
        return point.x >= rectMin.x && point.x <= rectMax.x &&
               point.y >= rectMin.y && point.y <= rectMax.y;
    }

    // 示例用法
    void Start()
    {
        // 定义外矩形范围 (左下角和右上角坐标)
        Vector2 outerMin = new Vector2(0, 0);
        Vector2 outerMax = new Vector2(10, 8);
        
        // 定义内矩形范围
        Vector2 innerMin = new Vector2(2, 2);
        Vector2 innerMax = new Vector2(8, 6);
        
        // 获取环形区域内的随机点
        Vector2 randomPos = GetRandomPositionInRing(outerMin, outerMax, innerMin, innerMax);
        Debug.Log("环形区域内随机点: " + randomPos);
        
        // 可视化
        Debug.DrawLine(outerMin, new Vector2(outerMax.x, outerMin.y), Color.red, 10f); // 下边
        Debug.DrawLine(new Vector2(outerMax.x, outerMin.y), outerMax, Color.red, 10f); // 右边
        Debug.DrawLine(outerMax, new Vector2(outerMin.x, outerMax.y), Color.red, 10f); // 上边
        Debug.DrawLine(new Vector2(outerMin.x, outerMax.y), outerMin, Color.red, 10f); // 左边
        
        Debug.DrawLine(innerMin, new Vector2(innerMax.x, innerMin.y), Color.blue, 10f);
        Debug.DrawLine(new Vector2(innerMax.x, innerMin.y), innerMax, Color.blue, 10f);
        Debug.DrawLine(innerMax, new Vector2(innerMin.x, innerMax.y), Color.blue, 10f);
        Debug.DrawLine(new Vector2(innerMin.x, innerMax.y), innerMin, Color.blue, 10f);
        
        Debug.DrawRay(randomPos, Vector2.up * 0.2f, Color.green, 10f);
    }
}