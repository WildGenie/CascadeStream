#pragma once

struct AUTO_CRIT : CRITICAL_SECTION
{
	AUTO_CRIT()
	{
		InitializeCriticalSection(this);
	}
	~AUTO_CRIT()
	{
		DeleteCriticalSection(this);
	}
	void Lock()
	{
		EnterCriticalSection(this);
	}
	void Unlock()
	{
		LeaveCriticalSection(this);
	}
};

class AUTO_LOCK
{
	AUTO_CRIT &cr;

public:
	AUTO_LOCK(AUTO_CRIT &crit) : cr(crit)
	{
		cr.Lock();
	}
	~AUTO_LOCK()
	{
		cr.Unlock();
	}
};

