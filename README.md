# KatAzGame -- Utilisation des services cognitives Azure dans une application Unity

<p align="center">
  <img src="/Pictures/photo_katazGame.png">
</p>

## Contenu

- [Introduction](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#introduction)
- [Architecture générale](https://github.com/Louis-GRANGE/KatAzGame/blob/main/README.md#architecure-g%C3%A9n%C3%A9rale)
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

### Intégration du service speech-to-text

### Intégration du service LUIS

### Intégration du service de computer vision


```c#
GameObject res
```

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
