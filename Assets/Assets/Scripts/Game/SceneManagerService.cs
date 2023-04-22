
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerService : SingletonBehaviour<SceneManagerService>
{
	private AnnotationService _annotationService
	{
		get
		{
			return AnnotationService.Instance;
		}
	}

	public override void Awake()
	{
		base.Awake();

		SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
	}

    public override void OnDestroy()
    {
        base.OnDestroy();

		SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
		SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
	}


    private void SceneManager_sceneUnloaded(Scene arg0)
	{
		_annotationService.SetMarker("SceneUnloaded:" + arg0.name);
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		_annotationService.SetMarker("NewSceneLoaded:" + arg0.name);

	}

	internal void UnloadScene(string sceneName)
	{
		_annotationService.SetMarker("UnloadingScene:" + sceneName);
		SceneManager.UnloadSceneAsync(sceneName);
	}

	private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
	{
		_annotationService.SetMarker("ActiveSceneChanged:" + arg1.name);
	}

	public void LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		_annotationService.SetMarker("LoadingScene:" + sceneName);
		SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
	}

	public void LoadScene(string sceneName)
	{
		_annotationService.SetMarker("LoadingScene:" + sceneName);
		SceneManager.LoadScene(sceneName);

	}

	public void SetSceneActive(Scene scene)
	{
		_annotationService.SetMarker("SettingActiveScene:" + scene.name);
		SceneManager.SetActiveScene(scene);
	}


	public void UnloadGameScenes()
	{
	}
}
