//////////////////////////////////////////////////////////////////////
//FaceIndexingSDKLib.h

#ifndef _FACE_INDEXING2_SDK_LIB_hpp_
#define _FACE_INDEXING2_SDK_LIB_hpp_

namespace faceindexing2_sdk
{


//////////////////////////////////////////////////////////////////////
// Common data types

/*! This enum defines result codes, returned by identification SDK. */

enum EFACEID_RESULT
{
	efidOK,           /**< No errors. */
	efidERROR,        /**< Generic errors. */
	efidOUTOFMEMORY,  /**< Out of memory. */
	efidNOTIMPL,      /**< Method not implemented, reserved for future. */
	efidPOINTER,      /**< Zero pointer was passed. */
	efidNOLICENSE,    /**< Licensing error. */
	efidABORT,        /**< This result code can be asserted in client callbacks for stopping long-time operations. */
	efidRECOGFAILED,  /**< Recognition failed on specified image. */
	efidINVALIDPARAM, /**< Invalid parameter value, or combination of parameters. */
	efidINVALIDKEY,   /**< Invalid key. */
	//...
};

/*! This structure describes eye points on user image.
 * Y=0 for bottom image line, and grows in the up direction.
 * All values are in pixels.
 * Note, that left eye means here actually right human eye, that will seen as left on image.
 * The simple rule to be sure that all is correct is that x-coordinate of the left eye
 * is less than x-coordinate of the right eye.
 */

struct S_FACE_POINTS
{
	int
		lEyeX, /**< x-coordinate of left eye. */
		lEyeY, /**< y-coordinate of left eye. */
		rEyeX, /**< x-coordinate of right eye. */
		rEyeY; /**< y-coordinate of right eye. */
};

/*! This structure describes input grayscale image.
 */

struct S_IMAGE
{
	int   nWidth;  /**< Width of an input image, in pixels. */
	int   nHeight; /**< Height of an input image, in pixels. */
	int   nStride; /**< Stride of an input image, in bytes. */
	void* pPixels; /**< Pixels pointer of an input image, bottom line first (for positive strides). */
};

/*! This structure contains info about recognized image.
 * Reserved for future use.
 */

struct S_RECOGNITION_STATE
{
};

/*! Facekey extractor parameters.
 */

typedef enum _E_EXTRACTOR_PARAM
{
} E_EXTRACTOR_PARAM;

/*! Facekey verifier parameters.
 */

typedef enum _E_VERIFIER_PARAM
{
} E_VERIFIER_PARAM;

/*! Facekey identifier parameters.
 */

typedef enum _E_IDENTIFIER_PARAM
{
} E_IDENTIFIER_PARAM;

/*! SDK interface, implemented on the SDK side.
 * Biometric vector container. Used to store and load biometric keys.
 */

class IFaceKey
{
public:

	/*! This method returns size of biometric vector in bytes.
	 */
	virtual size_t GetVectorSize () = 0;

	/*! Checks validity of the key.
	 */
	virtual bool IsValid () = 0;

	/*! Creates biometric key from exported byte record.
	 */
	virtual EFACEID_RESULT Import (
		const size_t nSize,
		const unsigned char* pnBytes)
			= 0;

	/*! Exports byte record from internal biometric key representation.
	 */
	virtual EFACEID_RESULT Export (
		size_t* pnSize,
		unsigned char* pnBytes)
			= 0;

	/*! This method releases the key
	 */
	virtual EFACEID_RESULT Release () = 0;
};

/*! SDK interface, implemented on the SDK side.
 * Creates biometric vector from gray-scale image.
 */

class IFaceKeyExtractor
{
public:
	/*! This method extracts biometric vector from user image.
	 * Memory for the vector should be allocated before.
	 * Use GetFetureSize() method to get memory allocation size.
	 * You should specify initial feature points by hand or from face detector, using S_FACE_POINTS
	 * structure and providing pFA=0.
	 */
	virtual EFACEID_RESULT Extract (
		const S_IMAGE* pImg,       /**< Source grayscale image */
		const S_FACE_POINTS* pFP,  /**< Face feature points, from face detector or set by hand. */
		S_RECOGNITION_STATE* pRS,  /**< Recognition state */
		IFaceKey** ppKey)          /**< Receives created biometric key */
			= 0;

	/*! Get misc module parameters
	 */
	virtual EFACEID_RESULT GetParam (
		const E_EXTRACTOR_PARAM eParamID,
		void* pParam)
			= 0;

	/*! Set misc module parameters
	 */
	virtual EFACEID_RESULT SetParam (
		const E_EXTRACTOR_PARAM eParamID,
		void* pParam)
			= 0;

	/*! This method releases the extractor
	 */
	virtual EFACEID_RESULT Release () = 0;
};

/*! SDK interface, implemented on the SDK side.
 * Used to store biometric vectors in RAM.
 */

class IFaceKeyStorage
{
public:
	/*! Returns the number of biometric keys stored.
	 */
	virtual EFACEID_RESULT GetVectorsCount (
		size_t* pnVectorsCount) /**< Number of vectors. */
			= 0;

