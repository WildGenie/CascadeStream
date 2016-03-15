// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the FACERECOGNITIONWRAPPER_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// FACERECOGNITIONWRAPPER_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef FACERECOGNITIONWRAPPER_EXPORTS
#define FACERECOGNITIONWRAPPER_API __declspec(dllexport)
#else
#define FACERECOGNITIONWRAPPER_API __declspec(dllimport)
#endif

// This class is exported from the FaceRecognitionWrapper.dll
class FACERECOGNITIONWRAPPER_API CFaceRecognitionWrapper {
public:
	CFaceRecognitionWrapper(void);
	// TODO: add your methods here.
};

extern FACERECOGNITIONWRAPPER_API int nFaceRecognitionWrapper;

FACERECOGNITIONWRAPPER_API int fnFaceRecognitionWrapper(void);
