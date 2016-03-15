#include "stdafx.h"
#include "FaceRecognitionObject.h"


AUTO_CRIT FaceRecognitionObject::createFaceRecognition;

FaceRecognitionObject::FaceRecognitionObject()
{
	AUTO_LOCK _aa(createFaceRecognition);
	extractor = nullptr;
	verifier = nullptr;


	extractor = CreateFaceIdFactory()->CreateFaceKeyExtractor();
	verifier = CreateFaceIdFactory()->CreateFaceKeyVerifier();

	FaceCount = -1;

	IsFree = true;
}

FaceRecognitionObject::~FaceRecognitionObject()
{
	if (verifier != nullptr)
	{
		//printf("Verifier release\n");
		verifier->Release();
	}
	if (extractor != nullptr)
	{
		//printf("Extractor release\n");
		extractor->Release();
	}

}

void FaceRecognitionObject::ReleaseHandle(PBYTE key)
{
	AUTO_LOCK _aa(crit__);
	delete[] key;
}

int FaceRecognitionObject::GetVectorSize()
{
	AUTO_LOCK _aa(crit__);

	faceindexing2_sdk::EFACEID_RESULT res;
	faceindexing2_sdk::S_IMAGE img;
	ZeroMemory(&img, sizeof(img));
	img.nHeight = 100;
	img.nWidth = 100;
	img.nStride = 100;
	img.pPixels = new PBYTE[100 * 100];
	IFaceKey *k = verifier->CreateEmptyKey();
	faceindexing2_sdk::S_FACE_POINTS *fp
		= new faceindexing2_sdk::S_FACE_POINTS();
	fp->lEyeX = 0;
	fp->lEyeY = 0;
	fp->rEyeX = 50;
	fp->rEyeY = 0;
	res = extractor->Extract(&img, fp, nullptr, &k);
	static int vectorsize = k->GetVectorSize();

	k->Release();

	return vectorsize;
}

PBYTE FaceRecognitionObject::ExtractKey(int w, int h, int stride, void* pixels, int templateSize, faceindexing2_sdk::S_FACE_POINTS *fp)
{
	AUTO_LOCK _aa(crit__);
	faceindexing2_sdk::EFACEID_RESULT res;
	faceindexing2_sdk::S_IMAGE img;
	ZeroMemory(&img, sizeof(img));
	img.nHeight = h;
	img.nWidth = w;
	img.nStride = stride;
	img.pPixels = pixels;
	IFaceKey *k = verifier->CreateEmptyKey();
	res = extractor->Extract(&img, fp, nullptr, &k);

	PBYTE val = ExportKey(k, templateSize);
	k->Release();

	return val;
}

double FaceRecognitionObject::VerifyKeys(PBYTE t1, int templateLength1, PBYTE t2, int templateLength2)
{
	AUTO_LOCK _aa(crit__);
	float score = -1;
	auto k1 = verifier->CreateEmptyKey();
	k1->Import(templateLength1, t1);
	auto k2 = verifier->CreateEmptyKey();
	k2->Import(templateLength2, t2);
	verifier->GetScore(k1, k2, &score);
	k1->Release();
	k2->Release();
	return score;
}

double FaceRecognitionObject::VerifyFaceKeys(faceindexing2_sdk::IFaceKey* k1, faceindexing2_sdk::IFaceKey* k2)
{
	float score = -1;
	verifier->GetScore(k1, k2, &score);
	return score;
}

PBYTE FaceRecognitionObject::ExportKey(IFaceKey * key, int templateSize)
{
	size_t s = templateSize;
	PBYTE val = new BYTE[s];
	key->Export(&s, val);
	return val;
}

void FaceRecognitionObject::ReleaseKey(IFaceKey* key)
{
	AUTO_LOCK _aa(crit__);
	if (key != nullptr)
	{
		key->Release();
	}
}