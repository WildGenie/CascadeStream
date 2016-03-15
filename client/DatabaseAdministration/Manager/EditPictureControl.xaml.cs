// Decompiled with JetBrains decompiler
// Type: CascadeManager.EditPictureControl
// Assembly: Manager, Version=2.0.5674.31274, Culture=neutral, PublicKeyToken=null
// MVID: 82EB5CBD-88A7-4733-ADA4-0BF7E8DF7027
// Assembly location: D:\projects\КаскадПоток\Distr\client\DatabaseAdministration\Manager.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AForge.Imaging.Filters;
using DevExpress.XtraEditors;
using TS.Sdk.StaticFace.NetBinding;
using TS.Sdk.StaticFace.NetBinding.Utils;
using Application = System.Windows.Application;
using Brushes = System.Windows.Media.Brushes;
using Cursors = System.Windows.Input.Cursors;
using Image = System.Drawing.Image;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Pen = System.Windows.Media.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using UserControl = System.Windows.Controls.UserControl;

namespace CascadeManager
{
	public partial class EditPictureControl : UserControl, IComponentConnector
	{
		private bool _allowDrawEyes;
		public Point CanvasMainPoint = new Point(0.0, 0.0);
		public Vector ClickOffset;
		public bool CutAreaFlag = false;
		public double DpX = 96.0;
		public double DpY = 96.0;
		public IEngine Face = new Engine();
		public byte[] ImgChangedImage = new byte[0];
		public byte[] ImgDefaultPath1 = new byte[0];
		public byte[] ImgPath1 = new byte[0];
		public byte[] ImgSource1 = new byte[0];
		public bool IsChanged;
		public bool IsDraggingLeft;
		public bool IsDraggingRight;
		public bool Left1Drag;
		public bool Left2Drag = false;
		public MyVisual LeftEye1 = new MyVisual();
		public FrmPictureEdit Mainform;
		public bool Right1Drag;
		public bool Right2Drag = false;
		public MyVisual RightEye1 = new MyVisual();
		public int Scrollingcount;
		public MyVisual SelectionBorder1 = new MyVisual();
		public TransformGroup Tg1 = new TransformGroup();
		public TranslateTransform TranslateTransform1 = new TranslateTransform(0.0, 0.0);
		public MyVisual VsFace1;
		public double ZoomScale = 1.5;
		//   internal Grid Grid1;
		//   internal Border Bdr1;
		//   internal ScrollViewer ScrollViewer1;
		//   internal DrawingCanvas CanvasMain;
		//   private bool _contentLoaded;

		public EditPictureControl()
		{
			//     this.InitializeComponent();
			Tg1.Children.Add(TranslateTransform1);
			CanvasMain.RenderTransform = Tg1;
			CanvasMain.MouseLeftButtonDown += canvasMain_MouseLeftButtonDown;
			CanvasMain.MouseMove += canvasMain_MouseMove;
			CanvasMain.MouseLeftButtonUp += canvasMain_MouseLeftButtonUp;
			CanvasMain.MouseWheel += canvasMain_MouseWheel;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
				case 1:
					Grid1 = (Grid) target;
					break;
				case 2:
					Bdr1 = (Border) target;
					break;
				case 3:
					ScrollViewer1 = (ScrollViewer) target;
					ScrollViewer1.MouseRightButtonDown += canvasMain_MouseRightButtonDown;
					ScrollViewer1.MouseRightButtonUp += canvasMain_MouseRightButtonUp;
					ScrollViewer1.MouseLeftButtonDown += canvasMain_MouseLeftButtonDown;
					ScrollViewer1.MouseLeftButtonUp += canvasMain_MouseLeftButtonUp;
					ScrollViewer1.MouseMove += canvasMain_MouseMove;
					ScrollViewer1.MouseWheel += canvasMain_MouseWheel;
					ScrollViewer1.ScrollChanged += scrollViewer1_ScrollChanged;
					break;
				case 4:
					CanvasMain = (DrawingCanvas) target;
					break;
				default:
					_contentLoaded = true;
					break;
			}
		}

		public void SelectArea()
		{
			if (SelectionBorder1 == null || !CanvasMain.Contains(SelectionBorder1))
				return;
			using (var drawingContext = SelectionBorder1.RenderOpen())
			{
				var rect = new Rect(0.0, 0.0, CanvasMain.Width, CanvasMain.Height);
				drawingContext.DrawRectangle(Brushes.Transparent, new Pen
				{
					Brush = Brushes.Blue,
					Thickness = 0.8,
					DashStyle = DashStyles.Dot
				}, new Rect(SelectionBorder1.StartPoint, SelectionBorder1.NextPoint));
			}
		}

