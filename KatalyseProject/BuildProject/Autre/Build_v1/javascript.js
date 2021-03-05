    //var auth = require('./auth.json');
    // subscription key and region for speech services.
    var auth = {
      "subscriptionKey" :   "2c54a2e263e647f39efdc66b5a838fa2",
      "serviceRegion"   :   "francecentral",
      "language"        :   "fr-FR"
    }


    var subscriptionKey, serviceRegion;
    var authorizationToken;
    var SpeechSDK;
    var recognizer;
    var bstartRecognize = false;

    function fromMic()
    {
        bstartRecognize = !bstartRecognize;
        if (bstartRecognize) //Start Recognize Speech
        {
          recognizer.startContinuousRecognitionAsync();
          ReturnValue = "START Speaking!!";
        }
        else //Stop Recognize Speech
        {
          ReturnValue = "STOP Speaking!!";
          recognizer.stopContinuousRecognitionAsync();
        }
        unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
    }

    //When HTML page is loaded
    document.addEventListener("DOMContentLoaded", function ()
    {
        console.log("AUTHENTIFICATION " + auth);
        var speechConfig = SpeechSDK.SpeechConfig.fromSubscription(auth.subscriptionKey, auth.serviceRegion);
        speechConfig.speechRecognitionLanguage = auth.language;
        var audioConfig  = SpeechSDK.AudioConfig.fromDefaultMicrophoneInput();
        recognizer = new SpeechSDK.SpeechRecognizer(speechConfig, audioConfig);

        var ReturnValue = "Default";
        
        console.log("CAN Recognize you SPEECH: " + bstartRecognize);


          recognizer.recognizing = (s, e) => {
            ReturnValue = e.result.text;
            unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
          };
          recognizer.recognized = (s, e) => {
            if (e.result.reason == ResultReason.RecognizedSpeech)
            {
              ReturnValue = e.result.text;
              unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
            }
            else if (e.result.reason == ResultReason.NoMatch)
            {
              console.log("NOMATCH: Speech could not be recognized.");
              ReturnValue = "NOMATCH: Speech could not be recognized.";
              unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
            }
          };

          recognizer.canceled = (s, e) => {
            console.log(`CANCELED: Reason=${e.reason}`);
            ReturnValue = "CANCELED Speaking!!";

            if (e.reason == CancellationReason.Error) {
              console.log(`"CANCELED: ErrorCode=${e.errorCode}`);
              console.log(`"CANCELED: ErrorDetails=${e.errorDetails}`);
              console.log("CANCELED: Did you update the subscription info?"); 
            }
            unityInstance.SendMessage('JavascriptHook', 'ReturnRecognizeSpeechText', ReturnValue);
            recognizer.stopContinuousRecognitionAsync();
          };

          recognizer.sessionStopped = (s, e) => {
            recognizer.stopContinuousRecognitionAsync();
          };
    });