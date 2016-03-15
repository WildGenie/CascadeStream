#pragma once
#include "stdafx.h"
#include "AUTO_LOCK.h"
#include "facekey_identification_common.hpp"

using namespace faceindexing2_sdk;

class FaceRecognitionObject
{
private:
	static AUTO_CRIT createFaceRecognition;
	AUTO_CRIT crit__; 

public:
	bool IsFree;
	int FaceCount;
	IFaceKeyVerifier* verifier;
	IFaceKeyExtractor* extractor;

	FaceRecognitionObject();
	~FaceRecognitionObject();
	void ReleaseHandle(PBYTE key);
	int GetVectorSize();
	PBYTE ExtractKey(int w, int h, int stride, void* pixels, int templateSize, S_FACE_POINTS *fp);
	double VerifyKeys(PBYTE t1, int templateLength1, PBYTE t2, int templateLength2);
	double VerifyFaceKeys(IFaceKey* k1, IFaceKey* k2);
	PBYTE ExportKey(IFaceKey * key, int templateSize);
	void ReleaseKey(IFaceKey* key);
};

