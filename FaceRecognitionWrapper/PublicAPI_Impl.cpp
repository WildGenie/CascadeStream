#include "stdafx.h"
#include "ApiSingleton.h"

int InitializeA(unsigned int maxThreads)
{
	return ApiSingleton::instance().ApiInitialize(maxThreads);
}

int ReleaseResources()
{
	return ApiSingleton::instance().ApiReleaseResources();
}

int GetTemplateSize()
{
	return ApiSingleton::instance().ApiGetTemplateSize();
}


int GetVersionInfo(VersionInfo *info)
{
	return ApiSingleton::instance().ApiGetVersionInfo(info);
}

int Verify(PBYTE t1, int templateLength1, PBYTE t2, int templateLength2, double &score)
{
	return ApiSingleton::instance().ApiVerify(t1, templateLength1, t2, templateLength2, score);
}

int IdentifyBySearchSet(PBYTE temp, int templateLength, const char* setName, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length)
{
	return ApiSingleton::instance().ApiIdentify(temp, templateLength, setName, maxListLength, identificationThreshold, list, length);
}

int IdentifyByTemplates(PBYTE temp, int templateLength, Template** templates, int templatesCount, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length)
{
	return ApiSingleton::instance().ApiIdentify(temp, templateLength, templates, templatesCount, maxListLength, identificationThreshold, list, length);
}

int ExtractTemplate(Image* image, FaceInfo* face, Template * t, int& keySize)
{
	return ApiSingleton::instance().ApiExtractTemplate(image, face, t, keySize);
}

int GetSearchSetNames(char*** names, int &length)
{
	return ApiSingleton::instance().ApiGetSearchSetNames(names, length);
}

int AddSearchSet(const char* setName)
{
	return ApiSingleton::instance().ApiAddSearchSet(setName);
}

int UploadTemplate(const char* setName, Template* temp)
{
	return ApiSingleton::instance().ApiUploadTemplate(setName, temp);
}

int UploadTemplates(const char* setName, Template** templates, int length)
{
	return ApiSingleton::instance().ApiUploadTemplates(setName, templates, length);
}

int ClearTemplates(const char* setName)
{
	return ApiSingleton::instance().ApiClearTemplates(setName);	
}

int ContainsTemplate(const char* setName, GUID templateId, bool& result)
{
	return ApiSingleton::instance().ApiContainsTemplate(setName, templateId, result);
}

int GetTemplatesCount(const char* setName, int& result)
{
	return ApiSingleton::instance().ApiGetTemplatesCount(setName, result);
}

int RemoveTemplate(const char* setName, GUID templateId)
{
	return ApiSingleton::instance().ApiRemoveTemplate(setName, templateId);
}

int RemoveTemplates(const char* setName, long threshold, int& removedCount)
{
	return ApiSingleton::instance().ApiRemoveTemplates(setName, threshold, removedCount);
}

int ReleasePointer(PBYTE data)
{
	delete[] data;
	return NoError;
}
