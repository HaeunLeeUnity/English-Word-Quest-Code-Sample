#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>

static AVSpeechSynthesizer *synthesizer;
static float speechRate = 0.5; // 기본 말하기 속도
static float pitchMultiplier = 1.0; // 기본 음조
static float volume = 1.0; // 기본 볼륨
static dispatch_queue_t speechQueue;

#ifdef __cplusplus
extern "C" {
#endif
    void NativeSpeak(const char *text);
    void SetSpeechRate(float rate);
    void SetPitchMultiplier(float pitch);
    void SetVolume(float vol);
    void InitializeSynthesizerAsync(void);
#ifdef __cplusplus
}
#endif

void InitializeSynthesizerAsync(void) {
    if (!speechQueue) {
        speechQueue = dispatch_queue_create("com.speech.synthesizer", DISPATCH_QUEUE_SERIAL);
    }
    
    dispatch_async(speechQueue, ^{
        if (!synthesizer) {
            synthesizer = [[AVSpeechSynthesizer alloc] init];
            NSLog(@"Synthesizer initialized asynchronously");
        }
    });
}

void NativeSpeak(const char *text) {
    if (!synthesizer) {
        InitializeSynthesizerAsync();
    }
    
    dispatch_async(speechQueue, ^{
        while (!synthesizer) {
            // synthesizer가 초기화될 때까지 잠시 대기
            [NSThread sleepForTimeInterval:0.01];
        }
        
        NSString *message = [NSString stringWithUTF8String:text];
        AVSpeechUtterance *utterance = [AVSpeechUtterance speechUtteranceWithString:message];
        
        utterance.voice = [AVSpeechSynthesisVoice voiceWithIdentifier:@"com.apple.ttsbundle.Samantha-compact"];
        utterance.rate = speechRate;
        utterance.pitchMultiplier = pitchMultiplier;
        utterance.volume = volume;
        
        if ([synthesizer isSpeaking]) {
            [synthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
        }
        
        [synthesizer speakUtterance:utterance];
    });
}

void SetSpeechRate(float rate) {
    if (rate >= 0.1 && rate <= 2.0) {
        speechRate = rate;
    } else {
        NSLog(@"Invalid speech rate. Must be between 0.1 and 2.0");
    }
}

void SetPitchMultiplier(float pitch) {
    if (pitch >= 0.5 && pitch <= 2.0) {
        pitchMultiplier = pitch;
    } else {
        NSLog(@"Invalid pitch multiplier. Must be between 0.5 and 2.0");
    }
}

void SetVolume(float vol) {
    if (vol >= 0.0 && vol <= 1.0) {
        volume = vol;
    } else {
        NSLog(@"Invalid volume. Must be between 0.0 and 1.0");
    }
}