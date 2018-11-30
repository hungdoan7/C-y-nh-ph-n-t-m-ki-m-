﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Cay_Nhi_Phan
{

	public partial class Form1 : Form
	{
		const int PICTUREBOX_WIDE = 1024;
		const int EQUAL = 0;		// Can bang
		const int LEFT = -1;		// Trai
		const int RIGHT = 1;		// Phai

		int Total_Node = 0;		// tong so node
		int Total_Leaf_Node = 0;	// tong so leaf node
		int Total_Intermediate_Node = 0;	 // tong so node trung gian
		int The_Height_Tree = 0;		 // chieu cao cay
		
		public class_node Root;
		int Speed;
		Bitmap bitmap;
		Graphics g;
		RichTextBox Info_RichTextBox = new RichTextBox();
		List<int> Way = new List<int>();
		public Form1()
		{
			InitializeComponent();
			bitmap = new Bitmap(Main_PictureBox.Width, Main_PictureBox.Height);
			g = Graphics.FromImage(bitmap);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
		}

		#region Do Hoa
		public void VeCanh(PointF a, PointF b)
		{
			g.DrawLine(new Pen(Color.GreenYellow, 2), a.X + 20, a.Y + 39, b.X + 20, b.Y);
			Main_PictureBox.Image = bitmap;
		}
		public void DrawNode(class_node A)
		{
			g.DrawImage(Cay_Nhi_Phan.Properties.Resources.ellip_blue, A.vitri.X, A.vitri.Y);
			if (A.number<10)
			{
				g.DrawString(A.number.ToString(), new Font(FontFamily.GenericSerif, 15, FontStyle.Bold), new SolidBrush(Color.White), new PointF(A.vitri.X + 11, A.vitri.Y + 8f));
			}
			else
			{
				g.DrawString(A.number.ToString(), new Font(FontFamily.GenericSerif, 15, FontStyle.Bold), new SolidBrush(Color.White), new PointF(A.vitri.X + 7, A.vitri.Y + 8f));
			}
			Main_PictureBox.Image = bitmap;
		}
		public void DrawNodeRed(class_node A)
		{
			g.DrawImage(Cay_Nhi_Phan.Properties.Resources.ellip_red, A.vitri.X, A.vitri.Y);
			if (A.number < 10)
			{
				g.DrawString(A.number.ToString(), new Font(FontFamily.GenericSerif, 15, FontStyle.Bold), new SolidBrush(Color.White), new PointF(A.vitri.X + 11, A.vitri.Y + 8f));
			}
			else
			{
				g.DrawString(A.number.ToString(), new Font(FontFamily.GenericSerif, 15, FontStyle.Bold), new SolidBrush(Color.White), new PointF(A.vitri.X + 7, A.vitri.Y + 8f));
			}
			Main_PictureBox.Image = bitmap;
		}
		public void DrawSearch(class_node A)
		{
			g.DrawImage(Cay_Nhi_Phan.Properties.Resources.search, A.vitri.X, A.vitri.Y);
			Main_PictureBox.Image = bitmap;
		}
		public void DrawDelete(class_node A)
		{
			g.DrawImage(Cay_Nhi_Phan.Properties.Resources.delete, A.vitri.X, A.vitri.Y);
			Main_PictureBox.Image = bitmap;
		}
		public void DiChuyen(ref class_node A, PointF B,int cv)
		{
			A.locationOld = A.vitri;
			XacDinhTocDo();
			float a = (B.Y - A.vitri.Y) / (B.X - A.vitri.X);
			float b = B.Y - a * B.X;
			float deltaX = Math.Abs(B.X - A.locationOld.X);
			if (A.locationOld.X - B.X < 0)
			{
				//while (A.vitri.X - B.X < 0)			//A.vitri.X - B.X < 0
				for(int i=0;i<Speed;i++)
				{
					g.Clear(Color.White);
					A.vitri.X += deltaX/Speed ;
					A.vitri.Y = a * A.vitri.X + b;
					if (cv == 1)
					{
						VeCay_special(Root, A);
						DrawNodeRed(A);
					}
					else
					{
						if (cv == 2)
						{
							VeCay_normal(Root);
							DrawSearch(A);
						}
					}
					Thread.Sleep(1);
					Application.DoEvents();
				}
			}
			else
			{
				//while (A.vitri.X - B.X > 0)
				for (int i=0;i<Speed;i++)
				{
					g.Clear(Color.White);
					A.vitri.X -= (deltaX/Speed);
					A.vitri.Y = a * A.vitri.X + b;
					if (cv == 1)
					{
						VeCay_special(Root, A);
						DrawNodeRed(A);
					}
					else
					{
						if (cv == 2)
						{
							VeCay_normal(Root);
							DrawSearch(A);
						}
					}
					Thread.Sleep(1);
					Application.DoEvents();
				}
			}
		}



		// Di chuyển node xuống vị trí pLeft hoặc pRight
		public void MoveDown(ref class_node n, int heso, int cv)
		{
			if (heso == LEFT) //move_down_left
			{
				DiChuyen(ref n, nodeLR(n.vitri, LEFT),cv);
			}
			if (heso == RIGHT) //move_down_right
			{
				DiChuyen(ref n, nodeLR(n.vitri, RIGHT),cv);
			}
		}
		// Hàm nhận vào 1 node và trả về vị trí node pPeft hoặc pRight của node đó
		public PointF nodeLR(PointF l, int traiphai)
		{
			PointF kq = new PointF();
			int vt_y = ((Convert.ToInt32(l.Y - 20)) + 72) / 72;
			int partwidth = 1024 / (int)Math.Pow(2, (vt_y + 1));
			if (traiphai == -1)  //tra ve vi tri node con ben trai
			{
				kq.X = l.X - partwidth;
				kq.Y = l.Y + 72;
				return kq;
			}
			else  //tra ve vi tri node con ben phai 
			{
				kq.X = l.X + partwidth;
				kq.Y = l.Y + 72;
				return kq;
			}
		}
		// Vẽ cây bình thường
		public void VeCay_normal(class_node n)
		{
			if (Root != null)
				DrawNode(Root);
			if (n != null)
			{
				if (n.left != null)
				{ 
					DrawNode(n.left);
					VeCanh(n.vitri, n.left.vitri);
				}
				if (n.right != null)
				{

					DrawNode(n.right);
					VeCanh(n.vitri, n.right.vitri);
				}
				VeCay_normal(n.left);
				VeCay_normal(n.right);
			}
		}
		public void VeCay_special(class_node n, class_node a)
		{
			if (Root != null)
				DrawNode(Root);
			if (n != null)
			{
				if (n.left != null && n.left != a)
				{
					DrawNode(n.left);
					VeCanh(n.vitri, n.left.vitri);
				}
				if (n.right != null && n.right != a)
				{
					DrawNode(n.right);
					VeCanh(n.vitri, n.right.vitri);
				}
				VeCay_special(n.left, a);
				VeCay_special(n.right, a);
			}
		}
		// Xác dịnh lại vị trí cho node để vẽ cây
		public void Xd_ViTri(ref class_node n)
		{
			PointF vt = new PointF();
			if (Root != null)
			{
				vt = new PointF(512, 20);
				Root.vitri = vt;
			}
			if (n != null)
			{
				if (n.left != null)
				{
					vt = nodeLR(n.vitri, -1);
					n.left.vitri = vt;
				}
				if (n.right != null)
				{
					vt = nodeLR(n.vitri, 1);
					n.right.vitri = vt;
				}
				Xd_ViTri(ref n.left);
				Xd_ViTri(ref n.right);
			}
		}
		public void Xd_ViTriCu(ref class_node n)
		{
			if (Root != null)
			{
				Root.locationOld = Root.vitri;
			}
			if (n != null)
			{
				if (n.left != null)
				{
					n.left.locationOld = n.left.vitri;
				}
				if (n.right != null)
				{
					n.right.locationOld = n.right.vitri;
				}
				Xd_ViTriCu(ref n.left);
				Xd_ViTriCu(ref n.right);
			}
		}

		public void Xd_ViTriMoi(ref class_node n)
		{
			PointF vt = new PointF();
			if (Root != null)
			{
				vt = new PointF(512, 20);
				Root.locationNew = vt;
			}
			if (n != null)
			{
				if (n.left != null)
				{
					vt = nodeLR(n.locationNew, -1);
					n.left.locationNew = vt;
				}
				if (n.right != null)
				{
					vt = nodeLR(n.locationNew, 1);
					n.right.locationNew = vt;
				}
				Xd_ViTriMoi(ref n.left);
				Xd_ViTriMoi(ref n.right);
			}
		}

		public void DiChuyenCay(ref class_node node)
		{
			if (node != null)
			{
				if ( node.locationOld.X != node.locationNew.X)
				{
					DiChuyenSpecial(ref node, node.locationNew);
				}
				DiChuyenCay(ref node.left);
				DiChuyenCay(ref node.right);
			}
		}

		public void DiChuyenSpecial(ref class_node A, PointF B)
		{
			XacDinhTocDo();
			float a = (B.Y - A.locationOld.Y) / (B.X - A.locationOld.X);
			float b = B.Y - a * B.X;
			float deltaX = Math.Abs(B.X - A.locationOld.X);
			
			if (A.vitri.X - B.X < 0) //A.vitri.X - B.X < 0
			{
				A.vitri.X += (deltaX / Speed);
				A.vitri.Y = a * A.vitri.X + b;
			}
			else
			{	
				A.vitri.X -= (deltaX / Speed);
				A.vitri.Y = a * A.vitri.X + b;
			}
		}
		private void XacDinhTocDo()
		{
			if (Speed_ComboBox.Text != "1" && Speed_ComboBox.Text != "2" && Speed_ComboBox.Text != "3" && Speed_ComboBox.Text != "4")
			{
				Speed_ComboBox.Text = "2";
			}
			Speed = (5 - Convert.ToInt32(Speed_ComboBox.Text)) * 10;
		}
		private void ShowTextBox(class_node node)
		{
			Info_RichTextBox.Clear();
			if ((Convert.ToInt32(node.vitri.X + 200) >= Main_PictureBox.Width) || (Convert.ToInt32(node.vitri.Y + 200) >= Main_PictureBox.Height))
			{
				Info_RichTextBox.Location = new Point(Convert.ToInt32(node.vitri.X ), Convert.ToInt32(node.vitri.Y - 100));
			}
			else
			{
				Info_RichTextBox.Location = new Point(Convert.ToInt32(node.vitri.X + 50), Convert.ToInt32(node.vitri.Y + 50));
			}
			Info_RichTextBox.Size = new Size(160, 84);
			Info_RichTextBox.AppendText(" Node " + node.number);
			if (node == Root)
			{
				Info_RichTextBox.AppendText(" -  Node gốc ");
			}
			else
			{
				if (node.left == null && node.right == null)
				{
					Info_RichTextBox.AppendText(" -  Node lá ");
				}
				else
				{
					Info_RichTextBox.AppendText(" -  Node trung gian ");
				}
			}
			Info_RichTextBox.AppendText("\n- Hệ số cân bằng: ");
			switch (node.canbang)
			{
				case LEFT: Info_RichTextBox.AppendText("LH"); break;
				case EQUAL: Info_RichTextBox.AppendText("EH"); break;
				case RIGHT: Info_RichTextBox.AppendText("RH"); break;
			}
			if (node.left != null)
				Info_RichTextBox.AppendText("\n- Node pLeft: " + node.left.number);
			else
				Info_RichTextBox.AppendText("\n- Node pLeft rỗng");
			if (node.right != null)
				Info_RichTextBox.AppendText("\n- Node pRight: " + node.right.number);
			else
				Info_RichTextBox.AppendText("\n- Node pRight rỗng");

			//Info_RichTextBox.AppendText("\n <RightClick> to delete");
			//Info_RichTextBox.AppendText("\n <Esc> key to hide textbox");
			Main_PictureBox.Controls.Add(Info_RichTextBox);
		}
		#endregion

		#region Build Tree 
		void Rotate_Left_Left(ref class_node node)
		{
			class_node p;
			p = node.left;
			node.left = p.right;
			p.right = node;
			switch (p.canbang)
			{
				case LEFT:
					node.canbang = EQUAL;
					p.canbang = EQUAL;
					break;
				case EQUAL:
					p.canbang = RIGHT;
					node.canbang = LEFT;
					break;
			}
			node = p;
		}

		//cay con phai lech phai
		void Rotate_Right_Right(ref class_node node)
		{
			class_node p;
			p = node.right;
			node.right = p.left;
			p.left = node;
			switch (p.canbang)
			{
				case EQUAL:
					node.canbang = RIGHT;
					p.canbang = EQUAL;
					break;
				case RIGHT:
					p.canbang = EQUAL;
					node.canbang = EQUAL;
					break;
			}
			node = p;
		}

		//cay con phai lech trai
		void Rotate_Right_Left(ref class_node node)
		{
			class_node p1, p2;
			p1 = node.right;
			p2 = p1.left;
			node.right = p2.left;
			p1.left = p2.right;
			p2.left = node;
			p2.right = p1;
			switch (p2.canbang)
			{
				case LEFT:
					node.canbang = EQUAL;
					p1.canbang = RIGHT;
					break;
				case EQUAL:
					node.canbang = EQUAL;
					p1.canbang = EQUAL;
					break;
				case RIGHT:
					node.canbang = LEFT;
					p1.canbang = EQUAL;
					break;
			}
			p2.canbang = EQUAL;
			node = p2;
		}

		//cay con trai lech phai
		void Rotate_Left_Right(ref class_node node)
		{
			class_node p1, p2;
			p1 = node.left;
			p2 = p1.right;
			node.left = p2.right;
			p1.right = p2.left;
			p2.right = node;
			p2.left = p1;

			switch (p2.canbang)
			{
				case LEFT:
					p1.canbang = EQUAL;
					node.canbang = RIGHT;
					break;
				case EQUAL:
					node.canbang = EQUAL;
					p1.canbang = EQUAL;
					break;
				case RIGHT:
					node.canbang = EQUAL;
					p1.canbang = LEFT;
					break;
			}
			p2.canbang = EQUAL;
			node = p2;
		}

		//Can bang khi cay lech trai
		int BalanceLeft(ref class_node node)
		{
			class_node p;
			p = node.left;
			switch (p.canbang)
			{
				case LEFT:
					Rotate_Left_Left(ref node);
					return 2;
				case EQUAL:
					Rotate_Left_Left(ref node);
					return 1;
				case RIGHT:
					Rotate_Left_Right(ref node);
					return 2;
			}
			return 0;
		}

		//can bang cay lech phai
		int BalanceRight(ref class_node node)
		{
			class_node p;
			p = node.right;
			switch (p.canbang)
			{
				case RIGHT:
					Rotate_Right_Right(ref node);
					return 2;
				case EQUAL:
					Rotate_Right_Right(ref node);
					return 1;
				case LEFT:
					Rotate_Right_Left(ref node);
					return 2;
			}
			return 0;
		}
		private int InsertNode(ref class_node node, int number)
		{

			int Res;
			if (node == null)
			{
				node = new class_node(number);
				if (Way.Count != 0)
				{
					for (int i = 0; i < Way.Count; i++)
					{
						MoveDown(ref node, Way[i],1);
					}
				}
			}
			else
			{
				if (node.number == number)
				{
					return 0;
				}
				if (number < node.number)
				{
					Way.Add(LEFT);
					Res = InsertNode(ref node.left, number);
					if (Res < 2) return Res;

					//Res >= 2
					switch (node.canbang)
					{
						case RIGHT:
							node.canbang = EQUAL;
							return 1;
						case EQUAL:
							node.canbang = LEFT;
							return 2;
						case LEFT:
							BalanceLeft(ref node);
							return 1;
					}
				}
				else
				{
					Way.Add(RIGHT);
					Res = InsertNode(ref node.right, number);
					if (Res < 2) return Res;

					//Res >= 2
					switch (node.canbang)
					{
						case LEFT:
							node.canbang = EQUAL;
							return 1;
						case EQUAL:
							node.canbang = RIGHT;
							return 2;
						case RIGHT:
							BalanceRight(ref node);
							return 1;
					}

				}
			}
			return 2;
		}

		private int DelNode(ref class_node node, int number)
		{
			int Res;
			//Khong ton tai node nay tren cay
			if (node == null)
			{
				class_node Temp_Run = new class_node();
				if (Way.Count != 0)
				{
					for (int i = 0; i < Way.Count-1; i++)
					{
						MoveDown(ref Temp_Run, Way[i], 2);
					}
				}
				MessageBox.Show(" Khong tim thay node co gia tri can xoa ");
				return 0;
			}
			
			if (node.number == number)
			{

				class_node Temp_Run = new class_node();
				if (Way.Count != 0)
				{
					for (int i = 0; i < Way.Count; i++)
					{
						MoveDown(ref Temp_Run, Way[i], 2);
					}
				}
				g.Clear(Color.White);
				VeCay_normal(Root);
				DrawDelete(Temp_Run);
				
				MessageBox.Show(" Da tim thay node co gia tri " + number + " va se xoa ngay !");				
				
				//Root->info = x
				class_node Temp = node;

				if (node.left == null)
				{
					node = node.right;
					Res = 2;
				}
				else
				{
					if (node.right == null)
					{
						node = node.left;
						Res = 2;
					}
					else
					{
						Res = SearchStandFor(ref Temp,ref node.right);
						if (Res < 2) return Res;
						switch (node.canbang)
						{
							case RIGHT:
								node.canbang = EQUAL;
								return 2;
							case EQUAL:
								node.canbang = LEFT;
								return 1;
							case LEFT:
								return BalanceRight(ref node);
						}
					}
					Temp = null;
					return Res;
				}
			}
			else
			{
				//Root->info > x => Sang ben trai tim xoa
				if (node.number > number)
				{
					Way.Add(LEFT);
					Res = DelNode(ref node.left, number);
					if (Res < 2) return Res;

					//Chieu cao bi thay doi
					switch (node.canbang)
					{
						case LEFT:
							node.canbang = EQUAL;
							return 2;
						case EQUAL:
							node.canbang = RIGHT;
							return 1;
						case RIGHT:
							return BalanceRight(ref node);
					}
				}

				if (node.number < number)
				{
					Way.Add(RIGHT);
					Res = DelNode(ref node.right, number);

					if (Res < 2) return Res;

					switch (node.canbang)
					{
						case LEFT:
							return BalanceLeft(ref node);
						case EQUAL:
							node.canbang = LEFT;
							return 1;
						case RIGHT:
							node.canbang = EQUAL;
							return 2;
					}
				}
			}
			return -2;
		}

		//Tim node the mang
	private int SearchStandFor(ref class_node Temp, ref class_node node)
		{
			int Res;

			if (node.left!=null)
			{
				Res = SearchStandFor(ref Temp, ref node.left);

				if (Res < 2) return Res;

				switch (node.canbang)
				{
					case LEFT:
						node.canbang = EQUAL;
						return 1;
					case EQUAL:
						node.canbang = RIGHT;
						return 2;
					case RIGHT:
						return BalanceRight(ref Temp);
				}
			}

			Temp.number = node.number;
			Temp = node;
			node = node.right;
			return 2;
		}



		private void FindANode(class_node node, int number)
		{
			if (node == null)
			{
				class_node Temp = new class_node();
				if (Way.Count != 0)
				{
					for (int i = 0; i < Way.Count-1; i++)
					{
						MoveDown(ref Temp, Way[i], 2);
					}
				}
				MessageBox.Show(" Khong tim thay node nao co gia tri " + number+" !");
				Temp = null;
				g.Clear(Color.White);
				VeCay_normal(Root);
				Input_TextBox.Clear();
				return;
			}
			else
			{
				if (node == Root && node.number == number)
				{
					DrawSearch(Root);
					MessageBox.Show(" Da tim thay node co gia tri " + number + " !");
					g.Clear(Color.White);
					VeCay_normal(Root);
					Input_TextBox.Clear();
					return;
				}
				if (node.number == number)
				{
					class_node Temp = new class_node();
					if (Way.Count != 0)
					{
						for (int i = 0; i < Way.Count; i++)
						{
							MoveDown(ref Temp, Way[i], 2);
						}
					}
					MessageBox.Show(" Da tim thay node co gia tri " + number+" !");
					Temp = null;
					g.Clear(Color.White);
					VeCay_normal(Root);
					Input_TextBox.Clear();
					return;
				}
				if (number < node.number)
				{
					Way.Add(LEFT);
					FindANode(node.left, number);
				}
				else
				{
					Way.Add(RIGHT);
					FindANode(node.right, number);
				}
			}
		}

		private void InItTree(ref class_node node)
		{
			node = null;
		}

		private int High(class_node node)
		{
			if (node==null)
			{
				return 0;
			}
			int a = High(node.left);
			int b = High(node.right); 
			if (a > b)
			{
				return (a + 1);
			}
			return (b + 1);
		}
		private void TotalLeafNode(class_node node)
		{
			if (node != null)
			{
				if (node.left == null && node.right == null)
				{
					Total_Leaf_Node++;
				}
				TotalLeafNode(node.left);
				TotalLeafNode(node.right);
			}
		}
		private void TotalNode(class_node node)
		{
			if (node != null)
			{
				Total_Node++;
				TotalNode(node.left);
				TotalNode(node.right);
			}
		}
		private void XacDinhSoPhanTu()
		{
			Total_Node = 0;
			TotalNode(Root);
			Total_Node_TextBox.Text = Total_Node.ToString();
			Total_Leaf_Node = 0;
			TotalLeafNode(Root);
			Total_Leaf_Node_TextBox.Text = Total_Leaf_Node.ToString();
			if (Total_Node == 0 || Total_Node == 1)
			{
				Total_Intermediate_Node = 0;
				Total_Intermediate_Node_TextBox.Text = Total_Intermediate_Node.ToString();
			}
			else
			{
				Total_Intermediate_Node_TextBox.Text = (Total_Node - Total_Leaf_Node - 1).ToString();
			}
			The_Height_Tree = High(Root) - 1;
			if (The_Height_Tree == -1)
			{
				The_Height_Tree = 0;
			}
			The_Height_Tree_TextBox.Text = The_Height_Tree.ToString();
		}
		#endregion

		#region Control 

		//  xoa toan cay 
		private void Del_Tree_Button_Click(object sender, EventArgs e)
		{
			InItTree(ref Root);
			g.Clear(Color.White);		
			Main_PictureBox.Image = bitmap;
			XacDinhSoPhanTu();
			MessageBox.Show(" Da xoa thanh cong cay! ");
		}

		private void Find_Button_Click(object sender, EventArgs e)
		{
			if (Input_TextBox.Text.Length > 0)
			{
				try
				{
					int Temp = Convert.ToInt32(Input_TextBox.Text);
					Way.Clear();
					FindANode(Root, Temp);
				}
				catch
				{
					MessageBox.Show(" Gia tri nhap khong dung");
				}
			}
			else
			{
				MessageBox.Show(" Ban chua nhap gia tri ");
			}
		}

		private void Del_Node_Button_Click(object sender, EventArgs e)
		{
			if (Input_TextBox.Text.Length > 0)
			{
				try
				{
					int Temp = Convert.ToInt32(Input_TextBox.Text);
					Way.Clear();
					int StatusDelNode = DelNode(ref Root, Temp);
					if (StatusDelNode == 0)
					{
						g.Clear(Color.White);
						Xd_ViTri(ref Root);
						VeCay_normal(Root);
					}
					else
					{
						Xd_ViTriCu(ref Root);
						Xd_ViTriMoi(ref Root);
						XacDinhTocDo();
						for (int i = 0; i < Speed; i++)
						{
							DiChuyenCay(ref Root);
							g.Clear(Color.White);
							VeCay_normal(Root);
							Thread.Sleep(2);
							Application.DoEvents();
						}
						g.Clear(Color.White);
						Xd_ViTri(ref Root);
						VeCay_normal(Root);
						XacDinhSoPhanTu();
						Input_TextBox.Clear();
					}
				}
				catch
				{
					MessageBox.Show(" Gia tri nhap khong dung");
				}	
			}
			else
			{
				MessageBox.Show(" Ban chua nhap gia tri ");
			}
		}

		private void Input_TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && Input_TextBox.Text.Length > 0)
			{
				int StatusInsert;
				try
				{
					int Temp = Convert.ToInt32(Input_TextBox.Text);
					Way.Clear();
					StatusInsert = InsertNode(ref Root, Temp);
					if ( StatusInsert == 0)
					{
						MessageBox.Show(" Da ton tai gia tri ");
						return;
					}
				}
				catch
				{
					MessageBox.Show(" Gia tri nhap khong dung");
				}
				Xd_ViTriCu(ref Root);
				Xd_ViTriMoi(ref Root);
				XacDinhTocDo();
				for (int i = 0; i < Speed; i++)
				{
					DiChuyenCay(ref Root);
					g.Clear(Color.White);
					VeCay_normal(Root);
					Thread.Sleep(2);
					Application.DoEvents();
				}
				g.Clear(Color.White);
				Xd_ViTri(ref Root);
				VeCay_normal(Root);
				XacDinhSoPhanTu();
				Input_TextBox.Clear();
			}
		}
	
		private void Random_Button_Click(object sender, EventArgs e)
		{
			int N_Temp = 0;			// bien tang so lan random thanh cong de so sanh voi gia tri so lan random duoc chon
			Random ran = new Random();
			int StatusInsert ;
			if (Random_NumericUpDown.Value > 0)
			{
				while (N_Temp < Random_NumericUpDown.Value)
				{
					Way.Clear();
					int value = ran.Next(100);
					StatusInsert = InsertNode(ref Root, value);
					if (StatusInsert != 0 )
					{
						N_Temp ++;
					}
					Xd_ViTriCu(ref Root);
					Xd_ViTriMoi(ref Root);
					XacDinhTocDo();
					for (int i = 0; i < Speed; i++)
					{
						DiChuyenCay(ref Root);
						g.Clear(Color.White);
						VeCay_normal(Root);
						Thread.Sleep(1);
						Application.DoEvents();
					}
					g.Clear(Color.White);
					Xd_ViTri(ref Root);
					VeCay_normal(Root);
					XacDinhSoPhanTu();
					Input_TextBox.Clear();
				}
			}
		}
		private void Main_PictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			MyMouseMove(Root, e.Location);		
		}
		private void MyMouseMove(class_node node, PointF p)
		{
			if (node != null && ((p.X >= node.vitri.X -5 && p.X <= node.vitri.X + 30) && (p.Y >= node.vitri.Y && p.Y <= node.vitri.Y + 30)))
			{
				ShowTextBox(node);
				return;
			}
			if (node != null)
			{
				MyMouseMove(node.left, p);
				MyMouseMove(node.right, p);
			}
		}
		
		private void Main_PictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{

			}
		}
		private void NLR(class_node node)
		{

		}
		private void NLR_Button_Click(object sender, EventArgs e)
		{

		}
		#endregion


	}
}





