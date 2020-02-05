using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

public class TextureEditor : EditorWindow
{
    Texture2D directoryTexR;
    Texture2D directoryTexG;
    Texture2D directoryTexB;

    [MenuItem("Window/PNG Creator")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(TextureEditor));
    }
    void OnGUI()
    {
        EditorGUIUtility.LookLikeInspector();

        directoryTexR = (Texture2D)EditorGUILayout.ObjectField("Texture R Directory:", directoryTexR, typeof(Texture2D));
        directoryTexG = (Texture2D)EditorGUILayout.ObjectField("Texture G Directory:", directoryTexG, typeof(Texture2D));
        directoryTexB = (Texture2D)EditorGUILayout.ObjectField("Texture B Directory:", directoryTexB, typeof(Texture2D));

        Color colorPixel = new Color();
        Color[] color_gradient = new Color[256];
        int gradient_x = 0;
        float color_gray = 0;

        if (GUILayout.Button("Create PNG"))
        {
            var tex = new Texture2D(directoryTexR.width, directoryTexR.height, TextureFormat.RGB24, false);
            var tex_gradient = new Texture2D(256, 1, TextureFormat.RGB24, false);

            for (int x = 0; x < directoryTexR.height; x++)
            {
                for (int y = 0; y < directoryTexR.height; y++)
                {
                    colorPixel.r = color_pixel(directoryTexR.GetPixel(x, y), ref color_gradient, ref gradient_x, ref color_gray);
                    colorPixel.g = color_pixel(directoryTexG.GetPixel(x, y), ref color_gradient, ref gradient_x, ref color_gray);
                    colorPixel.b = color_pixel(directoryTexB.GetPixel(x, y), ref color_gradient, ref gradient_x, ref color_gray);
                    tex.SetPixel(x, y, colorPixel);
                    }
                }

            for (int i = 0; i < 256; i++)
            {
                tex_gradient.SetPixel(i, 0, color_gradient[i]);
            }

            File.WriteAllBytes(EditorUtility.SaveFilePanel("Save texture...", Application.dataPath, "texture", "png"), tex.EncodeToPNG());
            File.WriteAllBytes(EditorUtility.SaveFilePanel("Save texture...", Application.dataPath, "texture_gradient", "png"), tex_gradient.EncodeToPNG());
        }
    }
     float color_pixel(Color texture_pixel,ref Color[] _color_gradient, ref int _gradient_x, ref float _color_gray)
    {
        float color_chanel=0;
        if (_color_gradient.Contains(texture_pixel))
        {
            for (int i = 0; i < _color_gradient.Count(); i++)
            {
                if (_color_gradient[i] == texture_pixel)
                {
                    color_chanel= i * (1.0F / 255.0F);
                    break;
                }
            }
        }
        else
        {
            _color_gradient[_gradient_x] = texture_pixel;
            _gradient_x++;
            color_chanel = _color_gray;
            _color_gray = _color_gray + (1.0F / 255.0F);
        }
        return color_chanel;
    }
}
