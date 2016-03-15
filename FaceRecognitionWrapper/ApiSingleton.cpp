#include "stdafx.h"
#include "ApiSingleton.h"


ApiSingleton * ApiSingleton::p_instance = nullptr;
AUTO_CRIT ApiSingleton::_initializeLock;

SingletonDestroyer ApiSingleton::destroyer;

//public methods

SingletonDestroyer::~SingletonDestroyer()
{
	delete p_instance;
}

void SingletonDestroyer::initialize(ApiSingleton* p)
{
	p_instance = p;
}

ApiSingleton& ApiSingleton::instance()
{
	if (!p_instance)
	{
		AUTO_LOCK lock(_initializeLock);
		if (!p_instance)
		{
			p_instance = new ApiSingleton();
			destroyer.initialize(p_instance);
		}
	}
	return *p_instance;
}

int ApiSingleton::ApiGetTemplateSize()
{
	return _templateSize;
}

int ApiSingleton::ApiInitialize(unsigned int maxThreads)
{
	if (_isInitialized)
	{
		return SdkErrors::NoError;
	}

	AUTO_LOCK lock(_initializeLock);

	if (_isInitialized)
	{
		return SdkErrors::NoError;
	}

	try
	{
		//printf("1 - Loading template extraction\n");
		_extractorLibInstance = Load_FaceIndexing();
		auto pcount = GetObjectsCount(maxThreads);
		CreateRecognitionObjects(pcount);

		printf("SDK is _isInitialized\n");

		_isInitialized = true;
		return SdkErrors::NoError;
	}
	catch (const exception& ex)
	{
		cerr << "Initialize error: " << ex.what() << endl;
		return SdkErrors::UnknownError;
	}
}

int ApiSingleton::ApiReleaseResources()
{
	if (!_isInitialized)
	{
		return SdkErrors::NoError;
	}

	AUTO_LOCK lock(_initializeLock);

	if (!_isInitialized)
	{
		return SdkErrors::NoError;
	}

	//printf("Clear recognition objects\n");
	for (unsigned int i = 0; i < recObjects.size(); i++)
	{
		delete recObjects[i];
	}
	recObjects.clear();

	if (_extractorLibInstance != nullptr)
		FreeLibrary(_extractorLibInstance);

	_isInitialized = false;
	return SdkErrors::NoError;
}

int ApiSingleton::ApiGetVersionInfo(VersionInfo *info)
{
	try
	{
		info->Major = 3;
		info->Minor = 1;
		info->Patch = 4;
		info->Suffix[0] = 'b';
		info->Suffix[1] = 'e';
		info->Suffix[2] = 't';
		info->Suffix[3] = 'a';
		return NoError;
	}
	catch (const exception& ex)
	{
		cerr << "ApiGetVersionInfo error: " << ex.what() << endl;
		return UnknownError;
	}
}

int ApiSingleton::ApiVerify(PBYTE t1, int templateLength1, PBYTE t2, int templateLength2, double &score)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (t1 == nullptr || t2 == nullptr)
	{
		return NullTemplate;
	}
	if (templateLength1 != _templateSize || templateLength2 != _templateSize)
	{
		return InvalidTemplate;
	}

	FaceRecognitionObject *f = GetFreeRecObject();
	try
	{
		score = f->VerifyKeys(t1, templateLength1, t2, templateLength2);
		f->IsFree = true;

		return NoError;
	}
	catch (const exception& ex)
	{
		cerr << "ApiVerify error: " << ex.what() << endl;
		f->IsFree = true;
		return UnknownError;
	}
}

int ApiSingleton::ApiIdentify(PBYTE temp, int templateLength, const char* setName, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (temp == NULL)
	{
		return NullTemplate;
	}
	if (setName == NULL || strlen(setName) == 0)
	{
		return InvalidSetName;
	}
	if (templateLength != _templateSize)
	{
		return InvalidTemplate;
	}

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}

	try
	{
		if (identificationThreshold <= 0)
		{
			identificationThreshold = DefaultIdentificationThreshold;
		}
		return searchSet->IdentifyTemplates(temp, templateLength, maxListLength, identificationThreshold, list, length);
	}
	catch (const exception& ex)
	{
		cerr << "ApiIdentify error: " << ex.what() << endl;
		return UnknownError;
	}
}

