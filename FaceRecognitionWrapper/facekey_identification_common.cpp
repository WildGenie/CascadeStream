#include "stdafx.h"
#include "facekey_identification_common.hpp"

TypeOf_CreateFaceIdFactory CreateFaceIdFactory = nullptr;



HINSTANCE Load_FaceIndexing()
{
	auto hmod= LoadLibrary("facekey_identification_cl.dll");

	CreateFaceIdFactory=reinterpret_cast<TypeOf_CreateFaceIdFactory>(GetProcAddress(hmod, "CreateFaceIdFactory"));
	if(!CreateFaceIdFactory)
	{
		FreeLibrary(hmod);
		hmod= nullptr;
		return hmod;
	}
	
	return hmod;
}
