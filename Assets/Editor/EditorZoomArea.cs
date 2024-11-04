using UnityEngine;
using UnityEditor;

public static class EditorZoomArea
{
    private static Matrix4x4 prevGuiMatrix;

    public static Rect Begin(float zoomScale, Rect screenCoordsArea)
    {
        GUI.EndGroup();

        Rect clippedArea = ScaleRectBy(screenCoordsArea, 1.0f / zoomScale, screenCoordsArea.TopLeft());
        clippedArea.y += EditorGUIUtility.singleLineHeight;
        GUI.BeginGroup(clippedArea);

        prevGuiMatrix = GUI.matrix;
        Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
        Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
        GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

        return clippedArea;
    }

    public static void End()
    {
        GUI.matrix = prevGuiMatrix;
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0.0f, EditorGUIUtility.singleLineHeight, Screen.width, Screen.height));
    }

    private static Vector2 TopLeft(this Rect rect)
    {
        return new Vector2(rect.xMin, rect.yMin);
    }

    private static Rect ScaleRectBy(Rect rect, float scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x = pivotPoint.x + (rect.x - pivotPoint.x) * scale;
        result.y = pivotPoint.y + (rect.y - pivotPoint.y) * scale;
        result.width *= scale;
        result.height *= scale;
        return result;
    }
}
