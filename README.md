# KatAzGame -- Utilisation des services cognitives Azure dans une application Unity

<p align="center">
  <img src="/Pictures/photo_katazGame.png">
</p>

## Contenu

- [Introduction](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#introduction)
- [Architecture générale](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#architecure-g%C3%A9n%C3%A9rale)
- [Création des services cognitifs](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-des-services-cognitifs)
- [Création du service speech-to-text](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-du-service-speech-to-text)
- [Création et entrainement du service LUIS](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-et-entrainement-du-service-luis)
- [Création du service OCR (Computer Vision)](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-du-service-ocr-computer-vision)
- [Intégration de ces services dans le jeu](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-de-ces-services-dans-le-jeu)
- [Création de la connection entre Unity WebGL et Javascript](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-de-la-connection-entre-unity-webgl-et-javascript)
- [Intégration du service speech-to-text](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-speech-to-text)
- [Intégration du service LUIS](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-luis)
- [Intégration du service de computer vision](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-de-computer-vision)
- [Déploiement des services @Edge](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#d%C3%A9ploiement-des-services-edge)
- [Déploiement de l'application dans Azure](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#d%C3%A9ploiement-de-lapplication-dans-azure)
- [Cas d'utilisation des services cognitifs](https://github.com/Louis-GRANGE/KatAzGame#les-services-cognitifs-dazure--principes-et-cas-dutilisation)
- [Conclusion](https://github.com/Louis-GRANGE/KatAzGame#conclusion)

## Introduction

<p align="justify">
&emsp; Cet article présente un Proof of Concept des services cognitifs Azure. L’objectif est de vous donner une meilleure vision des possibilités industrielles offertes par ce service d’intelligence artificielle du cloud Microsoft Azure.
</p>

<p align="justify">
&emsp; Les services cognitifs d’Azure permettent d’infuser des intelligences artificielles très performantes dans une application de manière simple et rapide. Il faut d’abord entraîner des IAs pré-construites à effectuer une tâche précise. Il suffit ensuite de les intégrer à notre solution par des appels d’APIs. Ces IAs permettent principalement de faire de l’analyse d’images ou de l’analyse de textes et de langages. Ce service d’analytique permet d’infuser une forme d’intelligence à une solution. 
</p>

<p align="justify">
&emsp; L’objectif de notre projet est donc d’intégrer de l’intelligence artificielle dans une application. Plus précisément, nous avons développé un jeu de type escape-game à l’aide du moteur 3D Unity. Un personnage doit réaliser plusieurs actions pour parvenir à résoudre une énigme. Ces actions sont dictées par notre propre voix. Des services cognitifs Azure vont capter les différents ordres que l’on donne à notre personnage qui réalisera l’action demandée. Un autre service cognitif de Computer Vision interviendra pour vérifier que le personnage a correctement résolu l’énigme posée. 
</p>

<p align="justify">
&emsp; Nous avons décidé de ne pas réaliser un PoC sur un domaine précis de l’industrie mais plutôt sur un jeu pour le côté ludique. Nous aborderons d’abord la démarche générale du projet puis le développement du jeu et de l’inclusion des services cognitifs dans le jeu. Nous présenterons en dernier lieu des cas d’usages très concrets autour de l’utilisation des services cognitifs dans divers domaines comme le retail, la logistique ou encore dans l’industrie. 
</p>

<p align="justify">
&emsp; Dans ce jeu de type escape game, un personnage se retrouve enfermé dans une maison. Son objectif est d’en sortir le plus rapidement possible. Il doit résoudre une énigme pour parvenir à s’échapper. Le joueur ne dispose que de sa voix puis de sa souris pour résoudre cette énigme. Il pourra parler au personnage. Cela fera appel successivement à deux services cognitifs. Le premier transformera l’audio en texte, c’est le service speech-to-text. Le deuxième service transformera le texte en une action (ou une intention), c’est le service LUIS (Language Understanding). Une fois que le joueur aura résolu l’énigme, il utilisera la souris pour écrire le code qui va délivrer le personnage. Un dernier service cognitif de computer vision permettra d’extraire le texte écrit à l’écran puis on vérifiera si le code est bon et si le joueur a gagné.
</p>

## Architecture générale

<p align="justify">
&emsp; Au niveau des différents composants du projet, nous développons le jeu sous Unity. Nous souhaitons que ce jeu soit disponible sous la forme d’une application web. Nous utiliserons donc le format d’exportation WebGL qui créera un build et une page html à partir du projet Unity. Il suffira de lancer cette page html pour avoir accès à l’application. 
</p>

<p align="justify">
&emsp; Nous devons aussi créer les services cognitifs, les entraîner et les mettre en production dans le cloud Azure (dans un premier temps). Une fois que les services sont prêts, on les appellera depuis le code Unity. Ces services seront appelés à chaque fois que nous donnerons un ordre au personnage afin de transformer ces ordres en actions concrètes.  
</p>

<p align="justify">
&emsp; Nous ne pouvons pas utiliser le microphone de l’ordinateur ou d’un smartphone directement sous Unity WebGL sans package payant. On passera ainsi par du javascript pour demander l’accès au microphone et appeler un service cognitif qui va d’abord transformer l’audio en texte puis nous enverrons le texte à Unity qui appellera un autre service cognitif qui transformera le texte en une action.
</p>

<p align="justify">
&emsp; Une latence apparaîtra entre le moment où nous appelons nos services et le moment où l’action se répercute sur le personnage. Pour réduire cette latence, nous déploierons les services cognitifs @Edge (dans un deuxième temps). Cela va permettre d’embarquer les services cognitifs au plus près de l’application et ainsi supprimer la latence avec le cloud. 
</p>

<p align="justify">
&emsp; Nous pouvons résumer les différentes communications entre les composantes dans le schéma ci-dessous : 
</p>

<p align="center">
  <img width="425" height="350" src="/Pictures/Archi3b.jpg">
</p>

<p align="justify">
&emsp; Nous souhaitons que le jeu soit disponible sous la forme d’une application web. Pour cela, nous créons un conteneur dans un Azure Storage puis nous stockons le build de notre projet et la page html créée par WebGL dans ce conteneur. L’application sera ensuite disponible à l’aide d’une URL et la page principale html sera ainsi lancée.
</p>

<p align="justify">
&emsp; Dans la suite de cet article, nous allons reprendre les différentes étapes de conception du projet pour les expliquer et les détailler. Nous verrons en particulier l’insertion des services cognitifs dans l'application, le développement du jeu à partir de l’utilisation de ces services, le déploiement des services cognitifs @Edge et le déploiement de l’application web à l’aide d’Azure.
</p>

## Création des services cognitifs

### Création du service speech-to-text

### Création et entrainement du service LUIS

### Création du service OCR (Computer Vision)

## Intégration de ces services dans le jeu

### Création de la connection entre Unity WebGL et Javascript

<p align="justify">
&emsp; Comme le schéma, l’indique lorsque vous appuyez sur le bouton pour commencer la reconnaissance vocale, Il va en réalité envoyer une information au Javascript pour lui indiquer qu’il doit demander l’accès au microphone et commencer la reconnaissance vocale avec un appel d’API. Donc dans une classe en C# sur Unity on va devoir définir une fonction externe à Unity:
</p>
```c#
[DllImport("__Internal")]
    private static extern void RecognizedSpeech();
```

<p align="justify">
&emsp; Pour cela il faudra bien inclure la bibliothèque suivante:
</p>
```c#
using System.Runtime.InteropServices;
```

<p align="justify">
&emsp; La fonction externe définie précédemment peut être appelée quand l’on veut mais il faudra tout d’abord l’initialiser et c’est dans le répertoire des Assets de Unity que l’on va le faire. Dans ce dossier, créer un sous dossier nommé Plugins et créer un fichier en .jslib où l’on ajoutera notre fonction à l'intérieur, ce qui donne dans notre cas: Assets > Plugins > SpeechRecognized.jslib

Et c’est donc dans ce fichier là que la fonction d’appel d’une fonction javascript va être faite:
</p>
```js
mergeInto(LibraryManager.library, {

  RecognizedSpeech: function () {
    fromMic();
  },
});
```

<p align="justify">
&emsp; Donc maintenant le lien est fait, lorsque l’on appelle la fonction Unity RecognizedSpeech(), ceci fait un appel à la fonction fromMic() dans le javascript.
Et c’est ici que nous allons activer le microphone et lancer la reconnaissance vocale.
Bien-sûr pour un appel d’API il vous faudra différente information telle que votre subscriptionKey, serviceRegion et la langue de reconnaissance. Toutes ces informations sont sur la page de votre service Azure. Pour initialiser l’appel et le microphone:
</p>
```js
var speechConfig = SpeechSDK.SpeechConfig.fromSubscription(auth.subscriptionKey, auth.serviceRegion);
speechConfig.speechRecognitionLanguage = auth.language;
var audioConfig  = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);
```

<p align="justify">
&emsp; Et pour démarrer la reconnaissance vocale en continue:
</p>
```js
recognizer.startContinuousRecognitionAsync();
```

<p align="justify">
&emsp; Nous utilisons ici le module javascript pour la reconnaissance vocale, ce qui permet d’accéder à différents callbacks pour le Speech-To-Text (Documentation). On utilisera ici le recognized car il nous retourne le string de la reconnaissance après un temps de pose dans la voix. Ce qui est différent du recognizing qui lui permet d’envoyer en continue la reconnaissance.
</p>
```js
recognizer.recognized = (s, e) => {
  ReturnValue = e.result.text;
  unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
};
```

<p align="justify">
&emsp; Ainsi c’est ici que l’envoi du string dans Unity sera fait pour pouvoir traiter ces données. Le premier argument (‘JavascriptHook’) correspond au nom du GameObject dans la scène de Unity, le second argument correspond à la fonction d’un script qu’il y a dans le GameObject et les derniers arguments sont des paramètres de fonction. ‘ReturnValue’ est le string reconnu par la reconnaissance vocale.
De là nous allons créer notre fonction correspondante:
</p>
```c#
//Get Recognized Speech to String
public void ReturnRecognizeSpeechText(string str)
{
  sRecognizedSpeech = str;
  StartCoroutine(LuisManager.getInstance().SubmitRequestToLuis(str));
  SetRecognizeSpeechText();
}
```

<p align="justify">
&emsp; Et ainsi appeler LUIS en fonction de ce que nous avons dit pour pouvoir récupérer un ordre donné. Ce qui donne:
</p>
```c#
public IEnumerator SubmitRequestToLuis(string dictationResult)
{
  string queryString = string.Concat(Uri.EscapeDataString(dictationResult));
  using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(luisEndpoint + queryString))
  {
    yield return unityWebRequest.SendWebRequest();

    if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
    {
      Debug.Log(unityWebRequest.error);
    }
    else
    {
      JsonDataOfLUIS.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfLUIS.Root>(unityWebRequest.downloadHandler.text);

      //analyse the elements of the response 
      AnalyseResponseElements(analysedQuery);
               
    }
    yield return null;
  }
}
```

<p align="justify">
&emsp; On utilise une coroutine pour les appels d’API dans Unity car elle sont exécuté en Asynchrone afin de récupérer le body de la requête GET et donc le Json de la réponse donné par le service. Un package pratique de Unity permet de désérialiser facilement des jsons pour ensuite les utiliser dans l’application. Installable facilement depuis le package manager de Unity, accessible ici: Window > Package Manager
</p>
<p align="center">
  <img src="/Pictures/photo_JsonPackage.png">
</p>

<p align="justify">
&emsp; Un autre outils extrêmement utile pour le désérialisation de classe est le lien https://json2csharp.com/ afin de traduire le body reçu en classe C#.
</p>

<p align="justify">
&emsp; PS: Cocher ces cases suivante :
</p>
<p align="center">
  <img src="/Pictures/photo_Json2Csharp.png">
</p>
<p align="justify">
&emsp; Ce qui permet d'ajouter au sein de la classe retourné les attributs des propriété Json afin de correspondre correctement au body sans pour autant avoir le même nom d'attribut. Comme on peut avoir certain problème comme un nom d'attribut $instance par exemple que Unity ne permettrait pas.
</p>
```c#
[JsonProperty("$instance")]
public Instance Instance { get; set; }
```

<p align="justify">
&emsp; Afin d’analyser la réponse retourné, nous allons définir une fonction supplémentaire qui traitera ces données:
</p>
```c#
private void AnalyseResponseElements(JsonDataOfLUIS.Root aQuery)
{
  string topIntent = aQuery.Prediction.TopIntent;

  switch (aQuery.Prediction.TopIntent)
  {
```

<p align="justify">
&emsp; Avec un switch sur le TopIntent qui correspond à la prédiction ayant le score le plus élevé  (entre 0 et 100) sur toute les intentions que vous avez ajouté dans votre Luis. Ainsi il sera possible de faire depuis cette fonction ce que vous souhaitez par rapport à l’intention donnée. Voici un exemple pour notre déplacement:

Donc ici nous avons notre intention de Déplacement avec dans les entitées la direction voulu et si dans nos entitées il y a un nombre, c’est que le joueur voulait un nombre de pas prédéfini. Ou dans le cas contraire le personnage va avancer jusqu’à être bloqué par un obstacle.
</p>
```c#
switch (aQuery.Prediction.TopIntent)
{
    case "Deplacement":
    {
        if (aQuery.Prediction.Entities.Direction != null)
        {
            foreach (var item in aQuery.Prediction.Entities.Direction)
            {
                switch (item[0].ToString())
                {
                    case "droite":
                    {
                        pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Right);
                        pmPlayerMovement.RotateTo(90);
                        pmPlayerMovement.setCanGoForward(true);
                        break;
                    }
                    case "gauche":
                    {
                        pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Left);
                        pmPlayerMovement.RotateTo(270);
                        pmPlayerMovement.setCanGoForward(true);
                        break;
                    }
                    case "haut":
                    {
                        pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Up);
                        pmPlayerMovement.setCanGoForward(true);
                        break;
                    }
                    case "bas":
                    {
                        pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Down);
                        pmPlayerMovement.RotateTo(180);
                        pmPlayerMovement.setCanGoForward(true);
                        break;
                    }
                    case "stop":
                    {
                        pmPlayerMovement.jhJavascriptHook.SetImageDirInfo(JavascriptHook.Direction.Stop);
                        pmPlayerMovement.setCanGoForward(false);
                        break;
                    }
                }
            }
        }
        if (aQuery.Prediction.Entities.Number != null)
        {
            foreach (var item in aQuery.Prediction.Entities.Number)
            {
                pmPlayerMovement.SetMaxDistance(item);
            }
        }
        else
        {
            pmPlayerMovement.SetMaxDistance(-1);
        }
        break;
    }
```
<p align="justify">
&emsp; “paPlayerMovement : classe de déplacement du perso. Le perso a la classe paPlayerMovement d’attribuée”.
</p>

<p align="justify">
&emsp; Pour la dernière énigme, nous allons utiliser le service OCR dans Computer Vision de Microsoft Azure.
Pour celà comme précédemment il faudra récupérer depuis le portail Azure de votre service différéntes données afin de l’utiliser, comme votre authorizationKey, ocpApimSubscriptionKeyHeader et votre visionAnalysisEndpoint. Dans le principe nous ferons un appel similaire au LUIS car c’est aussi un simple appel d’API.  Cependant ce service nécessite une requête Post pour l’envoie de donné c'est-à-dire l’image pour la recherche de texte. Et lors de l’appel de la requête un lien nous est donner dans le header qui correspondra a la réponse du service où c’est ici que l’on renverra une autre requête en GET cette fois pour enfin récupérer le body de la requête GET et donc le Json de la réponse donné par le service.
&emsp; Tout d’abord il faudra mettre en place dans le jeu le système de capture et c’est à l’aide d’une caméra que cela sera possible, et de la fonction suivante qui nous renverra une image en tableau de byte ce qui est nécessaire dans l’appel de l’API.
</p>
```c#
public byte[] Capture()
{
    RenderTexture activeRenderTexture = RenderTexture.active;
    RenderTexture.active = _camera.targetTexture;

    _camera.Render();

    Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
    image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
    image.Apply();
    RenderTexture.active = activeRenderTexture;
    return image.EncodeToJPG();
}
```

<p align="justify">
&emsp; Ensuite il faudra donc envoyer l’image via une requête POST:
</p>
```c#
public IEnumerator AnalyseLastImageCaptured()
{
	//POST
	print("POST");
	WWWForm webForm = new WWWForm();
	using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(visionAnalysisEndpoint, webForm))
	{
		// gets a byte array out of the saved image
		imageBytes = ccCameraCapture.Capture();
		unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
		unityWebRequest.SetRequestHeader(ocpApimSubscriptionKeyHeader, authorizationKey);

		// the download handler will help receiving the analysis from Azure
		unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

		// the upload handler will help uploading the byte array with the request
		unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
		unityWebRequest.uploadHandler.contentType = "application/octet-stream";

		yield return unityWebRequest.SendWebRequest();

		long responseCode = unityWebRequest.responseCode;

		try
		{
			sResponseHeader = unityWebRequest.GetResponseHeaders()["Operation-Location"];
			StartCoroutine(AnalyseResult(sResponseHeader));
		}
		catch (Exception exception)
		{
			Debug.Log("Json exception.Message: " + exception.Message);
		}

		yield return null;
	}
}
```

<p align="justify">
&emsp; Et c’est avec le lien fourni dans le header de la requête que nous exécuterons une nouvelle requête en GET afin d’obtenir le résultat puis de le désérialisé dans une classe comme fait dans le LUIS:
</p>
```c#
public IEnumerator AnalyseResult(string url)
{
	yield return new WaitForSeconds(1f);
	//GET RESULTS
	using (UnityWebRequest unityWebGetRequest = UnityWebRequest.Get(url))
	{
		unityWebGetRequest.SetRequestHeader(ocpApimSubscriptionKeyHeader, authorizationKey);
		yield return unityWebGetRequest.SendWebRequest();

		if (unityWebGetRequest.isNetworkError || unityWebGetRequest.isHttpError)
		{
			Debug.Log(unityWebGetRequest.error);
		}
		else
		{
			try
			{
				print(unityWebGetRequest.downloadHandler.text);
				JsonDataOfCVR.Root analysedQuery = JsonConvert.DeserializeObject<JsonDataOfCVR.Root>(unityWebGetRequest.downloadHandler.text);

				AnalyseResponseElements(analysedQuery);
			}
			catch (Exception exception)
			{
				Debug.Log("GET Request Exception Message: " + exception.Message);
			}
		}
		yield return null;
	}
}
```

<p align="justify">
&emsp; Et ainsi on va analyser la réponse désérialiser en fonction de si le statut du résultat de la réponse est "succeeded" c’est-à-dire que le service à terminé le traitement, on récupère le texte donné pour l'utiliser et vérifié la validité du code fournie.
</p>
```c#
private void AnalyseResponseElements(JsonDataOfCVR.Root aQuery)
{
	string sFullText = "";
	if (aQuery.Status == "succeeded")
	{
		foreach (var results in aQuery.AnalyzeResult.ReadResults)
		{
			foreach (var line in results.Lines)
			{
				foreach (var word in line.Words)
				{
					sFullText += word.Text + " ";
				}
			}
		}
	}
	else
	{
		StartCoroutine(AnalyseResult(sResponseHeader));
		return;
	}

	sFullText = sFullText.Remove(sFullText.IndexOf(' '));
	GameManager gmGameManager = GameManager.getInstance();
	if (sFullText.ToLower() == gmGameManager.egEnigmeGenerator.GetTrueStringCode()) // In case of good code
	{
		gmGameManager.egEnigmeGenerator.Victory("VICTOIRE!!");
		gmGameManager.cmCameraMovement.bisTopMapView = true;
		gmGameManager.dDrawing.DeleteDraw();
		gmGameManager.goPlayer.GetComponentInChildren<PlayerMovement>().SetShowWay(true);
	}
	else // In case of bad code
	{
		gmGameManager.tiTextInformation.SetText("Détecté: " + sFullText + " => Mauvais Code... ");
		gmGameManager.dDrawing.DeleteDraw();
	}
}
```

### Intégration du service speech-to-text

### Intégration du service LUIS

### Intégration du service de computer vision

## Déploiement des services @Edge

## Déploiement de l'application dans Azure

## Les services cognitifs d'Azure : principes et cas d'utilisation

<p align="justify">
&emsp; Nous avons vu que les services cognitifs d’Azure permettent d’infuser des intelligences artificielles très performantes dans une application de manière simple et rapide. Il existe soit des services qui sont directement prêt à l'emploi (comme le service speech-to-text), soit des services qu’il faut entraîner (LUIS) pour le personnaliser à un cas d'usage précis.  
</p>

<p align="justify">
&emsp; L'avantage des services cognitifs est qu’ils sont entraînés ou pré-entraînés avec une énorme quantité de données. Cela explique la qualité et la performance des IAs construites.  Pour les services pré-entraînés, il suffit de leur apporter de nouvelles données pour finaliser l'entraînement et les personnaliser à un scénario. Plus la quantité de ces nouvelles données est importante, plus l’IA sera performante. Il n’est cependant pas forcément nécessaire d’avoir une quantité importante de nouvelles données pour avoir une IA efficace (car les services sont déjà pré-entraînés). 
</p>

<p align="justify">
&emsp; On a aussi vu que les services sont facilement entraînables puisqu’ils disposent d’une interface web pour cela et qu’il n’y a pas besoin d’écrire de code lors de la création de l’IA. Une fois que ces services sont prêts, ils sont facilement déployables et accessibles via un point de terminaison. Ils sont stockés dans le cloud Azure. Il suffit ensuite de les intégrer à notre solution par des appels d’APIs. 
</p>

<p align="justify">
&emsp;  Le déploiement classique des services cognitifs est donc un déploiement dans le cloud. On peut aussi les déployer @Edge. En effet, lorsque le système n’a pas d’accès internet constant ou si l’IA doit retourner des réponses le plus rapidement possible, il est préférable d’embarquer l’IA avec le système et ainsi éviter le temps de latence du cloud pour améliorer le temps de réponse. C’est ce que nous avons fait dans le KatAz game. On utilise souvent les technologies @Edge pour les scénarios faisant intervenir l’IoT.
</p>

<p align="justify">
&emsp; L’idée de notre projet était donc d’inclure de l’intelligence artificielle dans notre jeu. Ceci démontre que l’on peut utiliser l’IA dans pleins de domaines différents en apportant une réelle valeur. L’utilisation de ces services cognitifs peut intervenir dans des milliers de cas d’usage différents et dans tous les secteurs possibles. Ils permettent de faire de l’analyse de textes ou de langages. On peut par exemple transcrire du texte en audio ou de l’audio en texte, on peut analyser des textes, ou utiliser LUIS et QnA Maker pour des chatbots par exemple. On utilise aussi ces services pour de la computer vision. On peut ainsi analyser des visages à partir de photos ou de vidéos, faire de l’extraction de textes à partir d’images ou analyser le contenu d’images ou de vidéos. Prenons un premier exemple au niveau de la grande distribution. 
</p>

<p align="justify">
&emsp; Dans un magasin alimentaire par exemple, nous pouvons utiliser des caméras puis un service de computer vision permettant de détecter si l’un des rayons du magasin est vide.En fonction des résultats, une alerte est envoyée pour réapprovisionner les stocks. Cela permettrait ainsi d’éviter à la fois la colère des clients et la perte d’une partie du chiffre d'affaires. On peut aussi utiliser un service de computer vision pour compter le nombre de clients et détecter leur sexe et leur âge.  Il suffit de posséder une caméra et d’envoyer les enregistrements à un service cognitif préalablement entraîné qui analysera les images. Cela permettra de connaître le profil des clients pour chaque plage horaire et ainsi d’orienter la politique commerciale en fonction des résultats précédents.
</p>

<p align="justify">
&emsp;  On peut aussi utiliser un autre exemple. Prenons un entrepôt qui collecte, stocke et restitue des colis. Un ouvrier est chargé de confectionner des colis comportant plusieurs pièces qui sont stockées à des endroits différents de l'entrepôt. Il possède un chariot élévateur. On peut utiliser des services cognitifs permettant de déclencher une action à partir des ordres vocaux de l’employé. La personne indique l’allée et le numéro du rayon du produit et le chariot élévateur se déplace automatiquement. Une fois que la mise en préparation du colis est terminée, on peut utiliser une caméra qui scanne l’étiquette sur le colis, puis envoyer les images à un service cognitif qui détectera les informations contenues sur le colis. Ces informations seront par la suite inscrites dans une base de données. 
</p>


## Conclusion

<p align="justify">
&emsp; 
Nous espérons que cet article et ce projet vous permettront d’avoir une vision précise des possibilités qu'offrent les services cognitifs d’Azure. 
</p>
