# KatAzGame -- Utilisation des services cognitives Azure dans une application Unity

<p align="center">
  <img src="/Pictures/KatAzGame_Picture.png">
</p>

## Contenu

- [Introduction](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#introduction)
- [Architecture générale](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#architecture-g%C3%A9n%C3%A9rale)
- [Création des services cognitifs](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-des-services-cognitifs)
- [Création du service speech-to-text et du service OCR (Computer Vision)](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-du-service-speech-to-text-et-du-service-ocr-computer-vision)
- [Création et entrainement du service LUIS](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#cr%C3%A9ation-et-entrainement-du-service-luis)
- [Intégration des services cognitifs dans le jeu](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-de-ces-services-dans-le-jeu)
- [Intégration du service speech-to-text](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-speech-to-text)
- [Intégration du service LUIS](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-luis)
- [Intégration du service de computer vision](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#int%C3%A9gration-du-service-de-computer-vision)
- [Infrastructure Serverless](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#infrastructure-serverless)
- [Les services cognitifs Serverless](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#les-services-cognitifs-serverless)
- [Passage par une Azure Function](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#passage-par-une-azure-function)
- [Publication de l'application Serverless](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#publication-de-lapplication-serverless)
- [Cas d'utilisation des services cognitifs](https://github.com/Louis-GRANGE/KatAzGame#les-services-cognitifs-dazure--principes-et-cas-dutilisation)
- [Conclusion](https://github.com/Louis-GRANGE/KatAzGame#conclusion)

## Introduction

<p align="justify">
Cet article présente un Proof of Concept des services cognitifs Azure. L’objectif est de vous donner une meilleure vision des possibilités industrielles offertes par ce service d’intelligence artificielle du cloud Microsoft Azure.
</p>

<p align="justify">
Les services cognitifs d’Azure permettent d’infuser des intelligences artificielles très performantes dans une application de manière simple et rapide. Il faut d’abord entraîner des IAs pré-construites à effectuer une tâche précise. Il suffit ensuite de les intégrer à notre solution par des appels d’APIs. Ces IAs permettent principalement de faire de l’analyse d’images ou de l’analyse de textes et de langages. Ce service d’analytique permet d’infuser une forme d’intelligence à une solution. 
</p>

<p align="justify">
L’objectif de notre projet est donc d’intégrer de l’intelligence artificielle dans une application. Plus précisément, nous avons développé un jeu de type escape-game à l’aide du moteur 3D Unity. Un personnage doit réaliser plusieurs actions pour parvenir à résoudre une énigme. Ces actions sont dictées par notre propre voix. Des services cognitifs Azure vont capter les différents ordres que l’on donne à notre personnage qui réalisera l’action demandée. Un autre service cognitif de Computer Vision interviendra pour vérifier que le personnage a correctement résolu l’énigme posée. 
</p>

<p align="justify">
Nous avons décidé de ne pas réaliser un PoC sur un domaine précis de l’industrie, mais plutôt sur un jeu pour le côté ludique. Nous aborderons d’abord la démarche générale du projet puis le développement du jeu et de l’inclusion des services cognitifs dans le jeu. Nous présenterons en dernier lieu des cas d’usages très concrets autour de l’utilisation des services cognitifs dans divers domaines comme le retail, la logistique ou encore dans l’industrie. 
</p>

<p align="justify">
Dans ce jeu de type escape game, un personnage se retrouve enfermé dans une maison. Son objectif est d’en sortir le plus rapidement possible. Il doit résoudre une énigme pour parvenir à s’échapper. Le joueur ne dispose que de sa voix puis de sa souris pour résoudre cette énigme. Il pourra parler au personnage. Cela fera appel successivement à deux services cognitifs. Le premier transformera l’audio en texte, c’est le service speech-to-text. Le deuxième service transformera le texte en une action (ou une intention), c’est le service LUIS (Language Understanding). Une fois que le joueur aura résolu l’énigme, il utilisera la souris pour écrire le code qui va délivrer le personnage. Un dernier service cognitif de computer vision permettra d’extraire le texte écrit à l’écran puis on vérifiera si le code est bon et si le joueur a gagné.
</p>

## Architecture générale

<p align="justify">
Au niveau des différents composants du projet, nous développons le jeu sous Unity. Nous souhaitons que ce jeu soit disponible sous la forme d’une application web. Nous utiliserons donc le format d’exportation WebGL qui créera un build et une page html à partir du projet Unity. Il suffira de lancer cette page html pour avoir accès à l’application. 
</p>

<p align="justify">
Nous devons aussi créer les services cognitifs, les entraîner et les mettre en production dans le cloud Azure (dans un premier temps). Une fois que les services sont prêts, on les appellera depuis le code Unity. Ces services seront appelés à chaque fois que nous donnerons un ordre au personnage afin de transformer ces ordres en actions concrètes.  
</p>

<p align="justify">
Nous ne pouvons pas utiliser le microphone de l’ordinateur ou d’un smartphone directement sous Unity WebGL sans package payant. On passera ainsi par du javascript pour demander l’accès du microphone à la page web et appeler un service cognitif qui va d’abord transformer l’audio en texte puis nous enverrons le texte à Unity qui appellera un autre service cognitif qui transformera le texte en une action.
</p>

<p align="justify">
Nous pouvons résumer les différentes communications entre les composantes dans le schéma ci-dessous : 
</p>

<p align="center">
  <img width="425" height="350" src="/Pictures/Archi3b.jpg">
</p>

<p align="justify">
Pour rendre accessible le jeu sous forme d'une application Web, nous créons un conteneur dans un Azure Storage pour les différents éléments du projet. L’application sera ensuite disponible à partir d'un lien http.
</p>

<p align="justify">
Dans la suite de cet article, nous allons reprendre les différentes étapes de conception du projet pour les expliquer et les détailler. Nous verrons en particulier l’insertion des services cognitifs dans l'application, le développement du jeu à partir de l’utilisation de ces services, le déploiement des services cognitifs @Edge et le déploiement de l’application web à l’aide d’Azure.
</p>

## Création des services cognitifs

### Création du service speech-to-text et du service OCR (Computer Vision)

<p align="justify">
Ces deux services cognitifs sont des services utilisables directement. Il ne nécessite pas d’entraînement préalable de modèles. Pour pouvoir utiliser ce genre de service, il suffit de créer une seule ressource Azure Cognitive Service. On pourra ensuite récupérer le point de terminaison et la clé d’authentification. A partir de ces deux informations, on pourra envoyer des requêtes vers tous les cognitives services qui ne requièrent pas d’entraînement spécifique.
</p>

<p align="justify">
On commence par chercher Azure Cognitive Service pour la créer. On clique sur "Créer" et on arrive sur la page ci-dessous :
</p>

<p align="center">
  <img src="/Pictures/screen1_cognitive.png">
</p>

<p align="justify">
On précise un certain nombre d'informations comme l'abonnement puis le groupe de ressource dans lequel la ressource est créée. On affecte également une région, un nom et une tarification à cette ressource. On clique ensuite sur "Vérifier + Créer" puis "Créer". Une fois que le déploiement est terminé, on accède à la ressource. 
</p>

<p align="justify">
La ressource est maintenant créée. On peut l'utiliser pour les services speech-to-text et OCR computer vision à l'aide de requête REST. Pour cela, il faut au préalable récupérer la clé et le point de terminaison. On clique sur "Clés et point de terminaison" dans la ressource pour avoir accès à ces informations. Voici le résultat :
</p>

<p align="center">
  <img src="/Pictures/screen3_cognitive.png">
</p>

<p align="justify">
La création est terminée. Nous pouvons désormais créer du code et lancer des requêtes vers différents services d'intelligence artificielle d'Azure Cognitive Service. Nous verrons cela dans les prochaines parties.
</p>

### Création et entrainement du service LUIS

<p align="justify">
La création du service LUIS est différente. En effet, c’est un service qu’il faut entraîner et personnaliser à notre cas d’utilisation. Pour cela, on va d’abord créer un service LUIS. Il faut affecter le service à un groupe de ressource et lui donner un nom. Ensuite, on entre des informations d'emplacement du service et des informations de tarification. Le service se situe en Europe et il est gratuit dans la limite de 5 appels par seconde et d'un million d'appels par mois.
</p>

<p align="center">
  <img src="/Pictures/screen5_cognitive.png">
</p>

<p align="justify">
Une fois le service créé, j'accède au portail LUIS. Ce portail va me permettre de configurer mon application. Je n'ai pas besoin d'écrire de code, c'est très intuitif. Je commence par ajouter une nouvelle application en cliquant sur le bouton "New App" (voir ci-dessous). Une application "action_perso" est créée.  
</p>

<p align="center">
  <img src="/Pictures/screen6_cognitive.png">
</p>

<p align="justify">
Nous allons maintenant configurer cette application. L'objectif de LUIS est d'affecter une action précise à partir d'une phrase. Cette action se décline en deux niveaux d'abstractions différentes. Il y a les intentions et les entités. Prenons un exemple. La phrase "Je veux aller à gauche de 3 pas" possède une intention et deux entités. Le sens global de la phrase correspond à l'intention. Dans cet exemple, l'intention est de se déplacer. Les entités sont des précisions sur l'intention. Ici, on a les entités "nombre de pas" et "Direction" qui ont respectivement les valeurs "gauche" et "3". Il faut donc paramétrer les intentions et les entités. Pour notre jeu, nous avons ici 5 intentions (Code, Déplacement, Décalage, Ouvrir, Vue) en plus de l'intention de base (None). Quand on enverra une phrase vers ce service, il nous indiquera quelle est l'intention de la phrase. Si la phrase ne correspond à aucune intention, il renverra l'intention "None". Voici ce que l'on obtient sur l'application :
</p>

<p align="center">
  <img src="/Pictures/screen7_cognitive.png">
</p>

<p align="justify">
Il faut ensuite définir les entités. Les entités ne sont pas obligatoires. Dans notre jeu, nous n'avons, pour la plupart des intentions, pas besoin de plus de précisions. Il n'y a donc aucune entité attachée à ces intentions. Par contre, nous avons besoin de plusieurs entités pour l'intention de déplacement par exemple. Pour un déplacement, nous voulons avoir la direction, la vitesse de déplacement et la quantité du déplacement (le nombre de pas). 
</p>

<p align="center">
  <img src="/Pictures/screen9_cognitive.png">
</p>

<p align="justify">
Il existe plusieurs types d'entités. On peut prendre des entités préconstruites pour repérer un nombre, une date, une heure dans une phrase. Nous avons utilisé l'entité préconstruite "number" dans notre cas. Un autre type d'entité est la liste. C'est celle que nous avons choisie pour l'entité "Direction". Comme on peut le voir ci-dessous, il suffit d'attribuer différentes valeurs à l'entité "Direction". On peut aussi affecter des synonymes. Par exemple, on peut dire que l'on veut aller "tout droit" ou "haut". Cela traduit une même direction. 
</p>

<p align="center">
  <img src="/Pictures/screen10_cognitive.png">
</p>

<p align="justify">
Une fois que les intentions et les entités sont définies, il faut fournir des données à notre service pour qu'il puisse s'entraîner à affecter les bonnes intentions et les bonnes entités aux nouvelles phrases. Pour cela, on fournit différentes phrases d'entraînement à chaque intention. Il reconnaîtra automatiquement les entités associées à ces intentions comme on peut le voir dans la capture d'écran suivante :
</p>

<p align="center">
  <img src="/Pictures/screen8_cognitive.png">
</p>

<p align="justify">
On peut ensuite entraîner notre application, il suffit de cliquer sur le bouton prévu à cet effet en haut à droite. On va pouvoir ensuite tester le service avant de le déployer. Dans l'onglet "Test", on entraîne une phrase, par exemple "tu peux tourner à droite". On s'aperçoit que le service à bien détecter l'intention de se déplacer et qu'il a repéré l'entité "Direction" avec la valeur "droite". 
</p>

<p align="center">
  <img src="/Pictures/screen11_cognitive.png">
</p>

<p align="justify">
L'application peut maintenant être déployée. Il suffit de cliquer sur le bouton "Publish". On choisit ensuite "Production slot" puis on valide.
Lorsque l'application sera utilisée pour scorer de nouvelles données, l'application conservera l'historique. Si des prédictions sont mauvaises, on pourra alors affecter les bonnes intentions et les bonnes entités à ces nouvelles phrases pour effectuer un nouvel entraînement puis un nouveau déploiement afin d'améliorer le service. 
</p>

<p align="center">
  <img src="/Pictures/screen12_cognitive.png">
</p>

<p align="justify">
L'application est maintenant entraînée et déployée. On peut se rendre dans l'onglet "Manage" pour obtenir les informations sur l'application. On a accès à l'identifiant de l'application ou encore le point de terminaison et les clés primaires et secondaire. On a aussi un exemple de requêtes que l'on peut utiliser pour interroger l'application.
</p>

<p align="center">
  <img src="/Pictures/screen13_cognitive.png">
</p>

<p align="justify">
Nos services cognitifs sont créés, nous pouvons maintenant les intégrer dans notre jeu. 
</p>

## Intégration de ces services dans le jeu

### Intégration du service speech-to-text

<p align="justify">
Le premier service que l’on souhaite intégrer dans notre jeu est le service speech-to-text. Ce service cognitif nécessite un accès au microphone de l’ordinateur puisqu’il prend en entrée l'enregistrement de notre voix. L’usage du microphone par Unity WebGL n’est pas possible directement. Pour contourner ce problème, nous allons utiliser javascript pour demander l'accès au microphone et utiliser ce service. Il faudra au préalable installer le package speechSDK d'Azure.
</p>

#### Envoie d'informations de Unity vers Javascript

<p align="justify">
Au niveau du jeu, lorsqu’on appuie sur le bouton “Start Recognize” pour commencer la reconnaissance vocale, il va en réalité envoyer une information au Javascript pour lui indiquer qu’il doit demander l’accès au microphone et commencer la reconnaissance vocale. Pour créer cette connexion, on commence par importer la librairie suivante : 
</p>

```c#
using System.Runtime.InteropServices;
```
<p align="justify">
On définit puis on implémente la fonction RecognizedSpeech(). Cette fonction permet d'appeler une fonction écrite en Javascript. 
</p>

```c#
[DllImport("__Internal")]
private static extern void RecognizedSpeech();
```

<p align="justify">
La fonction externe définie précédemment peut être appelée quand on veut, mais il faudra tout d’abord l’initialiser et c’est dans le répertoire des Assets de Unity que l’on va le faire. Dans ce dossier, créer un sous dossier nommé Plugins et créer un fichier en .jslib où l’on ajoutera notre fonction à l'intérieur, ce qui donne dans notre cas: Assets > Plugins > SpeechRecognized.jslib

Et c’est donc dans ce fichier là que la fonction d’appel d’une fonction javascript est réalisée:
</p>

```js
mergeInto(LibraryManager.library, {

  RecognizedSpeech: function () {
    fromMic();
  },
});
```

<p align="justify">
Une fois que le lien est fait, lorsque l’on appelle la fonction Unity RecognizedSpeech(), ceci fait un appel à la fonction fromMic() écrite en javascript.
Cette fonction fromMic active le microphone et lance la reconnaissance vocale.
</p>

#### Appel du service speech-to-text

<p align="justify">
Dès le lancement du jeu, on initialise l’objet recognizer. Cet objet est instancié à la fin du chargement de la page internet. Pour l’instanciation de l’objet, il vous faudra différentes informations telles que la subscriptionKey et le serviceRegion pour localiser le bon service cognitif dans le cloud. Il vous faudra aussi connaître la langue de reconnaissance de l’enregistrement. Toutes ces informations sont disponibles sur la page Azure du service. On précise aussi que le microphone que l’on souhaite utiliser est celui par défaut.
</p>

```js
var speechConfig = SpeechSDK.SpeechConfig.fromSubscription(auth.subscriptionKey, auth.serviceRegion);
speechConfig.speechRecognitionLanguage = auth.language;
var audioConfig  = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);
```

<p align="justify">
Une fois l'objet recognizer instancié, il suffit de lancer la fonction startContinuousRecognitionAsync() du module speechSDK dans la fonction fromMic() pour commencer l'enregistrement. On appellera la fonction stopContinuousRecognitionAsync() pour stopper l'enregistrement.
</p>

```js
recognizer.startContinuousRecognitionAsync();

recognizer.stopContinuousRecognitionAsync();
```

<p align="justify">
Les deux fonctions ci-dessus utiliseront différents callbacks. On utilisera ici le callback recognized, car il nous retourne la chaîne de caractères de la reconnaissance vocale après un temps de pose dans la voix. Ce callback est différent du callback recognizing qui lui permet d’envoyer en continue la reconnaissance. Le callback recognized sera appelé à chaque nouvelle phrase. Voici le callback recognized :
</p>

```js
recognizer.recognized = (s, e) => {
  ReturnValue = e.result.text;
  unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
};
```

#### Envoie de données de Javascript vers Unity

<p align="justify">
Une fois que l'on a la chaîne de caractères, on va l'envoyer vers l'instance du jeu Unity. On utilise la fonction SendMessage pour cela. Le premier argument ('JavascriptHook') correspond au nom du GameObject dans la scène de Unity, le second argument correspond à la fonction d’un script qu’il y a dans le GameObject Unity et les derniers arguments sont des paramètres de fonction. La fonction Unity ReturnRecognizeSpeechText sera alors appelée avec pour argument 'ReturnValue' qui est la chaîne de caractères reconnue par le service de reconnaissance vocale. 
Nous avons ainsi la fonction suivante :
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
Nous récupérons sous Unity la chaîne de caractères dans la variable sRecognizedSpeech. Nous avons ainsi intégré le premier service dans notre application. 
</p>

### Intégration du service LUIS

<p align="justify">
Nous allons maintenant intégrer le service LUIS (Language Understanding) à notre application. Ce service prendra en entrée la chaîne de caractères reconnue par le service précédent et nous donnera l’action que doit réaliser le personnage. Dans la fonction ci-dessus (ReturnRecognizeSpeechText), on reçoit la chaîne de caractères depuis javascript puis on lance une coroutine qui appellera le service LUIS. On note que la fonction setRecognizeSpeechText sert à afficher la phrase reconnue à l’écran.
</p>

<p align="justify">
Voici la fonction qui appelle LUIS par rapport à ce que nous avons dit pour pouvoir récupérer un ordre donné :
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
Dans cette fonction, on envoie une requête de type GET contenant le point de terminaison et la clé d'authentification au service (variable luisEndpoint) ainsi que le texte que l'on veut analyser. On utilise les coroutines pour les appels d’API dans Unity, car elles sont exécutées de manière asynchrone afin de récupérer le corps de la requête GET. On récupère les résultats sous la forme d'un Json qui est la réponse donné par le service. 
</p>


<p align="justify">
Par exemple, envoyons la requête GET suivante au service LUIS :
</p>

```python
https://test-ia-cognitive-service.cognitiveservices.azure.com/luis/prediction/v3.0/apps/08dc6c5a-edc4-4e63-94bd-02020bad0437/slots/production/predict?subscription-key=2c54a2e263e**************5a838fa2&verbose=true&show-all-intents=true&log=true&query="je veux tourner à droite"
```

<p align="justify">
Nous envoyons le texte "Je veux aller à droite". La réponse obtenue est le fichier JSON suivant contenant l'ensemble des intentions que nous avons programmées. Un score est attribué à chacune de ces intentions. Ici, l'intention qui a obtenu le score maximal est le déplacement. On peut en conclure que le personngage doit effectuer l'action de se déplacer. Le fichier nous donne aussi les entités associées à la top intention. Ici, on a l'intention de se déplacer vers la direction droite.
</p>

```json
{
    "query": "\"je veux tourner à droite\"",
    "prediction": {
        "topIntent": "Deplacement",
        "intents": {
            "Deplacement": {
                "score": 0.62555987
            },
            "None": {
                "score": 0.0708326
            },
            "Vue": {
                "score": 0.021084117
            },
            "Décalage": {
                "score": 0.016989253
            },
            "Code": {
                "score": 0.00955888
            },
            "Ouvrir": {
                "score": 0.008551776
            }
        },
        "entities": {
            "Direction": [
                [
                    "droite"
                ]
            ],
            "$instance": {
                "Direction": [
                    {
                        "type": "Direction",
                        "text": "droite",
                        "startIndex": 19,
                        "length": 6,
                        "modelTypeId": 5,
                        "modelType": "List Entity Extractor",
                        "recognitionSources": [
                            "model"
                        ]
                    }
                ]
            }
        }
    }
}
```

<p align="justify">
Lorsque nous recevons ce fichier Json sous Unity, il faut le désérialiser pour pouvoir l'analyser.
</p>

<p align="justify">
Un package pratique de Unity permet de désérialiser facilement des Jsons pour ensuite les utiliser dans l’application. Le package peut-être installé facilement depuis le package manager d'Unity, accessible ici: Window > Package Manager.
</p>

<p align="center">
  <img src="/Pictures/JsonPackage.png">
</p>

<p align="justify">
Un autre outil extrêmement utile pour la désérialisation de classe est le lien https://json2csharp.com/ afin de traduire le corps de la requête reçu en classe C#.
</p>

<p align="justify">
PS: Cocher ces cases suivantes :
</p>
<p align="center">
  <img src="/Pictures/Json2Csharp.png">
</p>

<p align="justify">
Cela permet d'ajouter au sein de la classe retournée les attributs des propriétés Json afin de correspondre correctement au corps de la requête sans pour autant avoir le même nom d'attribut. Comme on peut avoir certains problèmes comme un nom d'attribut $instance par exemple qu'Unity ne permettrait pas.
</p>

```c#
[JsonProperty("$instance")]
public Instance Instance { get; set; }
```

<p align="justify">
Afin d’analyser la réponse retournée, nous allons définir une fonction supplémentaire qui traitera ces données. Voici le corps de la fonction :
</p>

```c#
private void AnalyseResponseElements(JsonDataOfLUIS.Root aQuery)
{
  switch (aQuery.Prediction.TopIntent)
  {
  	// Mettre l'ensemble des intentions définies ici
	// ...
  }
}
```
<p align="justify">
On récupère la top intention puis on va effectuer certaines actions en fonction de la top intention récupérée. Ici, on exécute une action par rapport à la top intention mais on pourrait effectuer une action à partir d'un certain score. Par exemple, si la top intention n'obtient pas un score de 0.6 minimums, aucune action est enclenchée. 
	
On effectue un switch sur l'intention ayant le score le plus élevé (TopIntent) parmi toutes les intentions que nous avons ajoutées dans notre LUIS. Depuis cette fonction, on pourra effectuer les actions souhaitées par rapport à la TopIntent.

</p>

<p align="justify">
Dans le prochain extrait de code, voyons le cas du déplacement du personnage :  
Dans l'exemple suivant, nous avons une seule entité associée à l'intention du déplacement. Dans notre jeu, nous avons en fait plusieurs entitées pour chaque intention. Si la valeur de l'entité est "droite". Le personnage ira vers la droite tant qu'il peut. S'il y a un nombre dans nos entités, le personnage se déplacera d'un certain nombre de pas vers la direction indiquée.
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
    // OTHER CASE ...
}  
```
<p align="justify">
Si la top intention est le déplacement, on va récupérer la liste des entités liées à l'action du déplacement. Pour chaque direction, on va appliquer un certain nombre d'actions sur le personnage pour qu'il puisse se déplacer. La classe pmPlayerMovement est la classe permettant de déplacer le personnage. Nous avons seulement vu l'action du déplacement, mais il faut affecter des actions aux autres intentions programmées dans le LUIS.
</p>


<p align="justify">
Nous avons intégré le second service cognitif. Grâce à ces deux services cognitifs, nous pouvons diriger le personnage à partir de notre voix et lui demander d'effectuer certaines actions.
</p>

### Intégration du service de computer vision

<p align="justify">
Nous cherchons à intégrer un dernier service cognitif. Nous allons envoyer une image vers ce service cognitif qui nous renverra le texte contenu sur cette image. C'est un algorithme d'extraction de textes sur une image. C'est le service OCR d'Azure Computer Vision. Plus précisément, il va falloir écrire un code à l'aide de la souris. Si le code est correct, cela signifie que l'énigme est résolue et que le joueur a gagné. Lorsque le joueur souhaitera tester le code, le système prendra une capture d'écran et enverra cette capture vers le service OCR. Le service OCR nous renverra le code qui sera testé pour voir s'il est correct. </p>
	
	
<p align="justify">
L'utilisation de ce service dans notre application implique l'appel de requêtes POST puis GET. Nous effectuons une requête POST pour l’envoi des données (ie l’image pour la recherche de textes). Le service nous renverra alors un JSON contenant le lien vers la réponse que l'on souhaite. Nous enverrons une deuxième requête GET pour récupérer le texte reconnu, toujours sous la forme d'un fichier JSON.
</p>

<p align="justify">
Voici le code Unity permettant d'effectuer une capture d'écran :
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
Le code ci-dessous va permettre d'envoyer la requête POST vers le service. Il faut au préalable récupérer les différentes données comme l'authorizationKey et le point de terminaison dans le portail Azure pour pouvoir se connecter au service.
</p>

```c#
public IEnumerator AnalyseLastImageCaptured()
{
	//POST
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
Nous récupérons dans le code ci-dessus le lien du fichier JSON de la réponse dans la variable sResponseHeader. Nous appelons ensuite la fonction AnalyseResult() avec ce lien comme argument. Dans la fonction AnalyseResult() (voir ci-dessous), nous allons exécuter une nouvelle requête GET afin d’obtenir le résultat puis de le désérialisé dans une classe comme nous avons fait dans le service LUIS. Voici le code correspondant :
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
Nous avons enfin la fonction permettant de savoir si nous avons gagné et donc si le code reconnu par le service OCR était correct. Il suffit de tester le code reconnu par le service avec le code que le joueur devait obtenir. Une série d'actions se déclenche si la partie est gagnée. Voici la fonction AnalysedResponseElements :
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

<p align="justify">
Nous avons intégré ce troisième service cogntif à notre application. Nous pouvons maintenant diriger le personnage et réaliser certaines actions à l'aide des services speech-to-text et LUIS puis faire de l'extraction de textes avec le service cognitif OCR de computer vision. Le processus d'appel des services stockés dans le cloud ou @Edge est le même. Une fois que les services sont prêts, il suffit d'envoyer des requêtes REST avec des données puis de récupérer les résultats. 
</p>

## Infrastructure Serverless

### Les services cognitifs Serverless
### Passage par une Azure Function
### Publication de l'application Serverless

## Les services cognitifs d'Azure : principes et cas d'utilisation

<p align="justify">
Nous avons vu que les services cognitifs d’Azure permettent d’infuser des intelligences artificielles très performantes dans une application de manière simple et rapide. Il existe soit des services qui sont directement prêt à l'emploi (comme le service speech-to-text), soit des services qu’il faut entraîner (LUIS) pour le personnaliser à un cas d'usage précis.  
</p>

<p align="justify">
L'avantage des services cognitifs est qu’ils sont entraînés ou pré-entraînés avec une énorme quantité de données. Cela explique la qualité et la performance des IAs construites. Pour les services pré-entraînés, il suffit de leur apporter de nouvelles données pour finaliser l'entraînement et les personnaliser à un scénario. Plus la quantité de ces nouvelles données est importante, plus l’IA sera performante. Il n’est cependant pas forcément nécessaire d’avoir une quantité importante de nouvelles données pour avoir une IA efficace (car les services sont déjà pré-entraînés). 
</p>

<p align="justify">
On a aussi vu que les services sont facilement entraînables puisqu’ils disposent d’une interface web pour cela et qu’il n’y a pas besoin d’écrire de code lors de la création de l’IA. Une fois que ces services sont prêts, ils sont facilement déployables et accessibles via un point de terminaison. Ils sont stockés dans le cloud Azure. Il suffit ensuite de les intégrer à notre solution par des appels d’APIs. 
</p>

<p align="justify">
Le déploiement classique des services cognitifs est donc un déploiement dans le cloud. On peut aussi les déployer @Edge. En effet, lorsque le système n’a pas d’accès internet constant ou si l’IA doit retourner des réponses le plus rapidement possible, il est préférable d’embarquer l’IA avec le système et ainsi éviter le temps de latence du cloud pour améliorer le temps de réponse. C’est ce que nous avons fait dans le KatAz game. On utilise souvent les technologies @Edge pour les scénarios faisant intervenir l’IoT.
</p>

<p align="justify">
L’idée de notre projet était donc d’inclure de l’intelligence artificielle dans notre jeu. Ceci démontre que l’on peut utiliser l’IA dans plein de domaines différents en apportant une réelle valeur. L’utilisation de ces services cognitifs peut intervenir dans des milliers de cas d’usage différents et dans tous les secteurs possibles. Nous avons intégré ces services dans un jeu mais nous pouvons les intégrer sur des machines ou des systèmes. Ces services permettent de faire de l’analyse de textes ou de langages. On peut par exemple transcrire du texte en audio ou de l’audio en texte, on peut analyser des textes ou utiliser LUIS et QnA Maker pour des chatbots par exemple. On utilise aussi ces services pour de la computer vision. On peut ainsi analyser des visages à partir de photos ou de vidéos, faire de l’extraction de textes à partir d’images ou analyser le contenu d’images ou de vidéos. Prenons un premier exemple au niveau de la grande distribution. 
</p>

<p align="justify">
Dans un magasin alimentaire par exemple, nous pouvons utiliser des caméras puis un service de computer vision permettant de détecter si l’un des rayons du magasin est vide. En fonction des résultats, une alerte est envoyée pour réapprovisionner les stocks. Cela permettrait ainsi d’éviter à la fois la colère des clients et la perte d’une partie du chiffre d'affaires. On peut aussi utiliser un service de computer vision pour compter le nombre de clients et détecter leur sexe et leur âge. Il suffit de posséder une caméra et d’envoyer les enregistrements à un service cognitif préalablement entraîné qui analysera les images. Cela permettra de connaître le profil des clients pour chaque plage horaire et ainsi d’orienter la politique commerciale en fonction des résultats précédents.
</p>

<p align="justify">
On peut aussi utiliser un autre exemple. Prenons un entrepôt qui collecte, stocke et restitue des colis. Un ouvrier est chargé de confectionner des colis comportant plusieurs pièces qui sont stockées à des endroits différents de l'entrepôt. Il possède un chariot élévateur. On peut utiliser des services cognitifs permettant de déclencher une action à partir des ordres vocaux de l’employé. La personne indique l’allée et le numéro du rayon du produit et le chariot élévateur se déplace automatiquement. Une fois que la mise en préparation du colis est terminée, on peut utiliser une caméra qui scanne l’étiquette sur le colis, puis envoyer les images à un service cognitif qui détectera les informations contenues sur le colis. Ces informations seront par la suite inscrites dans une base de données. 
</p>


## Conclusion

<p align="justify">
Nous espérons que cet article et ce projet vous permettront d’avoir une vision précise des possibilités qu'offrent les services cognitifs d’Azure. 
</p>
