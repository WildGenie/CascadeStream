#pragma once

#include "stdafx.h"
#include <algorithm>
#include <map>
#include "SdkCommon.h"
#include "facekey_identification_common.hpp"
#include "FaceRecognitionTypes.h"
#include "AUTO_LOCK.h"

class TemplateInfo
{
public:
	GUID ID;
	PBYTE templateData;
	faceindexing2_sdk::IFaceKey* FaceKey;
	long EnrollDate;
public:
	~TemplateInfo()
	{
		//printf("Inside template destructor\n");
		if (NULL != FaceKey)
		{
			//printf("Release FaceKey\n");
			FaceKey->Release();
		}
		if (NULL != templateData)
		{
			//printf("Delete templateData\n");
			delete[] templateData;
		}
	}
};

class SearchSet
{
private:
	AUTO_CRIT crit4;
	std::string Name;
public:
	std::vector<TemplateInfo*> templates;
	faceindexing2_sdk::IFaceKeyVerifier* verifier;
public:
	SearchSet(const char* name)
	{
		AUTO_LOCK aa(crit4);

		//printf("1, constructor enter\n");
		Name.assign(name);
		//printf("2, name %s\n", Name.c_str());

		verifier = CreateFaceIdFactory()->CreateFaceKeyVerifier();

		//printf("3, verifier created\n");
	}

	~SearchSet()
	{
		verifier->Release();
	}
	const char* GetName(){ return Name.c_str(); }
	int IdentifyTemplates(PBYTE t1, int templateLength, int MaxListCount, double identificationThreshold, SimilarityListRecord** recList, int &listLength);
	int RemoveTemplate2(GUID templateId);
	int ClearTemplates2();
	int RemoveTemplates2(DATE threshold, int& removedCount);
	int UploadTemplates2(Template** newTemplates, int length, int templateSize);
	int UploadTemplate2(Template *newTemplate, int templateSize);
	int Contains(GUID templateId, bool& result);

private:
	std::vector<TemplateInfo*>::iterator FindTemplate(GUID templateId)
	{
		return find_if(
			templates.begin(),
			templates.end(),
			[templateId](TemplateInfo* t)
		{
			/*printf("t->ID = {%08lX-%04hX-%04hX-%02hhX%02hhX-%02hhX%02hhX%02hhX%02hhX%02hhX%02hhX}\n",
				t->ID.Data1, t->ID.Data2, t->ID.Data3,
				t->ID.Data4[0], t->ID.Data4[1], t->ID.Data4[2], t->ID.Data4[3],
				t->ID.Data4[4], t->ID.Data4[5], t->ID.Data4[6], t->ID.Data4[7]);*/

			return IsEqualGUID(t->ID, templateId) == TRUE;
		});
	}

	void AddNewTemplate(Template* t, int templateSize)
	{
		TemplateInfo * info = new TemplateInfo();
		info->FaceKey = verifier->CreateEmptyKey();
		info->FaceKey->Import(templateSize, t->Data);
		info->ID = t->ID;
		templates.push_back(info);
	}

	int CheckTemplate(Template* tpl)
	{
		if (NULL == tpl)
			return NullTemplate;

		auto templateId = tpl->ID;
		/*printf("templateId = {%08lX-%04hX-%04hX-%02hhX%02hhX-%02hhX%02hhX%02hhX%02hhX%02hhX%02hhX}\n",
			templateId.Data1, templateId.Data2, templateId.Data3,
			templateId.Data4[0], templateId.Data4[1], templateId.Data4[2], templateId.Data4[3],
			templateId.Data4[4], templateId.Data4[5], templateId.Data4[6], templateId.Data4[7]);*/

		/*printf("templates size %llu; begin == end %d\n", templates.size(), templates.begin() == templates.end());*/

		auto findResult = FindTemplate(templateId);

	/*	printf("findResult == begin %d\n", findResult == templates.begin());
		printf("findResult == end %d\n", findResult == templates.end());*/

		if (findResult == templates.end())
		{
			//printf("template is ok\n");
			return NoError;
		}

		//printf("found duplicate template\n");
		return DuplicateTemplateId;
	}
};

