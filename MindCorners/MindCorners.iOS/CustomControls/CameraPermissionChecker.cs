using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AudioToolbox;
using AVFoundation;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraPermissionChecker))]

namespace MindCorners.Droid.CustomControl
{

public class CameraPermissionChecker: ICameraPermissionChecker
    {
		public bool HaveCameraPermissions()
		{
			AVAuthorizationStatus authStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
			return authStatus != AVAuthorizationStatus.Denied && authStatus != AVAuthorizationStatus.Restricted;
		}
	}
}