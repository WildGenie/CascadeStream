#pragma once
#include <stddef.h>

typedef struct ILandmarkDetector
{
	virtual ~ILandmarkDetector() {}
};

typedef struct LD_Point
{
	int x;
	int y;

	LD_Point(): x(0), y(0) {}
};
typedef struct LD_Params
{
	int width;		// input
	int height;		//
	int stride;		//
	void *pixels;		//
	LD_Point eyes[2];	//

	LD_Point landmarks[6];	// output: left/right eyes, left/right mouth corners, left/right nose wings

	LD_Params(): width(0), height(0), stride(0), pixels(NULL) {}
};

#ifdef __cplusplus
#define CV_CPLUSPLUS
#define CV_EXTERN_C extern "C"
#define CV_EXTERN_C_BEGIN extern "C" {
#define CV_EXTERN_C_END }
#else
#define CV_EXTERN_C extern
#define CV_EXTERN_C_BEGIN
#define CV_EXTERN_C_END
#endif /* __cplusplus */

/* Realtime detection interface */

CV_EXTERN_C_BEGIN

ILandmarkDetector*	LandmarkDetector_Create();
void		LandmarkDetector_Destroy(ILandmarkDetector *obj);
bool		LandmarkDetector_Detect(ILandmarkDetector *obj, LD_Params *params);
CV_EXTERN_C_END