		public void DrawSquare(MyVisual visual)
		{
			if (visual == null)
				return;
			using (var drawingContext = visual.RenderOpen())
			{
				RenderOptions.SetBitmapScalingMode(visual, BitmapScalingMode.HighQuality);
				if (ImgPath1 != null && ImgPath1.Length > 0)
				{
					try
					{
						var bitmapImage = new BitmapImage();
						bitmapImage.BeginInit();
						bitmapImage.StreamSource = new MemoryStream((byte[]) ImgPath1.Clone());
						bitmapImage.EndInit();
						bitmapImage.Freeze();
						drawingContext.DrawImage(bitmapImage, new Rect(new Point(0.0, 0.0), new Size(CanvasMain.Width, CanvasMain.Height)));
					}
					catch
					{
					}
				}
			}
		}

		public void DrawEyes()
		{
			if (LeftEye1.InArea)
			{
				using (var drawingContext = LeftEye1.RenderOpen())
				{
					var position = LeftEye1.Position;
					drawingContext.DrawLine(new Pen(Brushes.Green, 2.5), new Point(position.X, position.Y - 25.0),
						new Point(position.X, position.Y + 25.0));
					drawingContext.DrawLine(new Pen(Brushes.Green, 2.5), new Point(position.X - 25.0, position.Y),
						new Point(position.X + 25.0, position.Y));
				}
			}
			if (!RightEye1.InArea)
				return;
			using (var drawingContext = RightEye1.RenderOpen())
			{
				var position = RightEye1.Position;
				drawingContext.DrawLine(new Pen(Brushes.Green, 2.5), new Point(position.X, position.Y - 25.0),
					new Point(position.X, position.Y + 25.0));
				drawingContext.DrawLine(new Pen(Brushes.Green, 2.5), new Point(position.X - 25.0, position.Y),
					new Point(position.X + 25.0, position.Y));
			}
		}

		public void RefreshCursor()
		{
			if (Mainform.btEye.Down)
				CanvasMain.Cursor = Cursors.Cross;
			else
				CanvasMain.Cursor = Cursors.Arrow;
		}

		public void ReloadBc()
		{
			if (ImgDefaultPath1 == null || ImgDefaultPath1.Length <= 0 || VsFace1 == null)
				return;
			try
			{
				var image1 = new Bitmap(new MemoryStream((byte[]) ImgDefaultPath1.Clone()));
				var image2 = new Bitmap(new MemoryStream((byte[]) ImgChangedImage.Clone()));
				var brightnessCorrection = new BrightnessCorrection();
				brightnessCorrection.AdjustValue = Mainform.btBrightness.Value;
				var image3 = brightnessCorrection.Apply(image1);
				var image4 = brightnessCorrection.Apply(image2);
				var contrastCorrection = new ContrastCorrection();
				contrastCorrection.Factor = Mainform.btContrast.Value;
				var bitmap1 = contrastCorrection.Apply(image3);
				var bitmap2 = contrastCorrection.Apply(image4);
				var memoryStream = new MemoryStream();
				bitmap1.Save(memoryStream, ImageFormat.Bmp);
				ImgPath1 = (byte[]) memoryStream.GetBuffer().Clone();
				bitmap2.Save(memoryStream, ImageFormat.Bmp);
				ImgChangedImage = (byte[]) memoryStream.GetBuffer().Clone();
				memoryStream.Close();
				bitmap1.Dispose();
				bitmap2.Dispose();
				DrawSquare(VsFace1);
			}
			catch
			{
			}
		}

