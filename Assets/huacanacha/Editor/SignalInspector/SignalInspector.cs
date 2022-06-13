using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using huacanacha.signal;
using huacanacha.unity.signal;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SignalInspector : EditorWindow
{
    #region UIElement, Name, Path

    private const String ASSET_ROOT = "Assets/huacanacha/Editor/SignalInspector/UIToolkit/";
    private const String REFRESH_BUTTON = "refresh-button";
    private Button refreshButton;
    private const String CONTENT_ROOT = "content-root";
    private VisualElement contentRoot;

    private VisualTreeAsset signalContextViewTreeAsset;
    private VisualTreeAsset signalProviderViewTreeAsset;
    private VisualTreeAsset signalViewTreeAsset;

    //private const String C

    #endregion

    private bool repaintInspector = false;
    
    [MenuItem("Window/Signal Inspector")]
    public static void Init() {
        SignalInspector wnd = GetWindow<SignalInspector>();
        wnd.titleContent = new GUIContent("Signal Inspector");
        Vector2 size = new Vector2(300, 600);
        wnd.minSize = size;
        wnd.maxSize = size;
    }
    private void CreateGUI() {
        Debug.LogWarning("CreateGUI");
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> 
            ($"{ASSET_ROOT}SignalInspector.uxml");
        signalContextViewTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> 
            ($"{ASSET_ROOT}SignalContextView.uxml");
        signalProviderViewTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> 
            ($"{ASSET_ROOT}SignalProviderView.uxml");
        signalViewTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> 
            ($"{ASSET_ROOT}SignalView.uxml");
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>                        
            ($"{ASSET_ROOT}EditorStyle.uss");
        contentRoot = rootVisualElement.Q<VisualElement>(CONTENT_ROOT);
        rootVisualElement.styleSheets.Add(styleSheet);
        refreshButton = rootVisualElement.Q<Button>(REFRESH_BUTTON);
        refreshButton.clicked += (MarkInspectorRepaint);
        MarkInspectorRepaint();
    }

    private void RefreshInspector() {
        if (repaintInspector) {
            repaintInspector = false;
        }
        else {
            return;
        }
        if (contentRoot == null) {
            return;
        }
        Debug.Log("Refresh Signalling Context");
        contentRoot.Clear();
        
        var signallingContexts = GameObject.FindObjectsOfType<BaseSignallingContext>();
        foreach (var signallingContext in signallingContexts) {
            var signalContextView = signalContextViewTreeAsset.Instantiate();
            var contextName = signalContextView.Q<Label>("signal-context-name");
            var contextType = signalContextView.Q<Label>("signal-context-type");
            contextName.text = signallingContext.Name;
            contextType.text = signallingContext.GetType().ToString().Split(".").Last();
            contentRoot.Add(signalContextView);
            Debug.Log($"Find SignalContext {signallingContext.Name}, Type of {signallingContext.GetType().ToString().Split(".").Last()}");
            var signalProviders = signallingContext.GetComponents<ISignalProvider>();
            var viewContentRoot = signalContextView.Q<VisualElement>(CONTENT_ROOT);
            foreach (var signalProvider in signalProviders) {
                var signalView = signalProviderViewTreeAsset.Instantiate();
                viewContentRoot.Add(signalView);
                Foldout foldout = signalView.Q<Foldout>("fold-out");
                foldout.text = signalProvider.GetType().ToString();
                var signals = signalProvider.GetType().GetFields();
                foreach (var signal in signals) {
                    // if (signal.ReflectedType.BaseType) {
                    //     
                    // }
                    if (signal.FieldType.ToString().Contains("Signal")) {
                        var signalViewRoot = signalViewTreeAsset.Instantiate();
                        signalViewRoot.Q<Label>("signal-name").text = signal.Name;
                        signalViewRoot.Q<Label>("signal-type").text = signal.FieldType.ToString();
                        foldout.Add(signalViewRoot);
                        Debug.LogWarning(signal.FieldType);
                        Debug.LogWarning(signal.FieldType.BaseType == typeof(Signal));
                        Debug.Log(signal.Name);
                    }
                }
            }
        }
    }
    private void MarkInspectorRepaint() {
        repaintInspector = true;
    }
    
    private void OnProjectChange() {
        Debug.Log("OnProjectChanged");
    }

    private void OnEnable() {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnInspectorUpdate() {
        RefreshInspector();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        MarkInspectorRepaint();
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.EnteredEditMode) {
            MarkInspectorRepaint();
        }
    }
}
