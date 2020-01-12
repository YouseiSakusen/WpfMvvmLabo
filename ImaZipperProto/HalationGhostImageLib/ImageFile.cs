using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace HalationGhost.WinApps.Images
{
	public class ImageFile
	{
		public static ImageSpecification GetImageSpecification(string filePath)
		{
			using (var img = Cv2.ImRead(filePath))
			{
				return new ImageSpecification(filePath, img.Width, img.Height);
			}
		}

		public static void SplitVerticalToFile(string sourcePath, string leftImagePath, string rightImagePath)
		{
			using (var image = Cv2.ImRead(sourcePath))
			{
				var leftWidth = image.Width / 2;

				using (var leftImage = image.Clone(new Rect(0, 0, leftWidth, image.Height)))
				{
					Cv2.ImWrite(leftImagePath, leftImage);
				}

				var leftPoint = image.Width - leftWidth;

				using (var rightImage = image.Clone(new Rect(leftPoint,
															 0,
															 image.Width - leftPoint,
															 image.Height)))
				{
					Cv2.ImWrite(rightImagePath, rightImage);
				}
			}
		}
	}
}