		public void canvasMain_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0 && Convert.ToInt64(CanvasMain.Width*ZoomScale) < int.MaxValue &&
			    Convert.ToInt64(CanvasMain.Height*ZoomScale) < int.MaxValue)
			{
				var position1 = e.GetPosition(CanvasMain);
				++Scrollingcount;
				CanvasMain.Width = CanvasMain.Width*ZoomScale;
				CanvasMain.Height = CanvasMain.Height*ZoomScale;
				var myVisual1 = LeftEye1;
				var position2 = LeftEye1.Position;
				var x1 = position2.X*ZoomScale;
				position2 = LeftEye1.Position;
				var y1 = position2.Y*ZoomScale;
				var point1 = new Point(x1, y1);
				myVisual1.Position = point1;
				var myVisual2 = RightEye1;
				position2 = RightEye1.Position;
				var x2 = position2.X*ZoomScale;
				position2 = RightEye1.Position;
				var y2 = position2.Y*ZoomScale;
				var point2 = new Point(x2, y2);
				myVisual2.Position = point2;
				if (VsFace1 != null && ImgPath1 != null)
					DrawSquare(VsFace1);
				DrawEyes();
				if (SelectionBorder1 != null)
				{
					SelectionBorder1.NextPoint = new Point(SelectionBorder1.NextPoint.X*ZoomScale,
						SelectionBorder1.NextPoint.Y*ZoomScale);
					SelectionBorder1.StartPoint = new Point(SelectionBorder1.StartPoint.X*ZoomScale,
						SelectionBorder1.StartPoint.Y*ZoomScale);
					SelectArea();
				}
				var vector = new Point(position1.X*ZoomScale, position1.Y*ZoomScale) - position1;
				TranslateTransform1.X -= vector.X;
				TranslateTransform1.Y -= vector.Y;
			}
			else
			{
				if (e.Delta >= 0 || Convert.ToInt64(CanvasMain.Width/ZoomScale) <= 5L ||
				    Convert.ToInt64(CanvasMain.Height/ZoomScale) <= 5L)
					return;
				--Scrollingcount;
				var position1 = e.GetPosition(CanvasMain);
				CanvasMain.Width = CanvasMain.Width/ZoomScale;
				CanvasMain.Height = CanvasMain.Height/ZoomScale;
				if (VsFace1 != null && ImgPath1 != null)
					DrawSquare(VsFace1);
				var myVisual1 = LeftEye1;
				var position2 = LeftEye1.Position;
				var x1 = position2.X/ZoomScale;
				position2 = LeftEye1.Position;
				var y1 = position2.Y/ZoomScale;
				var point1 = new Point(x1, y1);
				myVisual1.Position = point1;
				var myVisual2 = RightEye1;
				position2 = RightEye1.Position;
				var x2 = position2.X/ZoomScale;
				position2 = RightEye1.Position;
				var y2 = position2.Y/ZoomScale;
				var point2 = new Point(x2, y2);
				myVisual2.Position = point2;
				DrawEyes();
				var vector = new Point(position1.X/ZoomScale, position1.Y/ZoomScale) - position1;
				SelectionBorder1.NextPoint = new Point(SelectionBorder1.NextPoint.X/ZoomScale,
					SelectionBorder1.NextPoint.Y/ZoomScale);
				SelectionBorder1.StartPoint = new Point(SelectionBorder1.StartPoint.X/ZoomScale,
					SelectionBorder1.StartPoint.Y/ZoomScale);
				SelectArea();
				TranslateTransform1.X -= vector.X;
				TranslateTransform1.Y -= vector.Y;
			}
		}

		public void canvasMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IsDraggingRight = false;
			IsDraggingLeft = false;
			Left1Drag = false;
			Right1Drag = false;
			RefreshCursor();
		}

		public void canvasMain_MouseMove(object sender, MouseEventArgs e)
		{
			if (Left1Drag && CanvasMain.Contains(LeftEye1) && _allowDrawEyes)
			{
				Point point1;
				int num;
				if (e.GetPosition(CanvasMain).X >= 0.0)
				{
					point1 = e.GetPosition(CanvasMain);
					if (point1.Y >= 0.0)
					{
						point1 = e.GetPosition(CanvasMain);
						if (point1.X <= CanvasMain.Width)
						{
							point1 = e.GetPosition(CanvasMain);
							num = point1.Y > CanvasMain.Height ? 1 : 0;
							goto label_6;
						}
					}
				}
				num = 1;
				label_6:
				if (num == 0)
				{
					var myVisual = LeftEye1;
					point1 = e.GetPosition(CanvasMain) + ClickOffset;
					var x = point1.X + 20.0;
					point1 = e.GetPosition(CanvasMain) + ClickOffset;
					var y = point1.Y + 20.0;
					var point2 = new Point(x, y);
					myVisual.Position = point2;
				}
				DrawEyes();
			}
			else if (Right1Drag && CanvasMain.Contains(RightEye1) && _allowDrawEyes)
			{
				Point point1;
				int num;
				if (e.GetPosition(CanvasMain).X >= 0.0)
				{
					point1 = e.GetPosition(CanvasMain);
					if (point1.Y >= 0.0)
					{
						point1 = e.GetPosition(CanvasMain);
						if (point1.X <= CanvasMain.Width)
						{
							point1 = e.GetPosition(CanvasMain);
							num = point1.Y > CanvasMain.Height ? 1 : 0;
							goto label_15;
						}
					}
				}
				num = 1;
				label_15:
				if (num == 0)
				{
					var myVisual = RightEye1;
					point1 = e.GetPosition(CanvasMain) + ClickOffset;
					var x = point1.X + 20.0;
					point1 = e.GetPosition(CanvasMain) + ClickOffset;
					var y = point1.Y + 20.0;
					var point2 = new Point(x, y);
					myVisual.Position = point2;
				}
				DrawEyes();
			}
			else if (e.RightButton == MouseButtonState.Pressed)
			{
				TranslateTransform1.X = e.GetPosition(this).X - CanvasMainPoint.X;
				TranslateTransform1.Y = e.GetPosition(this).Y - CanvasMainPoint.Y;
			}
			else
			{
				if (e.LeftButton != MouseButtonState.Pressed || Mainform.btEye.Down)
					return;
				SelectionBorder1.NextPoint = e.GetPosition(CanvasMain);
				SelectArea();
			}
		}

		public void canvasMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var position1 = e.GetPosition(CanvasMain);
			if (e.LeftButton == MouseButtonState.Pressed && Mainform.btEye.Down && _allowDrawEyes)
			{
				var myVisual = (MyVisual) CanvasMain.GetVisual(position1);
				if (LeftEye1.InArea && RightEye1.InArea)
				{
					IsChanged = true;
					if (myVisual == LeftEye1)
					{
						Left1Drag = true;
						ClickOffset = new Point(myVisual.Position.X - 20.0, myVisual.Position.Y - 20.0) - position1;
					}
					else if (myVisual == RightEye1)
					{
						Right1Drag = true;
						ClickOffset = new Point(myVisual.Position.X - 20.0, myVisual.Position.Y - 20.0) - position1;
					}
				}
				else if (!LeftEye1.InArea && !RightEye1.InArea)
				{
					LeftEye1 = new MyVisual();
					CanvasMain.AddEye(LeftEye1);
					IsChanged = true;
					LeftEye1.InArea = true;
					LeftEye1.Position = position1;
				}
				else if (LeftEye1.InArea && !RightEye1.InArea)
				{
					var position2 = LeftEye1.Position;
					if (position2.X > position1.X)
					{
						RightEye1 = new MyVisual();
						RightEye1.InArea = true;
						RightEye1.Position = LeftEye1.Position;
						LeftEye1.Position = position1;
						CanvasMain.AddEye(RightEye1);
						IsChanged = true;
					}
					else
					{
						position2 = LeftEye1.Position;
						if (position2.X < position1.X)
						{
							RightEye1 = new MyVisual();
							RightEye1.InArea = true;
							RightEye1.Position = position1;
							CanvasMain.AddEye(RightEye1);
						}
					}
				}
				DrawEyes();
			}
			else
			{
				CanvasMain.DeleteVisual(SelectionBorder1);
				SelectionBorder1 = new MyVisual();
				SelectionBorder1.Type = MyVisualType.SelectImageRect;
				SelectionBorder1.StartPoint = e.GetPosition(CanvasMain);
				SelectionBorder1.NextPoint = e.GetPosition(CanvasMain);
				CanvasMain.AddVisual(SelectionBorder1);
				IsChanged = true;
			}
		}

		public void rdbEye_Click(object sender, RoutedEventArgs e)
		{
			CanvasMain.Cursor = Cursors.Cross;
			if (!Mainform.btEye.Down)
			{
				_allowDrawEyes = false;
				CanvasMain.DeleteVisual(LeftEye1);
				CanvasMain.DeleteVisual(RightEye1);
			}
			else if (ImgDefaultPath1 != null && ImgDefaultPath1.Length > 0)
			{
				try
				{
					CanvasMain.DeleteVisual(LeftEye1);
					CanvasMain.DeleteVisual(RightEye1);
				}
				catch
				{
				}
				LeftEye1 = new MyVisual();
				RightEye1 = new MyVisual();
				CanvasMain.DeleteVisual(LeftEye1);
				CanvasMain.DeleteVisual(RightEye1);
				if (Face.DetectAllFaces(new Bitmap(new MemoryStream((byte[]) ImgPath1.Clone())).ConvertFrom(), null).Length == 0)
					_allowDrawEyes =
						XtraMessageBox.Show(Messages.NoFaceWasFoundDoYouWantToSet, Messages.Message, MessageBoxButtons.YesNo,
							MessageBoxIcon.Question) == DialogResult.Yes;
			}
		}

		public void canvasMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			CanvasMain.Cursor = Cursors.Hand;
			var position = e.GetPosition(this);
			CanvasMainPoint = new Point(position.X - TranslateTransform1.X, position.Y - TranslateTransform1.Y);
		}

		public void canvasMain_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			IsDraggingRight = false;
			IsDraggingLeft = false;
			RefreshCursor();
		}

		public byte[][] GetRgb(Bitmap bmp)
		{
			var bitmapdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb);
			var scan0 = bitmapdata.Scan0;
			var length1 = bmp.Width*bmp.Height;
			var length2 = bitmapdata.Stride*bmp.Height;
			var num1 = bitmapdata.Stride - bmp.Width*3;
			var index1 = 0;
			var num2 = 1;
			var numArray1 = new byte[length1];
			var numArray2 = new byte[length1];
			var numArray3 = new byte[length1];
			var destination = new byte[length2];
			Marshal.Copy(scan0, destination, 0, length2);
			var index2 = 0;
			while (index2 < length2 - 3)
			{
				if (index2 == bitmapdata.Stride*num2 - num1)
				{
					index2 += num1;
					++num2;
				}
				numArray1[index1] = destination[index2];
				numArray2[index1] = destination[index2 + 1];
				numArray3[index1] = destination[index2 + 2];
				++index1;
				index2 += 3;
			}
			bmp.UnlockBits(bitmapdata);
			return new byte[3][]
			{
				numArray1,
				numArray2,
				numArray3
			};
		}

		public Image AutoCrop(Bitmap bmp)
		{
			var rgb = GetRgb(bmp);
			var num1 = bmp.Height - 1;
			var width = bmp.Width;
			var y = 0;
			var num2 = num1;
			var num3 = bmp.Width;
			var num4 = 0;
			var num5 = 0;
			var num6 = 95;
			var flag = false;
			for (var index = 0; index < rgb[0].Length; ++index)
			{
				var num7 = index%width;
				var num8 = (int) Math.Floor((decimal) (index/width));
				var num9 = byte.MaxValue*num6/100;
				if (rgb[0][index] >= num9 && rgb[1][index] >= num9 && rgb[2][index] >= num9)
				{
					++num5;
					num4 = num7 <= num4 || num5 != 1 ? num4 : num7;
				}
				else
				{
					num3 = num7 >= num3 || num5 < 1 ? num3 : num7;
					num4 = num7 != width - 1 || num5 != 0 ? num4 : width - 1;
					num5 = 0;
				}
				if (num5 == width)
				{
					y = num8 - y < 3 ? num8 : y;
					num2 = !flag || num7 != width - 1 || num8 <= y + 1 ? num2 : num8;
				}
				num3 = num7 != 0 || num5 != 0 ? num3 : 0;
				num2 = num8 != num1 || num7 != width - 1 || num5 == width || !flag ? num2 : num1 + 1;
				if (num7 == width - 1)
				{
					flag = num5 < width;
					num5 = 0;
				}
			}
			var num10 = num4 == 0 ? width : num4;
			var x = num3 == width ? 0 : num3;
			return bmp.Clone(new Rectangle(x, y, num10 - x + 1, num2 - y), bmp.PixelFormat);
		}

		public void LoadImage(byte[] stream)
		{
			try
			{
				Mainform.btBrightness.Value = 0;
				Mainform.btContrast.Value = 0;
				Mainform.btSize.Value = 100;
				ImgSource1 = stream;
				IsChanged = true;
				CanvasMain.Clear();
				Scrollingcount = 0;
				VsFace1 = new MyVisual();
				VsFace1.Type = MyVisualType.Image;
				ImgPath1 = new byte[stream.Length];
				ImgPath1 = (byte[]) stream.Clone();
				ImgDefaultPath1 = (byte[]) ImgPath1.Clone();
				ImgChangedImage = (byte[]) ImgPath1.Clone();
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = new MemoryStream((byte[]) ImgDefaultPath1.Clone());
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				CanvasMain.Width = bitmapImage.PixelWidth;
				CanvasMain.Height = bitmapImage.PixelHeight;
				if (ScrollViewer1.ActualWidth > CanvasMain.Width)
					TranslateTransform1.X = (ScrollViewer1.ActualWidth - CanvasMain.Width)/2.0;
				if (ScrollViewer1.ActualHeight > CanvasMain.Height)
					TranslateTransform1.Y = (ScrollViewer1.ActualHeight - CanvasMain.Height)/2.0;
				CanvasMain.AddVisual(VsFace1);
				DrawSquare(VsFace1);
				if (!Mainform.btEye.Down)
					return;
				rdbEye_Click(new object(), new RoutedEventArgs());
			}
			catch
			{
			}
		}

		public void rdbOpen1_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				CanvasMain.Cursor = Cursors.Arrow;
				var openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image Files (*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF)|*.BMP;*.JPG;*.GIF;*.TIFF;*.TIF;*.png;*.jpeg";
				if (openFileDialog.ShowDialog() != DialogResult.OK)
					return;
				ImgSource1 = null;
				Mainform.btBrightness.EditValue = 0;
				Mainform.btContrast.EditValue = 0;
				Mainform.btSize.EditValue = 100;
				IsChanged = true;
				Scrollingcount = 0;
				CanvasMain.Clear();
				VsFace1 = new MyVisual();
				VsFace1.Type = MyVisualType.Image;
				var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
				var numArray = new byte[fileStream.Length];
				fileStream.Read(numArray, 0, numArray.Length);
				ImgSource1 = (byte[]) numArray.Clone();
				LoadImage(numArray);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				ImgPath1 = null;
				var num = (int) MessageBox.Show(ex.Message, Messages.Error, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		public void rdbSave_Click(object sender, RoutedEventArgs e)
		{
			var saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Bitmap Image (*.BMP)|*.bmp";
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
				return;
			new Bitmap(new MemoryStream(ImgPath1)).Save(saveFileDialog.FileName);
		}

		public void rdbFlip_Click(object sender, RoutedEventArgs e)
		{
			if (ImgPath1 == null || ImgPath1.Length <= 0)
				return;
			IsChanged = true;
			var bitmap = new Bitmap(new MemoryStream((byte[]) ImgPath1.Clone()));
			bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
			var memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Bmp);
			ImgPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgDefaultPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgChangedImage = (byte[]) memoryStream.GetBuffer().Clone();
			DrawSquare(VsFace1);
		}

		public void SaveImage(Visual visual, int width, int height, string filePath)
		{
			var form = new Form();
			var graphics = form.CreateGraphics();
			DpX = graphics.DpiX;
			DpY = graphics.DpiY;
			graphics.Dispose();
			form.Dispose();
			var renderTargetBitmap = new RenderTargetBitmap(width, height, DpX, DpY, PixelFormats.Pbgra32);
			RenderOptions.SetBitmapScalingMode(visual, BitmapScalingMode.HighQuality);
			renderTargetBitmap.Render(visual);
			var bmpBitmapEncoder = new BmpBitmapEncoder();
			bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			using (Stream stream = File.Create(filePath))
				bmpBitmapEncoder.Save(stream);
		}

		public void rdbCutArea_Click(object sender, RoutedEventArgs e)
		{
			if (!CanvasMain.Contains(SelectionBorder1) || !(SelectionBorder1.StartPoint != SelectionBorder1.NextPoint))
				return;
			IsChanged = true;
			TranslateTransform1.X = 0.0;
			TranslateTransform1.Y = 0.0;
			CanvasMain.Clear();
			if (Scrollingcount > 0)
			{
				SelectionBorder1.StartPoint = new Point(
					SelectionBorder1.StartPoint.X/Math.Pow(ZoomScale, Math.Abs(Scrollingcount)),
					SelectionBorder1.StartPoint.Y/Math.Pow(ZoomScale, Math.Abs(Scrollingcount)));
				SelectionBorder1.NextPoint = new Point(SelectionBorder1.NextPoint.X/Math.Pow(ZoomScale, Math.Abs(Scrollingcount)),
					SelectionBorder1.NextPoint.Y/Math.Pow(ZoomScale, Math.Abs(Scrollingcount)));
			}
			else if (Scrollingcount < 0)
			{
				SelectionBorder1.StartPoint = new Point(
					SelectionBorder1.StartPoint.X*Math.Pow(ZoomScale, Math.Abs(Scrollingcount)),
					SelectionBorder1.StartPoint.Y*Math.Pow(ZoomScale, Math.Abs(Scrollingcount)));
				SelectionBorder1.NextPoint = new Point(SelectionBorder1.NextPoint.X*Math.Pow(ZoomScale, Math.Abs(Scrollingcount)),
					SelectionBorder1.NextPoint.Y*Math.Pow(ZoomScale, Math.Abs(Scrollingcount)));
			}
			var bitmap = new Bitmap((int) Math.Abs(SelectionBorder1.NextPoint.X - SelectionBorder1.StartPoint.X),
				(int) Math.Abs(SelectionBorder1.NextPoint.Y - SelectionBorder1.StartPoint.Y));
			Graphics.FromImage(bitmap);
			var x = (int) SelectionBorder1.StartPoint.X;
			var y = (int) SelectionBorder1.StartPoint.Y;
			var width = (int) Math.Abs(SelectionBorder1.StartPoint.X - SelectionBorder1.NextPoint.X);
			var height = (int) Math.Abs(SelectionBorder1.StartPoint.Y - SelectionBorder1.NextPoint.Y);
			if (SelectionBorder1.NextPoint.X < SelectionBorder1.StartPoint.X)
				x = (int) SelectionBorder1.NextPoint.X;
			if (SelectionBorder1.NextPoint.Y < SelectionBorder1.StartPoint.Y)
				y = (int) SelectionBorder1.NextPoint.Y;
			var sourceRect = new Int32Rect(x, y, width, height);
			try
			{
				var myVisual = new MyVisual();
				using (var drawingContext = myVisual.RenderOpen())
				{
					myVisual.Clip = new RectangleGeometry(new Rect(0.0, 0.0, width, height));
					var memoryStream = new MemoryStream(ImgDefaultPath1);
					var bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.EndInit();
					bitmapImage.Freeze();
					var croppedBitmap = new CroppedBitmap(bitmapImage, sourceRect);
					drawingContext.DrawImage(croppedBitmap, new Rect(0.0, 0.0, sourceRect.Width, sourceRect.Height));
					memoryStream.Close();
				}
				try
				{
					var str = (string) (object) Guid.NewGuid() + (object) ".bmp";
					SaveImage(myVisual, width, height, MainForm.ApplicationData + str);
					var fileStream = new FileStream(MainForm.ApplicationData + str, FileMode.Open);
					ImgPath1 = new byte[fileStream.Length];
					ImgPath1 = (byte[]) ImgPath1.Clone();
					fileStream.Read(ImgPath1, 0, (int) fileStream.Length);
					fileStream.Close();
					File.Delete(MainForm.ApplicationData + str);
				}
				catch
				{
				}
				ImgDefaultPath1 = (byte[]) ImgPath1.Clone();
				ImgChangedImage = (byte[]) ImgPath1.Clone();
				SelectionBorder1.StartPoint = new Point(0.0, 0.0);
				SelectionBorder1.NextPoint = new Point(0.0, 0.0);
				LeftEye1.InArea = false;
				RightEye1.InArea = false;
				CanvasMain.Width = bitmap.Width;
				CanvasMain.Height = bitmap.Height;
				VsFace1 = new MyVisual();
				VsFace1.Type = MyVisualType.Image;
				CanvasMain.AddVisual(VsFace1);
				Scrollingcount = 0;
				SelectArea();
				DrawSquare(VsFace1);
			}
			catch (Exception ex)
			{
				var num = (int) XtraMessageBox.Show(ex.Message, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		public void rdbRotate_Click(object sender, RoutedEventArgs e)
		{
			if (ImgPath1 == null || ImgPath1.Length <= 0)
				return;
			IsChanged = true;
			var bitmap = new Bitmap(new MemoryStream((byte[]) ImgPath1.Clone()));
			bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
			var memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Bmp);
			ImgPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgDefaultPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgChangedImage = (byte[]) memoryStream.GetBuffer().Clone();
			if (Scrollingcount > 0)
			{
				CanvasMain.Width = bitmap.Width*Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
				CanvasMain.Height = bitmap.Height*Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
			}
			else if (Scrollingcount < 0)
			{
				CanvasMain.Width = bitmap.Width/Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
				CanvasMain.Height = bitmap.Height/Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
			}
			else
			{
				CanvasMain.Width = bitmap.Width;
				CanvasMain.Height = bitmap.Height;
			}
			DrawSquare(VsFace1);
		}

		public void rdbRotateRight_Click(object sender, RoutedEventArgs e)
		{
			if (ImgPath1 == null || ImgPath1.Length <= 0)
				return;
			IsChanged = true;
			var bitmap = new Bitmap(new MemoryStream((byte[]) ImgPath1.Clone()));
			bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
			var memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Bmp);
			ImgPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgDefaultPath1 = (byte[]) memoryStream.GetBuffer().Clone();
			ImgChangedImage = (byte[]) memoryStream.GetBuffer().Clone();
			if (Scrollingcount > 0)
			{
				CanvasMain.Width = bitmap.Width*Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
				CanvasMain.Height = bitmap.Height*Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
			}
			else if (Scrollingcount < 0)
			{
				CanvasMain.Width = bitmap.Width/Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
				CanvasMain.Height = bitmap.Height/Math.Pow(ZoomScale, Math.Abs(Scrollingcount));
			}
			else
			{
				CanvasMain.Width = bitmap.Width;
				CanvasMain.Height = bitmap.Height;
			}
			DrawSquare(VsFace1);
		}

		public void rdbCancelEdit_Click(object sender, RoutedEventArgs e)
		{
			if (ImgSource1 == null)
				return;
			IsChanged = false;
			Mainform.btBrightness.EditValue = 0;
			Mainform.btContrast.EditValue = 0;
			Mainform.btSize.EditValue = 100;
			CanvasMain.Clear();
			Mainform.btEye.Down = false;
			LoadImage(ImgSource1);
		}

		public void slContrast_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			ReloadBc();
			Mainform.lbContrast.Text = Messages.Contrast + (object) ": " + (string) (object) Convert.ToInt32(e.NewValue/10.0);
		}

		public void slSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var num = e.NewValue/100.0;
			if (ImgChangedImage != null && ImgChangedImage.Length > 0 && num != 0.0)
			{
				try
				{
					var original = Image.FromStream(new MemoryStream((byte[]) ImgChangedImage.Clone()));
					if (original.Width*num*original.Height*num < 6000000.0)
					{
						var bitmap = new Bitmap(original, new System.Drawing.Size((int) (original.Width*num), (int) (original.Height*num)));
						var memoryStream = new MemoryStream();
						bitmap.Save(memoryStream, ImageFormat.Bmp);
						Scrollingcount = 0;
						ImgPath1 = (byte[]) memoryStream.GetBuffer().Clone();
						ImgDefaultPath1 = (byte[]) ImgPath1.Clone();
						memoryStream.Close();
						CanvasMain.Width = bitmap.Width;
						CanvasMain.Height = bitmap.Height;
						TranslateTransform1.X = 0.0;
						TranslateTransform1.Y = 0.0;
						DrawSquare(VsFace1);
						DrawEyes();
						bitmap.Dispose();
					}
					original.Dispose();
					Thread.Sleep(200);
				}
				catch
				{
				}
			}
			Mainform.lbSize.Text = string.Concat((object) Messages.Size, (object) ": ", (object) (int) (num*100.0), (object) "%");
		}

		public void slBrightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			ReloadBc();
			Mainform.lbBrightness.Text = Messages.Brightness + (object) ": " + (string) (object) Convert.ToInt32(e.NewValue);
		}

		public void lbContrast_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Mainform.btContrast.EditValue = 0;
			slContrast_ValueChanged(new object(), new RoutedPropertyChangedEventArgs<double>(0.0, 100.0));
		}

		public void lbBrightness_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Mainform.btBrightness.EditValue = 0;
			slBrightness_ValueChanged(new object(), new RoutedPropertyChangedEventArgs<double>(0.0, 0.0));
		}

		public void lbSize_MouseDown(object sender, MouseButtonEventArgs e)
		{
		}

		public void scrollViewer1_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (_contentLoaded)
				return;
			_contentLoaded = true;
			Application.LoadComponent(this, new Uri("/Manager;component/editpicturecontrol.xaml", UriKind.Relative));
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}
	}
}