int ApiSingleton::ApiIdentify(PBYTE temp, int templateLength, Template** templates, int templatesCount, int maxListLength, double identificationThreshold, SimilarityListRecord** list, int& length)
{
	if (!_isInitialized)
	{
		return SdkErrors::InitializeError;
	}

	if (temp == nullptr)
	{
		return SdkErrors::NullTemplate;
	}
	if (templateLength != _templateSize)
	{
		return SdkErrors::InvalidTemplate;
	}

	try
	{
		if (identificationThreshold <= 0)
		{
			identificationThreshold = DefaultIdentificationThreshold;
		}

		auto searchSet = new SearchSet("__temp__");
		auto res = searchSet->UploadTemplates2(templates, templatesCount, _templateSize);
		if (res != SdkErrors::NoError)
		{
			delete searchSet;
			return res;
		}
		auto result = searchSet->IdentifyTemplates(temp, templateLength, maxListLength, identificationThreshold, list, length);
		delete searchSet;
		return result;
	}
	catch (const exception& ex)
	{
		cerr << "ApiIdentify error: " << ex.what() << endl;
		return SdkErrors::UnknownError;
	}
}

int ApiSingleton::ApiExtractTemplate(Image* image, FaceInfo* face, Template * t, int& keySize)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if ((face->LeftEye.X == 0 && face->RightEye.X == 0) || (face->LeftEye.Y == 0 && face->RightEye.Y == 0))
	{
		return ProcessingGeneral;
	}
	

	FaceRecognitionObject *f = GetFreeRecObject();
	try
	{
		faceindexing2_sdk::S_FACE_POINTS points;
		ZeroMemory(&points, sizeof(faceindexing2_sdk::S_FACE_POINTS));
		points.lEyeX = face->LeftEye.X;
		points.lEyeY = image->Height - face->LeftEye.Y;
		points.rEyeX = face->RightEye.X;
		points.rEyeY = image->Height - face->RightEye.Y;

		PBYTE key = f->ExtractKey(image->Width, image->Height, image->Stride, image->Data, _templateSize, &points);

		keySize = _templateSize;

		t->Data = key;
		t->DataLength = _templateSize;

		f->IsFree = true;
		return NoError;
	}
	catch (const exception& ex)
	{
		cerr << "ApiExtractTemplate error: " << ex.what() << endl;
		f->IsFree = true;
		return UnknownError;
	}
}

int ApiSingleton::ApiGetSearchSetNames(char*** names, int &length)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	try
	{
		SIZE_T stSizeOfArray = sizeof(char*) * searchSets.size();
		*names = (char**)::CoTaskMemAlloc(stSizeOfArray);
		memset(*names, 0, stSizeOfArray);
		for (unsigned int i = 0; i < searchSets.size(); i++)
		{
			(*names)[i] = (char*)::CoTaskMemAlloc(strlen(searchSets[i]->GetName()) + 10);
			strcpy((*names)[i], searchSets[i]->GetName());
		}
		length = searchSets.size();
		return NoError;
	}
	catch (const exception& ex)
	{
		cerr << "ApiGetSearchSetNames error: " << ex.what() << endl;
		return UnknownError;
	}
}

int ApiSingleton::ApiAddSearchSet(const char* setName)
{
	if (!_isInitialized)
	{
		return SdkErrors::InitializeError;
	}

	bool isNew = false;
	GetOrCreateSearchSet(setName, &isNew);
	if (true == isNew)
	{
		return NoError;
	}
	else
	{
		return DuplicateSetName;
	}
}

int ApiSingleton::ApiUploadTemplate(const char* setName, Template* temp)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (temp == NULL)
	{
		return NullTemplate;
	}
	if (setName == NULL || strlen(setName) == 0)
	{
		return InvalidSetName;
	}

	bool isNew = false;
	SearchSet * searchSet = GetOrCreateSearchSet(setName, &isNew);

	try
	{
		//printf("adding new template to set %s\n", setName);
		return searchSet->UploadTemplate2(temp, _templateSize);
	}
	catch (const exception& ex)
	{
		cerr << "ApiUploadTemplate error: " << ex.what() << endl;
		return UnknownError;
	}
}

int ApiSingleton::ApiUploadTemplates(const char* setName, Template** templates, int length)
{
	if (!_isInitialized)
	{
		return SdkErrors::InitializeError;
	}

	if (templates == NULL)
	{
		return SdkErrors::NullTemplate;
	}
	if (setName == NULL || strlen(setName) == 0)
	{
		return SdkErrors::InvalidSetName;
	}

	bool isNew = false;
	SearchSet * searchSet = GetOrCreateSearchSet(setName, &isNew);
	try
	{
		int res = searchSet->UploadTemplates2(templates, length, _templateSize);
		return res;
	}
	catch (const exception& ex)
	{
		cerr << "ApiUploadTemplates error: " << ex.what() << endl;
		return SdkErrors::UnknownError;
	}
}

