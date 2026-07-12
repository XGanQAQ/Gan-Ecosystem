#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using GanEcosystem.UI.Core;

namespace GanEcosystem.UI.UnityRuntime
{
    public static class CreateUIPresenterViewerScript
    {
        private const string MenuRoot = "Assets/Create/GanFramework/UI/";
        private const int MenuPriority = 80;

        [MenuItem(MenuRoot + "Viewer Script", false, MenuPriority)]
        public static void CreateViewerScript()
        {
            string folderPath = GetSelectedFolderPath();
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, "NewUIViewer.cs").Replace("\\", "/"));
            string className = BuildClassNameFromAssetPath(assetPath);

            string content =
$@"using GanFramework.Modules.UI;

[ViewerAttribute(UILayer.Normal, ""Prefab/UI/{className}.prefab"")]
public class {className} : ViewerBase
{{
}}";

            WriteScriptAsset(assetPath, content);
        }

        [MenuItem(MenuRoot + "Presenter Script", false, MenuPriority + 1)]
        public static void CreatePresenterScript()
        {
            string folderPath = GetSelectedFolderPath();
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, "NewUIPresenter.cs").Replace("\\", "/"));
            string presenterClassName = BuildClassNameFromAssetPath(assetPath);
            string viewerClassName = GetViewerClassNameForPresenter(presenterClassName);
            string viewerFieldName = ToCamelCase(viewerClassName);

            string content =
$@"using GanFramework.Modules.UI;

public class {presenterClassName} : PresenterBase
{{
    private {viewerClassName} {viewerFieldName} => autoViewer as {viewerClassName};

    public override void Init()
    {{
        base.Init();
    }}
}}";

            WriteScriptAsset(assetPath, content);
        }

        [MenuItem(MenuRoot + "Viewer Script", true)]
        [MenuItem(MenuRoot + "Presenter Script", true)]
        public static bool ValidateCreateScripts()
        {
            return !EditorApplication.isPlaying;
        }

        private static void WriteScriptAsset(string assetPath, string content)
        {
            string projectRoot = Directory.GetCurrentDirectory();
            string normalizedAssetPath = assetPath.Replace("\\", "/");
            string absolutePath = Path.Combine(projectRoot, normalizedAssetPath);

            File.WriteAllText(absolutePath, content, new UTF8Encoding(false));
            AssetDatabase.ImportAsset(normalizedAssetPath);

            MonoScript createdScript = AssetDatabase.LoadAssetAtPath<MonoScript>(normalizedAssetPath);
            if (createdScript != null)
            {
                ProjectWindowUtil.ShowCreatedAsset(createdScript);
            }
        }

        private static string GetSelectedFolderPath()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                return "Assets";
            }

            string selectedPath = AssetDatabase.GetAssetPath(selectedObject);
            if (string.IsNullOrEmpty(selectedPath))
            {
                return "Assets";
            }

            if (AssetDatabase.IsValidFolder(selectedPath))
            {
                return selectedPath.Replace("\\", "/");
            }

            string parentFolder = Path.GetDirectoryName(selectedPath);
            if (string.IsNullOrEmpty(parentFolder))
            {
                return "Assets";
            }

            return parentFolder.Replace("\\", "/");
        }

        private static string BuildClassNameFromAssetPath(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            var builder = new StringBuilder(fileName.Length);
            bool previousWasSeparator = false;

            for (int i = 0; i < fileName.Length; i++)
            {
                char current = fileName[i];
                if (char.IsLetterOrDigit(current) || current == '_')
                {
                    if (builder.Length == 0)
                    {
                        if (char.IsDigit(current))
                        {
                            builder.Append('_');
                        }

                        builder.Append(char.ToUpperInvariant(current));
                    }
                    else
                    {
                        builder.Append(previousWasSeparator ? char.ToUpperInvariant(current) : current);
                    }

                    previousWasSeparator = false;
                }
                else
                {
                    previousWasSeparator = true;
                }
            }

            if (builder.Length == 0)
            {
                return "NewUIScript";
            }

            return builder.ToString();
        }

        private static string GetViewerClassNameForPresenter(string presenterClassName)
        {
            const string presenterSuffix = "Presenter";
            if (presenterClassName.EndsWith(presenterSuffix))
            {
                return presenterClassName.Substring(0, presenterClassName.Length - presenterSuffix.Length) + "Viewer";
            }

            return presenterClassName + "Viewer";
        }

        private static string ToCamelCase(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return "viewer";
            }

            if (className.Length == 1)
            {
                return className.ToLowerInvariant();
            }

            return char.ToLowerInvariant(className[0]) + className.Substring(1);
        }
    }
}
#endif
