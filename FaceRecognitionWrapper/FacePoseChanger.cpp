#include "stdafx.h"
#include "FacePoseChanger.h"

TypeOf_CreateFacePoseChanger CreateFacePoseChanger =NULL;

static HINSTANCE hmod=NULL;

bool LoadFacePoseChanger()
{
	if(hmod) return true;
	
	hmod=LoadLibrary(_T("FacePoseChanger.dll"));
	if(!hmod) return false;
	
#define INIT_PROC(proc_name) proc_name=(TypeOf_##proc_name)GetProcAddress(hmod, #proc_name)
	
	INIT_PROC(CreateFacePoseChanger);
	
	if(!(CreateFacePoseChanger))
	{
		FreeLibrary(hmod);
		hmod=NULL;
		return false;
	}
	
	return true;
}
