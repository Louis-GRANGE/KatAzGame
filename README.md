# KatAzGame -- Utilisation des services cognitives Azure dans une application 

<p align="center">
  <img width="925" height="450" src="/Pictures/photo_katazGame.png">
</p>

## Contenu

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
Nous avons décidé de ne pas réaliser un PoC sur un domaine précis de l’industrie mais plutôt sur un jeu pour le côté ludique. Nous aborderons d’abord la démarche générale du projet puis le développement du jeu et de l’inclusion des services cognitifs dans le jeu. Nous présenterons en dernier lieu des cas d’usages très concrets autour de l’utilisation des services cognitifs dans divers domaines comme le retail, la logistique ou encore dans l’industrie. 
</p>

<p align="justify">
Dans ce jeu de type escape game, un personnage se retrouve enfermé dans une maison. Son objectif est d’en sortir le plus rapidement possible. Il doit résoudre une énigme pour parvenir à s’échapper de cette maison. Le joueur ne dispose que de sa voix puis de sa souris pour résoudre cette énigme. Il pourra parler au personnage. Cela fera appel successivement à deux services cognitifs. Le premier transformera l’audio en texte, c’est le service speech-to-text. Le deuxième service transformera le texte en une action (ou une intention), c’est le service LUIS (Language Understanding). Une fois que le joueur aura résolu l’énigme, il utilisera la souris pour écrire le code qui va délivrer le personnage. Un dernier service cognitif de computer vision permettra d’extraire le texte écrit à l’écran puis on vérifiera si le code est bon et si le joueur a gagné.
</p>

<p align="justify">
Au niveau des différents composants du projet, nous développons le jeu sous Unity. Nous souhaitons que ce jeu soit disponible sous la forme d’une application web. Nous utiliserons donc le format d’exportation WebGL qui créera un build et une page html à partir du projet Unity. Il suffira de lancer cette page html pour avoir accès à l’application. 
</p>

<p align="justify">
Nous devons aussi créer les services cognitifs, les entraîner et les mettre en production dans le cloud Azure (dans un premier temps). Une fois que les services sont prêts, on les appellera depuis le code Unity. Ces services seront appelés à chaque fois que nous donnerons un ordre au personnage afin de transformer ces ordres en actions concrètes.  
</p>

<p align="justify">
Nous ne pouvons pas utiliser le microphone de l’ordinateur ou d’un smartphone directement sous Unity WebGL sans package payant. On passera ainsi par du javascript pour demander l’accès au microphone et appeler un service cognitif qui va d’abord transformer l’audio en texte puis nous enverrons le texte à Unity qui appellera un autre service cognitif qui transformera le texte en une action.
</p>

<p align="justify">
Une latence apparaîtra entre le moment où nous appelons nos services et le moment où l’action se répercute sur le personnage. Pour réduire cette latence, nous déploierons les services cognitifs @Edge (dans un deuxième temps). Cela va permettre d’embarquer les services cognitifs au plus près de l’application et ainsi supprimer la latence avec le cloud. 
</p>

<p align="justify">
Nous pouvons résumer les différentes communications entre les composantes dans le schéma ci-dessous : 
</p>

<p align="center">
  <img width="425" height="350" src="/Pictures/Archi2.png">
</p>

<p align="justify">
Nous souhaitons que le jeu soit disponible sous la forme d’une application web. Pour cela, nous créons un conteneur dans un Azure Storage puis nous stockons le build de notre projet et la page html créée par WebGL dans ce conteneur. L’application sera ensuite disponible à l’aide d’une URL et la page principale html sera ainsi lancée.
</p>

<p align="justify">
Dans la suite de cet article, nous allons reprendre les différentes étapes de conception du projet pour les expliquer et les détailler. Nous verrons en particulier l’insertion des services cognitifs dans l'application, le développement du jeu à partir de l’utilisation de ces services, le déploiement des services cognitifs @Edge et le déploiement de l’application web à l’aide d’Azure.
</p>

## Conclusion
