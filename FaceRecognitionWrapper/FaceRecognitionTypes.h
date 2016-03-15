#pragma once

typedef struct VersionInfo
{
	int	Major;
	int	Minor;
	int Patch;
	char Suffix[128];
} VersionInfo;


typedef struct RectangleF
{
	int X;
	int Y;
	int Width;
	int Height;
} RectangleF;

typedef struct Point
{
	int X;
	int Y;
} Point;

typedef struct FaceInfo
{
	double YawAngle;
	double PitchAngle;
	double DetectionProbability;
	Point LeftEye;
	Point RightEye;
	RectangleF FaceRectangle;
} FaceInfo;

typedef struct Image
{
	int Stride;
	int Width;
	int Height;
	unsigned char* Data;
} Image;


typedef struct Template
{
	GUID ID;
	int DataLength;
	unsigned char* Data;
} Template;

typedef struct SimilarityListRecord
{
	GUID ID;
	double Score;
	operator double()	{ return -Score; }
} SimilarityListRecord;
