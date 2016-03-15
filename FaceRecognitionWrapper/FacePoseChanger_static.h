////////////////////////
//FacePoseChanger.h

#ifndef _FACEPOSECHANGER_h_
#define _FACEPOSECHANGER_h_

class IFacePoseChanger
{
public:

	enum E_TYPE
	{
		eLEPP,	// LEFT EYE PUPIL POINT
		eLEIC,	// LEFT EYE INNER CENTER
		eLEOC,	// LEFT EYE OUTER CENTER
		eLEBM,	// LEFT EYE BOTTOM MIDDLE
		eLETM,	// LEFT EYE TOP MIDDLE   
		eREPP,	// RIGHT EYE PUPIL POINT
		eREIC,	// RIGHT EYE INNER CENTER
		eREOC,	// RIGHT EYE OUTER CENTER
		eREBM,	// RIGHT EYE BOTTOM MIDDLE
		eRETM,	// RIGHT EYE TOP MIDDLE   
		eLMC,	// LEFT MOUTH CENTER
		eRMC,	// RIGHT MOUTH CENTER
	};

	struct S_FEATURE
	{
		int
			x, y;

		E_TYPE
			type;
	};

	virtual bool SetSourceImage (
		const unsigned char* pGrayPixels,
		const int nWidth,
		const int nHeight,
		const int nStride
		) = 0;

	virtual bool DetectFeatureLocationsAndAssign (
		const int nFaceX,
		const int nFaceY,
		const int nFaceW,
		const int nFaceH
		) = 0;

	virtual bool SetFeatureLocations (
		const int nFeatures,
		const S_FEATURE* pFeatures
		) = 0;

	virtual bool Assign () = 0;

	virtual bool SetPose (
		const float fAngleX,
		const float fAngleY,
		const float fAngleZ,
		const float fScale
		) = 0;

	virtual bool SetLighting (
		const float fIntensity,
		const float fAngleX,
		const float fAngleY
		) = 0;

	virtual bool Render (
		unsigned char* pGrayPixels,
		const int nWidth,
		const int nHeight,
		const int nStride,
		const int nModelCenterX,
		const int nModelCenterY
		) = 0;

	virtual bool IsLicensed () = 0;

	virtual bool Release () = 0;
};

IFacePoseChanger* __cdecl CreateFacePoseChanger (
	const int nOutputWidth,
	const int nOutputHeight);

#endif //_FACEPOSECHANGER_h_
