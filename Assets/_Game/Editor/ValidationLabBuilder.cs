using System;
using System.IO;
using Game.Bootstrap;
using Game.Features.Signal;
using Game.UI;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.EditorTools
{
    public static class ValidationLabBuilder
    {
        private const string DefinitionPath = "Assets/_Game/Features/Signal/Content/NeutralSignalDefinition.asset";
        private const string InputActionsPath = "Assets/_Game/Content/Input/ValidationInputActions.inputactions";
        private const string PresenterPrefabPath = "Assets/_Game/Prefabs/Signal/SignalLabPresenter.prefab";
        internal const string BootstrapScenePath = "Assets/_Game/Scenes/Bootstrap.unity";
        internal const string ValidationScenePath = "Assets/_Game/Scenes/ValidationLab.unity";

        [MenuItem("Validation/Build Technical Lab")]
        public static void BuildTechnicalLab()
        {
            MoveTemplateRenderingSettings();

            var definition = CreateSignalDefinition();
            var inputActions = CreateInputActions();
            var presenterPrefab = CreatePresenterPrefab();

            CreateBootstrapScene(definition, inputActions, presenterPrefab);
            CreateValidationScene();

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootstrapScenePath, true),
                new EditorBuildSettingsScene(ValidationScenePath, true),
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Technical Validation Lab created.");
        }

        private static SignalDefinition CreateSignalDefinition()
        {
            AssetDatabase.DeleteAsset(DefinitionPath);
            var definition = ScriptableObject.CreateInstance<SignalDefinition>();
            AssetDatabase.CreateAsset(definition, DefinitionPath);

            var serializedDefinition = new SerializedObject(definition);
            serializedDefinition.FindProperty("contentId").stringValue = "neutral-signal";
            serializedDefinition.FindProperty("displayName").stringValue = "Neutral Signal";
            serializedDefinition.ApplyModifiedPropertiesWithoutUndo();
            return definition;
        }

        private static InputActionAsset CreateInputActions()
        {
            AssetDatabase.DeleteAsset(InputActionsPath);
            var inputActions = ScriptableObject.CreateInstance<InputActionAsset>();
            inputActions.name = "ValidationInputActions";

            var labMap = inputActions.AddActionMap("Lab");
            var toggleAction = labMap.AddAction("Toggle", InputActionType.Button);
            toggleAction.AddBinding("<Keyboard>/space");

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), InputActionsPath), inputActions.ToJson());
            AssetDatabase.ImportAsset(InputActionsPath, ImportAssetOptions.ForceSynchronousImport);
            UnityEngine.Object.DestroyImmediate(inputActions);
            var persistedInputActions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(InputActionsPath);
            EditorBuildSettings.AddConfigObject("com.unity.input.settings.actions", persistedInputActions, true);
            return persistedInputActions;
        }

        private static GameObject CreatePresenterPrefab()
        {
            AssetDatabase.DeleteAsset(PresenterPrefabPath);

            var presenterObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            presenterObject.name = "SignalLabPresenter";
            presenterObject.transform.localScale = new Vector3(2f, 2f, 2f);
            var presenter = presenterObject.AddComponent<SignalLabPresenter>();

            var serializedPresenter = new SerializedObject(presenter);
            serializedPresenter.FindProperty("indicator").objectReferenceValue = presenterObject.GetComponent<Renderer>();
            serializedPresenter.ApplyModifiedPropertiesWithoutUndo();

            var savedPrefab = PrefabUtility.SaveAsPrefabAsset(presenterObject, PresenterPrefabPath);
            UnityEngine.Object.DestroyImmediate(presenterObject);
            return savedPrefab;
        }

        private static void CreateBootstrapScene(
            SignalDefinition definition,
            InputActionAsset inputActions,
            GameObject presenterPrefab)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraObject = new GameObject("Main Camera", typeof(Camera));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetPositionAndRotation(new Vector3(0f, 0f, -8f), Quaternion.identity);

            var lightObject = new GameObject("Directional Light", typeof(Light));
            lightObject.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
            lightObject.GetComponent<Light>().type = LightType.Directional;

            var bootstrapObject = new GameObject("Bootstrap", typeof(GameBootstrap));
            var bootstrap = bootstrapObject.GetComponent<GameBootstrap>();
            var serializedBootstrap = new SerializedObject(bootstrap);
            serializedBootstrap.FindProperty("signalDefinition").objectReferenceValue = definition;
            serializedBootstrap.FindProperty("inputActions").objectReferenceValue = inputActions;
            serializedBootstrap.FindProperty("presenterPrefab").objectReferenceValue = presenterPrefab;
            serializedBootstrap.FindProperty("validationSceneName").stringValue = "ValidationLab";
            serializedBootstrap.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        private static void CreateValidationScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "ValidationFloor";
            floor.transform.localScale = new Vector3(2f, 1f, 2f);
            EditorSceneManager.SaveScene(scene, ValidationScenePath);
        }

        private static void MoveTemplateRenderingSettings()
        {
            const string sourceFolder = "Assets/Settings";
            const string destinationFolder = "Assets/_Game/Content/Rendering";
            EnsureFolder(destinationFolder);

            foreach (var sourcePath in new[]
                     {
                         "Assets/Settings/DefaultVolumeProfile.asset",
                         "Assets/Settings/Mobile_RPAsset.asset",
                         "Assets/Settings/Mobile_Renderer.asset",
                         "Assets/Settings/PC_RPAsset.asset",
                         "Assets/Settings/PC_Renderer.asset",
                         "Assets/Settings/SampleSceneProfile.asset",
                         "Assets/Settings/UniversalRenderPipelineGlobalSettings.asset",
                     })
            {
                if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(sourcePath) == null)
                {
                    continue;
                }

                var destinationPath = destinationFolder + "/" + Path.GetFileName(sourcePath);
                var error = AssetDatabase.MoveAsset(sourcePath, destinationPath);
                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException(error);
                }
            }

            if (AssetDatabase.IsValidFolder(sourceFolder))
            {
                AssetDatabase.DeleteAsset(sourceFolder);
            }
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parentPath = Path.GetDirectoryName(path)?.Replace('\\', '/');
            var folderName = Path.GetFileName(path);
            if (string.IsNullOrEmpty(parentPath) || string.IsNullOrEmpty(folderName))
            {
                throw new InvalidOperationException($"Invalid asset path: {path}");
            }

            EnsureFolder(parentPath);
            AssetDatabase.CreateFolder(parentPath, folderName);
        }
    }

    public static class ValidationBuild
    {
        [MenuItem("Validation/Build Windows Mono")]
        public static void BuildWindowsMono()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64))
            {
                throw new InvalidOperationException("Windows Standalone support is not installed.");
            }

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

            var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "WindowsMono");
            Directory.CreateDirectory(outputDirectory);
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ValidationLabBuilder.BootstrapScenePath, ValidationLabBuilder.ValidationScenePath },
                locationPathName = Path.Combine(outputDirectory, "UnityFoundationValidation.exe"),
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.Development | BuildOptions.AllowDebugging,
            });

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Windows Mono build failed: {report.summary.result}");
            }

            Debug.Log($"Windows Mono build succeeded: {report.summary.outputPath}");
        }
    }
}
