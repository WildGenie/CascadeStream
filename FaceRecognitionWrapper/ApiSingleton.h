#pragma once
#include "stdafx.h"
#include <vector>
#include "FaceRecognitionObject.h"
#include "SearchSet.h"
#include "AUTO_LOCK.h"

using namespace std;

class ApiSingleton;

class SingletonDestroyer
{
private:
	ApiSingleton* p_instance;
public:
	~SingletonDestroyer();
	void initialize(ApiSingleton* p);
};

class ApiSingleton
{
private:
	static ApiSingleton* p_instance;
	static SingletonDestroyer destroyer;
	static AUTO_CRIT _initializeLock;


	AUTO_CRIT _searchSetLock;
	AUTO_CRIT _recObjectsLock;


	HINSTANCE _extractorLibInstance;


	int _templateSize;
	bool _isInitialized;

	vector<FaceRecognitionObject*> recObjects;
	vector<SearchSet*> searchSets;

	unsigned int GetObjectsCount(unsigned int maxThreads);
	void CreateRecognitionObjects(unsigned int pcount); 
	FaceRecognitionObject* GetFreeRecObject(); 
	SearchSet* CreateNewSearchSet(const char* setName);
	SearchSet* FindSearchSet(const char* setName);
	SearchSet* GetOrCreateSearchSet(const char* setName, bool* isNew);

protected:
	ApiSingleton() 
	{		
		_isInitialized = false;
		_templateSize = 0;
				
	}
	ApiSingleton(const ApiSingleton&);
	ApiSingleton& operator=(ApiSingleton&);
	~ApiSingleton()
	{

	}
	friend class SingletonDestroyer;

public:
	static ApiSingleton& instance();
	int ApiGetTemplateSize();
	int ApiInitialize(unsigned int maxThreads);
	int ApiReleaseResources(); 
	int ApiGetVersionInfo(VersionInfo* info);
	int ApiVerify(PBYTE t1, int templateLength1, PBYTE t2, int templateLength2, double &score);
	int ApiIdentify(PBYTE temp, int templateLength, const char* setName, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length);
	int ApiIdentify(PBYTE temp, int templateLength, Template** templates, int templatesCount, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length);
	int ApiExtractTemplate(Image* image, FaceInfo* face, Template * t, int& keySize);
	int ApiGetSearchSetNames(char*** names, int &length);
	int ApiAddSearchSet(const char* setName);
	int ApiUploadTemplate(const char* setName, Template* temp);
	int ApiUploadTemplates(const char* setName, Template** templates, int length);
	int ApiClearTemplates(const char* setName);
	int ApiContainsTemplate(const char* setName, GUID templateId, bool& result);
	int ApiGetTemplatesCount(const char* setName, int& result);
	int ApiRemoveTemplate(const char* setName, GUID templateId);
	int ApiRemoveTemplates(const char* setName, long threshold, int& removedCount);
};