int ApiSingleton::ApiClearTemplates(const char* setName)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (setName == nullptr || strlen(setName) == 0)
	{
		return InvalidSetName;
	}

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}

	return searchSet->ClearTemplates2();;
}

int ApiSingleton::ApiContainsTemplate(const char* setName, GUID templateId, bool& result)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (setName == nullptr || strlen(setName) == 0)
	{
		return InvalidSetName;
	}

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}

	return searchSet->Contains(templateId, result);
}

int ApiSingleton::ApiGetTemplatesCount(const char* setName, int& result)
{
	if (!_isInitialized)
	{
		return InitializeError;
	}

	if (setName == nullptr || strlen(setName) == 0)
	{
		return InvalidSetName;
	}

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}

	result = searchSet->templates.size();
	return NoError;
}

int ApiSingleton::ApiRemoveTemplate(const char* setName, GUID templateId)
{
	if (!_isInitialized)
		return InitializeError;

	if (setName == NULL || strlen(setName) == 0)
		return InvalidSetName;

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}
	return searchSet->RemoveTemplate2(templateId);
}

int ApiSingleton::ApiRemoveTemplates(const char* setName, long threshold, int& removedCount)
{
	if (!_isInitialized)
		return InitializeError;

	if (setName == NULL || strlen(setName) == 0)
		return InvalidSetName;

	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		return InvalidSetName;
	}
	return searchSet->RemoveTemplates2(threshold, removedCount);
}


//private methods

//¬озвращает количество объектов распознавани€, которые могут быть созданы
unsigned int ApiSingleton::GetObjectsCount(unsigned int maxThreads)
{
	SYSTEM_INFO s;
	memset(&s, 0, sizeof(SYSTEM_INFO));
	GetSystemInfo(&s);
	unsigned int pcount = s.dwNumberOfProcessors;

	//printf("Total processor count from SYSTEM_INFO = %u\n", pcount);

	if (maxThreads > 0)
	{
		pcount = maxThreads;
		//printf("System info: threads count after configuration = %u\n", pcount);
	}

	//искусственное ограничение, надо разбиратьс€ с многопоточностью
	if (pcount > 8)
	{
		pcount = 8;
		printf("Set artificial limitation of recognition object's count due tue bugs: %u\n", pcount);
	}

	return pcount;
}

void ApiSingleton::CreateRecognitionObjects(unsigned int pcount)
{
	//printf("Creating recognition objects\n");
	for (unsigned int i = 0; i < pcount; i++)
	{
		auto rec = new FaceRecognitionObject();
		if (_templateSize == 0)
		{
			_templateSize = rec->GetVectorSize();
		}
		recObjects.push_back(rec);
		//printf("Created FaceRecognition object %d\n", i);
	}

	//printf("All recognition objects are created successfully\n");
}

FaceRecognitionObject* ApiSingleton::GetFreeRecObject()
{
	AUTO_LOCK _aa(_recObjectsLock);
	while (true)
	{
		for (unsigned int i = 0; i < recObjects.size(); i++)
		{
			if (recObjects[i]->IsFree)
			{
				recObjects[i]->IsFree = false;
				return recObjects[i];
			}
		}
		Sleep(10);
	}
}

//—оздает поисковую выборку
SearchSet* ApiSingleton::CreateNewSearchSet(const char* setName)
{
	//printf("Creating new search set %s\n", setName);
	auto searchSet = new SearchSet(setName);

	searchSets.push_back(searchSet);
	//printf("New search set %s is created\n", setName);
	return searchSet;
}

SearchSet* ApiSingleton::FindSearchSet(const char* setName)
{
	auto findResult = find_if(
		searchSets.begin(),
		searchSets.end(),
		[setName](SearchSet* selection){ return strcmp(selection->GetName(), setName) == 0; });
	if (findResult == searchSets.end())
	{
		return NULL;
	}
	return *findResult;
}

SearchSet* ApiSingleton::GetOrCreateSearchSet(const char* setName, bool* isNew)
{
	AUTO_LOCK _aa(_searchSetLock);
	//printf("find search set %s\n", setName);
	auto searchSet = FindSearchSet(setName);

	if (NULL == searchSet)
	{
		//printf("Search set not found, creating new one\n");
		*isNew = true;
		return CreateNewSearchSet(setName);
	}
	else
	{
		printf("existing search set is found\n");
		*isNew = false;
		return searchSet;
	}
}