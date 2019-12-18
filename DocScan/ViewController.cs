using Foundation;
using System;
using System.Diagnostics;
using UIKit;
using VisionKit;

namespace DocScan
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidAppear(bool animated)
        {
            var documentCameraController = new VNDocumentCameraViewController();
            var documentscanDelegate = new DocumentScanDelegate();
            documentscanDelegate.OnScanTaken += (VNDocumentCameraScan scan) =>
            {
                documentCameraController.DismissViewController(true, null);
                Debug.WriteLine($"{scan.PageCount} Pages!");
            };

            documentscanDelegate.OnCanceled += () =>
            {
                documentCameraController.DismissViewController(true, null);
            };

            documentCameraController.Delegate = documentscanDelegate;
            documentCameraController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            PresentViewController(documentCameraController, true, null);
        }
    }

    class DocumentScanDelegate : VNDocumentCameraViewControllerDelegate
    {
        public delegate void ScanTakenEventHandler(VNDocumentCameraScan scan);
        public event ScanTakenEventHandler OnScanTaken;

        public delegate void ScanCanceledEventHandler();
        public event ScanCanceledEventHandler OnCanceled;

        public override void DidCancel(VNDocumentCameraViewController controller)
        {
            Debug.WriteLine("DocumentScanDelegate:DidCancel");
            OnCanceled();
        }

        public override void DidFinish(VNDocumentCameraViewController controller, VNDocumentCameraScan scan)
        {
            Debug.WriteLine("DocumentScanDelegate:DidFinish");
            OnScanTaken(scan);
        }


        public override void DidFail(VNDocumentCameraViewController controller, NSError error)
        {
            Debug.WriteLine("Failed scanning photo");
        }
    }
}