	/*! Inserts biometric key into keyset.
	 */
	virtual EFACEID_RESULT InsertKey (
		IFaceKey* pKey)
			= 0;

	/*! This method releases the storage
	 */
	virtual EFACEID_RESULT Release () = 0;
};

/*! This is an abstract interface, to be implemented on the user side.
 * Used for retrieving search results from identification engine.
 */

class IResultList
{
public:
	/*! Notification about found items. Called first. */
	virtual EFACEID_RESULT SetListSize (
		int nSize) /**< Size of results list. */
			= 0;

	/*! Notification about next found item. */
	virtual EFACEID_RESULT InsertItem (
		const float fScore, /**< Biometric score. */
		int nID)            /**< ID of the corresponding biometric vector */
			= 0;

	/*! Notification about searching completion. Called last. */
	virtual EFACEID_RESULT SearchCompleted () = 0;
};

/*! This is an abstract interface, to be implemented on the user side.
 * Unused in this SDK version.
 */
class ILongTimeOperationCallback
{
public:
};

/*! SDK interface, implemented on the SDK side.
 * Verifies two biometric vectors.
 */
class IFaceKeyVerifier
{
public:

	/*! This method calculates score value for two biometric vectors.
	 */
	virtual EFACEID_RESULT GetScore (
		IFaceKey* pKeyA, /**< Key A */
		IFaceKey* pKeyB, /**< Key B */
		float* pfScore)  /**< Receives floating-point score value. Score value is a number from 0.0 to 1.0. */
			= 0;

	/*! Get misc module parameters
	 */
	virtual EFACEID_RESULT GetParam (
		const E_VERIFIER_PARAM eParamID,
		void* pParam)
			= 0;

	/*! Set misc module parameters
	 */
	virtual EFACEID_RESULT SetParam (
		const E_VERIFIER_PARAM eParamID,
		void* pParam)
			= 0;

	/*! This method creates the empty key
	 */
	virtual IFaceKey* CreateEmptyKey () = 0;

	/*! This method releases the verifier
	 */
	virtual EFACEID_RESULT Release () = 0;
};

/*! SDK interface, implemented on the SDK side.
 *
 */
class IFaceKeyIdentifier
{
public:
	/*! This method attaches the identifier to client biometric vector storage.
	 */
	virtual EFACEID_RESULT AttachToStorage (
		IFaceKeyStorage* pKeyStorage) /**< Pointer to vector storage. */
			= 0;

	/*! This method do the search in the database using vector storage, or vector storage + index if specified.
	 */
	virtual EFACEID_RESULT Identify (
		IFaceKey* pKey,                  /**< Pointer to the query biometric vector. */
		size_t nMaxResultSize,           /**< Maximum result list size. If number of vectors found is greater, than maximum allowed, only best results will returned. */
		ILongTimeOperationCallback* pCB, /**< Callback for controlling searching operation. Reserved, not used. */
		IResultList* pList)              /**< Pointer to the client-implemented results list. Results returned are sorted internally in order from best to worst. */
			= 0;

	/*! Get misc module parameters
	 */
	virtual EFACEID_RESULT GetParam (
		const E_IDENTIFIER_PARAM eParamID,
		void* pParam)
			= 0;

	/*! Set misc module parameters
	 */
	virtual EFACEID_RESULT SetParam (
		const E_IDENTIFIER_PARAM eParamID,
		void* pParam)
			= 0;

	/*! This method releases the object
	 */
	virtual EFACEID_RESULT Release () = 0;
};

/*! SDK interfaces factory.
 *
 */
class IFaceIdClassFactory
{
public:
	virtual const char* GetVersion () = 0; /**< Returns SDK version as a char string. */
	virtual bool IsLicensed () = 0;        /**< Returns 'true' if SDK is licensed, 'false' otherwise. */

	virtual IFaceKeyExtractor*        CreateFaceKeyExtractor ()        = 0; /**< Creates biometric vector extractor. */
	virtual IFaceKeyVerifier*         CreateFaceKeyVerifier ()         = 0; /**< Creates biometric vector verificator. */
	virtual IFaceKeyStorage*          CreateFaceKeyStorage ()          = 0; /**< Creates biometric vector storage. */
	virtual IFaceKeyIdentifier*       CreateFaceKeyIdentifier ()       = 0; /**< Creates biometric vector search module. */

	virtual EFACEID_RESULT Release () = 0; /**< Releases class factory. */
};

/*! Root SDK function. Call this function first to get SDK interfaces factory pointer.
 */
//IFaceIdClassFactory* CreateFaceIdFactory ();

};

typedef faceindexing2_sdk::IFaceIdClassFactory* (*TypeOf_CreateFaceIdFactory) ();
extern TypeOf_CreateFaceIdFactory CreateFaceIdFactory;

HINSTANCE Load_FaceIndexing();

#endif /*_FACE_INDEXING2_SDK_LIB_hpp_*/
