// FaceRecognitionWrapper.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "FaceRecognitionWrapper.h"


// This is an example of an exported variable
FACERECOGNITIONWRAPPER_API int nFaceRecognitionWrapper=0;

// This is an example of an exported function.
FACERECOGNITIONWRAPPER_API int fnFaceRecognitionWrapper(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see FaceRecognitionWrapper.h for the class definition
CFaceRecognitionWrapper::CFaceRecognitionWrapper()
{
	return;
}
