#include "stdafx.h"
#include <vector>
#include <algorithm>
#include "SearchSet.h"
#include "SdkCommon.h"


int SearchSet::IdentifyTemplates(PBYTE t1, int templateLength, int MaxListCount, double identificationThreshold, SimilarityListRecord** recList, int &listLength)
{
	AUTO_LOCK aa(crit4);

	int length = templates.size();

	unsigned int maxSize = MaxListCount;
	if (maxSize > templates.size())
		maxSize = templates.size();

	faceindexing2_sdk::IFaceKey* fkey = verifier->CreateEmptyKey();
	fkey->Import(templateLength, t1);

	//������������� ���������������� ������, ��������������� �� ���� �������� ���� �������� (� ������ - ������������ ����)
	std::vector<SimilarityListRecord>recList2;
	for (int i = 0; i < length; i++)
	{
		float score = 0;
		verifier->GetScore(fkey, templates[i]->FaceKey, &score);

		//������������� ��������, �� ���������� �������� �����
		if (score < identificationThreshold)
		{
			continue;
		}

		//����� ������� ��� ������� ������ ��������
		auto resultPos = std::find_if(
			recList2.begin(),
			recList2.end(),
			[score](SimilarityListRecord record){ return record.Score < score; });

		//���� ��������� �������� ����� ���� �������� ����� ����������� � ������, � ������ ��� �������� �� ������������ �����, 
		//�� ����� �������� �������������
		if (resultPos == recList2.end() && recList2.size() == maxSize)
		{
			continue;
		}

		//���� ������ ��� �������� ����������� ���������� ���������� ��������� � ����� ������� ������ ���� ������� � ������,
		//�� ��������� ��������� (� ����������� ����� ��������)
		if (resultPos!=recList2.end() && recList2.size() == maxSize)
		{
			recList2.erase(recList2.end()-1);
		}

		SimilarityListRecord record;
		ZeroMemory(&record, sizeof(SimilarityListRecord));
		record.ID = templates[i]->ID;
		record.Score = static_cast<double>(score);
		recList2.insert(resultPos, record);
	}

	fkey->Release();

	//��������������� ���������� ����� ����������������� ������
	listLength = recList2.size();

	//���������� ���������������� ������ � �������� ������
	*recList = static_cast<SimilarityListRecord*>(CoTaskMemAlloc(listLength * sizeof(SimilarityListRecord)));
	SimilarityListRecord *rec = *recList;
	
	for (int i = 0; i < listLength; i++, rec++)
	{
		rec->ID = recList2[i].ID;
		rec->Score = recList2[i].Score;
	}
	recList2.clear();

	return NoError;
}

int SearchSet::RemoveTemplate2(GUID templateId)
{
	AUTO_LOCK aa(crit4);
	
	auto t = FindTemplate(templateId);

	if (t == templates.end())
		return InvalidTemplateId;

	auto templateToDelete = *t;
	delete templateToDelete;
	templates.erase(t);

	return NoError;
}

int SearchSet::ClearTemplates2()
{
	//printf("Clear templates for set %s\n", Name.c_str());

	AUTO_LOCK aa(crit4);
	int length = templates.size();

	for (int i = 0; i < length; i++)
	{
		TemplateInfo * t = templates[i];
		delete t;
	}
	templates.clear();
	return NoError;
}

int SearchSet::RemoveTemplates2(DATE threshold, int& removedCount)
{
	AUTO_LOCK aa(crit4);

	for (unsigned int i = 0; i >= 0 && i < templates.size(); i++)
	{
		if (templates[i]->EnrollDate < threshold)
		{
			TemplateInfo* t = templates[i];
			templates.erase(templates.begin() + i);
			i--;
			removedCount++;
			delete t;
		}
	}
	return NoError;
}

int SearchSet::UploadTemplates2(Template** newTemplates, int length, int templateSize)
{
	AUTO_LOCK aa(crit4);

	//�������� ���� �������� �� NULL � �� �����, ����� �������� �������� � ��������� ����������� �������
	for (int i = 0; i < length; i++)
	{
		auto res = CheckTemplate(newTemplates[i]);
		if (NoError != res)
		{
			return res;
		}
	}

	for (int i = 0; i < length; i++)
	{
		AddNewTemplate(newTemplates[i], templateSize);
	}
	return NoError;
}

int SearchSet::UploadTemplate2(Template *newTemplate, int templateSize)
{
	AUTO_LOCK aa(crit4);

	//printf("Check template\n");
	auto res = CheckTemplate(newTemplate);
	//printf("Check template result %d\n", res);
	if (NoError != res)
	{
		return res;
	}

	AddNewTemplate(newTemplate, templateSize);

	return NoError;
}

int SearchSet::Contains(GUID templateId, bool& result)
{
	AUTO_LOCK aa(crit4);

	auto t = FindTemplate(templateId);

	result = (t != templates.end());

	return NoError;
}
