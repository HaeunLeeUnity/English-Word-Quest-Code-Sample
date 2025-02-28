package com.superpixel.plugin;

import android.app.Activity;
import android.speech.tts.TextToSpeech;
import android.util.Log;
import java.util.Locale;
import android.os.Bundle;

public class TTSPlugin {
    private static TextToSpeech tts;
    private static Activity unityActivity;
    private static float currentVolume = 1.0f;

    public static void initializeTTS(Activity activity) {
        unityActivity = activity;
        tts = new TextToSpeech(activity, status -> {
            if (status == TextToSpeech.SUCCESS) {
                tts.setLanguage(Locale.US);
            } else {
                Log.e("TTSPlugin", "Initialization failed");
            }
        });
    }

    public static void speak(String text) {
        if (tts != null) {
            Bundle params = new Bundle();
            params.putFloat(TextToSpeech.Engine.KEY_PARAM_VOLUME, currentVolume);
            
            tts.speak(text, TextToSpeech.QUEUE_FLUSH, params, null);
        }
    }

    public static void shutdownTTS() {
        if (tts != null) {
            tts.stop();
            tts.shutdown();
        }
    }

    public static void setPitch(float pitch)
    {
        if (tts != null)
        {
            tts.setPitch(pitch);
        }
    }
    
    public static void setSpeechRate(float rate) {
            if (tts != null)
            {
                tts.setSpeechRate(rate);
            }
    }
    
    public static void setVolume(float volume)
    {
           currentVolume = volume;
    }
}
