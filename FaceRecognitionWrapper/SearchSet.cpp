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

	//промежуточный рекомендательный список, отсортированный по мере убывания меры схожести (в начале - максимальная мера)
	std::vector<SimilarityListRecord>recList2;
	for (int i = 0; i < length; i++)
	{
		float score = 0;
		verifier->GetScore(fkey, templates[i]->FaceKey, &score);

		//отбрасываются значения, не проходящие заданный порог
		if (score < identificationThreshold)
		{
			continue;
		}

		//поиск позиции для вставки нового элемента
		auto resultPos = std::find_if(
			recList2.begin(),
			recList2.end(),
			[score](SimilarityListRecord record){ return record.Score < score; });

		//Если очередной кандидат имеет меру схожести менее минимальной в списке, и список уже заполнен на максимальную длину, 
		//то такой кандидат отбрасывается
		if (resultPos == recList2.end() && recList2.size() == maxSize)
		{
			continue;
		}

		//если список уже содержит максимально допустимое количество элементов и новый элемент должен быть помещен в список,
		//то удаляется последний (с минимальной мерой схожести)
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

	//Устанавливается актуальная длина рекомендательного списка
	listLength = recList2.size();

	//Копируется рекомендательный список в выходной массив
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

	//Проверка всех шаблонов на NULL и на дубли, чтобы избежать ситуации с частичным обновлением выборки
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
