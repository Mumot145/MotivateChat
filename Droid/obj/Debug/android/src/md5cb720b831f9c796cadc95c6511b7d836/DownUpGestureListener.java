package md5cb720b831f9c796cadc95c6511b7d836;


public class DownUpGestureListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MR.Gestures.Android.DownUpGestureListener, MR.Gestures.Android, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null", DownUpGestureListener.class, __md_methods);
	}


	public DownUpGestureListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DownUpGestureListener.class)
			mono.android.TypeManager.Activate ("MR.Gestures.Android.DownUpGestureListener, MR.Gestures.Android, Version=1.4.2.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
