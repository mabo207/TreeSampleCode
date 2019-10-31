using System;
using System.Collections.Generic;

namespace Tree
{
	class Program
	{
		class MazeNode
		{
			// ノードを表現するクラス
			public MazeNode(int i_index,List<MazeNode> i_children)
			{
				// コンストラクタ
				nodeIndex = i_index;
				children = i_children;
				foreach (MazeNode child in children)
				{
					child.parent = this;	// 子要素の親を自分にする
				}
				parent = null;	// 初期化時は自分の親は無いものとする
			}
			public void Print()
			{
				// 自分自身についての情報だけ出力し、子要素の情報の出力は子に任せる
				System.Console.Write(nodeIndex);	// ノード番号を出力
				int size = children.Count;	// 子要素の数
				if (size > 0)
				{
					// 子要素がある場合は、自分の番号のすぐ隣に{}で囲まれた子要素の情報が出力される
					System.Console.Write("{");
					for (int i = 0; i < size; i++)
					{
						children[i].Print();	// 子要素の情報の出力は子要素が責任を持って行う
						if (i+1<size)
						{
							// 末尾の子要素の右隣には","は現れない
							System.Console.Write(",");
						}
					}
					System.Console.Write("}");
				}
			}
			public void PrintRoots()
			{
				// 自分のルーツを出力する
				if (parent!=null)
				{
					// 親ノードより祖先側のノードのルーツを出力
					parent.PrintRoots();
				}
				System.Console.Write(" ");
				System.Console.Write(nodeIndex);
			}
			public int nodeIndex;	// ノードの番号
			public List<MazeNode> children; // 子要素を可変長配列で持つ、要素が無ければ末尾要素である事が分かる
			public MazeNode parent;	// 親ノード(null初期化されるので扱いに注意)
		}
		static MazeNode Create()
		{
			MazeNode node16 = new MazeNode(16, new List<MazeNode>());
			MazeNode node11 = new MazeNode(11, new List<MazeNode>());
			MazeNode node12 = new MazeNode(12, new List<MazeNode> { node16 });
			MazeNode node15 = new MazeNode(15, new List<MazeNode> { node11 });
			MazeNode node8 = new MazeNode(8, new List<MazeNode> { node12 });
			MazeNode node14 = new MazeNode(14, new List<MazeNode> { node15 });
			MazeNode node6 = new MazeNode(6, new List<MazeNode>());
			MazeNode node4 = new MazeNode(4, new List<MazeNode>());
			MazeNode node7 = new MazeNode(7, new List<MazeNode> { node8 });
			MazeNode node10 = new MazeNode(10, new List<MazeNode> { node6 });
			MazeNode node13 = new MazeNode(13, new List<MazeNode> { node14 });
			MazeNode node3 = new MazeNode(3, new List<MazeNode> { node4, node7 });
			MazeNode node9 = new MazeNode(9, new List<MazeNode> { node10, node13 });
			MazeNode node2 = new MazeNode(2, new List<MazeNode> { node3 });
			MazeNode node5 = new MazeNode(5, new List<MazeNode> { node9 });
			MazeNode node1 = new MazeNode(1, new List<MazeNode> { node2, node5 });
			return node1;
		}
		static void Main(string[] args)
		{
			MazeNode parent = Create();	// 迷路の作成
			parent.Print();	// 迷路の出力
			System.Console.WriteLine();	// 改行出力

			const int goal = 16;
			MazeNode goalNode = null;   // ゴールのノードをここに格納する
			// 深さ優先探索
			goalNode = DepthSearch(goal, parent);
			if (goalNode != null)
			{
				// ゴールが見つかった場合は、ルーツを表示
				System.Console.Write("Depth Search:");
				goalNode.PrintRoots();
				System.Console.WriteLine();	// 改行出力
			}
			// 幅優先探索
			goalNode = WidthSearch(goal, parent);
			if (goalNode!=null)
			{
				// ゴールが見つかった場合は、ルーツを表示
				System.Console.Write("Width Search:");
				goalNode.PrintRoots();
				System.Console.WriteLine();	// 改行出力
			}
		}
		static MazeNode DepthSearch(int goal, MazeNode parent)
		{
			MazeNode goalNode = null;
			List<MazeNode> depthSearchNode = new List<MazeNode>();	// 未探索ノード一覧（先入れ先出し）
			depthSearchNode.Add(parent);	// 探索用ノードに祖先ノードを追加
			while (depthSearchNode.Count != 0)
			{
				MazeNode searching = depthSearchNode[depthSearchNode.Count - 1];	// 末尾要素が探索対象
				// widthSearchNodeがなくなるまで探索処理を続ける
				if (searching.nodeIndex == goal)
				{
					// ゴールが見つかったので探索終了
					goalNode = searching;
					break;
				}
				else
				{
					// 先頭要素（今調べたノード）を削除
					depthSearchNode.RemoveAt(depthSearchNode.Count - 1);	// 末尾要素は(Count-1)番目の要素
					// 子要素を探索用ノードの末尾に追加
					depthSearchNode.AddRange(searching.children);
				}
			}
			return goalNode;
		}
		static MazeNode WidthSearch(int goal, MazeNode parent)
		{
			MazeNode goalNode = null;
			List<MazeNode> widthSearchNode = new List<MazeNode>();	// 未探索ノード一覧（先入れ後出し）
			widthSearchNode.Add(parent);	// 探索用ノードに祖先ノードを追加
			while (widthSearchNode.Count != 0)
			{
				MazeNode searching = widthSearchNode[0];	// 先頭要素が探索対象
				// widthSearchNodeがなくなるまで探索処理を続ける
				if (searching.nodeIndex == goal)
				{
					// ゴールが見つかったので探索終了
					goalNode = searching;
					break;
				}
				else
				{
					// 先頭要素（今調べたノード）を削除
					widthSearchNode.RemoveAt(0);	// 先頭要素は0番目の要素
					// 子要素を探索用ノードの末尾に追加
					widthSearchNode.AddRange(searching.children);
				}
			}
			return goalNode;
		}
	}
}
