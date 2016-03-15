#include "stdafx.h"


TypeOf_CheckFaceQuality	CheckFaceQuality = NULL;

static HINSTANCE hmod=NULL;

bool Load_FaceQualityChecker()
{	
	
	if(hmod) return true;		
	hmod=LoadLibrary("face_quality_checker.dll");	
	
	if(!hmod) return false;
	
#define INIT_PROC(proc_name) proc_name=(TypeOf_##proc_name)GetProcAddress(hModule, #proc_name)
		
	INIT_PROC(CheckFaceQuality);
	
	if(!(CheckFaceQuality ))
	{
		FreeLibrary(hmod);
		hmod=NULL;
		return false;
	}	
	return true;
}

